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
 
    public class ChainHttpTests: BaseTest
    {
        private readonly ChainHttp _chainHttp;

        public ChainHttpTests()
        {
            _chainHttp = new ChainHttp(BaseUrl);
        }
      


        [Fact]
        public async Task Get_Chain_Score()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToObject(@"Testdata\\Chain\\BlockScore.json");

                httpTest.RespondWithJson(fakeJson);

                var score = await _chainHttp.GetBlockScore();

                score.Should().BeGreaterThan(0);
            }
        }


        [Fact]
        public async Task Get_Chain_Height()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToObject(@"Testdata\\Chain\\BlockHeight.json");

                httpTest.RespondWithJson(fakeJson);

                var height = await _chainHttp.GetBlockHeight();

                height.Should().BeGreaterThan(0);
            }
        }
    }

  
}
