//
// Copyright 2018 NEM
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 

using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Blockchain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test.Model.AccountTest
{
    [TestClass]
    public class AddressTest
    {
        private const string PublicKey = "c2f93346e27ce6ad1a9f8f5e3066f8326593a406bdf357acb041e2f9ab402efe";

        [TestMethod]
        public void TestCreateAddressFromPublicKeyForMainNet()
        {
            var address = Address.CreateFromPublicKey(PublicKey, NetworkType.Types.MAIN_NET);
            Assert.AreEqual("XCTVW23D2MN5VE4AQ4TZIDZENGNOZXPRPRVLFKS6", address.Plain);
            Assert.AreEqual(NetworkType.Types.MAIN_NET, address.NetworkByte);
        }

        [TestMethod]
        public void TestCreateAddressFromPublicKeyForTestNet()
        {
            var address = Address.CreateFromPublicKey(PublicKey, NetworkType.Types.TEST_NET);
            Assert.AreEqual("VCTVW23D2MN5VE4AQ4TZIDZENGNOZXPRPR3HTEHT", address.Plain);
            Assert.AreEqual(NetworkType.Types.TEST_NET, address.NetworkByte);
        }

        [TestMethod]
        public void TestCreateAddressFromPublicKeyForPrivate()
        {
            var address = Address.CreateFromPublicKey(PublicKey, NetworkType.Types.PRIVATE);
            Assert.AreEqual("ZCTVW23D2MN5VE4AQ4TZIDZENGNOZXPRPR2FNT66", address.Plain);
            Assert.AreEqual(NetworkType.Types.PRIVATE, address.NetworkByte);
        }

        [TestMethod]
        public void TestCreateAddressFromPublicKeyForPrivateTest()
        {
            var address = Address.CreateFromPublicKey(PublicKey, NetworkType.Types.PRIVATE_TEST);
            Assert.AreEqual("WCTVW23D2MN5VE4AQ4TZIDZENGNOZXPRPSIBCI5Q", address.Plain);
            Assert.AreEqual(NetworkType.Types.PRIVATE_TEST, address.NetworkByte);
        }

        [TestMethod]
        public void TestCreateAddressFromPublicKeyForMijin()
        {
            var address = Address.CreateFromPublicKey(PublicKey, NetworkType.Types.MIJIN);
            Assert.AreEqual("MCTVW23D2MN5VE4AQ4TZIDZENGNOZXPRPR72DYSX", address.Plain);
            Assert.AreEqual(NetworkType.Types.MIJIN, address.NetworkByte);
        }

        [TestMethod]
        public void TestCreateAddressFromPublicKeyForMijinTest()
        {
            var address = Address.CreateFromPublicKey(PublicKey, NetworkType.Types.MIJIN_TEST);
            Assert.AreEqual("SCTVW23D2MN5VE4AQ4TZIDZENGNOZXPRPRLIKCF2", address.Plain);
            Assert.AreEqual(NetworkType.Types.MIJIN_TEST, address.NetworkByte);
        }

        [TestMethod]
        public void TestCreateAddressForMainNet()
        {
            var address = Address.CreateFromRawAddress("XCTVW23D2MN5VE4AQ4TZIDZENGNOZXPRPRVLFKS6");
            Assert.AreEqual("XCTVW23D2MN5VE4AQ4TZIDZENGNOZXPRPRVLFKS6", address.Plain);
            Assert.AreEqual(NetworkType.Types.MAIN_NET, address.NetworkByte);
        }

        [TestMethod]
        public void TestCreateAddressForTestNet()
        {
            var address = Address.CreateFromRawAddress("VCTVW23D2MN5VE4AQ4TZIDZENGNOZXPRPR3HTEHT");
            Assert.AreEqual("VCTVW23D2MN5VE4AQ4TZIDZENGNOZXPRPR3HTEHT", address.Plain);
            Assert.AreEqual(NetworkType.Types.TEST_NET, address.NetworkByte);
        }

        [TestMethod]
        public void TestCreateAddressForPrivate()
        {
            var address = Address.CreateFromRawAddress("ZCTVW23D2MN5VE4AQ4TZIDZENGNOZXPRPR2FNT66");
            Assert.AreEqual("ZCTVW23D2MN5VE4AQ4TZIDZENGNOZXPRPR2FNT66", address.Plain);
            Assert.AreEqual(NetworkType.Types.PRIVATE, address.NetworkByte);
        }

        [TestMethod]
        public void TestCreateAddressForPrivateTest()
        {
            var address = Address.CreateFromRawAddress("WCTVW23D2MN5VE4AQ4TZIDZENGNOZXPRPSIBCI5Q");
            Assert.AreEqual("WCTVW23D2MN5VE4AQ4TZIDZENGNOZXPRPSIBCI5Q", address.Plain);
            Assert.AreEqual(NetworkType.Types.PRIVATE_TEST, address.NetworkByte);
        }

        [TestMethod]
        public void TestCreateAddressForMijin()
        {
            var address = Address.CreateFromRawAddress("MCTVW23D2MN5VE4AQ4TZIDZENGNOZXPRPR72DYSX");
            Assert.AreEqual("MCTVW23D2MN5VE4AQ4TZIDZENGNOZXPRPR72DYSX", address.Plain);
            Assert.AreEqual(NetworkType.Types.MIJIN, address.NetworkByte);
        }

        [TestMethod]
        public void TestCreateAddressForMijinTest()
        {
            var address = Address.CreateFromRawAddress("SCTVW23D2MN5VE4AQ4TZIDZENGNOZXPRPRLIKCF2");
            Assert.AreEqual("SCTVW23D2MN5VE4AQ4TZIDZENGNOZXPRPRLIKCF2", address.Plain);
            Assert.AreEqual(NetworkType.Types.MIJIN_TEST, address.NetworkByte);
        }

        [TestMethod]
        public void TestAddressCreation()
        {
            Address address = new Address("SDGLFW-DSHILT-IUHGIB-H5UGX2-VYF5VN-JEKCCD-BR26", NetworkType.Types.MIJIN_TEST);
            Assert.AreEqual("SDGLFWDSHILTIUHGIBH5UGX2VYF5VNJEKCCDBR26", address.Plain);
        }

        [TestMethod]
        public void TestAddressWithSpacesCreation()
        {
            Address address = new Address(" SDGLFW-DSHILT-IUHGIB-H5UGX2-VYF5VN-JEKCCD-BR26 ", NetworkType.Types.MIJIN_TEST);
            Assert.AreEqual("SDGLFWDSHILTIUHGIBH5UGX2VYF5VNJEKCCDBR26", address.Plain);
        }

        [TestMethod]
        public void TestLowerCaseAddressCreation()
        {
            Address address = new Address("sdglfw-dshilt-iuhgib-h5ugx2-vyf5vn-jekccd-br26", NetworkType.Types.MIJIN_TEST);
            Assert.AreEqual("SDGLFWDSHILTIUHGIBH5UGX2VYF5VNJEKCCDBR26", address.Plain);
        }

        [TestMethod]
        public void AddressInPrettyFormat()
        {
            Address address = new Address("SDRDGF-TDLLCB-67D4HP-GIMIHP-NSRYRJ-RT7DOB-GWZY", NetworkType.Types.MIJIN_TEST);
            Assert.AreEqual("SDRDGF-TDLLCB-67D4HP-GIMIHP-NSRYRJ-RT7DOB-GWZY", address.Pretty);
        }

        [TestMethod]
        public void Equality()
        {
            Address address1 = new Address("SDRDGF-TDLLCB-67D4HP-GIMIHP-NSRYRJ-RT7DOB-GWZY", NetworkType.Types.MIJIN_TEST);
            Address address2 = new Address("SDRDGFTDLLCB67D4HPGIMIHPNSRYRJRT7DOBGWZY", NetworkType.Types.MIJIN_TEST);
            Assert.AreEqual(address1.Pretty, address2.Pretty);
            Assert.AreEqual(address1.Plain, address2.Plain);
        }

        [TestMethod]
        public void NoEquality()
        {
            Address address1 = new Address("SRRRRR-TTTTTT-555555-GIMIHP-NSRYRJ-RT7DOB-GWZY", NetworkType.Types.MIJIN_TEST);
            Address address2 = new Address("SDRDGF-TDLLCB-67D4HP-GIMIHP-NSRYRJ-RT7DOB-GWZY", NetworkType.Types.MIJIN_TEST);
            Assert.AreNotEqual(address1.Pretty, address2.Pretty);
            Assert.AreNotEqual(address1.Plain, address2.Plain);
        }


    }
}



//private static Stream<Arguments> provider()
//{
//    return Stream.of(
//            Arguments.of("SDGLFW-DSHILT-IUHGIB-H5UGX2-VYF5VN-JEKCCD-BR26", NetworkType.MIJIN_TEST),
//            Arguments.of("MDGLFW-DSHILT-IUHGIB-H5UGX2-VYF5VN-JEKCCD-BR26", NetworkType.MIJIN),
//            Arguments.of("TDGLFW-DSHILT-IUHGIB-H5UGX2-VYF5VN-JEKCCD-BR26", NetworkType.TEST_NET),
//            Arguments.of("NDGLFW-DSHILT-IUHGIB-H5UGX2-VYF5VN-JEKCCD-BR26", NetworkType.MAIN_NET)
//    );
//}
//
//private static Stream<Arguments> assertExceptionProvider()
//{
//    return Stream.of(
//            Arguments.of("SDGLFW-DSHILT-IUHGIB-H5UGX2-VYF5VN-JEKCCD-BR26", NetworkType.MIJIN),
//            Arguments.of("MDGLFW-DSHILT-IUHGIB-H5UGX2-VYF5VN-JEKCCD-BR26", NetworkType.MAIN_NET),
//            Arguments.of("TDGLFW-DSHILT-IUHGIB-H5UGX2-VYF5VN-JEKCCD-BR26", NetworkType.MAIN_NET),
//            Arguments.of("NDGLFW-DSHILT-IUHGIB-H5UGX2-VYF5VN-JEKCCD-BR26", NetworkType.TEST_NET)
//    );
//}
//
//private static Stream<Arguments> publicKeys()
//{
//    return Stream.of(
//            Arguments.of("b4f12e7c9f6946091e2cb8b6d3a12b50d17ccbbf646386ea27ce2946a7423dcf", NetworkType.MIJIN_TEST, "SARNASAS2BIAB6LMFA3FPMGBPGIJGK6IJETM3ZSP"),
//            Arguments.of("b4f12e7c9f6946091e2cb8b6d3a12b50d17ccbbf646386ea27ce2946a7423dcf", NetworkType.MIJIN, "MARNASAS2BIAB6LMFA3FPMGBPGIJGK6IJE5K5RYU"),
//            Arguments.of("b4f12e7c9f6946091e2cb8b6d3a12b50d17ccbbf646386ea27ce2946a7423dcf", NetworkType.TEST_NET, "TARNASAS2BIAB6LMFA3FPMGBPGIJGK6IJE47FYR3"),
//            Arguments.of("b4f12e7c9f6946091e2cb8b6d3a12b50d17ccbbf646386ea27ce2946a7423dcf", NetworkType.MAIN_NET, "NARNASAS2BIAB6LMFA3FPMGBPGIJGK6IJFJKUV32")
//    );
//}
//
//}
//}
//