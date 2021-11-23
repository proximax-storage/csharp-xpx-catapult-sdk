using FluentAssertions;
using Flurl.Http.Testing;
using ProximaX.Sirius.Chain.Sdk.Infrastructure;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions.Messages;
using ProximaX.Sirius.Chain.Sdk.Tests.Utils;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ProximaX.Sirius.Chain.Sdk.Tests.Infrastructure
{
    public class BlockHttpTests : BaseTest
    {
        private readonly BlockHttp _blockchainHttp;

        public BlockHttpTests()
        {
            _blockchainHttp = new BlockHttp(BaseUrl) { NetworkType = NetworkType.TEST_NET };
        }

        [Fact]
        public async Task Should_Return_BlockInfo_By_Height()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToObject(@"Testdata\\Block\\BlockByHeight.json");

                httpTest.RespondWithJson(fakeJson);

                var blockInfo = await _blockchainHttp.GetBlockByHeight(1);

                blockInfo.Hash.Should().BeEquivalentTo("1A94C1BA04CF1217883F2E349AEE75D691BA1117C1143D8616E4D3AE4C278696");
                blockInfo.GenerationHash.Should().BeEquivalentTo("943CC0B70D0ACB0A1C1C6809055719BC170AFDD29F22930266E6ED70ECF6B13B");
                blockInfo.TotalFee.Should().Be(0);
                blockInfo.PreviousBlockHash.Should().BeEquivalentTo("AC87FDA8FD94B72F3D0790A7D62F248111BD5E37B95B16E4216DA99C212530A5");
                blockInfo.BlockTransactionsHash.Should().BeEquivalentTo("0000000000000000000000000000000000000000000000000000000000000000");
                blockInfo.BlockReceiptsHash.Should().BeEquivalentTo("F6C9D6BF16DA02372AE28B9C5C94A355A2C32F32616764FE007A7FCBBE58BDE9");
                blockInfo.BlockStateHash.Should().BeEquivalentTo("A114AAADE77B2EFBC8F5EB6876311F4C6920C9EECA099495BA3B9B0BB4B8AE47");
                blockInfo.NetworkType.Should().Be(NetworkType.TEST_NET);
            }
        }

        [Fact]
        public async Task Should_Return_The_Generation_Hash()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToObject(@"Testdata\\Block\\BlockByHeightGenerationHash.json");

                httpTest.RespondWithJson(fakeJson);

                var generationHash = await _blockchainHttp.GetGenerationHash();
                generationHash.Should().BeEquivalentTo("B750FC8ADD9FAB8C71F0BB90B6409C66946844F07C5CADB51F27A9FAF219BFC7");
            }
        }

        [Fact]
        public async Task Should_Return_The_Receipt_From_Block()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToObject(@"Testdata\\Block\\GetReceiptFromBlock.json");

                httpTest.RespondWithJson(fakeJson);

                var receipts = await _blockchainHttp.GetBlockReceipts(2);
                receipts.Should().NotBeNull();
                //receipts.MosaicResolutionStatements.Should().HaveCountGreaterThan(0);
                //receipts.TransactionStatement.Should().HaveCountGreaterThan(0);
            }
        }

        [Fact]
        public async Task Get_Block_By_Height_With_Limit()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                   TestHelper.LoadJsonFileToArray(@"Testdata\\Block\\BlockByHeightWithLimit.json");
                httpTest.RespondWithJson(fakeJson);
                var blockHeight = await _blockchainHttp.GetBlockByHeightWithLimit(7, BlocksLimit.LIMIT_25);
                blockHeight.Should().NotBeNullOrEmpty();
            }
        }
    }
}