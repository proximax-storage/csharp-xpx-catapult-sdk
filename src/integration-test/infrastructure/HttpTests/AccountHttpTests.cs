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
            var expected = "SAZJ2CI6OQFZHM4EIDTBQPP7ITOSZG33DAFSE4IV";

            var response = await new AccountHttp(host).GetAccountInfo(new PublicAccount("D49C902E2FBAC09AF732D0A10DB8481E41C8D71B12B76D04DCC2869C1E549779", NetworkType.Types.MIJIN_TEST));

            Assert.AreEqual(expected, response.Address.Plain);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetAccountInfoFromAddress()
        {
            var expected = "SCMEYH3HEOFJMWRPIFZTVQB4SGARVS5YCY7TKN3S";

            var response = await new AccountHttp(host).GetAccountInfo(Address.CreateFromRawAddress("SCMEYH3HEOFJMWRPIFZTVQB4SGARVS5YCY7TKN3S"));
            
            Assert.AreEqual(expected, response.Address.Plain);

            Assert.AreEqual("D525AD41D95FCF29", response.Mosaics[0].MosaicId.HexId);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetAccountsInfoPublicKeys()
        {
            var expected1 = "SAZJ2CI6OQFZHM4EIDTBQPP7ITOSZG33DAFSE4IV";

            var accounts = new List<PublicAccount>()
            {
                new PublicAccount("D49C902E2FBAC09AF732D0A10DB8481E41C8D71B12B76D04DCC2869C1E549779", NetworkType.Types.MIJIN_TEST)
            };

            var response = await new AccountHttp(host).GetAccountsInfo(accounts);

            Assert.AreEqual(expected1, response[0].Address.Plain);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetAccountsInfoAddresses()
        {
            var expected1 = "SAZJ2CI6OQFZHM4EIDTBQPP7ITOSZG33DAFSE4IV";

            var accounts = new List<Address>()
            {
                Address.CreateFromRawAddress("SAZJ2CI6OQFZHM4EIDTBQPP7ITOSZG33DAFSE4IV")
            };

            var response = await new AccountHttp(host).GetAccountsInfo(accounts);

            Assert.AreEqual(expected1, response[0].Address.Plain);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetIncomingTransactions()
        {
            var expected = "SAZJ2CI6OQFZHM4EIDTBQPP7ITOSZG33DAFSE4IV";

            var response = await new AccountHttp(host).IncomingTransactions(new PublicAccount("D49C902E2FBAC09AF732D0A10DB8481E41C8D71B12B76D04DCC2869C1E549779", NetworkType.Types.MIJIN_TEST));

            Assert.AreEqual(expected, ((TransferTransaction)response[0]).Address.Plain);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetOutgoingTransactions()
        {
            var expected = "D49C902E2FBAC09AF732D0A10DB8481E41C8D71B12B76D04DCC2869C1E549779";

            var response = await new AccountHttp(host).OutgoingTransactions(new PublicAccount("D49C902E2FBAC09AF732D0A10DB8481E41C8D71B12B76D04DCC2869C1E549779", NetworkType.Types.MIJIN_TEST));

            Assert.AreEqual(expected, response[0].Signer.PublicKey);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetTransactions()
        {
            var expected = "93A73126673750942371293E4C7A28A5E68763605A97CDC5E0B373E6F281F251";

            var response = await new AccountHttp(host).Transactions(new PublicAccount("779184937205947C1B36B0AD0FCC0D7C10A08061FFB6C0F3F4022BD3CD6A709A", NetworkType.Types.MIJIN_TEST));

            var trans = (TransferTransaction) response[0];

            Assert.AreEqual(expected, trans.TransactionInfo.Hash);
            Assert.AreEqual(Address.CreateFromRawAddress("SA6VBFEOYF4CWLMARDQQT5FH2D3P4KNL4SJKBT3N").Plain, trans.Address.Plain);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetMultisigGraphInfo()
        {
            var response = await new AccountHttp(host).GetMultisigAccountGraphInfo(new PublicAccount(KeyPair.CreateFromPrivateKey("F8E3C9B17923889138F262494E53D7E4604658CEECEFC67DF6A4BD30D421C0B0").PublicKeyString, NetworkType.Types.MIJIN_TEST));

            Assert.AreEqual(2, response.MultisigAccounts[0][0].MinApproval);
            Assert.AreEqual(2, response.MultisigAccounts[0][0].MinRemoval);
            Assert.AreEqual("205B8C27461DCBD9EAF8BB2A8C673E72638A079378F3BF290C406F92EC3A9EB8", response.MultisigAccounts[0][0].Account.PublicKey);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetMultisigGraphInfoAddress()
        {
            var response = await new AccountHttp(host).GetMultisigAccountGraphInfo(Address.CreateFromRawAddress("SCMEYH3HEOFJMWRPIFZTVQB4SGARVS5YCY7TKN3S"));

            Assert.AreEqual(2, response.MultisigAccounts[0][0].MinApproval);

            Assert.AreEqual("205B8C27461DCBD9EAF8BB2A8C673E72638A079378F3BF290C406F92EC3A9EB8", response.MultisigAccounts[0][0].Account.PublicKey);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetMultisigInfo()
        {
             var keyPair = KeyPair.CreateFromPrivateKey("F8E3C9B17923889138F262494E53D7E4604658CEECEFC67DF6A4BD30D421C0B0");

            var response = await new AccountHttp(host).GetMultisigAccountInfo(new PublicAccount(keyPair.PublicKeyString, NetworkType.Types.MIJIN_TEST));

            Assert.AreEqual("205B8C27461DCBD9EAF8BB2A8C673E72638A079378F3BF290C406F92EC3A9EB8", response.Account.PublicKey);
            Assert.AreEqual("SCMEYH3HEOFJMWRPIFZTVQB4SGARVS5YCY7TKN3S", response.Account.Address.Plain);
            Assert.AreEqual(2, response.MinApproval);
            Assert.AreEqual(2, response.MinRemoval);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetMultisigInfoAddress()
        {
            var response = await new AccountHttp(host).GetMultisigAccountInfo(Address.CreateFromRawAddress("SCMEYH3HEOFJMWRPIFZTVQB4SGARVS5YCY7TKN3S"));

            Assert.AreEqual("205B8C27461DCBD9EAF8BB2A8C673E72638A079378F3BF290C406F92EC3A9EB8", response.Account.PublicKey);
            Assert.AreEqual(2, response.MinApproval);
            Assert.AreEqual(2, response.MinRemoval);
        }
    }
}
