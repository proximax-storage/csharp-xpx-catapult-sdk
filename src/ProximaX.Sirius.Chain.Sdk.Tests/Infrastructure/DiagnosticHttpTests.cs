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
    public class DiagnosticHttpTests : BaseTest
    {
        private readonly DiagnosticHttp _diagnosticHttp;

        public DiagnosticHttpTests()
        {
            _diagnosticHttp = new DiagnosticHttp(BaseUrl);
        }

        [Fact]
        public async Task Get_Diagnostic_ServerInfo()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToObject(@"TestData\\Diagnostic\\GetDiagnosticServer.json");

                httpTest.RespondWithJson(fakeJson);

                var storage = await _diagnosticHttp.GetBlockStorage();
                storage.Should().NotBeNull();
                //   storage.SdkVersion.Should().BeNullOrEmpty();
                //  storage.RestVersion.Should().BeNullOrEmpty();
            }
        }

        /*  [Fact]
          public async Task Get_Block_Storage()
          {
              using (var httpTest = new HttpTest())
              {
                  var fakeJson =
                      TestHelper.LoadJsonFileToObject(@"Testdata\\Block\\GetBlockStorage.json");

                  httpTest.RespondWithJson(fakeJson);

                  var blockInfo = await _diagnosticHttp.GetBlockStorage();

                  blockInfo.NumBlocks.Equals(169582);
                  blockInfo.NumTransactions.Equals(16761);
                  blockInfo.NumAccounts.Equals(5422);
                  blockInfo.Should().NotBeNull();
              }
          }*/
    }
}