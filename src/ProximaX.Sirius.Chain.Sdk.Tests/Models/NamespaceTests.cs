using FluentAssertions;
using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Model.Namespaces;
using ProximaX.Sirius.Chain.Sdk.Utils;
using Xunit;

namespace ProximaX.Sirius.Chain.Sdk.Tests.Models
{
    public class NamespaceTests
    {
        [Fact]
        public void Should_Create_Namespace_From_UInt64DTO()
        {
            var id = new UInt64DTO
            {
                388731997,2432469584
            };

            var namespaceId = new NamespaceId(id.ToUInt64());
            
            namespaceId.HexId.Should().BeEquivalentTo("90FC8A50172B945D");
        }

        [Fact]
        public void Should_Create_Namespace_From_Name()
        {
            const string namespaceName = "nspeeccbe.subnsaf3628";
            var namespaceId = new NamespaceId(namespaceName);
            namespaceId.HexId.Should().BeEquivalentTo("9E088FD2585C3518");
        }

    }
}
