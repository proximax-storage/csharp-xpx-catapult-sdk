using FluentAssertions;
using Flurl.Http.Testing;
using ProximaX.Sirius.Chain.Sdk.Infrastructure;
using ProximaX.Sirius.Chain.Sdk.Tests.Utils;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ProximaX.Sirius.Chain.Sdk.Tests.Infrastructure
{
    public class BlockHttpTests: BaseTest
    {
        private readonly BlockHttp _blockchainHttp;

        public BlockHttpTests()
        {
            _blockchainHttp = new BlockHttp(BaseUrl);
        }

        [Fact]
        public async Task Get_Blockchain_Score()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToObject(@"Testdata\\Block\\BlockScore.json");

                httpTest.RespondWithJson(fakeJson);

                var score = await _blockchainHttp.GetBlockScore();

                score.Should().BeGreaterThan(0);

            }
        }
    }
}
