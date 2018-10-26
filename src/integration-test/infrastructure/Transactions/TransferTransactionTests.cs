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
    public class TransferTransactionTests
    {
        private Listener listener { get; }

        public TransferTransactionTests()
        {
            listener = new Listener(Config.Domain);

            listener.Open().Wait();
        }

        public async Task AnnounceTransaction(ulong amount = 10)
        {
            var keyPair =
                KeyPair.CreateFromPrivateKey(Config.PrivateKeyTransfer);

            var transaction =  TransferTransaction.Create(
                NetworkType.Types.MIJIN_TEST,
                Deadline.CreateHours(2), 
                Address.CreateFromRawAddress("SCEYFB35CYFF2U7UZ32RYXXZ5JTPCSKU4P6BRXZR"),
                new List<Mosaic> { Mosaic.CreateFromIdentifier("nem:xem", amount) }, 
                PlainMessage.Create("hello")  
                ).SignWith(keyPair);

             await new TransactionHttp("http://" + Config.Domain + ":3000").Announce(transaction);
        }
        
        [TestMethod, Timeout(40000)]
        public async Task AnnounceTransferTransactionWithMosaicWithMessage()
        {
            var keyPair = KeyPair.CreateFromPrivateKey(Config.PrivateKeyTransfer);

            var account = new Account(Config.PrivateKeyTransfer, NetworkType.Types.MIJIN_TEST);

            var transaction = TransferTransaction.Create(
                NetworkType.Types.MIJIN_TEST,
                Deadline.CreateHours(2),
                account.Address,
                new List<Mosaic> { Xem.CreateRelative(1) },
                PlainMessage.Create("111111111111111111111111111111111111111111111111111111111111111111111111111" +
                                    "111111111111111111111111111111111111111111111111111111111111111111111111111" +
                                    "111111111111111111111111111111111111111111111111111111111111111111111111111" +
                                    "111111111111111111111111111111")
            ).SignWith(keyPair);

            listener.TransactionStatus(Address.CreateFromPublicKey(transaction.Signer, NetworkType.Types.MIJIN_TEST))
                .Subscribe(e =>
                {
                    Console.WriteLine(e.Status);
                });

            await new TransactionHttp("http://" + Config.Domain + ":3000").Announce(transaction);
           
            var status = await listener.ConfirmedTransactionsGiven(Address.CreateFromPublicKey(transaction.Signer, NetworkType.Types.MIJIN_TEST)).Take(1);

            Assert.AreEqual(keyPair.PublicKeyString, status.Signer.PublicKey);
        }

        [TestMethod, Timeout(20000)]
        public async Task AnnounceTransferTransactionWithMosaicWithSecureMessage()
        {
            var keyPair = KeyPair.CreateFromPrivateKey(Config.PrivateKeyAggregate1);

            var transaction = TransferTransaction.Create(
                NetworkType.Types.MIJIN_TEST,
                Deadline.CreateHours(2),
                Address.CreateFromRawAddress("SAAA57-DREOPY-KUFX4O-G7IQXK-ITMBWK-D6KXTV-BBQP"),
                new List<Mosaic> { Mosaic.CreateFromIdentifier("nem:xem", 10) },
                SecureMessage.Create("hello2", Config.PrivateKeyAggregate1, "5D8BEBBE80D7EA3B0088E59308D8671099781429B449A0BBCA6D950A709BA068")
                ).SignWith(keyPair);

            await new TransactionHttp("http://" + Config.Domain + ":3000").Announce(transaction);

            listener.TransactionStatus(Address.CreateFromPublicKey(keyPair.PublicKeyString, NetworkType.Types.MIJIN_TEST))
                .Subscribe(e => Console.WriteLine(e.Status));

            var status = await listener.ConfirmedTransactionsGiven(Address.CreateFromPublicKey(transaction.Signer, NetworkType.Types.MIJIN_TEST)).Take(1);

            Assert.AreEqual(keyPair.PublicKeyString, status.Signer.PublicKey);
        }

        [TestMethod, Timeout(20000)]
        public async Task AnnounceTransferTransactionWithMultipleMosaicsWithSecureMessage()
        {
            var keyPair =
                KeyPair.CreateFromPrivateKey(Config.PrivateKeyAggregate1);

            var transaction = TransferTransaction.Create(
                NetworkType.Types.MIJIN_TEST,
                Deadline.CreateHours(2),
                Address.CreateFromRawAddress("SAOV4Y5W627UXLIYS5O43SVU23DD6VNRCFP222P2"),
                new List<Mosaic>()
                {
                    Mosaic.CreateFromIdentifier("happy:test4", 10),
                    Mosaic.CreateFromIdentifier("nem:xem", 100),
                    
                },
                SecureMessage.Create("hello2", Config.PrivateKeyMain, "5D8BEBBE80D7EA3B0088E59308D8671099781429B449A0BBCA6D950A709BA068")
                
            ).SignWith(keyPair);

            await new TransactionHttp("http://" + Config.Domain + ":3000").Announce(transaction);

            var status = await listener.ConfirmedTransactionsGiven(Address.CreateFromPublicKey(transaction.Signer, NetworkType.Types.MIJIN_TEST)).Take(1);

            Assert.AreEqual(keyPair.PublicKeyString, status.Signer.PublicKey);
        }

        [TestMethod, Timeout(20000)]
        public async Task AnnounceTransferTransactionWithMultipleMosaicsWithoutMessage()
        {
            var keyPair =
                KeyPair.CreateFromPrivateKey(Config.PrivateKeyAggregate1);

            var transaction = TransferTransaction.Create(
                NetworkType.Types.MIJIN_TEST,
                Deadline.CreateHours(2),
                Address.CreateFromRawAddress("SAAA57-DREOPY-KUFX4O-G7IQXK-ITMBWK-D6KXTV-BBQP"),
                new List<Mosaic>()
                {
                    
                    Mosaic.CreateFromIdentifier("happy:test2", 10),
                    Mosaic.CreateFromIdentifier("nem:xem", 10),
                },
                EmptyMessage.Create()        
            ).SignWith(keyPair);

            await new TransactionHttp("http://" + Config.Domain + ":3000").Announce(transaction);

            listener.TransactionStatus(Address.CreateFromPublicKey(transaction.Signer, NetworkType.Types.MIJIN_TEST))
                .Subscribe(e => Console.WriteLine(e.Status));

            var status = await listener.ConfirmedTransactionsGiven(Address.CreateFromPublicKey(transaction.Signer, NetworkType.Types.MIJIN_TEST)).Take(1);

            Assert.AreEqual(keyPair.PublicKeyString, status.Signer.PublicKey);
        }

        internal static TransferTransaction CreateInnerTransferTransaction(string mosaic, ulong amount = 10)
        {
            return TransferTransaction.Create(
                        NetworkType.Types.MIJIN_TEST,
                        Deadline.CreateHours(2),
                        Address.CreateFromRawAddress("SAAA57-DREOPY-KUFX4O-G7IQXK-ITMBWK-D6KXTV-BBQP"),
                        new List<Mosaic> {Mosaic.CreateFromIdentifier(mosaic, amount)}, 
                        PlainMessage.Create("hey")
                        
                    );
        }
    }
}
