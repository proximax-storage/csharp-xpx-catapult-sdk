using ProximaX.Sirius.Chain.Sdk.Infrastructure;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http.Testing;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Tests.Utils;
using Xunit;
using System;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using Xunit.Abstractions;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;

namespace ProximaX.Sirius.Chain.Sdk.Tests.Infrastructure
{
    public class MosaicHttpTests : BaseTest
    {
        private readonly ITestOutputHelper Log;

        private readonly MosaicHttp _mosaicHttp;

        public MosaicHttpTests(ITestOutputHelper log)
        {
            Log = log;
            _mosaicHttp = new MosaicHttp(BaseUrl)
            { NetworkType = NetworkType.TEST_NET };
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

                const string mosaicHex = "037C5AF6052A9F7D";
                var mosaicId = new MosaicId(mosaicHex);
                var mosaicInfo = await _mosaicHttp.GetMosaic(mosaicId);
                mosaicInfo.Should().NotBeNull();
                mosaicInfo.MetaId.Should().Equals("611B3B866E4BF54EE822308B");
            }
        }

        [Fact]
        public async Task Should_Get_Mosaic_Richlist()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToArray(@"Testdata\\Mosaic\\GetMosaicRichList.json");

                httpTest.RespondWithJson(fakeJson);

                const string mosaicHex = "1b7c19d23872520c";
                var mosaicId = new MosaicId(mosaicHex);
                var mosaicInfo = await _mosaicHttp.GetMosaicRichlist(mosaicId);
                var address1 = Address.CreateFromHex("A8998D6DF14E095266CB6E36613F037616C72F224DA95C2BF4");
                var address2 = Address.CreateFromHex("A82A4F7D4608AECF685B992B4EFFC9759845CC657AF8A8482E");

                mosaicInfo.Should().NotBeNullOrEmpty();
                mosaicInfo[0].Address.Should().BeEquivalentTo(address1);
                // mosaicInfo[0].PublicKey.Should().Equals("D9A659A3AA42FD62BE88E1D96B0F10EB91F6097F8D24EC8FD7C94EC6455735EC");
                mosaicInfo[1].Address.Should().BeEquivalentTo(address2);
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
                    "037C5AF6052A9F7D",
                    "34B40B8AD0CEE3F3"
                };
                var mosaicInfoList = await _mosaicHttp.GetMosaicListAsync(list);
                mosaicInfoList[0].MetaId.Should().Equals("611B3B866E4BF54EE822308B");
                mosaicInfoList[1].MetaId.Should().Equals("611B3B866E4BF54EE822308A");
            }
        }

        [Fact]
        public async Task Get_Mosaic_Names()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToArray(@"Testdata\\Mosaic\\GetMosaicNames.json");

                httpTest.RespondWithJson(fakeJson);

                var list = new List<string>
                {
                    "037C5AF6052A9F7D",
                    "34B40B8AD0CEE3F3",
                };
                var mosaicNames = await _mosaicHttp.GetMosaicNames(list);
                mosaicNames.Should().NotBeNullOrEmpty();
                mosaicNames[0].Names.Should().Equals("prx.so");
                mosaicNames[1].Names.Should().Equals("prx.sm");
            }
        }

        [Fact]
        public async Task Get_Mosaic_Levy()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToObject(@"Testdata\\Mosaic\\GetMosaicLevyInfo.json");

                httpTest.RespondWithJson(fakeJson);

                const string mosaicHex = "5AFBB2258FA666AF";
                var mosaicId = new MosaicId(mosaicHex);

                var mosaicLevyInfo = await _mosaicHttp.GetMosaicLevyInfo(mosaicId);
                mosaicLevyInfo.Should().NotBeNull();
                mosaicLevyInfo.Recipent.Address.Plain.Should().BeEquivalentTo("A88167455099E7676758B38BD8282B2FEC00416C1F4AA6906A");
            }
        }
    }
}