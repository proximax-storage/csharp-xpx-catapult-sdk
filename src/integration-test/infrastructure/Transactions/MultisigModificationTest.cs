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
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Reactive.Linq;
using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Infrastructure.Listeners;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Blockchain;
using io.nem2.sdk.Model.Transactions;

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
            var signer =
                KeyPair.CreateFromPrivateKey("9abcd1bd50b994799f8a2e27f7ffd952831ef16a443a48241f1f78942322d5c6");

            var mods = new List<MultisigCosignatoryModification>
            {
                new MultisigCosignatoryModification(MultisigCosignatoryModificationType.Type.Add, new PublicAccount("10cc07742437c205d9a0bc0434dc5b4879e002114753de70cdc4c4bd0d93a64a", NetworkType.Types.MIJIN_TEST)),
                new MultisigCosignatoryModification(MultisigCosignatoryModificationType.Type.Add, new PublicAccount("31bd180037ba8a8cc5a55a6256390592d6326e55753f3b6e6074dec86a39eea6", NetworkType.Types.MIJIN_TEST)),
                new MultisigCosignatoryModification(MultisigCosignatoryModificationType.Type.Add, new PublicAccount("2ecf1decef6818bd9c38985afd6efc1c981e64e9a1ecc1e7b6b25eb30454cce0", NetworkType.Types.MIJIN_TEST))
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
    }
}
