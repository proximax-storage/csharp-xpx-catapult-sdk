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

namespace IntegrationTests.Infrastructure.HttpTests
{
    [TestClass]
    public class TransactionHttpTests
    {
        readonly string host = "http://" + Config.Domain + ":3000";

        [TestMethod, Timeout(20000)]
        public async Task GetTransaction()
        {
            var expected = "80293A007FDC873424A315DCCC109BDA62BA5B48912B8D6065AD44F1467F07A0";
           
            var tx = await new TransactionHttp(host).GetTransaction("5B5272534156C3000134798A");

            Assert.AreEqual(expected, tx.TransactionInfo.Hash);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetTransactions()
        {
            var expected = "80293A007FDC873424A315DCCC109BDA62BA5B48912B8D6065AD44F1467F07A0";

            var txs = await new TransactionHttp(host).GetTransactions(new List<string> { "5B5272534156C3000134798A"});
           
            Assert.AreEqual(expected, txs[0].TransactionInfo.Hash);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetTransactionStatus()
        {
            var expected = "80293A007FDC873424A315DCCC109BDA62BA5B48912B8D6065AD44F1467F07A0";

            var status = await new TransactionHttp(host).GetTransactionStatus("80293A007FDC873424A315DCCC109BDA62BA5B48912B8D6065AD44F1467F07A0");

            Assert.AreEqual(expected, status.Hash);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetTransactionStatuses()
        {
            var expected = "80293A007FDC873424A315DCCC109BDA62BA5B48912B8D6065AD44F1467F07A0";

            var statuses = await new TransactionHttp(host).GetTransactionStatuses(new List<string>() { "80293A007FDC873424A315DCCC109BDA62BA5B48912B8D6065AD44F1467F07A0" });

            Assert.AreEqual(expected, statuses[0].Hash);
        }
    }
}
