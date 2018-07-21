using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Infrastructure.Listeners;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Blockchain;
using io.nem2.sdk.Model.Mosaics;
using io.nem2.sdk.Model.Transactions;

namespace IntegrationTests.Infrastructure.Transactions
{
    [TestClass]
    public class HashLockTransactionTest
    {
        private readonly String host = "http://" + Config.Domain + ":3000";

        private Listener listener { get; }

        public HashLockTransactionTest()
        {
            listener = new Listener(Config.Domain);

            listener.Open().Wait();
        }

        internal void WatchForFailure(SignedTransaction transaction)
        {
            listener.TransactionStatus(Address.CreateFromPublicKey(transaction.Signer, NetworkType.Types.MIJIN_TEST))
                .Subscribe(
                    e =>
                    {
                        Console.WriteLine(e.Status);

                    });
        }

        [TestMethod, Timeout(20000)]
        public async Task LockFundsTransactionTest()
        {
            var signer = KeyPair.CreateFromPrivateKey(Config.PrivateKeyAggregate1);
            var innerSigner1 = KeyPair.CreateFromPrivateKey(Config.PrivateKeyAggregate3);
            var aggregateTransaction = AggregateTransaction.CreateBonded(
                NetworkType.Types.MIJIN_TEST,
                    Deadline.CreateHours(2),
                    new List<Transaction>
                    {
                        TransferTransactionTests.CreateInnerTransferTransaction(
                            "nem:xem").ToAggregate(PublicAccount.CreateFromPublicKey(signer.PublicKeyString, NetworkType.Types.MIJIN_TEST)),
                        TransferTransactionTests.CreateInnerTransferTransaction(
                            "nem:xem").ToAggregate(PublicAccount.CreateFromPublicKey(innerSigner1.PublicKeyString, NetworkType.Types.MIJIN_TEST))
                    },
                    null)
                .SignWith(signer);

            var hashLock =  LockFundsTransaction.Create(
                NetworkType.Types.MIJIN_TEST,
                Deadline.CreateHours(2), 
                0,
                new Mosaic(new MosaicId("nem:xem"), 10000000), 
                10000,
                aggregateTransaction)
                .SignWith(KeyPair.CreateFromPrivateKey(Config.PrivateKeyAggregate1));

            WatchForFailure(hashLock);

            await new TransactionHttp(host).Announce(hashLock);

            var status = await listener.ConfirmedTransactionsGiven(Address.CreateFromPublicKey(hashLock.Signer, NetworkType.Types.MIJIN_TEST)).Where(e => e.TransactionInfo.Hash == hashLock.Hash).Take(1);

            Assert.AreEqual(signer.PublicKeyString, status.Signer.PublicKey);

            await new TransactionHttp(host).AnnounceAggregateBonded(aggregateTransaction);

            var status2 = await listener.AggregateBondedAdded(Address.CreateFromPublicKey(aggregateTransaction.Signer, NetworkType.Types.MIJIN_TEST)).Where(e => e.TransactionInfo.Hash == aggregateTransaction.Hash).Take(1);

            Assert.AreEqual(signer.PublicKeyString, status2.Signer.PublicKey);
        }
    }
}
