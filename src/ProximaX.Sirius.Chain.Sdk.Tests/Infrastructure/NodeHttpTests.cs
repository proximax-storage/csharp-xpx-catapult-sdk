using FluentAssertions;
using Flurl.Http.Testing;
using ProximaX.Sirius.Chain.Sdk.Infrastructure;
using ProximaX.Sirius.Chain.Sdk.Model.Node;
using ProximaX.Sirius.Chain.Sdk.Tests.Utils;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ProximaX.Sirius.Chain.Sdk.Tests.Infrastructure
{
    public class NodeHttpTests : BaseTest
    {
        private readonly NodeHttp _nodeHttp;

        public NodeHttpTests() : base()
        {
            _nodeHttp = new NodeHttp(BaseUrl);
        }


        [Fact]
        public async Task Should_Return_NodeTime()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToObject(@"Testdata\\Node\\GetNodeTime.json");

                // Fake response
                httpTest.RespondWithJson(fakeJson);

                // Arrange
                var nodeTime = await _nodeHttp.GetNodeTime();

                // Test
                nodeTime.SendTimestamp.Should().BeGreaterThan(0);
                nodeTime.ReceiveTimestamp.Should().BeGreaterThan(0);
            }
        }

        [Fact]
        public async Task Should_Return_NodeInfo()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToObject(@"Testdata\\Node\\GetNodeInfo.json");

                // Fake response
                httpTest.RespondWithJson(fakeJson);

                // Arrange
                var nodeInfo = await _nodeHttp.GetNodeInfo();

                // Test
                nodeInfo.PublicKey.Should().BeEquivalentTo("DEE6E5B71F1555D9D62E8A3ED460594606525B6A2AF9EA66EF1027CB99D299EF");
                nodeInfo.Port.Should().Be(7900);
                nodeInfo.NetworkIdentifier.Should().Be(168);
                nodeInfo.Version.Should().Be(0);
                nodeInfo.Roles.Should().Be(RoleType.ApiNode);
                nodeInfo.FriendlyName.Should().BeEquivalentTo("stg-api-1");
                nodeInfo.Host.Should().BeEquivalentTo("10.24.8.150");
            }
        }

    }
}
