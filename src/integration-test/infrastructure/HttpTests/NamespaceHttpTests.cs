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
using io.nem2.sdk.Model.Mosaics;
using io.nem2.sdk.Model.Namespace;

namespace IntegrationTests.Infrastructure.HttpTests
{
    [TestClass]
    public class NamespaceHttpTests
    {
        readonly string host = "http://" + Config.Domain + ":3000";

        [TestMethod, Timeout(20000)]
        public async Task GetNamespace()
        {
            var expected = "779184937205947C1B36B0AD0FCC0D7C10A08061FFB6C0F3F4022BD3CD6A709A";

            var a = await new NamespaceHttp(host).GetNamespace(Xem.NamespaceId);

            Assert.AreEqual(expected, a.Owner.PublicKey);

        }

        [TestMethod, Timeout(20000)]
        public async Task GetNamespacesFromAccount()
        {
            var expected = "779184937205947C1B36B0AD0FCC0D7C10A08061FFB6C0F3F4022BD3CD6A709A";

            var a = await new NamespaceHttp(host).GetNamespacesFromAccount(new PublicAccount("779184937205947C1B36B0AD0FCC0D7C10A08061FFB6C0F3F4022BD3CD6A709A", NetworkType.Types.MIJIN_TEST));

            Assert.AreEqual(expected, a[0].Owner.PublicKey);

        }

        [TestMethod, Timeout(20000)]
        public async Task GetNamespacesFromAccountAddress()
        {
            var expected = "779184937205947C1B36B0AD0FCC0D7C10A08061FFB6C0F3F4022BD3CD6A709A";

            var a = await new NamespaceHttp(host).GetNamespacesFromAccount(Address.CreateFromPublicKey("779184937205947C1B36B0AD0FCC0D7C10A08061FFB6C0F3F4022BD3CD6A709A", NetworkType.Types.MIJIN_TEST));

            Assert.AreEqual(expected, a[0].Owner.PublicKey);

        }

        [TestMethod, Timeout(20000)]
        public async Task GetNamespacesFromAccountWithLimit()
        {
            var expected = "779184937205947C1B36B0AD0FCC0D7C10A08061FFB6C0F3F4022BD3CD6A709A";

            var a = await new NamespaceHttp(host).GetNamespacesFromAccount(new PublicAccount("779184937205947C1B36B0AD0FCC0D7C10A08061FFB6C0F3F4022BD3CD6A709A", NetworkType.Types.MIJIN_TEST), new QueryParams(10));

            Assert.AreEqual(expected, a[0].Owner.PublicKey);

        }

        [TestMethod, Timeout(20000)]
        public async Task GetNamespacesFromAccounts()
        {
            var expected = "779184937205947C1B36B0AD0FCC0D7C10A08061FFB6C0F3F4022BD3CD6A709A";

            var a = await new NamespaceHttp(host).GetNamespacesFromAccounts(new List<PublicAccount>() { new PublicAccount("779184937205947C1B36B0AD0FCC0D7C10A08061FFB6C0F3F4022BD3CD6A709A", NetworkType.Types.MIJIN_TEST) });

            Assert.AreEqual(expected, a[0].Owner.PublicKey);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetNamespacesFromAccountsAddress()
        {
            var expected = "779184937205947C1B36B0AD0FCC0D7C10A08061FFB6C0F3F4022BD3CD6A709A";

            var a = await new NamespaceHttp(host).GetNamespacesFromAccounts(new List<Address>() { Address.CreateFromPublicKey("779184937205947C1B36B0AD0FCC0D7C10A08061FFB6C0F3F4022BD3CD6A709A", NetworkType.Types.MIJIN_TEST) });

            Assert.AreEqual(expected, a[0].Owner.PublicKey);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetNamespacesFromAccountsWithLimit()
        {
            var expected = "779184937205947C1B36B0AD0FCC0D7C10A08061FFB6C0F3F4022BD3CD6A709A";

            var a = await new NamespaceHttp(host).GetNamespacesFromAccounts(new List<PublicAccount>() { new PublicAccount("779184937205947C1B36B0AD0FCC0D7C10A08061FFB6C0F3F4022BD3CD6A709A", NetworkType.Types.MIJIN_TEST) }, new QueryParams(10));

            Assert.AreEqual(expected, a[0].Owner.PublicKey);
        }

        [TestMethod, Timeout(20000)]
        public async Task GetNamespacesNames()
        {
            var expected = "84B3552D375FFA4B";

            var a = await new NamespaceHttp(host).GetNamespacesNames(new List<NamespaceId> { Xem.NamespaceId });

            Assert.AreEqual(expected, a[0].HexId);
        }
    }
}
