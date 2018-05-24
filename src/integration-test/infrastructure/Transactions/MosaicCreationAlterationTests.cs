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
    public class MosaicCreationAlterationTests
    {
        private readonly String host = "http://" + Config.Domain + ":3000";

        private Listener listener { get; }

        public MosaicCreationAlterationTests()
        {
            listener = new Listener(Config.Domain);

            listener.Open().Wait();
        }

        [TestMethod, Timeout(20000)]
        public async Task MosaicSupplyIncreaseTransction()
        {
            var signer = KeyPair.CreateFromPrivateKey(Config.PrivateKeyMain);

            var transaction =  MosaicSupplyChangeTransaction.Create(
                    NetworkType.Types.MIJIN_TEST,
                    Deadline.CreateHours(2),                  
                    new MosaicId("happy:test2"), 
                    MosaicSupplyType.Type.INCREASE,
                    (ulong)new Random().Next(100000000, 1100000000)
                    
                ).SignWith(signer);

            await new TransactionHttp("http://" + Config.Domain + ":3000").Announce(transaction);

            var status = await listener.ConfirmedTransactionsGiven(Address.CreateFromPublicKey(transaction.Signer, NetworkType.Types.MIJIN_TEST)).Take(1);

            Assert.AreEqual(signer.PublicKeyString, status.Signer.PublicKey);
        }

        [TestMethod, Timeout(20000)]
        public async Task MosaicSupplyDecreaseTransction()
        {
            var signer = KeyPair.CreateFromPrivateKey(Config.PrivateKeyMain);

            var transaction = MosaicSupplyChangeTransaction.Create(
                NetworkType.Types.MIJIN_TEST,
                Deadline.CreateHours(2),
                new MosaicId("happy:test2"),             
                MosaicSupplyType.Type.DECREASE,
                10000)
                .SignWith(signer);

            await new TransactionHttp("http://" + Config.Domain + ":3000").Announce(transaction);

            listener.TransactionStatus(Address.CreateFromPublicKey(transaction.Signer, NetworkType.Types.MIJIN_TEST))
                .Subscribe(e => Console.WriteLine(e.Status));

            var status = await listener.ConfirmedTransactionsGiven(Address.CreateFromPublicKey(transaction.Signer, NetworkType.Types.MIJIN_TEST)).Take(1);

            Assert.AreEqual("B974668ABED344BE9C35EE257ACC246117EFFED939EAF42391AE995912F985FE", status.Signer.PublicKey);
        }

        [TestMethod, Timeout(20000)]
        public async Task MutableMosaicCreationTransaction()
        {
            var signer = KeyPair.CreateFromPrivateKey(Config.PrivateKeyMain);

            var transaction = MosaicDefinitionTransaction.Create(
                NetworkType.Types.MIJIN_TEST,
                Deadline.CreateHours(1), 
                "happy", 
                "test2", 
                new MosaicProperties(true, true, false, 0x04, 100000)) 
                .SignWith(signer);

            await new TransactionHttp(host).Announce(transaction);

            var status = await listener.TransactionStatus(Address.CreateFromPublicKey(signer.PublicKeyString, NetworkType.Types.MIJIN_TEST)).Where(e => e.Hash == transaction.Hash).Take(1);

            Assert.AreEqual("Failure_Mosaic_Modification_No_Changes", status.Status);
        }

        [TestMethod, Timeout(20000)]
        public async Task ImmutableMosaicCreationTransaction()
        {
            var signer = KeyPair.CreateFromPrivateKey(Config.PrivateKeyMain);
       
            var transaction = MosaicDefinitionTransaction.Create(
                    NetworkType.Types.MIJIN_TEST,
                    Deadline.CreateHours(1),
                    "happy",
                    "test4",
                    new MosaicProperties(false, true, false, 0x04, 11000))
                .SignWith(signer);
       
            await new TransactionHttp(host).Announce(transaction);

            listener.TransactionStatus(
                    Address.CreateFromPublicKey(signer.PublicKeyString, NetworkType.Types.MIJIN_TEST))
                .Subscribe(e => Console.WriteLine(e.Status));

            var status = await listener.TransactionStatus(Address.CreateFromPublicKey(signer.PublicKeyString, NetworkType.Types.MIJIN_TEST)).Where(e => e.Hash == transaction.Hash).Take(1);
       
            Assert.AreEqual("Failure_Mosaic_Modification_No_Changes", status.Status);
        }

        [TestMethod, Timeout(20000)]
        public async Task MosaicSupplyIncreaseShouldFailImmutableSupply()
        {
            var signer = KeyPair.CreateFromPrivateKey(Config.PrivateKeyMain);

            var transaction = MosaicSupplyChangeTransaction.Create(
                NetworkType.Types.MIJIN_TEST,
                    Deadline.CreateHours(2),
                    new MosaicId("happy:test3"),                
                    MosaicSupplyType.Type.INCREASE,
                    10000)
                .SignWith(signer);

            await new TransactionHttp(host).Announce(transaction);
            listener.ConfirmedTransactionsGiven(Address.CreateFromPublicKey(signer.PublicKeyString,
                NetworkType.Types.MIJIN_TEST)).Subscribe(e => Console.WriteLine(e.TransactionInfo.Hash));
            var status = await listener.TransactionStatus(Address.CreateFromPublicKey(signer.PublicKeyString, NetworkType.Types.MIJIN_TEST)).Where(e => e.Hash == transaction.Hash).Take(1);

            Assert.AreEqual("Failure_Mosaic_Supply_Immutable", status.Status);
        }
    }
}
