﻿using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http.Testing;
using ProximaX.Sirius.Chain.Sdk.Infrastructure;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Tests.Utils;
using Xunit;

namespace ProximaX.Sirius.Chain.Sdk.Tests.Infrastructure
{

    public class NetworkHttpTests : BaseTest
    {
        private readonly NetworkHttp _networkHttp;

        public NetworkHttpTests() : base()
        {
            _networkHttp = new NetworkHttp(BaseUrl) { NetworkType = NetworkType.TEST_NET };
        }

        [Fact]
        public async Task Should_Return_NetworkType()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                  TestHelper.LoadJsonFileToObject(@"Testdata\\Node\\GetNetwork.json");

                httpTest.RespondWithJson(fakeJson);

                // Arrange
                var networkType = await _networkHttp.GetNetworkType();

                // Actual
                var actual = NetworkTypeExtension.GetRawValue("publicTest");

                // Test
                networkType.Should().BeEquivalentTo(actual);
            }
           
        }
        [Fact]
        public async Task Get_Config_Height()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                  TestHelper.LoadJsonFileToObject(@"Testdata\\Network\\GetBlockConfig.json");

                httpTest.RespondWithJson(fakeJson);

                // Arrange
                var config_Height = await _networkHttp.GetBlockConfiguration(1);


                config_Height.Should().NotBeNull();
            
            }

        }
    }
}
