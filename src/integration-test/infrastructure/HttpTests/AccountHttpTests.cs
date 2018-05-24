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

using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using io.nem2.sdk.Infrastructure.Buffers.Model;
using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Blockchain;
using io.nem2.sdk.Model.Transactions;

namespace IntegrationTests.Infrastructure.HttpTests
{
    [TestClass]
    public class AccountHttpTests
    {
        readonly string host = "http://" + Config.Domain + ":3000";

        [TestMethod, Timeout(20000)]
        public async Task GetAccountInfo()
        {
            var expected = "SARNASAS2BIAB6LMFA3FPMGBPGIJGK6IJETM3ZSP";

            var response = await new AccountHttp(host).GetAccountInfo(new PublicAccount("B4F12E7C9F6946091E2CB8B6D3A12B50D17CCBBF646386EA27CE2946A7423DCF", NetworkType.Types.MIJIN_TEST));

            Assert.AreEqual(expected, response.Address.Plain);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetAccountInfoFromAddress()
        {
            var expected = "SCEYFB35CYFF2U7UZ32RYXXZ5JTPCSKU4P6BRXZR";

            var response = await new AccountHttp(host).GetAccountInfo(Address.CreateFromEncoded("SCEYFB35CYFF2U7UZ32RYXXZ5JTPCSKU4P6BRXZR"));

            Assert.AreEqual(expected, response.Address.Plain);

            Assert.AreEqual("D525AD41D95FCF29", response.Mosaics[0].MosaicId.HexId);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetAccountsInfoPublicKeys()
        {
            var expected1 = "SCEYFB35CYFF2U7UZ32RYXXZ5JTPCSKU4P6BRXZR";

            var accounts = new PublicKeysDTO()
            {
                PublicKeys = new[]
                {
                    "B974668ABED344BE9C35EE257ACC246117EFFED939EAF42391AE995912F985FE"
                }
            };

            var response = await new AccountHttp(host).GetAccountsInfo(accounts);

            Assert.AreEqual(expected1, response[0].Address.Plain);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetAccountsInfoAddresses()
        {
            var expected1 = "SCEYFB35CYFF2U7UZ32RYXXZ5JTPCSKU4P6BRXZR";

            var accounts = new AddressesDTO()
            {
                Addresses = new[]
                {
                    "SCEYFB35CYFF2U7UZ32RYXXZ5JTPCSKU4P6BRXZR"
                }
            };

            var response = await new AccountHttp(host).GetAccountsInfo(accounts);

            Assert.AreEqual(expected1, response[0].Address.Plain);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetIncomingTransactions()
        {
            var expected = "SCEYFB35CYFF2U7UZ32RYXXZ5JTPCSKU4P6BRXZR";

            var response = await new AccountHttp(host).IncomingTransactions(new PublicAccount("B974668ABED344BE9C35EE257ACC246117EFFED939EAF42391AE995912F985FE", NetworkType.Types.MIJIN_TEST));

            Assert.AreEqual(expected, ((TransferTransaction)response[0]).Address.Plain);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetOutgoingTransactions()
        {
            var expected = "B4F12E7C9F6946091E2CB8B6D3A12B50D17CCBBF646386EA27CE2946A7423DCF";

            var response = await new AccountHttp(host).OutgoingTransactions(new PublicAccount("B4F12E7C9F6946091E2CB8B6D3A12B50D17CCBBF646386EA27CE2946A7423DCF", NetworkType.Types.MIJIN_TEST));

            Assert.AreEqual(expected, response[0].Signer.PublicKey);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetTransactions()
        {
            var expected = "72B4DC358676BFED48DA63AF13727377E55DB5072FC6150D4A101367E93A78FA";

            var response = await new AccountHttp(host).Transactions(new PublicAccount("b4f12e7c9f6946091e2cb8b6d3a12b50d17ccbbf646386ea27ce2946a7423dcf", NetworkType.Types.MIJIN_TEST));

            var trans = (TransferTransaction) response[0];

            Assert.AreEqual(expected, trans.TransactionInfo.Hash);
            Assert.AreEqual(Address.CreateFromEncoded("SBIN2SDQWFD47RFB3IOJ4IVA4GTCKHIC6YIHR3XX").Plain, trans.Address.Plain);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetMultisigGraphInfo()
        {
            var response = await new AccountHttp(host).GetMultisigAccountGraphInfo(new PublicAccount(KeyPair.CreateFromPrivateKey("9abcd1bd50b994799f8a2e27f7ffd952831ef16a443a48241f1f78942322d5c6").PublicKeyString, NetworkType.Types.MIJIN_TEST));

            Assert.AreEqual(2, response.MultisigAccounts[0][0].MinApproval);
            Assert.AreEqual(2, response.MultisigAccounts[0][0].MinRemoval);
            Assert.AreEqual("A8FCF4371B9C4B26CE19A407BA803D3813647608D57ABC1550925A54AEE2C9EA", response.MultisigAccounts[0][0].Account.PublicKey);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetMultisigGraphInfoAddress()
        {
            var response = await new AccountHttp(host).GetMultisigAccountGraphInfo(Address.CreateFromEncoded("SBHQVG3J27J47X7YQJF6WE7KGUP76DBASFNOZGEO"));

            Assert.AreEqual(0, response.MultisigAccounts[0][0].MinApproval);

            Assert.AreEqual("10CC07742437C205D9A0BC0434DC5B4879E002114753DE70CDC4C4BD0D93A64A", response.MultisigAccounts[0][0].Account.PublicKey);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetMultisigInfo()
        {
             var keyPair = KeyPair.CreateFromPrivateKey("9abcd1bd50b994799f8a2e27f7ffd952831ef16a443a48241f1f78942322d5c6");

            var response = await new AccountHttp(host).GetMultisigAccountInfo(new PublicAccount(keyPair.PublicKeyString, NetworkType.Types.MIJIN_TEST));

            Assert.AreEqual("A8FCF4371B9C4B26CE19A407BA803D3813647608D57ABC1550925A54AEE2C9EA", response.Account.PublicKey);
            Assert.AreEqual("SA4UASX7UPK3WM375H5CCGBCCDXEBTQIMYWPSO3X", response.Account.Address.Plain);
            Assert.AreEqual(2, response.MinApproval);
            Assert.AreEqual(2, response.MinRemoval);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetMultisigInfoAddress()
        {
            var response = await new AccountHttp(host).GetMultisigAccountInfo(Address.CreateFromEncoded("SBHQVG3J27J47X7YQJF6WE7KGUP76DBASFNOZGEO"));

            Assert.AreEqual("10CC07742437C205D9A0BC0434DC5B4879E002114753DE70CDC4C4BD0D93A64A", response.Account.PublicKey);
            Assert.AreEqual(0, response.MinApproval);
            Assert.AreEqual(0, response.MinRemoval);
        }
    }
}
