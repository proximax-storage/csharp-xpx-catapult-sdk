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
            var a = await new BlockchainHttp(host).GetBlockchainHeight();

            Assert.IsTrue(a > 100);    
        }

        [TestMethod, Timeout(20000)]
        public async Task GetBlockByHeight()
        {
            var a = await new BlockchainHttp(host).GetBlockByHeight(3);
           
            Assert.AreEqual("CE6B3124DF98010E7B53E5182EE0C699FFCE2F90D75DACFF5430CF224A138E80", a.Hash);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetBlockByHeightWithLimit()
        {
            var a = await new BlockchainHttp(host).GetBlockByHeightWithLimit(2, 10);

            Assert.AreEqual("A3EB6D8DC3980BCF390F65321899F34197A2778F7B288E31BC512B37E50E7C5E", a[0].Hash);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetBlockTransactions()
        {
            var a = await new BlockchainHttp(host).GetBlockTransactions(1);

            Assert.AreEqual("1644E712A9E183C58397B7790253B8971D3274811ED82E432672C8A821ECF621", a[0].TransactionInfo.Hash);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetBlockchainDiagnosticBlocksWithLimit()
        {
            var a = await new BlockchainHttp(host).GetBlockchainDiagnosticBlocksWithLimit(2, 10);

            Assert.AreEqual("58F7244D49DA757EF72EAF3B7FB6E545DC7410708986F168C1AEEA6B67A0668A", a[0].Hash);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetBlockchainDiagnosticStorage()
        {
            var a = await new BlockchainHttp(host).GetBlockchainDiagnosticStorage();

            Assert.IsTrue(a.NumAccounts > 1);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetBlockchainDiagnosticStorageWithLimit()
        {
            var a = await new BlockchainHttp(host).GetBlockchainDiagnosticBlocksWithLimit(1, 10);

            Assert.IsTrue(a[9].Height == 1);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetBlockchainScore() //TODO: verify
        {
            var a = await new BlockchainHttp(host).GetBlockchainScore();

            Assert.IsTrue(a > 1);
        }
    }
}
