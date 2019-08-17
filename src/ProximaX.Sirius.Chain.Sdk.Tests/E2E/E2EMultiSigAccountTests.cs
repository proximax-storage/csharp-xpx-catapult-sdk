using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions.Messages;
using Xunit;
using Xunit.Abstractions;
using Xunit.Priority;

namespace ProximaX.Sirius.Chain.Sdk.Tests.E2E
{
    //[Collection("E2ETestFixtureCollection")]
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Assembly)]
    public class E2EMultiSigAccountTests: IClassFixture<E2ETestFixture>
    {
        private readonly E2ETestFixture _fixture;

        private readonly ITestOutputHelper _output;

        private Account cosignatory1;
        private Account cosignatory2;
        private Account cosignatory3;
        private Account account;
        private NetworkType networkType;
        private string networkGenerationHash;

        public E2EMultiSigAccountTests(E2ETestFixture fixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            _output = output;
            account = _fixture.MultiSigAccount;
            cosignatory1 = _fixture.Cosignatory1;
            cosignatory2 = _fixture.Cosignatory2;
            cosignatory3 = _fixture.Cosignatory3;
            networkType = _fixture.Client.NetworkHttp.GetNetworkType().Wait();
            networkGenerationHash =  _fixture.Client.BlockHttp.GetGenerationHash().Wait();
            //Initialise().Wait();
            // multiMultisigAccount = Account.GenerateNewAccount(_fixture.Client.NetworkHttp.GetNetworkType().Wait());
            //Account.GenerateNewAccount(_fixture.Client.NetworkHttp.GetNetworkType().Wait());
            // alice = Account.GenerateNewAccount(_fixture.Client.NetworkHttp.GetNetworkType().Wait());
            // bob = Account.GenerateNewAccount(_fixture.Client.NetworkHttp.GetNetworkType().Wait());
            // cosig3 = Account.GenerateNewAccount(_fixture.Client.NetworkHttp.GetNetworkType().Wait());
        }

     

        /*
        [Fact, Priority(0)]
        public async Task Should_Create_MultiSig_AggregateBondedAccount()
        {
            var networkType = _fixture.Client.NetworkHttp.GetNetworkType().Wait();
            multisigAccount = await _fixture.GenerateAccountAndSendSomeMoney(1000);

            // await _fixture.WebSocket.Listener.Open();

            var tx = _fixture.WebSocket.Listener.ConfirmedTransactionsGiven(multisigAccount.Address).Take(1)
                .Timeout(TimeSpan.FromSeconds(500));



            var aliceTx = _fixture.WebSocket.Listener.ConfirmedTransactionsGiven(alice.Address).Take(1)
                .Timeout(TimeSpan.FromSeconds(500));

            _output.WriteLine($"Going to create multisig for account {multisigAccount}");

            var changeToMultisig = ModifyMultisigAccountTransaction.Create(
                Deadline.Create(),
                1,
                1,
                new List<MultisigCosignatoryModification>()
                {
                    new MultisigCosignatoryModification(MultisigCosignatoryModificationType.ADD,
                        alice.PublicAccount),
                    new MultisigCosignatoryModification(MultisigCosignatoryModificationType.ADD,
                        bob.PublicAccount),
                },
                networkType);

            var changeToMultisigAggregate = AggregateTransaction.CreateBonded(
               Deadline.Create(),
               new List<Transaction>
               {
                    changeToMultisig.ToAggregate(multisigAccount.PublicAccount)
               },
               networkType);

            //var signedChangeToMultisig = multisigAccount.Sign(changeToMultisig, _fixture.Environment.GenerationHash);
            var cosignatories = new List<Account>
            {
                alice, bob
            };

            var signedChangeToMultisig = changeToMultisigAggregate.SignTransactionWithCosigners(multisigAccount, cosignatories, _fixture.Environment.GenerationHash);

            // lock fund transaction first
            var lockFundsTransaction = LockFundsTransaction.Create(
                Deadline.Create(),
                NetworkCurrencyMosaic.CreateRelative(10),
                10000,
                signedChangeToMultisig,
                networkType);

            var signLockFund = lockFundsTransaction.SignWith(multisigAccount, _fixture.Environment.GenerationHash);

            _output.WriteLine($"Going to announce multisig transaction {signLockFund.Hash}");

            _fixture.WatchForFailure(signLockFund);

            await _fixture.Client.TransactionHttp.Announce(signLockFund);

            var result = await tx;

            await _fixture.Client.TransactionHttp.AnnounceAggregateBonded(signedChangeToMultisig);

            _output.WriteLine($"Request announce multisig confirmed with transaction {result.TransactionInfo.Hash}");


            var aggregateBondedTransactions = await _fixture.Client.AccountHttp.AggregateBondedTransactions(alice.PublicAccount);
            foreach (var t in aggregateBondedTransactions)
            {
                var agT = t as AggregateTransaction;
                if (agT.IsSignedByAccount(alice.PublicAccount))
                {
                    //do cosign transaction
                    var cosignatureTransaction = CosignatureTransaction.Create(agT);
                    var cosignAggregateBondedTransaction = alice.SignCosignatureTransaction(cosignatureTransaction);

                    _output.WriteLine($"Going to announce cosignAggregateBondedTransaction with parent Hash {cosignAggregateBondedTransaction.ParentHash}");

                    await _fixture.Client.TransactionHttp.AnnounceAggregateBondedCosignatureAsync(cosignAggregateBondedTransaction);

                    var resultTx = await aliceTx;

                    _output.WriteLine($"Request announce announce cosignAggregateBondedTransaction confirmed with transaction {resultTx.TransactionInfo.Hash}");

                }
            }



        }

        [Fact, Priority(1)]
        public async Task Should_Create_MultiSig_Account_Aggregate()
        {

            var networkType = _fixture.Client.NetworkHttp.GetNetworkType().Wait();
            var aggMulti = Account.GenerateNewAccount(networkType);

            await _fixture.WebSocket.Listener.Open();

            var tx = _fixture.WebSocket.Listener.ConfirmedTransactionsGiven(aggMulti.Address).Take(1)
                .Timeout(TimeSpan.FromSeconds(500));

            _output.WriteLine($"Going to create multisig for account {aggMulti}");

            var changeToMultisig = ModifyMultisigAccountTransaction.Create(
                Deadline.Create(),
                1,
                1,
                new List<MultisigCosignatoryModification>()
                {
                    new MultisigCosignatoryModification(MultisigCosignatoryModificationType.ADD,
                        alice.PublicAccount),
                    new MultisigCosignatoryModification(MultisigCosignatoryModificationType.ADD,
                        bob.PublicAccount),
                },
                networkType);

            var aggregateTransaction = AggregateTransaction.CreateComplete(
                Deadline.Create(),
                new List<Transaction>()
                {
                    changeToMultisig.ToAggregate(aggMulti.PublicAccount)
                },
                networkType);

            var signedChangeToMultisig = aggMulti.Sign(aggregateTransaction, _fixture.Environment.GenerationHash);

            _output.WriteLine($"Going to announce multisig transaction {signedChangeToMultisig.Hash}");

            _fixture.WatchForFailure(signedChangeToMultisig);

            await _fixture.Client.TransactionHttp.Announce(signedChangeToMultisig);

            var result = await tx;

            _output.WriteLine($"Request announce multisig confirmed with transaction {result.TransactionInfo.Hash}");

            var mai = await _fixture.Client.AccountHttp.GetMultisigAccountInfo(aggMulti.Address);
            mai.IsMultisig.Should().BeTrue();
            mai.MinRemoval.Should().Be(1);
            mai.MinApproval.Should().Be(1);
            mai.Cosignatories.Should().HaveCount(2);

            var graphInfo = await _fixture.Client.AccountHttp.GetMultisigAccountGraphInfo(aggMulti.Address);
            graphInfo.GetLevelsNumber().Should().HaveCount(2);
        }


        [Fact, Priority(2)]
        public async Task Should_Transfer_From_MultiSig1_Of_2_Aggregate()
        {
            var networkType = _fixture.Client.NetworkHttp.GetNetworkType().Wait();
            var seedAccount = _fixture.SeedAccount;

            await _fixture.WebSocket.Listener.Open();

            var transfer = TransferTransaction.Create(
                Deadline.Create(),
                seedAccount.Address,
                new List<Mosaic>()
                {
                    NetworkCurrencyMosaic.CreateAbsolute(1)
                },
                PlainMessage.Create("Empty"),
                networkType);

            var aggregateTransaction = AggregateTransaction.CreateComplete(
                Deadline.Create(),
                new List<Transaction>()
                {
                    transfer.ToAggregate(multisigAccount.PublicAccount)
                },
                networkType);

            var signedTransaction = alice.Sign(aggregateTransaction, _fixture.Environment.GenerationHash);

            //_fixture.WatchForFailure(signedTransaction);

            await _fixture.Client.TransactionHttp.Announce(signedTransaction);

            var status = await _fixture.WebSocket.Listener.TransactionStatus(alice.Address)
                .Where(e => e.Hash == signedTransaction.Hash).Take(1).Timeout(TimeSpan.FromSeconds(200));

            status.Status.Should().BeEquivalentTo("Failure_Aggregate_Ineligible_Cosigners");

        }
        */

        [Fact, Priority(0)]
        public async Task Should_Convert_Account_To_1to2_MultiSig()
        {
            // Get current network type 
        

            // Create an account to be converted to multisig with some money
            // var account = await _fixture.GenerateAccountAndSendSomeMoney(100);
            _output.WriteLine($"MultiSig account {account}");
            // var cosignatory1 = await _fixture.GenerateAccountAndSendSomeMoney(100);
            // var cosignatory2 = await _fixture.GenerateAccountAndSendSomeMoney(100);
            //var account = GetAccountFromKey("F40DAD4B22DAFBBAA6BD33531E14C51810AFEF5BDDBFFE5E2ACB1C1D77EF0D93", networkType);
            // var cosignatory2 = GetAccountFromKey("574D23DEF92D1C89DD50750DF303EA2A4C26ADBC9B966546B6BACAB827761088", networkType);
            // var cosignatory1 = GetAccountFromKey("5F558CE1B471980D76BE8C716334E098DEDDCACA71736620BA7B48436C5972D2", networkType);

            // Create two other accounts 
            // var cosignatory1 = Account.GenerateNewAccount(networkType);
            _output.WriteLine($"MultiSig cosignatory1 {cosignatory1}");

            // var cosignatory2 = Account.GenerateNewAccount(networkType);
            _output.WriteLine($"MultiSig cosignatory2 {cosignatory2}");

            // Create a modify multisig account transaction to convert the shared account into a multisig account
            // 1 to 2 multisig
            var convertIntoMultisigTransaction = ModifyMultisigAccountTransaction.Create(
                Deadline.Create(),
                1,
                1,
                new List<MultisigCosignatoryModification>
                {
                    new MultisigCosignatoryModification(MultisigCosignatoryModificationType.ADD,
                        cosignatory1.PublicAccount),
                    new MultisigCosignatoryModification(MultisigCosignatoryModificationType.ADD,
                        cosignatory2.PublicAccount),
                },
                networkType);

            // Create an aggregate bonded transaction, wrapping the modify multisig account transaction
            var aggregateTransaction = AggregateTransaction.CreateBonded(
                Deadline.Create(),
                new List<Transaction>
                {
                    convertIntoMultisigTransaction.ToAggregate(account.PublicAccount)
                },
                networkType);

          

            // Sign the aggregate transaction using the private key of the multisig account
            var signedTransaction = account.Sign(aggregateTransaction, networkGenerationHash);

            // Before sending an aggregate bonded transaction,
            // the future multisig account needs to lock at least 10 cat.currency.
            // This transaction is required to prevent network spamming and ensure that 
            // the inner transactions are cosigned
            var hashLockTransaction = HashLockTransaction.Create(
                Deadline.Create(),
                NetworkCurrencyMosaic.CreateRelative(10),
                (ulong)700,
                signedTransaction,
                networkType);

            var hashLockTransactionSigned = account.Sign(hashLockTransaction, networkGenerationHash);

            // register transaction with web socket
            try
            {
                // await _fixture.WebSocket.Listener.Open();

                var hashLocktx = _fixture.WebSocket.Listener
                    .ConfirmedTransactionsGiven(account.Address).Take(1)
                    .Timeout(TimeSpan.FromSeconds(500));

                _fixture.WatchForFailure(hashLockTransactionSigned);

                _output.WriteLine($"Going to announce hash lock transaction {hashLockTransactionSigned.Hash}");

                // Announce the hash lock transaction
                await _fixture.Client.TransactionHttp.Announce(hashLockTransactionSigned);

                // Wait for the hash lock transaction to be confirmed
                var hashLockConfirmed = await hashLocktx;

                // After the hash lock transaction has been confirmed,
                // announce the aggregate transaction.
                if (hashLockConfirmed.TransactionInfo.Hash == hashLockTransactionSigned.Hash)
                {
                    var aggBonded = _fixture.WebSocket.Listener
                       .AggregateBondedAdded(account.Address).Take(1)
                     .Timeout(TimeSpan.FromSeconds(2000));

                    var aggBondedConfirmed = _fixture.WebSocket.Listener
                      .ConfirmedTransactionsGiven(account.Address).Take(1)
                      .Timeout(TimeSpan.FromSeconds(2000));

                    _fixture.WatchForFailure(signedTransaction);

                    _output.WriteLine($"Going to announce aggregate bonded transaction {signedTransaction.Hash}");

                    // Announce the hash lock transaction
                    await _fixture.Client.TransactionHttp.AnnounceAggregateBonded(signedTransaction);

                    // sleep for await
                    Thread.Sleep(5000);

                    //var aggBondedTx = await aggBondedConfirmed;

                    
                    //if(aggBondedTx.IsConfirmed())
                    // {
                    // Cosign the aggregate transaction with cosignatory1
                    var cosignatory1Cosigned = _fixture.WebSocket.Listener
                            .CosignatureAdded(cosignatory1.Address).Take(1)
                            .Timeout(TimeSpan.FromSeconds(2000));

                    var cosignatory1AggTxs = await _fixture.Client.AccountHttp.AggregateBondedTransactions(cosignatory1.PublicAccount);
                    foreach (AggregateTransaction tx in cosignatory1AggTxs)
                    {
                        if (!tx.IsSignedByAccount(cosignatory1.PublicAccount))
                        {
                            var cosignatureSignedTransaction = CosignAggregateBondedTransaction(tx, cosignatory1);

                            _fixture.WatchForFailure(cosignatureSignedTransaction);

                            await _fixture.Client.TransactionHttp.AnnounceAggregateBondedCosignatureAsync(cosignatureSignedTransaction);

                            Thread.Sleep(2000);
                            //var resultTx = await cosignatory1Cosigned;

                            //_output.WriteLine($"Completed Cosign 1 {resultTx}");

                        }
                    }

                    // Cosign the aggregate transaction with cosignatory2
                    var cosignatory2Cosigned = _fixture.WebSocket.Listener
                          .CosignatureAdded(cosignatory2.Address).Take(1)
                          .Timeout(TimeSpan.FromSeconds(2000));


                    var cosignatory2AggTxs = await _fixture.Client.AccountHttp.AggregateBondedTransactions(cosignatory2.PublicAccount);
                    foreach (AggregateTransaction tx in cosignatory2AggTxs)
                    {
                        if (!tx.IsSignedByAccount(cosignatory2.PublicAccount))
                        {
                            var cosignatureSignedTransaction = CosignAggregateBondedTransaction(tx, cosignatory2);

                            _fixture.WatchForFailure(cosignatureSignedTransaction);

                            await _fixture.Client.TransactionHttp.AnnounceAggregateBondedCosignatureAsync(cosignatureSignedTransaction);

                            Thread.Sleep(2000);
                            //var resultTx = await cosignatory2Cosigned;

                            //_output.WriteLine($"Completed Cosign 2 {resultTx}");

                        }
                    }

                    // }
                    Thread.Sleep(10000);
                    // verify the account is multisig
                    var multiSigAcc = await _fixture.Client.AccountHttp.GetMultisigAccountInfo(account.Address);
                    _output.WriteLine($"Multisig account {multiSigAcc}");

                    multiSigAcc.IsMultisig.Should().BeTrue();
                    multiSigAcc.MinApproval.Should().Be(1);
                    multiSigAcc.MinRemoval.Should().Be(1);
                    multiSigAcc.Cosignatories.Should().HaveCount(2);

                    _output.WriteLine($"Completed");


                }
            }
            catch (Exception e)
            {
                _output.WriteLine(e.Message);
            }
            finally
            {
                try
                {
                    // _fixture.WebSocket.Listener.Close();
                }
                catch (Exception)
                {
                    //do nothing
                }

            }

        }

        [Fact, Priority(1)]
        public async Task Should_Convert_Account_To_2to2_MultiSig()
        {
             //account = Account.CreateFromPrivateKey("6681DC3BBEEEDF213160A27DDCA551B7AC8DC3BB79B8BDC059DD2CEA7B2E9C42", networkType);
             //cosignatory1 = Account.CreateFromPrivateKey("3B2B0AE238CF78E65E0F0B7110F7B4E73B8C56AB0282F98D22A39BB67D127609", networkType);

            _output.WriteLine($"MultiSig account {account}");
            _output.WriteLine($"Cosignatory1 account {cosignatory1}");

            
            // Define a modify multisig account transaction to increase the minAprovalDelta in one unit
            var modifyMultisigAccountTransaction = ModifyMultisigAccountTransaction.Create(
               Deadline.Create(),
               1,
               0,
               new List<MultisigCosignatoryModification>
               {
               },
               networkType);

            // Wrap the modify multisig account transaction in an aggregate transaction, 
            // attaching the multisig public key as the signer
            var aggregateTransaction = AggregateTransaction.CreateComplete(
                Deadline.Create(),
                new List<Transaction>
                {
                    modifyMultisigAccountTransaction.ToAggregate(account.PublicAccount)
                },
                networkType
                );

            var signedTransaction = cosignatory1.Sign(aggregateTransaction, networkGenerationHash);

            var cosignatory1ConfirmedTx = _fixture.WebSocket.Listener
                    .ConfirmedTransactionsGiven(cosignatory1.Address).Take(1)
                    .Timeout(TimeSpan.FromSeconds(2000));
           
            _output.WriteLine($"Going to announce aggregate completed transaction {signedTransaction.Hash}");
          
            // Announce the hash lock transaction
            await _fixture.Client.TransactionHttp.Announce(signedTransaction);

            Thread.Sleep(2000);
            var confirmedTx = await cosignatory1ConfirmedTx;

            // verify the account is multisig
            var multiSigAcc = await _fixture.Client.AccountHttp.GetMultisigAccountInfo(account.Address);
            _output.WriteLine($"Multisig account {multiSigAcc}");

            multiSigAcc.IsMultisig.Should().BeTrue();
            multiSigAcc.MinApproval.Should().Be(2);
            multiSigAcc.MinRemoval.Should().Be(1);
            multiSigAcc.Cosignatories.Should().HaveCount(2);
            

        }

        private CosignatureSignedTransaction CosignAggregateBondedTransaction(AggregateTransaction transaction, Account account)
        {
            var cosignatureTransaction = CosignatureTransaction.Create(transaction);
            return account.SignCosignatureTransaction(cosignatureTransaction);
        }


    }


}
