// Copyright 2019 ProximaX
//
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

using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http.Testing;
using ProximaX.Sirius.Chain.Sdk.Infrastructure;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Metadata;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Namespaces;
using ProximaX.Sirius.Chain.Sdk.Tests.Utils;
using Xunit;
using Xunit.Abstractions;

namespace ProximaX.Sirius.Chain.Sdk.Tests.Infrastructure
{
    public class MetadataHttpTests : BaseTest
    {
        private readonly MetadataHttp _metadataHttp;
        private readonly ITestOutputHelper Log;

        public MetadataHttpTests(ITestOutputHelper log)
        {
            _metadataHttp = new MetadataHttp(BaseUrl);
            Log = log;
        }

        [Fact]
        public async Task Get_MetadataList()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToArray(@"Testdata\\Metadata\\GetMetadataListByCompositeHashes.json");

                httpTest.RespondWithJson(fakeJson);
                var compositeHashes = new List<string>()
                {
                    "169C532681A239D83A0577E3A4320A96C4A4E441B69C0F5C5493A96D254FAB67",
                    "C24948F9448CDA7F1F0DB671A0BC74402FF424E5F8B310ECA564A6C69D2087E8",
                    "0505D822394D025A67A30967A1DA7ABA59053EFEB63D08A0AF8C8ECDB07FA64E"
                };
                var metadataInfo = await _metadataHttp.GetMetadata(compositeHashes);
                metadataInfo.Should().NotBeNull();
                metadataInfo.Should().HaveCount(3);
            }
        }

        [Fact]
        public async Task Get_Metadata()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToObject(@"Testdata\\Metadata\\GetMetadataByCompositeHash.json");

                httpTest.RespondWithJson(fakeJson);

                const string compositeHash = "5DB111FAFD1CD1AB10747B1BDDF895D6469965A1D11D73E8B74F0D44A16BBE8E";
                var metadataInfo = await _metadataHttp.GetMetadata(compositeHash);
                metadataInfo.Should().NotBeNull();
                //  metadataInfo.Fields.Should().HaveCount(2);
                metadataInfo.MetadataType.Should().Equals(MetadataType.MOSAIC);
            }
        }

        [Fact]
        public async Task Search_Metadata()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToObject(@"Testdata\\Metadata\\GetMetadataInfoByMetadataSearch.json");

                httpTest.RespondWithJson(fakeJson);

                var address = "VBSI675LOU33SWOVGLSSCQLAKL2QRGJA5AZOFU6N";
                var targetKey = "359BAB30BF217A592372FADBE1F39C36C7717AC58A592324826A8E50B6829C69";
                var metadataInfo = await _metadataHttp.SearchMetadata(new MetadataQueryParams(1, Order.ASC, 1, Address.CreateFromRawAddress(address), null, targetKey, null));
                metadataInfo.Entries.Should().NotBeEmpty();
                //  metadataInfo.Fields.Should().HaveCount(2);
                //  metadataInfo.Type.Should().BeEquivalentTo(MetadataType.MOSAIC);
                metadataInfo.Entries.Single(ap => ap.ValueSize == 1);
                metadataInfo.Entries.Single(ap => ap.Value == "74657374");
                metadataInfo.Entries.Single(ap => ap.TargetKey == "359BAB30BF217A592372FADBE1F39C36C7717AC58A592324826A8E50B6829C69");

                metadataInfo.Paginations.PageNumber.Equals(1);
                metadataInfo.Paginations.PageSize.Equals(1);
            }
        }
    }
}