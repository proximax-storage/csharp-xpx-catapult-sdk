using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;

using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions.Builders;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions.Messages;
using Xunit;
using Xunit.Abstractions;
using Xunit.Priority;

namespace ProximaX.Sirius.Chain.Sdk.Tests.E2E
{
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class E2EMultiSigAccountTests : IClassFixture<E2EBaseFixture>
    {
        private readonly E2EBaseFixture Fixture;
        private readonly ITestOutputHelper Log;

        public E2EMultiSigAccountTests(E2EBaseFixture fixture, ITestOutputHelper log)
        {
            Fixture = fixture;
            Log = log;
        }

        [Fact, Priority(0)]
        public async Task Should_Convert_Account_To_1to2_MultiSig()
        {
            // Get current network type

            // Create an account to be converted to multisig with some money
            // var account = await GenerateAccountAndSendSomeMoney(100);
            Log.WriteLine($"MultiSig account {Fixture.MultiSigAccount}");

            // Create two other accounts
            // var cosignatory1 = Account.GenerateNewAccount(NetworkType);
            Log.WriteLine($"MultiSig cosignatory1 {Fixture.Cosignatory1}");

            // var cosignatory2 = Account.GenerateNewAccount(NetworkType);
            Log.WriteLine($"MultiSig cosignatory2 {Fixture.Cosignatory2}");

            // Create a modify multisig account transaction to convert the shared account into a multisig account
            // 1 to 2 multisig
            var multiSigBuilder = new ModifyMultisigAccountTransactionBuilder();
            multiSigBuilder.SetDeadline(Deadline.Create())
                .SetMinApprovalDelta(1)
                .SetMinRemovalDelta(1)
                .SetModifications(new List<MultisigCosignatoryModification>
                {
                    new MultisigCosignatoryModification(MultisigCosignatoryModificationType.ADD,
                        Fixture.Cosignatory1.PublicAccount),
                    new MultisigCosignatoryModification(MultisigCosignatoryModificationType.ADD,
                        Fixture.Cosignatory2.PublicAccount),
                })
                .SetNetworkType(Fixture.NetworkType);
            var convertIntoMultisigTransaction = multiSigBuilder.Build();

            // Create an aggregate bonded transaction, wrapping the modify multisig account transaction
            var aggregateTransaction = AggregateTransaction.CreateBonded(
                Deadline.Create(),
                new List<Transaction>
                {
                    convertIntoMultisigTransaction.ToAggregate(Fixture.MultiSigAccount.PublicAccount)
                },
                Fixture.NetworkType);

            // Sign the aggregate transaction using the private key of the multisig account
            var signedTransaction = Fixture.MultiSigAccount.Sign(aggregateTransaction, Fixture.GenerationHash);

            // Before sending an aggregate bonded transaction,
            // the future multisig account needs to lock at least 10 cat.currency.
            // This transaction is required to prevent network spamming and ensure that
            // the inner transactions are cosigned
            var builder = new LockFundsTransactionBuilder();
            builder.SetDeadline(Deadline.Create())
                .SetDuration((ulong)700)
                .SetMosaic(NetworkCurrencyMosaic.CreateRelative(10))
                .SetSignedTransaction(signedTransaction)
                .SetNetworkType(Fixture.NetworkType);

            var hashLockTransaction = builder.Build();

            var hashLockTransactionSigned = Fixture.SeedAccount.Sign(hashLockTransaction, Fixture.GenerationHash);

            // register transaction with web socket
            try
            {
                var hashLocktx = Fixture.SiriusWebSocketClient.Listener
                    .ConfirmedTransactionsGiven(Fixture.SeedAccount.Address).Take(1)
                    .Timeout(TimeSpan.FromSeconds(500));

                Fixture.WatchForFailure(hashLockTransactionSigned);

                Log.WriteLine($"Going to announce hash lock transaction {hashLockTransactionSigned.Hash}");

                // Announce the hash lock transaction
                await Fixture.SiriusClient.TransactionHttp.Announce(hashLockTransactionSigned);

                // Wait for the hash lock transaction to be confirmed
                var hashLockConfirmed = await hashLocktx;

                // After the hash lock transaction has been confirmed,
                // announce the aggregate transaction.
                if (hashLockConfirmed.TransactionInfo.Hash == hashLockTransactionSigned.Hash)
                {
                    Fixture.WatchForFailure(signedTransaction);

                    Log.WriteLine($"Going to announce aggregate bonded transaction {signedTransaction.Hash}");

                    // Announce the hash lock transaction
                    await Fixture.SiriusClient.TransactionHttp.AnnounceAggregateBonded(signedTransaction);

                    // sleep for await
                    Thread.Sleep(5000);

                    // Cosign the aggregate transaction with cosignatory1
                    var cosignatory1Cosigned = Fixture.SiriusWebSocketClient.Listener.CosignatureAdded(Fixture.Cosignatory1.Address).Take(1).Timeout(TimeSpan.FromSeconds(2000));

                    var cosignatory1AggTxs = await Fixture.SiriusClient.AccountHttp.AggregateBondedTransactions(Fixture.Cosignatory1.PublicAccount);
                    foreach (AggregateTransaction tx in cosignatory1AggTxs.Transactions)
                    {
                        if (!tx.IsSignedByAccount(Fixture.Cosignatory1.PublicAccount))
                        {
                            var cosignatureSignedTransaction = CosignAggregateBondedTransaction(tx, Fixture.Cosignatory1);

                            Fixture.WatchForFailure(cosignatureSignedTransaction);

                            await Fixture.SiriusClient.TransactionHttp.AnnounceAggregateBondedCosignatureAsync(cosignatureSignedTransaction);

                            Thread.Sleep(8000);
                        }
                    }

                    // Cosign the aggregate transaction with cosignatory2
                    var cosignatory2Cosigned = Fixture.SiriusWebSocketClient.Listener
                          .CosignatureAdded(Fixture.Cosignatory2.Address).Take(1)
                          .Timeout(TimeSpan.FromSeconds(2000));
                    var cosignatory2AggTxs = await Fixture.SiriusClient.AccountHttp.AggregateBondedTransactions(Fixture.Cosignatory2.PublicAccount);

                    foreach (AggregateTransaction tx in cosignatory2AggTxs.Transactions)
                    {
                        if (!tx.IsSignedByAccount(Fixture.Cosignatory2.PublicAccount))
                        {
                            var cosignatureSignedTransaction = CosignAggregateBondedTransaction(tx, Fixture.Cosignatory2);

                            Fixture.WatchForFailure(cosignatureSignedTransaction);

                            await Fixture.SiriusClient.TransactionHttp.AnnounceAggregateBondedCosignatureAsync(cosignatureSignedTransaction);

                            Thread.Sleep(2000);
                        }
                    }

                    Thread.Sleep(10000);
                    // verify the account is multisig
                    var multiSigAcc = await Fixture.SiriusClient.AccountHttp.GetMultisigAccountInfo(Fixture.MultiSigAccount.Address);
                    Log.WriteLine($"Multisig account {multiSigAcc}");

                    multiSigAcc.IsMultisig.Should().BeTrue();
                    multiSigAcc.MinApproval.Should().Be(1);
                    multiSigAcc.MinRemoval.Should().Be(1);
                    multiSigAcc.Cosignatories.Should().HaveCount(2);

                    Log.WriteLine($"Completed");
                }
            }
            catch (Exception e)
            {
                Log.WriteLine(e.Message);
            }
            /*  finally
              {
                  try
                  {
                      //SiriusWebSocketClient.Listener.Close();
                  }
                  catch (Exception)
                  {
                      //do nothing
                  }
              }*/
        }

        [Fact, Priority(1)]
        public async Task Should_Send_Money_From_Multsi_From_1To2_Cosignatory()
        {
            var recipient = Account.GenerateNewAccount(Fixture.NetworkType);

            Log.WriteLine($"MultiSig account {Fixture.MultiSigAccount}");
            Log.WriteLine($"Recipient account {recipient}");
            Log.WriteLine($"Cosignatory1 account {Fixture.Cosignatory1}");
            var transferTransaction = TransferTransaction.Create(
                Deadline.Create(),
                Recipient.From(recipient.Address),
                new List<Mosaic> { NetworkCurrencyMosaic.CreateAbsolute(10) },
                PlainMessage.Create("sending 10 prx.xpx"),
                Fixture.NetworkType);

            var aggregateTransaction = AggregateTransaction.CreateComplete(
                Deadline.Create(),
                new List<Transaction> {
                    transferTransaction.ToAggregate(Fixture.MultiSigAccount.PublicAccount)
                },
                Fixture.NetworkType);

            var signedTransaction = Fixture.Cosignatory1.Sign(aggregateTransaction, Fixture.GenerationHash);

            var cosignatory1ConfirmedTx = Fixture.SiriusWebSocketClient.Listener
                .ConfirmedTransactionsGiven(Fixture.Cosignatory1.Address).Take(1)
                .Timeout(TimeSpan.FromSeconds(2000));
            Log.WriteLine($"Going to announce aggregate bonded transaction {signedTransaction.Hash}");
            await Fixture.SiriusClient.TransactionHttp.Announce(signedTransaction);

            var result = await cosignatory1ConfirmedTx;

            Thread.Sleep(5000);

            if (result.IsConfirmed())
            {
                var recipientAccountInfo = await Fixture.SiriusClient.AccountHttp.GetAccountInfo(recipient.Address);
                Log.WriteLine($"Recipient info {recipientAccountInfo.Address}");

                recipientAccountInfo.Mosaics.Should().HaveCount(1);
                recipientAccountInfo.Mosaics[0].Amount.Should().Be(10);
            }
        }

        [Fact, Priority(2)]
        public async Task Should_Convert_Account_To_2to2_MultiSig()
        {
            Log.WriteLine($"MultiSig account {Fixture.MultiSigAccount}");
            Log.WriteLine($"Cosignatory1 account {Fixture.Cosignatory1}");

            // Define a modify multisig account transaction to increase the minAprovalDelta in one unit
            var modifyMultisigAccountTransaction = ModifyMultisigAccountTransaction.Create(
               Deadline.Create(),
               1,
               0,
               new List<MultisigCosignatoryModification>
               {
               },
               Fixture.NetworkType);

            // Wrap the modify multisig account transaction in an aggregate transaction,
            // attaching the multisig public key as the signer
            var aggregateTransaction = AggregateTransaction.CreateComplete(
                Deadline.Create(),
                new List<Transaction>
                {
                    modifyMultisigAccountTransaction.ToAggregate(Fixture.MultiSigAccount.PublicAccount)
                },
                Fixture.NetworkType
                );

            var signedTransaction = Fixture.Cosignatory1.Sign(aggregateTransaction, Fixture.GenerationHash);

            var cosignatory1ConfirmedTx = Fixture.SiriusWebSocketClient.Listener
                    .ConfirmedTransactionsGiven(Fixture.Cosignatory1.Address).Take(1)
                    .Timeout(TimeSpan.FromSeconds(8000));
            var result = await cosignatory1ConfirmedTx;
            Log.WriteLine($"Going to announce aggregate completed transaction {result.TransactionInfo.Hash}");

            // Announce the hash lock transaction
            await Fixture.SiriusClient.TransactionHttp.Announce(signedTransaction);

            Thread.Sleep(5000);

            // verify the account is multisig
            var multiSigAcc = await Fixture.SiriusClient.AccountHttp.GetMultisigAccountInfo(Fixture.MultiSigAccount.Address);
            Log.WriteLine($"Multisig account {multiSigAcc}");

            multiSigAcc.IsMultisig.Should().BeTrue();
            multiSigAcc.MinApproval.Should().Be(2);
            multiSigAcc.MinRemoval.Should().Be(1);
            multiSigAcc.Cosignatories.Should().HaveCount(2);
        }

        [Fact, Priority(3)]
        public async Task Should_Add_Cosignatory()
        {
            Log.WriteLine($"MultiSig account {Fixture.MultiSigAccount}");
            Log.WriteLine($"Cosignatory1 account {Fixture.Cosignatory1}");
            Log.WriteLine($"Cosignatory3 account {Fixture.Cosignatory3}");

            // Define a modify multisig account transaction to increase the minAprovalDelta in one unit
            var modifyMultisigAccountTransaction = ModifyMultisigAccountTransaction.Create(
               Deadline.Create(),
               0,
               0,
               new List<MultisigCosignatoryModification>
               {
                   new MultisigCosignatoryModification(MultisigCosignatoryModificationType.ADD,
                        Fixture.Cosignatory1.PublicAccount),
                   new MultisigCosignatoryModification(MultisigCosignatoryModificationType.ADD,
                        Fixture.Cosignatory3.PublicAccount),
               },
               Fixture.NetworkType);

            // Wrap the modify multisig account transaction in an aggregate transaction,
            // attaching the multisig public key as the signer
            var aggregateTransaction = AggregateTransaction.CreateComplete(
                Deadline.Create(),
                new List<Transaction>
                {
                    modifyMultisigAccountTransaction.ToAggregate(Fixture.MultiSigAccount.PublicAccount)
                },
                Fixture.NetworkType
                );

            var signedTransaction = Fixture.MultiSigAccount.Sign(aggregateTransaction, Fixture.GenerationHash);
            Fixture.WatchForFailure(signedTransaction);

            // Before sending an aggregate bonded transaction,
            // the future multisig account needs to lock at least 10 cat.currency.
            // This transaction is required to prevent network spamming and ensure that
            // the inner transactions are cosigned
            var hashLockTransaction = HashLockTransaction.Create(
                Deadline.Create(),
                NetworkCurrencyMosaic.CreateRelative(10),
                (ulong)700,
                signedTransaction,
                Fixture.NetworkType);

            var hashLockTransactionSigned = Fixture.SeedAccount.Sign(hashLockTransaction, Fixture.GenerationHash);
            try
            {
                var hashLocktx = Fixture.SiriusWebSocketClient.Listener
                   .ConfirmedTransactionsGiven(Fixture.Cosignatory1.Address).Take(1)
                   .Timeout(TimeSpan.FromSeconds(500));
                Fixture.WatchForFailure(hashLockTransactionSigned);

                // Announce the hash lock transaction
                Log.WriteLine($"Going to announce hash lock transaction {hashLockTransactionSigned.Hash}");
                await Fixture.SiriusClient.TransactionHttp.Announce(hashLockTransactionSigned);

                var hashLockConfirmed = await hashLocktx;

                if (hashLockConfirmed.TransactionInfo.Hash == hashLockTransactionSigned.Hash)
                {
                    // Announce the AnnounceAggregateBonded transaction
                    await Fixture.SiriusClient.TransactionHttp.AnnounceAggregateBonded(signedTransaction);
                    Log.WriteLine($"Going to announce aggregate bonded transaction {signedTransaction.Hash}");

                    // sleep for await
                    Thread.Sleep(8000);
                    Fixture.WatchForFailure(signedTransaction);

                    var cosignatory3AggTxs = await Fixture.SiriusClient.AccountHttp.AggregateBondedTransactions(Fixture.Cosignatory3.PublicAccount);
                    foreach (AggregateTransaction tx in cosignatory3AggTxs.Transactions)
                    {
                        if (!tx.IsSignedByAccount(Fixture.Cosignatory3.PublicAccount))
                        {
                            var cosignatureSignedTransaction = CosignAggregateBondedTransaction(tx, Fixture.Cosignatory3);

                            Fixture.WatchForFailure(cosignatureSignedTransaction);
                            Log.WriteLine($"Going to announce cosign aggregate bonded transaction {cosignatureSignedTransaction.ParentHash}");
                            await Fixture.SiriusClient.TransactionHttp.AnnounceAggregateBondedCosignatureAsync(cosignatureSignedTransaction);

                            Thread.Sleep(2000);
                        }
                    }

                    var cosignatory2AggTxs = await Fixture.SiriusClient.AccountHttp.AggregateBondedTransactions(Fixture.Cosignatory2.PublicAccount);
                    foreach (AggregateTransaction tx in cosignatory2AggTxs.Transactions)
                    {
                        if (!tx.IsSignedByAccount(Fixture.Cosignatory2.PublicAccount))
                        {
                            var cosignatureSignedTransaction = CosignAggregateBondedTransaction(tx, Fixture.Cosignatory2);

                            Fixture.WatchForFailure(cosignatureSignedTransaction);

                            await Fixture.SiriusClient.TransactionHttp.AnnounceAggregateBondedCosignatureAsync(cosignatureSignedTransaction);

                            Thread.Sleep(2000);
                        }
                    }

                    Log.WriteLine($"Completed");
                }
            }
            catch (Exception e)
            {
                Log.WriteLine(e.Message);
            }
        }

        private CosignatureSignedTransaction CosignAggregateBondedTransaction(AggregateTransaction transaction, Account account)
        {
            var cosignatureTransaction = CosignatureTransaction.Create(transaction);

            return account.SignCosignatureTransaction(cosignatureTransaction);
        }
    }
}