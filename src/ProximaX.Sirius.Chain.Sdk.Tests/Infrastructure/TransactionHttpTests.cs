using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http.Testing;
using ProximaX.Sirius.Chain.Sdk.Infrastructure;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions.Messages;
using ProximaX.Sirius.Chain.Sdk.Tests.Utils;
using Xunit;

namespace ProximaX.Sirius.Chain.Sdk.Tests.Infrastructure
{
    public class TransactionHttpTests : BaseTest
    {
        private readonly TransactionHttp _transactionHttp;

        public TransactionHttpTests()
        {
            _transactionHttp = new TransactionHttp(BaseUrl) { NetworkType = NetworkType.MIJIN_TEST };
        }

        [Fact]
        public async Task Should_Return_Transfer_Transaction_Details()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson = TestHelper.LoadJsonFileToObject(@"Testdata\\Transactions\\TransferTransactionWithPlainMessage.json");

                httpTest.RespondWithJson(fakeJson);

                const string transactionHash = "D88C48782537108AE069225CF07F81122E068A60E5825B1C6E57373B0FC2437A";

                var transaction = await _transactionHttp.GetTransaction(transactionHash);

                var agTx = (TransferTransaction)transaction;

                transaction.Should().BeOfType<TransferTransaction>();
            }
        }

        [Fact]
        public async Task Get_Transaction_By_Hash_Should_Return_Aggregate_Transaction_Plain_Message()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson = TestHelper.LoadJsonFileToObject(@"Testdata\\Transactions\\AggreateTransactionWithPlainMessage.json");

                httpTest.RespondWithJson(fakeJson);

                const string transactionHash = "0E91936DFEF1DC659B1EA4F32F1CD8EE4AFADF804B812E3966245FD0888CDD74";

                var transaction = await _transactionHttp.GetTransaction(transactionHash);
                var agTx = (AggregateTransaction)transaction;

                transaction.Should().BeOfType<AggregateTransaction>();
            }
        }


        [Fact]
        public async Task Get_Transaction_By_Hash_Should_Return_Aggregate_Transaction()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson = TestHelper.LoadJsonFileToObject(@"Testdata\\Transactions\\CreateMosaicAggregateTransaction.json");

                httpTest.RespondWithJson(fakeJson);

                const string transactionHash = "60CBC50B2084F5ED5556A4D8F24CE63C4EEAEFF164CF3415A98C7569C5999A5D";

                var transaction = await _transactionHttp.GetTransaction(transactionHash);
                var agTx = (AggregateTransaction) transaction;
           
                transaction.Should().BeOfType<AggregateTransaction>();
            }
        }

        [Fact]
        public async Task Get_Transaction_By_Hash_Should_Return_MosaicSupplyChange_Transaction()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson = TestHelper.LoadJsonFileToObject(@"Testdata\\Transactions\\DecreaseMosaicSupplyTransaction.json");

                httpTest.RespondWithJson(fakeJson);

                const string transactionHash = "EE59F16783514F00C5B0E6F46E528BDE8170DD27D38C54DB2A82815D1407A52C";

                var transaction = await _transactionHttp.GetTransaction(transactionHash);
                
                transaction.Should().BeOfType<MosaicSupplyChangeTransaction>();
            }
        }

        [Fact]
        public async Task Get_Transaction_By_Hash_Should_Return_AliasTransaction_Link_Namespace_To_Mosaic()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson = TestHelper.LoadJsonFileToObject(@"Testdata\\Transactions\\LinkNamespaceToMosaic.json");

                httpTest.RespondWithJson(fakeJson);

                const string transactionHash = "4B34BE7C58DC23A6C75CB38F18AB4C3749FCDA68D9B686975996606398EDDFF8";

                var transaction = await _transactionHttp.GetTransaction(transactionHash);

                transaction.Should().BeOfType<AliasTransaction>();
                transaction.TransactionType.Should().BeEquivalentTo(EntityType.MOSAIC_ALIAS);
                ((AliasTransaction)transaction).MosaicId.Should().NotBeNull();
            }
        }


        [Fact]
        public async Task Get_Transaction_By_Hash_Should_Return_AliasTransaction_Link_Namespace_To_Address()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson = TestHelper.LoadJsonFileToObject(@"Testdata\\Transactions\\LinkNamespaceToAddress.json");

                httpTest.RespondWithJson(fakeJson);

                const string transactionHash = "A8472B76CE1C0D052F75C294E7194C16A73AA618750EE7FCEB059FB4AE10540B";

                var transaction = await _transactionHttp.GetTransaction(transactionHash);

                transaction.Should().BeOfType<AliasTransaction>();
                transaction.TransactionType.Should().BeEquivalentTo(EntityType.ADDRESS_ALIAS);
                ((AliasTransaction) transaction).Address.Should().NotBeNull();
            }
        }

        [Fact]
        public async Task Get_Transfer_Transaction_With_Secure_Message()
        {
            var transactionHttp = new TransactionHttp(BaseUrl) { NetworkType = NetworkType.MIJIN_TEST };

            using (var httpTest = new HttpTest())
            {
                var fakeJson = TestHelper.LoadJsonFileToObject(@"Testdata\\Transactions\TransferTransactionWithSecureMessage.json");

                httpTest.RespondWithJson(fakeJson);

                const string transactionHash = "1C296D86E85C0C80981FCD8302A96E41D9229316E1EE329E7BFECC4BF0282120";
                const string receiverPrivateKey = "EA947AB9CA50C31CFCD60B2C172173EAA7B0C56B173DDF235078A9A59AD218C5";
                const string senderPublicKey = "D03918E35573C66578B5A0EED723FE2A46208783E13498751D9315115CA06D4B";
                var transaction = await transactionHttp.GetTransaction(transactionHash);

                transaction.Should().BeOfType<TransferTransaction>();
                var transferTransaction = ((TransferTransaction)transaction);
                var messageType = MessageTypeExtension.GetRawValue(transferTransaction.Message.GetMessageType());
                messageType.Should().BeEquivalentTo(MessageType.SECURED_MESSAGE);

                var securedMessage = (SecureMessage)transferTransaction.Message;
                var payload = securedMessage.DecryptPayload(receiverPrivateKey, senderPublicKey);
                payload.Should().BeEquivalentTo("Test secure message");
                
            }
        }
    }
}
