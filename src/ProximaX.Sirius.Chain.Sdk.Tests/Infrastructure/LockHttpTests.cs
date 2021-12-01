using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http.Testing;
using Newtonsoft.Json;
using ProximaX.Sirius.Chain.Sdk.Infrastructure;
using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Tests.Utils;
using ProximaX.Sirius.Chain.Sdk.Utils;
using Xunit;

namespace ProximaX.Sirius.Chain.Sdk.Tests.Infrastructure
{
    public class LockHttpTests : BaseTest
    {
        private readonly LockHttp _lockHttp;

        public LockHttpTests()
        {
            _lockHttp = new LockHttp(BaseUrl) { NetworkType = NetworkType.TEST_NET };
        }

        [Fact]
        public async Task Get_AccountLockHash_By_Address()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToArray(@"Testdata\\Lock\\GetAccountLockHashByAddress.json");

                httpTest.RespondWithJson(fakeJson);

                const string rawAddress = "VAV7H3GWUP6INBEJS5QRQXCJS3GA4VGPOA4BIO6O";
                var address = Address.CreateFromRawAddress(rawAddress);
                var LockHash = await _lockHttp.GetAccountLockHash(address);
                LockHash.Should().NotBeNull();
                LockHash.Select(info => Address.CreateFromHex(info.Lock.AccountAddress).Should().Equals(rawAddress));
                //LockHash.Address.Plain.Should().BeEquivalentTo(rawAddress);
            }
        }

        [Fact]
        public async Task Get_AccountLockHash_By_Hash()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToObject(@"Testdata\\Lock\\GetAccountLockHashByHash.json");

                httpTest.RespondWithJson(fakeJson);
                const string rawAddress = "VAV7H3GWUP6INBEJS5QRQXCJS3GA4VGPOA4BIO6O";
                const string hash = "D4E21A3F512188768E411E9A132C7CA4EF80FB1F0E6BDD789891ABF1DD2C659B";
                var LockHash = await _lockHttp.GetLockHash(hash);
                LockHash.Should().NotBeNull();
                Address.CreateFromHex(LockHash.Lock.AccountAddress).Should().Equals(rawAddress);
            }
        }
    }
}