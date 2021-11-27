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
            BlockchainVersion ver = new BlockchainVersion(6, 7, 0, 0);

             ver.Major.Should().Be(6);
             ver.Minor.Should().Be(7);
             ver.Revision.Should().Be(0);
             ver.Build.Should().Be(0);

            var expectedValue = 1688879925035008UL;
            var version = ver.GetVersionValue();
            version.Should().Be(expectedValue);
        }

        [Fact]
        public void Should_Create_BlockchainVersion_Range_Zero()
        {
            BlockchainVersion ver = new BlockchainVersion(0, 0, 0, 0);

            ver.Major.Should().Be(0);
            ver.Minor.Should().Be(0);
            ver.Revision.Should().Be(0);
            ver.Build.Should().Be(0);

            var expectedValue = 0UL;
            var version = ver.GetVersionValue();
            version.Should().Be(expectedValue);
        }

        [Fact]
        public void Should_Create_BlockchainVersion_Range_Max()
        {
            BlockchainVersion ver = new BlockchainVersion(65535, 65535, 65535, 65535);

            ver.Major.Should().Be(65535);
            ver.Minor.Should().Be(65535);
            ver.Revision.Should().Be(65535);
            ver.Build.Should().Be(65535);

            var expectedValue = 18446744073709551615UL;
            var version = ver.GetVersionValue();
            version.Should().Be(expectedValue);
        }

       
    }
}
