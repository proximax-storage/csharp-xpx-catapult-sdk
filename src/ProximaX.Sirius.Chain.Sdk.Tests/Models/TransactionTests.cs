using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using ProximaX.Sirius.Chain.Sdk.Infrastructure;
using ProximaX.Sirius.Chain.Sdk.Infrastructure.Listener;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Namespaces;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions.Builders;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions.Messages;
using ProximaX.Sirius.Chain.Sdk.Tests.E2E;
using Xunit;
using Xunit.Abstractions;

namespace ProximaX.Sirius.Chain.Sdk.Tests.Models
{
    public class TransactionTests
    {
        [Fact]
        public async Task Should_Serialize_And_Sign_TransactionAsync()
        {
            var account2 = Account.CreateFromPrivateKey("D54AC0CB0FF50FB44233782B3A6B5FDE2F1C83B9AE2F1352119F93713F3AB923", NetworkType.TEST_NET);
            // var nonce = MosaicNonce.CreateRandom();
            var account = Account.CreateFromPrivateKey("feab2970d23c9a6f0170adf9b846575831c63084bcf1f0d7e70644818c546017", NetworkType.TEST_NET);
            // var mosaicId = MosaicId.CreateFromNonce(nonce, account.PublicAccount.PublicKey);
            var mosaicToTransfer = NetworkCurrencyMosaic.CreateRelative(1000);

            var transferTransaction = TransferTransaction.Create(Deadline.Create(),
                Recipient.From(Address.CreateFromRawAddress("VDVLDGVUEZ2X2CB5AZLOVHV6UK4DPCQHHWHCV75T")),
                new List<Mosaic>()
                {
                    mosaicToTransfer
                },
                PlainMessage.Create("test-message"),
                NetworkType.TEST_NET
            );

            var generationHash = "B750FC8ADD9FAB8C71F0BB90B6409C66946844F07C5CADB51F27A9FAF219BFC7";

            var signedTransaction = account.Sign(transferTransaction, generationHash);

            //var payload = signedTransaction.Payload;

            signedTransaction.Payload.Should().NotBeNullOrEmpty();
            signedTransaction.Hash.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void Should_Serialize_And_Sign_TransactionModifyMultisigAccountTransaction()
        {
            var account = Account.CreateFromPrivateKey("26b64cb10f005e5988a36744ca19e20d835ccc7c105aaa5f3b212da593180930", NetworkType.TEST_NET);
            var generationHash = "AC87FDA8FD94B72F3D0790A7D62F248111BD5E37B95B16E4216DA99C212530A5";
            var network = NetworkType.TEST_NET;
            var coginatory1 = Account.GenerateNewAccount(network);
            var coginatory2 = Account.GenerateNewAccount(network);

            var coginatories = new List<MultisigCosignatoryModification>
            {
                new MultisigCosignatoryModification(MultisigCosignatoryModificationType.ADD,
                        coginatory1.PublicAccount),
                new MultisigCosignatoryModification(MultisigCosignatoryModificationType.ADD,
                        coginatory2.PublicAccount),
            };

            var convertIntoMultisigTransaction = ModifyMultisigAccountTransaction.Create(
              Deadline.Create(),
              1,
              1,
              coginatories,
              network);

            var signedTransaction = convertIntoMultisigTransaction.SignWith(account, generationHash);

            signedTransaction.Payload.Should().NotBeNullOrEmpty();
            signedTransaction.Hash.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void Should_Serialize_And_Sign_TransactionModifyMultisigAccountTransaction_AggregateBonded()
        {
            var account = Account.CreateFromPrivateKey("26b64cb10f005e5988a36744ca19e20d835ccc7c105aaa5f3b212da593180930", NetworkType.TEST_NET);
            var generationHash = "AC87FDA8FD94B72F3D0790A7D62F248111BD5E37B95B16E4216DA99C212530A5";
            var network = NetworkType.TEST_NET;
            var multiSigAccount = Account.GenerateNewAccount(network);
            var coginatory1 = Account.GenerateNewAccount(network);
            var coginatory2 = Account.GenerateNewAccount(network);

            var coginatories = new List<MultisigCosignatoryModification>
            {
                new MultisigCosignatoryModification(MultisigCosignatoryModificationType.ADD,
                        coginatory1.PublicAccount),
                new MultisigCosignatoryModification(MultisigCosignatoryModificationType.ADD,
                        coginatory2.PublicAccount),
            };

            var convertIntoMultisigTransaction = ModifyMultisigAccountTransaction.Create(
              Deadline.Create(),
              1,
              1,
              coginatories,
              network);

            var aggregateTransaction = AggregateTransaction.CreateBonded(
              Deadline.Create(),
              new List<Transaction>
              {
                  convertIntoMultisigTransaction.ToAggregate(multiSigAccount.PublicAccount)
              },
              network);

            var signedTransaction = multiSigAccount.Sign(aggregateTransaction, generationHash);

            signedTransaction.Payload.Should().NotBeNullOrEmpty();
            signedTransaction.Hash.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public void Should_Serialize_And_Sign_LockFundTransaction()
        {
            var account = Account.CreateFromPrivateKey("26b64cb10f005e5988a36744ca19e20d835ccc7c105aaa5f3b212da593180930", NetworkType.TEST_NET);
            var generationHash = "AC87FDA8FD94B72F3D0790A7D62F248111BD5E37B95B16E4216DA99C212530A5";
            var network = NetworkType.TEST_NET;
            var multiSigAccount = Account.GenerateNewAccount(network);
            var coginatory1 = Account.GenerateNewAccount(network);
            var coginatory2 = Account.GenerateNewAccount(network);

            var coginatories = new List<MultisigCosignatoryModification>
            {
                new MultisigCosignatoryModification(MultisigCosignatoryModificationType.ADD,
                        coginatory1.PublicAccount),
                new MultisigCosignatoryModification(MultisigCosignatoryModificationType.ADD,
                        coginatory2.PublicAccount),
            };

            var convertIntoMultisigTransaction = ModifyMultisigAccountTransaction.Create(
              Deadline.Create(),
              1,
              1,
              coginatories,
              network);

            var aggregateTransaction = AggregateTransaction.CreateBonded(
              Deadline.Create(),
              new List<Transaction>
              {
                  convertIntoMultisigTransaction.ToAggregate(multiSigAccount.PublicAccount)
              },
              network);

            var signedTransaction = multiSigAccount.Sign(aggregateTransaction, generationHash);

            var builder = new LockFundsTransactionBuilder();
            builder.SetDeadline(Deadline.Create())
                .SetDuration((ulong)700)
                .SetMosaic(NetworkCurrencyMosaic.CreateRelative(10))
                .SetSignedTransaction(signedTransaction)
                .SetNetworkType(network);

            var hashLockTransaction = builder.Build();

            var hashLockTransactionSigned = multiSigAccount.Sign(hashLockTransaction, generationHash);

            hashLockTransactionSigned.Payload.Should().NotBeNullOrEmpty();
            hashLockTransactionSigned.Hash.Should().NotBeNullOrEmpty();
        }
    }
}