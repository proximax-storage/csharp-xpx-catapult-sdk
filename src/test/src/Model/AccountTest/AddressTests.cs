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