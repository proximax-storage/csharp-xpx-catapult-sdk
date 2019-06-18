using System;
using System.Collections.Generic;
using System.Text;
using FluentAssertions;
using ProximaX.Sirius.Chain.Sdk.Model.Namespaces;
using ProximaX.Sirius.Chain.Sdk.Utils;
using Xunit;

namespace ProximaX.Sirius.Chain.Sdk.Tests.Crypto
{

    public class ConverterTests
    {
        [Fact]
        public void Should_Convert_To_Namespace_Alias()
        {

            var namespaceId = new NamespaceId("foo");
            var byteArray = namespaceId.AliasToRecipient();
            byteArray.Length.Should().Be(25);
        }
    }
}
