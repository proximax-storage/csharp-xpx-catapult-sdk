using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions.Builders;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions.Messages;
using Xunit;

namespace ProximaX.Sirius.Chain.Sdk.Tests.Models
{
    public class TransactionTests
    {
        [Fact]
        public void Should_Serialize_And_Sign_Transaction()
        {

            var mosaics = new List<Mosaic>()
            {
                new Mosaic((new MosaicId(992621222383397347)), 20)
            };

            var transferTransaction = TransferTransaction.Create(Deadline.Create(1),
                Recipient.From(new Address("SBILTA367K2LX2FEXG5TFWAS7GEFYAGY7QLFBYKC", NetworkType.MIJIN_TEST)),
                mosaics,
                PlainMessage.Create("test-message"),
                NetworkType.MIJIN_TEST,
                0
            );
            var account = Account.CreateFromPrivateKey("26b64cb10f005e5988a36744ca19e20d835ccc7c105aaa5f3b212da593180930", NetworkType.MIJIN_TEST);
            var generationHash = "B750FC8ADD9FAB8C71F0BB90B6409C66946844F07C5CADB51F27A9FAF219BFC7";
            var signedTransaction = transferTransaction.SignWith(account, generationHash);
            // var payload = signedTransaction.Payload;
            signedTransaction.Payload.Should().NotBeNullOrEmpty();
            signedTransaction.Hash.Should().NotBeNullOrEmpty();

        }

        [Fact]
        public void Should_Serialize_And_Sign_TransactionModifyMultisigAccountTransaction()
        {
            var account = Account.CreateFromPrivateKey("26b64cb10f005e5988a36744ca19e20d835ccc7c105aaa5f3b212da593180930", NetworkType.MIJIN_TEST);
            var generationHash = "B750FC8ADD9FAB8C71F0BB90B6409C66946844F07C5CADB51F27A9FAF219BFC7";
            var network = NetworkType.MIJIN_TEST;
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
            var account = Account.CreateFromPrivateKey("26b64cb10f005e5988a36744ca19e20d835ccc7c105aaa5f3b212da593180930", NetworkType.MIJIN_TEST);
            var generationHash = "B750FC8ADD9FAB8C71F0BB90B6409C66946844F07C5CADB51F27A9FAF219BFC7";
            var network = NetworkType.MIJIN_TEST;
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
            var account = Account.CreateFromPrivateKey("26b64cb10f005e5988a36744ca19e20d835ccc7c105aaa5f3b212da593180930", NetworkType.MIJIN_TEST);
            var generationHash = "B750FC8ADD9FAB8C71F0BB90B6409C66946844F07C5CADB51F27A9FAF219BFC7";
            var network = NetworkType.MIJIN_TEST;
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
