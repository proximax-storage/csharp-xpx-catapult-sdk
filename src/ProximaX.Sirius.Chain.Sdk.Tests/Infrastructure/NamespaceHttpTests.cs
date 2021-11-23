using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http.Testing;
using ProximaX.Sirius.Chain.Sdk.Infrastructure;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Namespaces;
using ProximaX.Sirius.Chain.Sdk.Tests.Utils;
using Xunit;

namespace ProximaX.Sirius.Chain.Sdk.Tests.Infrastructure
{
    public class NamespaceHttpTests : BaseTest
    {
        private readonly NamespaceHttp _namespaceHttp;
        private readonly AccountHttp _accountHttp;

        public NamespaceHttpTests()
        {
            _namespaceHttp = new NamespaceHttp(BaseUrl) { NetworkType = NetworkType.TEST_NET };
            _accountHttp = new AccountHttp(BaseUrl) { NetworkType = NetworkType.TEST_NET };
        }

        [Fact]
        public async Task Get_Namespace_Should_Return_NamespaceInfo()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToObject(@"Testdata\\Namespace\\GetNamespaceInfo.json");

                httpTest.RespondWithJson(fakeJson);

                var namespaceId = new NamespaceId("ywhwdpaixh");

                const string expectedNamespaceIdHex = "EED8BF8F60D2FB68";
                namespaceId.HexId.Should().BeEquivalentTo(expectedNamespaceIdHex);
                var namespaceInfo = await _namespaceHttp.GetNamespace(namespaceId);
                namespaceInfo.Id.HexId.Should().BeEquivalentTo(expectedNamespaceIdHex);
            }
        }

        [Fact]
        public async Task Get_SubNamespace_Should_Return_SubNamespaceInfo()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson = TestHelper.LoadJsonFileToObject(@"Testdata\\Namespace\\GetSubNamespaceInfo.json");
                httpTest.RespondWithJson(fakeJson);

                const string expectedNamespaceIdHex = "b2968f567f04394a";
                const string expectedParentNamespaceIdHex = "b16d77fd8b6fb3be";
                var namespaceId = new NamespaceId("prx.sc");

                var namespaceInfo = await _namespaceHttp.GetNamespace(namespaceId);
                namespaceInfo.Should().NotBeNull();
                namespaceInfo.Id.HexId.Should().BeEquivalentTo(expectedNamespaceIdHex);
                namespaceInfo.ParentId.HexId.Should().BeEquivalentTo(expectedParentNamespaceIdHex);
            }
        }

        [Fact]
        public async Task Get_Namespaces_By_Account_Address_Should_Return_NamespaceInfo_List()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson = TestHelper.LoadJsonFileToArray(@"Testdata\\Namespace\\GetNamespacesbyAccountid.json");
                httpTest.RespondWithJson(fakeJson);
                const string expectedAddress = "VDPQS6FBYDN3SD2QJPHUWRYWNHSSOQ2Q35VI7TDP";
                var address = Address.CreateFromRawAddress(expectedAddress);
                // address.Plain.Should().BeEquivalentTo(expectedAddress);

                var namespaceInfoList = await _namespaceHttp.GetNamespacesFromAccount(address, null);
                //namespaceInfoList.Should().HaveCountGreaterThan(0);
                // namespaceInfoList.Should().BeEquivalentTo();
                namespaceInfoList.Should().NotBeEmpty();
            }
        }

        [Fact]
        public async Task Get_Namespaces_By_PublicKey_Should_Return_NamespaceInfo_List()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson = TestHelper.LoadJsonFileToArray(@"Testdata\\Namespace\\GetNamespacesbyAccountid.json");
                httpTest.RespondWithJson(fakeJson);
                var account = PublicAccount.CreateFromPublicKey("0D22E9D42F124072E14C4F804E4FC7F5431C831EAF03BEFD55D521B9A9D0B89D", NetworkType.TEST_NET);

                var namespaceInfoList = await _namespaceHttp.GetNamespacesFromAccount(account, null);
                // namespaceInfoList.
                namespaceInfoList.Should().NotBeEmpty();
            }
        }

        [Fact]
        public async Task Get_Namespaces_By_Account_Address_With_PageSize_Should_Return_NamespaceInfo_List()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson = TestHelper.LoadJsonFileToArray(@"Testdata\\Namespace\\GetNamespacesbyAccountid.json");
                httpTest.RespondWithJson(fakeJson);
                const string expectedAddress = "VDPQS6FBYDN3SD2QJPHUWRYWNHSSOQ2Q35VI7TDP";
                var address = Address.CreateFromRawAddress(expectedAddress);

                var query = new QueryParams(2, "", Order.DESC);
                var namespaceInfoList = await _namespaceHttp.GetNamespacesFromAccount(address, query);

                namespaceInfoList.Should().HaveCountGreaterOrEqualTo(2);
            }
        }

        [Fact]
        public async Task Get_Namespaces_By_Account_Address_With_id_Should_Return_NamespaceInfo_List()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson = TestHelper.LoadJsonFileToArray(@"Testdata\\Namespace\\GetNamespacesbyAccountid.json");
                httpTest.RespondWithJson(fakeJson);
                const string expectedAddress = "VDPQS6FBYDN3SD2QJPHUWRYWNHSSOQ2Q35VI7TDP";
                var address = Address.CreateFromRawAddress(expectedAddress);

                var query = new QueryParams(2, null);
                var namespaceInfoList = await _namespaceHttp.GetNamespacesFromAccount(address, query);

                namespaceInfoList.Should().HaveCountGreaterThan(0);
            }
        }

        [Fact]
        public async Task Get_Namespaces_Names_By_NamespaceId_List_Should_Return_NamespaceNames_List()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson = TestHelper.LoadJsonFileToArray(@"Testdata\\Namespace\\GetNameforNamespace.json");
                httpTest.RespondWithJson(fakeJson);
                var ns1 = new NamespaceId("ywhwdpaixh");
                var ns2 = new NamespaceId("ranbqsrnvs");

                var namespaceIds = new List<NamespaceId>
                {
                   ns1,
                   ns2
                };

                var namespaceNames = await _namespaceHttp.GetNamespacesNames(namespaceIds);
                namespaceNames.Should().HaveCountGreaterThan(0);
            }
        }

        [Fact]
        public async Task Get_Names_From_PublicKey()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson = TestHelper.LoadJsonFileToArray(@"Testdata\\Namespace\\GetNamesFromAccount.json");
                httpTest.RespondWithJson(fakeJson);

                var PublicAccount1 = PublicAccount.CreateFromPublicKey("0D22E9D42F124072E14C4F804E4FC7F5431C831EAF03BEFD55D521B9A9D0B89D", NetworkType.TEST_NET);
                var PublicAccount2 = PublicAccount.CreateFromPublicKey("C090C908E36A0AF2F7538B3BEE39B96915F011D077C83EF743B990E84DBA4408", NetworkType.TEST_NET);

                var publicAccounts = new List<PublicAccount>
                {
                    PublicAccount1,
                    PublicAccount2
                };

                var namespaceNames = await _accountHttp.GetNamesFromAccount(publicAccounts);
                namespaceNames.Should().HaveCountGreaterThan(1);
            }
        }

        [Fact]
        public async Task Get_Names_From_Address()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson = TestHelper.LoadJsonFileToArray(@"Testdata\\Namespace\\GetNamesFromAccount.json");
                httpTest.RespondWithJson(fakeJson);

                var address1 = Address.CreateFromRawAddress("VDPQS6FBYDN3SD2QJPHUWRYWNHSSOQ2Q35VI7TDP");
                var address2 = Address.CreateFromRawAddress("VCVDQ42ODLSWROON5QSQS6PUO4UJCUZDK7MI6C5G");

                var address = new List<Address>
                {
                    address1,
                    address2
                };

                var namespaceNames = await _accountHttp.GetNamesFromAccount(address);
                namespaceNames.Should().HaveCountGreaterThan(1);
            }
        }
    }
}