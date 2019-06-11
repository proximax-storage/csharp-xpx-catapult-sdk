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
using ProximaX.Sirius.Sdk.Infrastructure;
using ProximaX.Sirius.Sdk.Model.Accounts;
using ProximaX.Sirius.Sdk.Model.Metadata;
using ProximaX.Sirius.Sdk.Model.Mosaics;
using ProximaX.Sirius.Sdk.Model.Namespaces;
using ProximaX.Sirius.Sdk.Tests.Utils;
using Xunit;

namespace ProximaX.Sirius.Sdk.Tests.Infrastructure
{
    public class MetadataHttpTests : BaseTest
    {
        private readonly MetadataHttp _metadataHttp;

        public MetadataHttpTests()
        {
            _metadataHttp = new MetadataHttp(BaseUrl);
        }
       
        [Fact]
        public async Task GetMetadataByAddress()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToObject(@"Testdata\\Metadata\\GetMetadataByAddress.json");

                httpTest.RespondWithJson(fakeJson);

                const string metadataId = "SDEACLIEMSZUQCVZGFOAPCMZ6WYQCCAMMHOOYFLE";
                var address = Address.CreateFromRawAddress(metadataId);
                var metadataInfo = await _metadataHttp.GetMetadataFromAddress(address);
                metadataInfo.Should().NotBeNull();
                metadataInfo.Fields.Should().HaveCount(2);
                metadataInfo.Type.Should().BeEquivalentTo(MetadataType.ADDRESS);
                metadataInfo.Id.Should().BeEquivalentTo(address.Plain);
            }
        }

        [Fact]
        public async Task GetMetadataByMetadataId_AddressType()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToObject(@"Testdata\\Metadata\\GetMetadataByAddressMetadataId.json");

                httpTest.RespondWithJson(fakeJson);

                const string metadataId = "SDEACLIEMSZUQCVZGFOAPCMZ6WYQCCAMMHOOYFLE";
                var metadataInfo = await _metadataHttp.GetMetadata(metadataId);
                metadataInfo.Should().NotBeNull();
                metadataInfo.Fields.Should().HaveCount(2);
                metadataInfo.Type.Should().BeEquivalentTo(MetadataType.ADDRESS);
            }
        }

        [Fact]
        public async Task GetMetadataByMetadataId_MosaicType()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToObject(@"Testdata\\Metadata\\GetMetadataByMosaicMetadataId.json");

                httpTest.RespondWithJson(fakeJson);

                const string metadataId = "2AF446B15FD961AA";
                var metadataInfo = await _metadataHttp.GetMetadata(metadataId);
                metadataInfo.Should().NotBeNull();
                metadataInfo.Fields.Should().HaveCount(2);
                metadataInfo.Type.Should().BeEquivalentTo(MetadataType.MOSAIC);
            }
        }

        [Fact]
        public async Task GetMetadataByMetadataId_NamespaceType()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToObject(@"Testdata\\Metadata\\GetMetadataByNamespaceMetadataId.json");

                httpTest.RespondWithJson(fakeJson);

                const string metadataId = "840041D569D2C8CC";
                var metadataInfo = await _metadataHttp.GetMetadata(metadataId);
                metadataInfo.Should().NotBeNull();
                metadataInfo.Fields.Should().HaveCount(2);
                metadataInfo.Type.Should().BeEquivalentTo(MetadataType.NAMESPACE);
            }
        }

        [Fact]
        public async Task GetMetadataByMosaicId()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToObject(@"Testdata\\Metadata\\GetMetadataByMosaicId.json");

                httpTest.RespondWithJson(fakeJson);

                const string metadataId = "2AF446B15FD961AA";
                var mosaicId = new MosaicId(metadataId);
                var metadataInfo = await _metadataHttp.GetMetadataFromMosaic(mosaicId);
                metadataInfo.Should().NotBeNull();
                metadataInfo.Fields.Should().HaveCount(2);
                metadataInfo.Type.Should().BeEquivalentTo(MetadataType.MOSAIC);
                metadataInfo.Id.HexId.Should().BeEquivalentTo(mosaicId.HexId);
            }
        }


        [Fact]
        public async Task GetMetadataByNamespaceId()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToObject(@"Testdata\\Metadata\\GetMetadataByNamespaceId.json");

                httpTest.RespondWithJson(fakeJson);

                const string namespaceName = "nsp4d7e2e";
                var namespaceId = new NamespaceId(namespaceName);
                var metadataInfo = await _metadataHttp.GetMetadataFromNamespace(namespaceId);
                metadataInfo.Should().NotBeNull();
                metadataInfo.Fields.Should().HaveCount(2);
                metadataInfo.Type.Should().BeEquivalentTo(MetadataType.NAMESPACE);
                metadataInfo.Id.HexId.Should().BeEquivalentTo(namespaceId.HexId);
            }
        }

        [Fact]
        public async Task GetMetadataListByMetadataIds()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToArray(@"Testdata\\Metadata\\GetMetadataListByMetadataIds.json");

                httpTest.RespondWithJson(fakeJson);

                var metadataIds = new List<string>
                {
                    "840041D569D2C8CC",
                    "2AF446B15FD961AA",
                    "SDEACLIEMSZUQCVZGFOAPCMZ6WYQCCAMMHOOYFLE"
                };

                var metadataList = await _metadataHttp.GetMetadata(metadataIds);
                metadataList.Should().NotBeNullOrEmpty();
                metadataList.Should().HaveCount(3);
                metadataList.Select(a => a.Type == MetadataType.ADDRESS).Should().NotBeNull();
                metadataList.Select(a => a.Type == MetadataType.MOSAIC).Should().NotBeNull();
                metadataList.Select(a => a.Type == MetadataType.NAMESPACE).Should().NotBeNull();
            }
        }
    }
}