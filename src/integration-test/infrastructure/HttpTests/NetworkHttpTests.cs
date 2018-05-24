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
using io.nem2.sdk.Model.Blockchain;

namespace IntegrationTests.Infrastructure.HttpTests
{
    [TestClass]
    public class NetworkHttpTests
    {
        readonly string host = "http://" + Config.Domain + ":3000";

        [TestMethod, Timeout(20000)]
        public async Task GetNetworkType()
        {
            var expected = "mijinTest";

            var a = await new NetworkHttp(host).GetNetworkType();

            Assert.AreEqual(NetworkType.GetNetwork(expected), a);
        }
    }
}
