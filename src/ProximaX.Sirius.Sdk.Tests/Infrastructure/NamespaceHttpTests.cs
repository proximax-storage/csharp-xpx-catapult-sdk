using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http.Testing;
using ProximaX.Sirius.Sdk.Infrastructure;
using ProximaX.Sirius.Sdk.Model.Accounts;
using ProximaX.Sirius.Sdk.Model.Blockchain;
using ProximaX.Sirius.Sdk.Model.Namespaces;
using ProximaX.Sirius.Sdk.Tests.Utils;
using Xunit;

namespace ProximaX.Sirius.Sdk.Tests.Infrastructure
{
    
    public class NamespaceHttpTests : BaseTest
    {
        private readonly NamespaceHttp _namespaceHttp;

        public NamespaceHttpTests()
        {

            _namespaceHttp = new NamespaceHttp(BaseUrl) { NetworkType = NetworkType.MIJIN_TEST };
        }

        [Fact]
        public async Task Get_Namespace_Should_Return_NamespaceInfo()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToObject(@"Testdata\\Namespace\\GetNamespaceInfo.json");

                httpTest.RespondWithJson(fakeJson);

                var namespaceId = new NamespaceId("nsp274d22");

                const string expectedNamespaceIdHex = "BCE8AD8B2668C272";
                namespaceId.HexId.Should().BeEquivalentTo(expectedNamespaceIdHex);
                var namespaceInfo = await _namespaceHttp.GetNamespace(namespaceId);
                namespaceInfo.Id.HexId.Should().BeEquivalentTo(expectedNamespaceIdHex);
            }
          
        }

        /*
        [Fact]
        public async Task Get_SubNamespace_Should_Return_NamespaceInfo()
        {

            //using (var httpTest = new HttpTest())
           // {
            //    var fakeJson = TestHelper.LoadJsonFileToObject(@"Testdata\\GetSubNamespaceInfo.json");
           //     httpTest.RespondWithJson(fakeJson);

                const string expectedNamespaceIdHex = "AF6D6FD56D6E51CB";
                var namespaceId = new NamespaceId("nsp4daa64.subns0992ae");
                var namespaceInfo = await _namespaceHttp.GetNamespace(namespaceId);
                namespaceInfo.Id.HexId.Should().BeEquivalentTo(expectedNamespaceIdHex);
          //  }

        }

        [Fact]
        public async Task Get_Namespaces_By_Account_Address_Should_Return_NamespaceInfo_List()
        {
            const string expectedAddress = "VARNASAS2BIAB6LMFA3FPMGBPGIJGK6IJGOH3FCE";
            var address = Address.CreateFromRawAddress(expectedAddress);
            address.Plain.Should().BeEquivalentTo(expectedAddress);

            var namespaceInfoList = await _namespaceHttp.GetNamespacesFromAccount(address, null);
            namespaceInfoList.Should().HaveCountGreaterThan(0);
        }

        [Fact]
        public async Task Get_Namespaces_By_Account_Addresses_Should_Return_NamespaceInfo_List()
        {
            const string expectedAddress = "VARNASAS2BIAB6LMFA3FPMGBPGIJGK6IJGOH3FCE";
            var address = Address.CreateFromRawAddress(expectedAddress);
            var addresses = new List<Address>
            {
                address
            };

            var namespaceInfoList = await _namespaceHttp.GetNamespacesFromAccount(addresses, null);
            namespaceInfoList.Should().HaveCountGreaterThan(0);
        
        }

        [Fact]
        public async Task Get_Namespaces_By_PublicKey_Should_Return_NamespaceInfo_List()
        {
            var account = PublicAccount.CreateFromPublicKey("B4F12E7C9F6946091E2CB8B6D3A12B50D17CCBBF646386EA27CE2946A7423DCF", NetworkType.PUBLIC_TEST);
        
            var namespaceInfoList = await _namespaceHttp.GetNamespacesFromAccount(account, null);

            namespaceInfoList.Should().HaveCountGreaterThan(0);
        
        }

        [Fact]
        public async Task Get_Namespaces_By_Account_Address_With_PageSize_Should_Return_NamespaceInfo_List()
        {
            const string expectedAddress = "VARNASAS2BIAB6LMFA3FPMGBPGIJGK6IJGOH3FCE";
            var address = Address.CreateFromRawAddress(expectedAddress);

            var query = new QueryParams(2, null);
            var namespaceInfoList = await _namespaceHttp.GetNamespacesFromAccount(address, query);

            namespaceInfoList.Should().HaveCountGreaterThan(1);
      
        }


        [Fact]
        public async Task Get_Namespaces_Names_By_NamespaceId_List_Should_Return_NamespaceNames_List()
        {
            
            var ns = new NamespaceId("prx.xpx");
            var namespaceIds = new List<NamespaceId>
            {
                ns
            };

            var namespaceNames = await _namespaceHttp.GetNamespacesNames(namespaceIds);
            namespaceNames.Should().HaveCountGreaterThan(0);
           
        }*/
    }
}
