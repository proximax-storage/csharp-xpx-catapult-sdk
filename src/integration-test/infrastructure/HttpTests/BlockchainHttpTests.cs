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
using io.nem2.sdk.Infrastructure.HttpRepositories;

namespace IntegrationTests.Infrastructure.HttpTests
{
    [TestClass]
    public class BlockchainHttpTests
    {
        readonly string host = "http://" + Config.Domain + ":3000";

        [TestMethod, Timeout(20000)]
        public async Task GetHeight()
        {
            var height = await new BlockchainHttp(host).GetBlockchainHeight();

            Assert.IsTrue(height > 100);    
        }

        [TestMethod, Timeout(20000)]
        public async Task GetBlockByHeight()
        {
            var block = await new BlockchainHttp(host).GetBlockByHeight(3);
           
            Assert.AreEqual("D70DF7FD6FC7B3AD31C0EBCCAE0F6881CE39555BF72179472A31DEE5AB6A926B", block.Hash);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetBlockByHeightWithLimit()
        {
            var blocks = await new BlockchainHttp(host).GetBlockByHeightWithLimit(2, 10);

            Assert.AreEqual("32305811E9173FC5ED6897462E301B0495AD95F871A66341D95485FB4A59C8F5", blocks[0].Hash);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetBlockTransactions()
        {
            var txs = await new BlockchainHttp(host).GetBlockTransactions(1);

            Assert.AreEqual("124ACC56AF5055CEE7E134D0A6E809C45B1054C0FA96C31838669206030EDD2C", txs[0].TransactionInfo.Hash);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetBlockchainDiagnosticBlocksWithLimit()
        {
            var txs = await new BlockchainHttp(host).GetBlockchainDiagnosticBlocksWithLimit(2, 10);

            Assert.AreEqual("5277C6AF6CA15943A0F27A90C2232B989C4D62CEC212F0673B7F8B5D4F0C1834", txs[0].Hash);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetBlockchainDiagnosticStorage()
        {
            var diagnostics = await new BlockchainHttp(host).GetBlockchainDiagnosticStorage();

            Assert.IsTrue(diagnostics.NumAccounts > 1);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetBlockchainDiagnosticStorageWithLimit()
        {
            var diagnostics = await new BlockchainHttp(host).GetBlockchainDiagnosticBlocksWithLimit(1, 10);

            Assert.IsTrue(diagnostics[9].Height == 1);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetBlockchainScore()
        {
            var score = await new BlockchainHttp(host).GetBlockchainScore();

            Assert.IsTrue(score > 1);
        }
    }
}
