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
using io.nem2.sdk.Model.Transactions;

namespace IntegrationTests.Infrastructure.Transactions
{
    [TestClass]
    public class NamespaceRentalTests
    {
        private readonly string host = "http://" + Config.Domain + ":3000";

        private Listener listener { get; }

        public NamespaceRentalTests()
        {
            listener = new Listener(Config.Domain);

            listener.Open().Wait();
        }

        [TestMethod, Timeout(40000)]
        public async Task AggregateNamespaceRentalExtensionShouldSucceed()
        {
            var signer = KeyPair.CreateFromPrivateKey(Config.PrivateKeyMain);

            var transaction = RegisterNamespaceTransaction.CreateRootNamespace(
                NetworkType.Types.MIJIN_TEST,
                Deadline.CreateHours(2),
                "happy",
                10000)
                .ToAggregate(PublicAccount.CreateFromPublicKey("B974668ABED344BE9C35EE257ACC246117EFFED939EAF42391AE995912F985FE", NetworkType.Types.MIJIN_TEST));

            var agg = AggregateTransaction.CreateComplete(
                NetworkType.Types.MIJIN_TEST,
                Deadline.CreateHours(2), 
                new List<Transaction>() {transaction})
                .SignWith(signer);

            await new TransactionHttp(host).Announce(agg);

            listener.TransactionStatus(Address.CreateFromPublicKey(signer.PublicKeyString,NetworkType.Types.MIJIN_TEST)).Subscribe(
                e =>
                {
                    Console.WriteLine(e.Status);
                });

            var status = await listener.ConfirmedTransactionsGiven(Address.CreateFromPublicKey(signer.PublicKeyString, NetworkType.Types.MIJIN_TEST)).Where(e => e.TransactionInfo.Hash == agg.Hash).Take(1);


            Assert.AreEqual(signer.PublicKeyString, status.Signer.PublicKey);
        }

        [TestMethod, Timeout(20000)]
        public async Task ShouldFailSubNamespaceExists()
        {
            var signer =
                KeyPair.CreateFromPrivateKey(Config.PrivateKeyMain);

            var transaction =  RegisterNamespaceTransaction.CreateSubNamespace(
                NetworkType.Types.MIJIN_TEST,
                    Deadline.CreateHours(2), 
                    "happy", 
                    "yooo")
                .SignWith(signer);

            listener.ConfirmedTransactionsGiven(Address.CreateFromPublicKey(transaction.Signer, NetworkType.Types.MIJIN_TEST))
                .Subscribe(
                    e =>
                    {
                        Assert.Fail("Success");
                    });

            await new TransactionHttp(host).Announce(transaction);

            var status = await listener.TransactionStatus(Address.CreateFromPublicKey(signer.PublicKeyString, NetworkType.Types.MIJIN_TEST)).Where(e => e.Hash == transaction.Hash).Take(1);

            Assert.AreEqual("Failure_Namespace_Already_Exists", status.Status);
        }
    }
}

