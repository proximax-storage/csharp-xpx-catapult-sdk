using System;
using FluentAssertions;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Exceptions;
using Xunit;

namespace ProximaX.Sirius.Chain.Sdk.Tests.Models
{
    public class BlockchainTests
    {
        #region NetworkType Tests
        [Fact]
        public void Enum_PUBLIC_NET_Value_Is_184()
        {
            NetworkTypeValueIsX(NetworkType.PUBLIC, 184);
        }

        [Fact]
        public void Enum_PUBLIC_TEST_Value_Is_168()
        {
            NetworkTypeValueIsX(NetworkType.PUBLIC_TEST, 168);
        }

        [Fact]
        public void Enum_MIJIN_Value_Is_96()
        {
            NetworkTypeValueIsX(NetworkType.MIJIN, 96);
        }

        [Fact]
        public void Enum_MIJIN_TEST_Value_Is_144()
        {
            NetworkTypeValueIsX(NetworkType.MIJIN_TEST, 144);
        }

        [Fact]
        public void Enum_PRIVATE_Value_Is_200()
        {
            NetworkTypeValueIsX(NetworkType.PRIVATE, 200);
        }

        [Fact]
        public void Enum_PRIVATE_TEST_Value_Is_144()
        {
            NetworkTypeValueIsX(NetworkType.PRIVATE_TEST, 176);
        }

        [Fact]
        public void Enum_NOT_SUPPORT_Value_Is_0()
        {
            NetworkTypeValueIsX(NetworkType.NOT_SUPPORT, 0);
        }

        [Fact]
        public void Enum_PUBLIC_Value_In_Byte_Is_0xb8()
        {
            NetworkTypeValueInByteIsX(NetworkType.PUBLIC, 0xb8);
        }

        [Fact]
        public void Enum_PUBLIC_NET_Value_In_Byte_Is_0xa8()
        {
            NetworkTypeValueInByteIsX(NetworkType.PUBLIC_TEST, 0xa8);
        }

        [Fact]
        public void Enum_PRIVATE_Value_In_Byte_Is_0xc8()
        {
            NetworkTypeValueInByteIsX(NetworkType.PRIVATE, 0xc8);
        }

        [Fact]
        public void Enum_PRIVATE_TEST_Value_In_Byte_Is_0xb0()
        {
            NetworkTypeValueInByteIsX(NetworkType.PRIVATE_TEST, 0xb0);
        }


        [Fact]
        public void Enum_MIJIN_Value_In_Byte_Is_0x60()
        {
            NetworkTypeValueInByteIsX(NetworkType.MIJIN, 0x60);
        }

        [Fact]
        public void Enum_MIJIN_TEST_Value_In_Byte_Is_0x90()
        {
            NetworkTypeValueInByteIsX(NetworkType.MIJIN_TEST, 0x90);
        }

        [Fact]
        public void Enum_NOT_SUPPORT_Value_Is_144()
        {
            NetworkTypeValueIsX(NetworkType.NOT_SUPPORT, 0);
        }

        [Fact]
        public void Value_184_Is_PUBLIC()
        {
            ValueXIsNetWorkType(184, NetworkType.PUBLIC);
        }

        [Fact]
        public void Value_168_Is_PUBLIC_TEST()
        {
            ValueXIsNetWorkType(168, NetworkType.PUBLIC_TEST);
        }

        [Fact]
        public void Value_200_Is_PRIVATE()
        {
            ValueXIsNetWorkType(200, NetworkType.PRIVATE);
        }

        [Fact]
        public void Value_168Is_PRIVATE_TEST()
        {
            ValueXIsNetWorkType(176, NetworkType.PRIVATE_TEST);
        }

        [Fact]
        public void Value_96_Is_MIJIN()
        {
            ValueXIsNetWorkType(96, NetworkType.MIJIN);
        }

        [Fact]
        public void Value_144_Is_MIJIN_TEST()
        {
            ValueXIsNetWorkType(144, NetworkType.MIJIN_TEST);
        }

        [Fact]
        public void NetworkType_Name_Returns_Enum_NetworkType()
        {
            GetEnumTypeByName("MIJINTEST", NetworkType.MIJIN_TEST);
            GetEnumTypeByName("PUBLIC", NetworkType.PUBLIC);
            GetEnumTypeByName("MIJIN", NetworkType.MIJIN);
            GetEnumTypeByName("PUBLICTEST", NetworkType.PUBLIC_TEST);
            GetEnumTypeByName("PRIVATE", NetworkType.PRIVATE);
            GetEnumTypeByName("PRIVATETEST", NetworkType.PRIVATE_TEST);
            Action act = () => { NetworkTypeExtension.GetRawValue("PRIVATE_NET"); };
            act.Should().Throw<TypeNotSupportException>();
        }


        private static void NetworkTypeValueIsX(NetworkType networkType, int expectedValue)
        {
            networkType.GetValue().Should().Be(expectedValue);

        }

        private static void NetworkTypeValueInByteIsX(NetworkType networkType, byte expectedValue)
        {
            networkType.GetValueInByte().Should().Be(expectedValue);
        }

        private static void ValueXIsNetWorkType(int value, NetworkType expectedNetworkType)
        {
            NetworkTypeExtension.GetRawValue(value).Should().BeEquivalentTo(expectedNetworkType);

        }
        private static void GetEnumTypeByName(string value, NetworkType networkType)
        {
            NetworkTypeExtension.GetRawValue(value).Should().BeEquivalentTo(networkType);

        }
        #endregion

        #region BlockStorageInfo Tests
        [Fact]
        public void Create_New_BlockStorageInfo()
        {

            var blockchainStorageInfo = new BlockchainStorageInfo(1, 2, 3);

            blockchainStorageInfo.NumAccounts.Should().Be(1);
            blockchainStorageInfo.NumBlocks.Should().Be(2);
            blockchainStorageInfo.NumTransactions.Should().Be(3);

        }
        #endregion
    }
}
