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
using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Model.Mosaics;

namespace IntegrationTests.Infrastructure.HttpTests
{
    [TestClass]
    public class MosaicHttpTests
    {
        readonly string host = "http://" + Config.Domain + ":3000";

        [TestMethod, Timeout(20000)]
        public async Task GetMosaic()
        {

            var a = await new MosaicHttp(host).GetMosaic(Xem.Id.HexId);

            Assert.AreEqual(Xem.NamespaceId.HexId, a.NamespaceId.HexId);           
        }

        [TestMethod, Timeout(20000)]
        public async Task GetMosaics()
        {
            var a = await new MosaicHttp(host).GetMosaics(new List<string>{ Xem.Id.HexId } );

            Assert.AreEqual(Xem.NamespaceId.HexId, a[0].NamespaceId.HexId);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetMosaicsName()
        {
            var a = await new MosaicHttp(host).GetMosaicsName(new List<string> { Xem.Id.HexId });

            Assert.AreEqual(Xem.NamespaceId.HexId, a[0].ParentId.HexId);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetMosaicsFromNamespace()
        {
            var a = await new MosaicHttp(host).GetMosaicsFromNamespace(Xem.NamespaceId);

            Assert.AreEqual(Xem.NamespaceId.HexId, a[0].NamespaceId.HexId);
        }
    }
}
