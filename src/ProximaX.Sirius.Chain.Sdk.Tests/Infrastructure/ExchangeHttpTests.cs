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
    public class ExchangeHttpTests : BaseTest
    {
        private readonly ExchangeHttp _exchangeHttp;

        public ExchangeHttpTests()
        {
            _exchangeHttp = new ExchangeHttp(BaseUrl) { NetworkType = NetworkType.TEST_NET };
        }

        [Fact]
        public async Task Get_Account_Exchange_Offers_by_PublicKey()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToObject(@"Testdata\\Exchange\\GetAccountExchange.json");

                httpTest.RespondWithJson(fakeJson);

                var publickey = "D9A659A3AA42FD62BE88E1D96B0F10EB91F6097F8D24EC8FD7C94EC6455735EC";
                var publicAccount = PublicAccount.CreateFromPublicKey(publickey, NetworkType.TEST_NET);
                //const PublicAccount address = AccountIds(publickey, NetworkType.TEST_NET);
                // PublicAccount account = PublicAccount.CreateFromPublicKey(publickey, NetworkType.TEST_NET);

                var accountInfo = await _exchangeHttp.getAccountExchanges(publicAccount);
                accountInfo.Should().NotBeNull();
                accountInfo.Owner.Should().Equals(publickey);
            }
        }

        [Fact]
        public async Task Get_Account_Exchange_Offers_by_Address()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToObject(@"Testdata\\Exchange\\GetAccountExchange.json");

                httpTest.RespondWithJson(fakeJson);

                var Raw_address = "VCMY23PRJYEVEZWLNY3GCPYDOYLMOLZCJWUVYK7U";
                var address = Address.CreateFromRawAddress(Raw_address);
                //const PublicAccount address = AccountIds(publickey, NetworkType.TEST_NET);
                // PublicAccount account = PublicAccount.CreateFromPublicKey(publickey, NetworkType.TEST_NET);

                var accountInfo = await _exchangeHttp.getAccountExchanges(address);
                accountInfo.Should().NotBeNull();
                accountInfo.OwnerAddress.Equals(address);
            }
        }

        [Fact]
        public async Task Get_Exchange_Mosaic()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToArray(@"Testdata\\Exchange\\GetExchangeMosaic.json");

                httpTest.RespondWithJson(fakeJson);

                var accountInfo = await _exchangeHttp.GetExchangeMosaic();
                accountInfo.Should().NotBeNull();
            }
        }

        [Fact]
        public async Task Get_Exchange_Offers()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToArray(@"Testdata\\Exchange\\GetExchangeOffers.json");

                httpTest.RespondWithJson(fakeJson);

                var accountInfo = await _exchangeHttp.getExchangeOffers("sell", "037c5af6052a9f7d");
                accountInfo.Should().NotBeNull();
            }
        }
    }
}