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
                nodeInfo.PublicKey.Should().BeEquivalentTo("460458B98E2BAA36A8E95DE9B320379E89898885B71CF0174E02F1324FAFFAC1");
                nodeInfo.Port.Should().Be(7900);
                nodeInfo.NetworkIdentifier.Should().Be(144);
                nodeInfo.Version.Should().Be(0);
                nodeInfo.Roles.Should().Be(RoleType.ApiNode);
                nodeInfo.FriendlyName.Should().BeEquivalentTo("api-node-0");
                nodeInfo.Host.Should().BeEquivalentTo("catapult-api-node");
            }
        }

    }
}
