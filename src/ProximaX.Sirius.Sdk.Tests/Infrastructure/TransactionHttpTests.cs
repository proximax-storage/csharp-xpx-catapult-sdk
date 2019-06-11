using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http.Testing;
using ProximaX.Sirius.Sdk.Infrastructure;
using ProximaX.Sirius.Sdk.Model.Transactions;
using ProximaX.Sirius.Sdk.Tests.Utils;
using Xunit;

namespace ProximaX.Sirius.Sdk.Tests.Infrastructure
{
    public class TransactionHttpTests : BaseTest
    {
        private readonly TransactionHttp _transactionHttp;

        public TransactionHttpTests()
        {
            _transactionHttp = new TransactionHttp(BaseUrl);
        }

        [Fact]
        public async Task Get_Transaction_By_Hash_Should_Return_Aggregate_Transaction()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson = TestHelper.LoadJsonFileToObject(@"Testdata\\CreateMosaicAggregateTransaction.json");

                httpTest.RespondWithJson(fakeJson);

                const string transactionHash = "72CA2DA47931D995887592E3E9A93719DED92A12452655F1E88811CD2602ADCE";

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
                var fakeJson = TestHelper.LoadJsonFileToObject(@"Testdata\\DecreaseMosaicSupplyTransaction.json");

                httpTest.RespondWithJson(fakeJson);

                const string transactionHash = "549E6C78D2708CC6BA14DEB6D7F313A4E130A4655C2DCCD723EED4B6ECAA81AF";

                var transaction = await _transactionHttp.GetTransaction(transactionHash);
                
                transaction.Should().BeOfType<MosaicSupplyChangeTransaction>();
            }
        }

        [Fact]
        public async Task Get_Transaction_By_Hash_Should_Return_AliasTransaction_Link_Namespace_To_Mosaic()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson = TestHelper.LoadJsonFileToObject(@"Testdata\\LinkNamespaceToMosaic.json");

                httpTest.RespondWithJson(fakeJson);

                const string transactionHash = "EE038F41EDE24587C98E234675026DECA6625F77DA818F9FEA3BE56BCEB073E6";

                var transaction = await _transactionHttp.GetTransaction(transactionHash);

                transaction.Should().BeOfType<AliasTransaction>();
                transaction.TransactionType.Should().BeEquivalentTo(TransactionType.MOSAIC_ALIAS);
                ((AliasTransaction)transaction).MosaicId.Should().NotBeNull();
            }
        }


        [Fact]
        public async Task Get_Transaction_By_Hash_Should_Return_AliasTransaction_Link_Namespace_To_Address()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson = TestHelper.LoadJsonFileToObject(@"Testdata\\LinkNamespaceToAddress.json");

                httpTest.RespondWithJson(fakeJson);

                const string transactionHash = "79463E18A0F91B9D191E058037B0AF3A3B9003F9BA1B94B6A4A8CFE6AADE7C27";

                var transaction = await _transactionHttp.GetTransaction(transactionHash);

                transaction.Should().BeOfType<AliasTransaction>();
                transaction.TransactionType.Should().BeEquivalentTo(TransactionType.ADDRESS_ALIAS);
                ((AliasTransaction) transaction).Address.Should().NotBeNull();
            }
        }
    }
}
