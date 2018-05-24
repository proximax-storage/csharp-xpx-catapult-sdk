//
// Copyright 2018 NEM
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 

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
using io.nem2.sdk.Model.Transactions.Messages;


namespace IntegrationTests.Infrastructure.Transactions
{
    [TestClass]
    public class AggregateTransactionTests
    {
        private readonly String host = "http://" + Config.Domain + ":3000";

        private Listener listener { get; }

        public AggregateTransactionTests()
        {
            listener = new Listener(Config.Domain);

            listener.Open().Wait();
        }

        [TestMethod, Timeout(20000)]
        public async Task AggregateTransactionWithOnlyInitiator()
        {
            var keyPair = KeyPair.CreateFromPrivateKey(Config.PrivateKeyMain);

            var aggregateTransaction = AggregateTransaction.CreateComplete(
                NetworkType.Types.MIJIN_TEST,
                    Deadline.CreateHours(2),
                    new List<Transaction>
                    {
                        TransferTransactionTests.CreateInnerTransferTransaction("nem:xem")
                        .ToAggregate(PublicAccount.CreateFromPublicKey("B974668ABED344BE9C35EE257ACC246117EFFED939EAF42391AE995912F985FE", NetworkType.Types.MIJIN_TEST)),
                    }
                ).SignWith(keyPair);

            await new TransactionHttp(host).Announce(aggregateTransaction);

            var status2 = await listener.ConfirmedTransactionsGiven(Address.CreateFromPublicKey(aggregateTransaction.Signer, NetworkType.Types.MIJIN_TEST)).Where(e => e.TransactionInfo.Hash == aggregateTransaction.Hash).Take(1);

            Assert.AreEqual(keyPair.PublicKeyString, status2.Signer.PublicKey);
        }

        internal void WatchForFailure(SignedTransaction transaction)
        {
            listener.TransactionStatus(Address.CreateFromPublicKey(transaction.Signer, NetworkType.Types.MIJIN_TEST))
                .Subscribe(
                    e =>
                    {
                        Assert.Fail(e.Status);
                    });
        }

        [TestMethod, Timeout(40000)]
        public async Task AggregateTransactionWithMissingCosignatures()
        {
            var keyPair = KeyPair.CreateFromPrivateKey(Config.PrivateKeyMain);

            var keyPair2 = KeyPair.CreateFromPrivateKey(Config.PrivateKeySecond);

            var aggregateTransaction = AggregateTransaction.CreateBonded(
                NetworkType.Types.MIJIN_TEST,
                Deadline.CreateHours(2),
                new List<Transaction>
                {
                    RegisterNamespaceTransaction.CreateRootNamespace(NetworkType.Types.MIJIN_TEST, Deadline.CreateHours(2), "happy23", 10000)
                    .ToAggregate(PublicAccount.CreateFromPublicKey(keyPair.PublicKeyString, NetworkType.Types.MIJIN_TEST)),
                    TransferTransactionTests.CreateInnerTransferTransaction("nem:xem")
                    .ToAggregate(PublicAccount.CreateFromPublicKey(keyPair2.PublicKeyString, NetworkType.Types.MIJIN_TEST)),
                },
                null
            ).SignWith(keyPair);

            WatchForFailure(aggregateTransaction);

            var hashLock = LockFundsTransaction.Create(NetworkType.Types.MIJIN_TEST, Deadline.CreateHours(2), 0, new Mosaic(new MosaicId("nem:xem"), 10000000), 10000, aggregateTransaction)
                .SignWith(KeyPair.CreateFromPrivateKey(Config.PrivateKeyMain));

            WatchForFailure(hashLock);

            await new TransactionHttp(host).Announce(hashLock);

            var status = await listener.ConfirmedTransactionsGiven(Address.CreateFromPublicKey(hashLock.Signer, NetworkType.Types.MIJIN_TEST)).Where(e => e.TransactionInfo.Hash == hashLock.Hash).Take(1);

            Assert.AreEqual(keyPair.PublicKeyString, status.Signer.PublicKey);

            await new TransactionHttp(host).AnnounceAggregateBonded(aggregateTransaction);

            var status2 = await listener.AggregateBondedAdded(Address.CreateFromPublicKey(aggregateTransaction.Signer, NetworkType.Types.MIJIN_TEST)).Where(e => e.TransactionInfo.Hash == aggregateTransaction.Hash).Take(1);

            Assert.AreEqual(keyPair.PublicKeyString, status2.Signer.PublicKey);
        }

        [TestMethod, Timeout(20000)]
        public async Task SignAggregateTransactionComplete()
        {
            var signerKey =
                KeyPair.CreateFromPrivateKey(Config.PrivateKeyMain);

            var account = new Account(Config.PrivateKeySecond,
                NetworkType.Types.MIJIN_TEST);           

            var aggregateTransaction = AggregateTransaction.CreateComplete(
                NetworkType.Types.MIJIN_TEST,
                    Deadline.CreateHours(2),
                    new List<Transaction>
                    {
                        TransferTransactionTests.CreateInnerTransferTransaction("nem:xem")
                             .ToAggregate(PublicAccount.CreateFromPublicKey(signerKey.PublicKeyString, NetworkType.Types.MIJIN_TEST)),
                        TransferTransactionTests.CreateInnerTransferTransaction("nem:xem")
                            .ToAggregate(PublicAccount.CreateFromPublicKey(account.PublicKey, NetworkType.Types.MIJIN_TEST))
                    })
                .SignWithAggregateCosigners(signerKey, new List<Account>(){
                    Account.CreateFromPrivateKey(Config.PrivateKeySecond,
                    NetworkType.Types.MIJIN_TEST)}
                );

            WatchForFailure(aggregateTransaction);

            await new TransactionHttp(host).Announce(aggregateTransaction);

            var status = await listener.ConfirmedTransactionsGiven(Address.CreateFromPublicKey(account.PublicKey, NetworkType.Types.MIJIN_TEST)).Where(e => e.TransactionInfo.Hash == aggregateTransaction.Hash).Take(1);
            
            Assert.AreEqual(signerKey.PublicKeyString, status.Signer.PublicKey);
        }

        [TestMethod, Timeout(40000)]
        public async Task PartialTransactionWithMissingCosigner()
        {
            var keyPair = KeyPair.CreateFromPrivateKey(Config.PrivateKeyMain);

            var keyPair2 = KeyPair.CreateFromPrivateKey("14A239D2ADB96753CFC160BB262F27B01BCCC8C74599F51771BC6BD39980F4E7");

            var aggregateBonded = AggregateTransaction.CreateBonded(
                NetworkType.Types.MIJIN_TEST,
                    Deadline.CreateHours(2),
                    new List<Transaction>
                    {
                        TransferTransactionTests.CreateInnerTransferTransaction(
                            "nem:xem").ToAggregate(PublicAccount.CreateFromPublicKey(keyPair2.PublicKeyString, NetworkType.Types.MIJIN_TEST)),
                        TransferTransactionTests.CreateInnerTransferTransaction(
                            "nem:xem").ToAggregate(PublicAccount.CreateFromPublicKey(keyPair2.PublicKeyString, NetworkType.Types.MIJIN_TEST)),
                    },
                    null)
                .SignWith(keyPair);

            WatchForFailure(aggregateBonded);

            var hashLock = LockFundsTransaction.Create(
                NetworkType.Types.MIJIN_TEST,
                    Deadline.CreateHours(2),
                    0,
                    new Mosaic(new MosaicId("nem:xem"), 10000000),
                    10000,  
                    aggregateBonded
                    )
                .SignWith(KeyPair.CreateFromPrivateKey(Config.PrivateKeyMain));

            WatchForFailure(hashLock);

            await new TransactionHttp(host).Announce(hashLock);

            var status = await listener.ConfirmedTransactionsGiven(Address.CreateFromPublicKey(hashLock.Signer, NetworkType.Types.MIJIN_TEST)).Where(e => e.TransactionInfo.Hash == hashLock.Hash).Take(1);

            Assert.AreEqual(keyPair.PublicKeyString, status.Signer.PublicKey);

            await new TransactionHttp(host).AnnounceAggregateBonded(aggregateBonded);

            var status2 = await listener.AggregateBondedAdded(Address.CreateFromPublicKey(aggregateBonded.Signer, NetworkType.Types.MIJIN_TEST)).Where(e => e.TransactionInfo.Hash == aggregateBonded.Hash).Take(1);

            Assert.AreEqual(keyPair.PublicKeyString, status2.Signer.PublicKey);
        }

        internal static TransferTransaction CreateInnerTransferTransaction(String signer)
        {
            return TransferTransaction.Create(
                NetworkType.Types.MIJIN_TEST,
                Deadline.CreateHours(2),
                Address.CreateFromEncoded("SAAA57DREOPYKUFX4OG7IQXKITMBWKD6KXTVBBQP"),
                new List<Mosaic> {Mosaic.CreateFromIdentifier("nem:xem", 10)},
                PlainMessage.Create("hey"));
        }
    }
}
