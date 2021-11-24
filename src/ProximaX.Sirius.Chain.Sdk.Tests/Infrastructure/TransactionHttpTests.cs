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
using Xunit.Abstractions;

namespace ProximaX.Sirius.Chain.Sdk.Tests.Infrastructure
{
    public class TransactionHttpTests : BaseTest
    {
        private readonly ITestOutputHelper Log;

        private readonly _transactionHttp _transactionHttp;

        public TransactionHttpTests(ITestOutputHelper log)
        {
            Log = log;

            _transactionHttp = new _transactionHttp(BaseUrl) { NetworkType = NetworkType.TEST_NET };
        }

        [Fact]
        public async Task Should_Return_Transfer_Transaction_Details()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson = TestHelper.LoadJsonFileToObject(@"Testdata\\Transactions\\TransferTransactionWithPlainMessage.json");

                httpTest.RespondWithJson(fakeJson);

                const string transactionHash = "D9171BBD172856C80AD0521482D898EF80459643236C0AD7147A5301ACC650CC";

                var transaction = await _transactionHttp.GetTransaction(transactionHash);

                var agTx = (TransferTransaction)transaction;
                // transaction.Should().BeNull();
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

                const string transactionHash = "FDDD64488CA70664203A053D2A7729E8B85B2830598BB46060F1B337E7F14C42";

                var transaction = await _transactionHttp.GetTransaction(transactionHash);
                var agTx = (AggregateTransaction)transaction;
                // transaction.Should().BeNull();

                transaction.Should().BeOfType<AggregateTransaction>();

                Log.WriteLine($"announce Transaction: { transaction}");
            }
        }

        [Fact]
        public async Task Get_Transaction_By_Hash_Should_Return_Aggregate_Transaction()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson = TestHelper.LoadJsonFileToObject(@"Testdata\\Transactions\\CreateMosaicAggregateTransaction.json");

                httpTest.RespondWithJson(fakeJson);

                const string transactionHash = "B6ABAA040109C62E45D5F4664650C438CD8360B3366D41F9D83FCD1B0BDD148E";

                var transaction = await _transactionHttp.GetTransaction(transactionHash);
                var agTx = (AggregateTransaction)transaction;
                // transaction.Should().BeNull();

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

                const string transactionHash = "AEFF3A7068163C311CE92E4D76976C9B3DBDCA9E0787264A10585EA32133B2F7";

                var transaction = await _transactionHttp.GetTransaction(transactionHash);
                //transaction.Should().BeNull();

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

                const string transactionHash = "04BEBF85816911E0DFEAA673B6A4141B28B8BC783A994BE074C7E65C9F98DECA";

                var transaction = await _transactionHttp.GetTransaction(transactionHash);
                //transaction.Should().BeNull();

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

                const string transactionHash = "A7F2EE68FB35E72A920363975CE74A4B0C152E1921D2E4AB7E33E0753251FDD8";

                var transaction = await _transactionHttp.GetTransaction(transactionHash);
                //transaction.Should().NotBeNull();

                transaction.Should().BeOfType<AliasTransaction>();
                transaction.TransactionType.Should().BeEquivalentTo(EntityType.ADDRESS_ALIAS);
                ((AliasTransaction)transaction).Address.Should().NotBeNull();
            }
        }

        [Fact]
        public async Task Get_Transfer_Transaction_With_Secure_Message()
        {
            var transactionHttp = new _transactionHttp(BaseUrl) { NetworkType = NetworkType.TEST_NET };

            using (var httpTest = new HttpTest())
            {
                var fakeJson = TestHelper.LoadJsonFileToObject(@"Testdata\\Transactions\TransferTransactionWithSecureMessage.json");

                httpTest.RespondWithJson(fakeJson);

                const string transactionHash = "C517CE2E84289A7BD6BF211F0288BD4462C3B4E69DFB1183E1EFCD5F262C46E5";
                const string receiverPrivateKey = "60ab183da625b86aff8d48b2eda22275cff31070b5e80f28663582fdcacf7425";
                const string senderPublicKey = "F06FE22FBA1E116B8F0E673BA4EE424B16BD6EA7548ED259F3DCEBF8D74C49B9";
                var transaction = await transactionHttp.GetTransaction(transactionHash);
                // transaction.Should().BeNull();

                transaction.Should().BeOfType<TransferTransaction>();
                var transferTransaction = ((TransferTransaction)transaction);
                var messageType = MessageTypeExtension.GetRawValue(transferTransaction.Message.GetMessageType());
                messageType.Should().BeEquivalentTo(MessageType.SECURED_MESSAGE);

                var securedMessage = (SecureMessage)transferTransaction.Message;
                var payload = securedMessage.DecryptPayload(receiverPrivateKey, senderPublicKey);
                payload.Should().BeEquivalentTo("Test secure message");
            }
        }

        [Fact]
        public async Task Get_Embedded_transaction()
        {
            var transactionHttp = new _transactionHttp(BaseUrl) { NetworkType = NetworkType.TEST_NET };

            using (var httpTest = new HttpTest())
            {
                var fakeJson = TestHelper.LoadJsonFileToObject(@"Testdata\\Transactions\TransferTransactionWithSecureMessage.json");

                httpTest.RespondWithJson(fakeJson);

                const string transactionHash = "C517CE2E84289A7BD6BF211F0288BD4462C3B4E69DFB1183E1EFCD5F262C46E5";
                const string receiverPrivateKey = "60ab183da625b86aff8d48b2eda22275cff31070b5e80f28663582fdcacf7425";
                const string senderPublicKey = "F06FE22FBA1E116B8F0E673BA4EE424B16BD6EA7548ED259F3DCEBF8D74C49B9";
                var transaction = await transactionHttp.GetTransaction(transactionHash);
                // transaction.Should().BeNull();

                transaction.Should().BeOfType<TransferTransaction>();
                var transferTransaction = ((TransferTransaction)transaction);
                var messageType = MessageTypeExtension.GetRawValue(transferTransaction.Message.GetMessageType());
                messageType.Should().BeEquivalentTo(MessageType.SECURED_MESSAGE);

                var securedMessage = (SecureMessage)transferTransaction.Message;
                var payload = securedMessage.DecryptPayload(receiverPrivateKey, senderPublicKey);
                payload.Should().BeEquivalentTo("Test secure message");
            }
        }

        [Fact]
        public async Task Get_MosaicLevy_transaction()
        {
            var transactionHttp = new _transactionHttp(BaseUrl) { NetworkType = NetworkType.TEST_NET };

            using (var httpTest = new HttpTest())
            {
                var fakeJson = TestHelper.LoadJsonFileToObject(@"Testdata\\Transactions\\MosaicLevyTransaction.json");

                httpTest.RespondWithJson(fakeJson);

                const string transactionHash = "02D2E02F6162A4CBDAD8703085A2D9DE8E7C938A969071D76D0189858761CD8E";

                var transaction = await _transactionHttp.GetTransaction(transactionHash);
                transaction.Should().NotBeNull();

                transaction.Should().BeOfType<ModifyMosaicLevyTransaction>();
                transaction.TransactionType.Should().BeEquivalentTo(EntityType.MODIFY_MOSAIC_LEVY);
                transaction.Signature.Should().BeEquivalentTo("DD7089233EC1899A2596EF2CB611F8EBDD7338006483EE936D6FC6EAF53EE29FB938BC4FC502C726C91E78B6F2981EDFA135F8811C0395729C1770CBE1D6CA09");
            }
        }

        [Fact]
        public async Task Get_transaction_by_group()
        {
            var transactionHttp = new _transactionHttp(BaseUrl) { NetworkType = NetworkType.TEST_NET };

            using (var httpTest = new HttpTest())
            {
                var fakeJson = TestHelper.LoadJsonFileToObject(@"Testdata\\Transactions\\GetTransactionByGroup.json");

                httpTest.RespondWithJson(fakeJson);

                var transaction = await _transactionHttp.SearchTransactions(TransactionGroupType.confirmed, "04BEBF85816911E0DFEAA673B6A4141B28B8BC783A994BE074C7E65C9F98DECA");
                transaction.Should().NotBeNull();
            }
        }
    }
}