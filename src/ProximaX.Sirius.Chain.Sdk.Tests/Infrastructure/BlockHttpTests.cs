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
    public class BlockHttpTests: BaseTest
    {
        private readonly BlockHttp _blockchainHttp;

        public BlockHttpTests()
        {
            
            _blockchainHttp = new BlockHttp(BaseUrl) { NetworkType = NetworkType.MIJIN_TEST };


        }

        [Fact]
        public async Task Should_Return_BlockInfo_By_Height()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToObject(@"Testdata\\Block\\BlockByHeight.json");

                httpTest.RespondWithJson(fakeJson);

                var blockInfo = await _blockchainHttp.GetBlockByHeight(12423);

                blockInfo.Hash.Should().BeEquivalentTo("0C88E34B34CDBDF2209E7CB568F9D1A9FBF47C566B71496758A6C76652AD3974");
                blockInfo.GenerationHash.Should().BeEquivalentTo("7C175E628183275364959BEA811897111C5AFCA014DD991FC0D87AD7C16BB41F");
                blockInfo.TotalFee.Should().Be(0);
                blockInfo.PreviousBlockHash.Should().BeEquivalentTo("22E94FC588AE3A05003FD887BF3E22B8D1EF9645EDD71EE75C7C74CD11B43B57");
                blockInfo.BlockTransactionsHash.Should().BeEquivalentTo("0000000000000000000000000000000000000000000000000000000000000000");
                blockInfo.BlockReceiptsHash.Should().BeEquivalentTo("4B92740450C7FB9027AFF3E38625274211A166A820CF41E796F6808C52968F4C");
                blockInfo.Version.Should().Be(3);
                blockInfo.NetworkType.Should().Be(NetworkType.MIJIN_TEST);
            }
        }


        [Fact]
        public async Task Should_Return_Block_Transactions()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToArray(@"Testdata\\Block\\GetBlockTransaction.json");

                httpTest.RespondWithJson(fakeJson);

                var transactions = await _blockchainHttp.GetBlockTransactions(7279);
                transactions.Should().HaveCountGreaterThan(0);
                transactions[0].TransactionType.Should().Be(TransactionType.MOSAIC_ALIAS);
                transactions[0].TransactionInfo.Hash.Should().BeEquivalentTo("4B34BE7C58DC23A6C75CB38F18AB4C3749FCDA68D9B686975996606398EDDFF8");
                transactions[0].NetworkType.Should().Be(NetworkType.MIJIN_TEST);
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
                generationHash.Should().BeEquivalentTo("7C175E628183275364959BEA811897111C5AFCA014DD991FC0D87AD7C16BB41F");
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

                var receipts = await _blockchainHttp.GetBlockReceipts(2830);
                receipts.MosaicResolutionStatements.Should().HaveCountGreaterThan(0);
                receipts.TransactionStatement.Should().HaveCountGreaterThan(0);
            }
        }
    }
}
