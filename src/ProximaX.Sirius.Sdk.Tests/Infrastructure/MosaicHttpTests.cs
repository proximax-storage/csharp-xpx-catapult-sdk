
using ProximaX.Sirius.Sdk.Infrastructure;
using ProximaX.Sirius.Sdk.Model.Mosaics;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http.Testing;
using ProximaX.Sirius.Sdk.Model.Blockchain;
using ProximaX.Sirius.Sdk.Tests.Utils;
using Xunit;

namespace ProximaX.Sirius.Sdk.Tests.Infrastructure
{

    public class MosaicHttpTests : BaseTest
    {
        private readonly MosaicHttp _mosaicHttp;

        public MosaicHttpTests()
        {

            _mosaicHttp = new MosaicHttp(BaseUrl)
            { NetworkType = NetworkType.MIJIN_TEST };

        }

        [Fact]
        public void Should_Initialize_MosaicHttp_Constructor()
        {
            var mosaicHttpCtor = new MosaicHttp(BaseUrl);
            mosaicHttpCtor.Should().BeOfType<MosaicHttp>();
        }


        [Fact]
        public async Task Should_Get_Mosaic_Info()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToObject(@"Testdata\\Mosaic\\GetMosaicInfoFromHex.json");

                httpTest.RespondWithJson(fakeJson);

                const string mosaicHex = "7D26BCA6BD15E3BF";
                var mosaicId = new MosaicId(mosaicHex);
                var mosaicInfo = await _mosaicHttp.GetMosaic(mosaicId);
                mosaicInfo.Should().NotBeNull();
                mosaicInfo.Divisibility.Should().Be(0);
                mosaicInfo.Duration.Should().Be(1000);
                mosaicInfo.IsLevyMutable.Should().BeFalse();
                mosaicInfo.IsSupplyMutable.Should().BeTrue();
                mosaicInfo.IsTransferable.Should().BeTrue();
                mosaicInfo.Supply.Should().Be(1000000);
            }
        }

        [Fact]
        public async Task Get_Mosaic_By_List_Should_Return_MosaicInfo_List()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToArray(@"Testdata\\Mosaic\\GetMosaicListInfo.json");

                httpTest.RespondWithJson(fakeJson);

                var list = new List<string>
                {
                    "53E98114D41A50CF",
                    "2AE5A69F86199FE0"
                };
                var mosaicInfoList = await _mosaicHttp.GetMosaicListAsync(list);
                mosaicInfoList.Should().HaveCountGreaterThan(0);
            }


        }
    }
}
