using FluentAssertions;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ProximaX.Sirius.Chain.Sdk.Tests.Models
{
    public class BlockchainVersionTests
    {
        [Fact]
        public void Should_Create_BlockchainVersion()
        {
            BlockchainVersion ver = new BlockchainVersion(1, 2, 3, 4);

            ver.Major.Should().Be(1);
            ver.Minor.Should().Be(2);
            ver.Revision.Should().Be(3);
            ver.Build.Should().Be(4);

            var expectedValue = 281483566841860UL;
            var version1 = BlockchainVersion.FromVersionValue(expectedValue);
            var version = ver.GetVersionValue();
            version.Should().Be(expectedValue);


        }
    }
}
