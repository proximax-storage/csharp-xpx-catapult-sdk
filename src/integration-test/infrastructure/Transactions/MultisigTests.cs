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
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reactive.Linq;
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
    public class MultisigModificationTest
    {
        private readonly string host = "http://" + Config.Domain + ":3000";

        private Listener listener { get; }

        public MultisigModificationTest()
        {
            listener = new Listener(Config.Domain);
            
            listener.Open().Wait();
        }

        [TestMethod, Timeout(20000)]
        public async Task MultisigModificationTransaction()
        {
            var signer = KeyPair.CreateFromPrivateKey(Config.MultisigPrivateKey);
            
            var cosig1 = Account.GenerateNewAccount(NetworkType.Types.MIJIN_TEST);
            var cosig2 = Account.GenerateNewAccount(NetworkType.Types.MIJIN_TEST);
            var cosig3 = Account.GenerateNewAccount(NetworkType.Types.MIJIN_TEST);


            var mods = new List<MultisigCosignatoryModification>
            {
                new MultisigCosignatoryModification(MultisigCosignatoryModificationType.Type.Add, new PublicAccount(cosig1.PublicKey, NetworkType.Types.MIJIN_TEST)),
                new MultisigCosignatoryModification(MultisigCosignatoryModificationType.Type.Add, new PublicAccount(cosig2.PublicKey, NetworkType.Types.MIJIN_TEST)),
                new MultisigCosignatoryModification(MultisigCosignatoryModificationType.Type.Add, new PublicAccount(cosig3.PublicKey, NetworkType.Types.MIJIN_TEST))
            };

            var transaction = ModifyMultisigAccountTransaction.Create(
                    NetworkType.Types.MIJIN_TEST,
                    Deadline.CreateHours(2),  
                    2,
                    2,
                    mods)
                .SignWith(signer);

            await new TransactionHttp(host).Announce(transaction);

            var status = await listener.TransactionStatus(Address.CreateFromPublicKey(signer.PublicKeyString, NetworkType.Types.MIJIN_TEST)).Where(e => e.Hash == transaction.Hash).Take(1);

            Assert.AreEqual("Failure_Multisig_Operation_Not_Permitted_By_Account", status.Status);
            
        }

        [Timeout(40000)]
        [TestMethod]
        public async Task SendMultisigTransaction()
        {
            Account cosignatoryAccount1 = Account.CreateFromPrivateKey(Config.Cosig1, NetworkType.Types.MIJIN_TEST);
            Account cosignatoryAccount2 = Account.CreateFromPrivateKey(Config.Cosig2, NetworkType.Types.MIJIN_TEST);
            Account cosignatoryAccount3 = Account.CreateFromPrivateKey(Config.Cosig3, NetworkType.Types.MIJIN_TEST);

            var multisigAccount = KeyPair.CreateFromPrivateKey(Config.MultisigPrivateKey);

            PublicAccount multisigPublicAccount = PublicAccount.CreateFromPublicKey(multisigAccount.PublicKeyString, NetworkType.Types.MIJIN_TEST);

            TransferTransaction transferTransaction = TransferTransaction.Create(
                NetworkType.Types.MIJIN_TEST,
                Deadline.CreateHours(2),
                Address.CreateFromEncoded("SAHKKLGIPLYFMTTV2VQKOOUCWGGCXLQEMKC4OWEF"),
                new List<Mosaic>() { Xem.CreateRelative(10) },
                PlainMessage.Create("sending 10 nem:xem")            
            );

            var aggregateTransaction = AggregateTransaction.CreateComplete(
                NetworkType.Types.MIJIN_TEST,
                Deadline.CreateHours(2),
                new List<Transaction>()
                {
                    transferTransaction.ToAggregate(multisigPublicAccount)
                }
            ).SignWithAggregateCosigners(cosignatoryAccount1.KeyPair, new List<Account>(){cosignatoryAccount2, cosignatoryAccount3});

            TransactionHttp transactionHttp = new TransactionHttp("http://"+Config.Domain+":3000");

            listener.TransactionStatus(Address.CreateFromPublicKey(cosignatoryAccount1.PublicKey, NetworkType.Types.MIJIN_TEST))
                .Subscribe(e =>
                {
                    Console.WriteLine(e.Status);
                });

            await transactionHttp.Announce(aggregateTransaction);

            var status = await listener.ConfirmedTransactionsGiven(Address.CreateFromPublicKey(multisigAccount.PublicKeyString, NetworkType.Types.MIJIN_TEST)).Take(1);

            Assert.AreEqual(cosignatoryAccount1.PublicKey, status.Signer.PublicKey);
        }
    }
}
