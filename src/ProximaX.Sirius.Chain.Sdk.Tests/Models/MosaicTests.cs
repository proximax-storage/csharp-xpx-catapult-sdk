using FluentAssertions;
using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Utils;
using Xunit;

namespace ProximaX.Sirius.Chain.Sdk.Tests.Models
{
    public class MosaicTests
    {
        private const string PublicKey = "b4f12e7c9f6946091e2cb8b6d3a12b50d17ccbbf646386ea27ce2946a7423dcf";

        [Fact]
        public void Should_Create_MosaicId_UInt64DTO()
        {

            var id = new UInt64DTO { 3294802500, 2243684972 };

            var mosaicId = new MosaicId(id.ToUInt64());

            const string expectedHex = "85BBEA6CC462B244";

            mosaicId.HexId.ToUpper().Should().BeEquivalentTo(expectedHex);

        }

        [Fact]
        public void Should_Create_MosaicId_FromHex()
        {

            var id = new UInt64DTO { 3294802500, 2243684972 };
            var expectedId = id.ToUInt64();

            var mosaicId = new MosaicId("85BBEA6CC462B244");
            mosaicId.Id.Should().Be(expectedId);

        }

        [Fact]
        public void Should_Create_Id_Given_Nonce_And_Owner()
        {
            var owner = PublicAccount.CreateFromPublicKey(PublicKey, NetworkType.MIJIN_TEST);
            var bytes = new byte[4]
            {
                0x0, 0x0, 0x0, 0x0
            };

            var mosaicNonce = new MosaicNonce(bytes);
            var mosaicId = MosaicId.CreateFromNonce(mosaicNonce, owner.PublicKey);

            var id = new UInt64DTO { 481110499, 231112638 };
            var expectedId = id.ToUInt64();
            mosaicId.Id.Should().Be(expectedId);
        }

        /*
        [Fact]
        public void Should_Create_Id_Given_Nonce_And_Owner_Hex_Prefix()
        {
            var owner = PublicAccount.CreateFromPublicKey(PublicKey, NetworkType.MIJIN_TEST);
            var bytes = new byte[4]
            {
                0x78, 0xE3, 0x6F, 0xB7
            };

            var mosaicNonce = new MosaicNonce(bytes);
            var mosaicId = MosaicId.CreateFromNonce(mosaicNonce, owner.PublicKey);

            var id = new UInt64DTO { 0xC0AFC518, 0x3AD842A8 };
            var expectedId = id.ToUInt64();
            mosaicId.Id.Should().Be(expectedId);
        }*/
    }
}
