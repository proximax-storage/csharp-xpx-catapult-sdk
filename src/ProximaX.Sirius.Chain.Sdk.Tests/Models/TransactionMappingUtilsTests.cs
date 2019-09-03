using FluentAssertions;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Utils;

using Xunit;

namespace ProximaX.Sirius.Chain.Sdk.Tests.Models
{
    public class TransactionMappingUtilsTests
    {
        [Fact]
        public void Should_Extract_TransactionVersion_From_Version()
        {
            var transactionVersion = -1879048189;

            var version = TransactionMappingUtils.ExtractTransactionVersion(transactionVersion);

            version.Should().Be(3);
        }

        [Fact]
        public void Should_Extract_NetworkType_From_Version()
        {
            var transactionVersion = -1879048189;

            var networkType = TransactionMappingUtils.ExtractNetworkType(transactionVersion);

            networkType.Should().Be(NetworkType.MIJIN_TEST);
        }
    }
}
