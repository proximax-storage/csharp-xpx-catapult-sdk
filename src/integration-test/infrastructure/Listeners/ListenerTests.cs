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

using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using IntegrationTests.Infrastructure.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Infrastructure.Listeners;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Blockchain;
using io.nem2.sdk.Model.Mosaics;
using io.nem2.sdk.Model.Transactions;

namespace IntegrationTests.Infrastructure.Listeners
{
    [TestClass]
    public class ListenerTests
    {
        [TestMethod, Timeout(30000)]
        public async Task ListenForBlock()
        {
            var listener = new Listener(Config.Domain);

            await listener.Open();

            var block = await listener.NewBlock().Take(1);
            
            Assert.AreEqual(3, block.Version);
        }

        [TestMethod, Timeout(20000)]
        public async Task ListenForUnconfirmedTransactionAdded()
        {
            var listener = new Listener(Config.Domain);

            await listener.Open();

            var tx = listener.UnconfirmedTransactionsAdded(Address.CreateFromEncoded("SCEYFB35CYFF2U7UZ32RYXXZ5JTPCSKU4P6BRXZR")).Take(1);

            await new TransferTransactionTests().AnnounceTransaction();

            var result = await tx;

            Assert.AreEqual("D49C902E2FBAC09AF732D0A10DB8481E41C8D71B12B76D04DCC2869C1E549779", result.Signer.PublicKey);
        }

        [TestMethod, Timeout(40000)]
        public async Task ListenForPartialTransactionAdded()
        {
            var keyPair = KeyPair.CreateFromPrivateKey(Config.PrivateKeyAggregate1);

            var aggregateTransaction = AggregateTransaction.CreateBonded(
                NetworkType.Types.MIJIN_TEST,
                Deadline.CreateHours(2),
                new List<Transaction>
                {
                    TransferTransactionTests.CreateInnerTransferTransaction("nem:xem"),
                },
                null
            ).SignWith(keyPair);

            var hashLock = LockFundsTransaction.Create(NetworkType.Types.MIJIN_TEST, Deadline.CreateHours(2), 0, duration: 10000, mosaic: new Mosaic(new MosaicId("nem:xem"), 10000000), transaction: aggregateTransaction)
                .SignWith(KeyPair.CreateFromPrivateKey(Config.PrivateKeyAggregate1));

           await new TransactionHttp("http://" + Config.Domain + ":3000").Announce(hashLock);

            var listener = new Listener(Config.Domain);

            await listener.Open();

            await listener.ConfirmedTransactionsGiven(Address.CreateFromPublicKey(
               keyPair.PublicKeyString,
               NetworkType.Types.MIJIN_TEST)
           ).Take(1);
           
           await new TransactionHttp("http://" + Config.Domain + ":3000").AnnounceAggregateBonded(aggregateTransaction);
           
           var result = await listener.AggregateBondedAdded(Address.CreateFromPublicKey(
               keyPair.PublicKeyString,
               NetworkType.Types.MIJIN_TEST)
           ).Take(1);

           Assert.AreEqual("E7E2B3BD88301718FA0AB4F10FC49AD8E547C8150F94817C84C56AC6A3BEF648", result.Signer.PublicKey);
        }


        public async Task ListenForUnconfirmedTransactionRemoved()
        {
            var listener = new Listener(Config.Domain);

            await listener.Open();

            var tx = listener.UnconfirmedTransactionsRemoved(Address.CreateFromEncoded("SBHQVG-3J27J4-7X7YQJ-F6WE7K-GUP76D-BASFNO-ZGEO")).Take(1);

            await new TransferTransactionTests().AnnounceTransaction();

            var result = await tx;

            Assert.AreEqual("10CC07742437C205D9A0BC0434DC5B4879E002114753DE70CDC4C4BD0D93A64A", result.Signer);
        }

        [TestMethod, Timeout(20000)]
        public async Task ListenForConfirmedTransactionAdded()
        {
            var listener = new Listener(Config.Domain);

            await listener.Open();

            var tx = listener.ConfirmedTransactionsGiven(Address.CreateFromEncoded("SCEYFB35CYFF2U7UZ32RYXXZ5JTPCSKU4P6BRXZR")).Take(1);

            await new TransferTransactionTests().AnnounceTransaction();

            var result = await tx;

            Assert.AreEqual("D49C902E2FBAC09AF732D0A10DB8481E41C8D71B12B76D04DCC2869C1E549779", result.Signer.PublicKey);
        }

        [TestMethod, Timeout(20000)]
        public async Task ListenForTransactionStatus()
        {
            var listener = new Listener(Config.Domain);

            await listener.Open();

            var tx = listener.TransactionStatus(Address.CreateFromEncoded("SCEYFB35CYFF2U7UZ32RYXXZ5JTPCSKU4P6BRXZR")).Take(1);

            await new TransferTransactionTests().AnnounceTransaction(2000000000000000000);

            var result = await tx;

            Assert.AreEqual("Failure_Core_Insufficient_Balance", result.Status);
        }
    }
}
