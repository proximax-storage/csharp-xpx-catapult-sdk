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

using io.nem2.sdk.Model.Blockchain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test.Model.BlockchainTests
{
    [TestClass]
    public class NetworkTypeTests
    {
        [TestMethod]
        public void MAIN_NETIs0xb8()
        {
            Assert.IsTrue(0xb8 == NetworkType.Types.MAIN_NET.GetNetworkByte());
            Assert.IsTrue(184 == NetworkType.Types.MAIN_NET.GetNetworkByte());
        }

        [TestMethod]
        public void TEST_NETIs0xa8()
        {
            Assert.IsTrue(0xa8 == NetworkType.Types.TEST_NET.GetNetworkByte());
            Assert.IsTrue(168 == NetworkType.Types.TEST_NET.GetNetworkByte());
        }

        [TestMethod]
        public void PRIVATEIs0xc8()
        {
            Assert.IsTrue(0xc8 == NetworkType.Types.PRIVATE.GetNetworkByte());
            Assert.IsTrue(200 == NetworkType.Types.PRIVATE.GetNetworkByte());
        }

        [TestMethod]
        public void PRIVATE_TESTIs0xb0()
        {
            Assert.IsTrue(0xb0 == NetworkType.Types.PRIVATE_TEST.GetNetworkByte());
            Assert.IsTrue(176 == NetworkType.Types.PRIVATE_TEST.GetNetworkByte());
        }

        [TestMethod]
        public void MIJINIs0x60()
        {
            Assert.IsTrue(0x60 == NetworkType.Types.MIJIN.GetNetworkByte());
            Assert.IsTrue(96 == NetworkType.Types.MIJIN.GetNetworkByte());
        }

        [TestMethod]
        public void MIJIN_TESTIs0x90()
        {
            Assert.IsTrue(0x90 == NetworkType.Types.MIJIN_TEST.GetNetworkByte());
            Assert.IsTrue(144 == NetworkType.Types.MIJIN_TEST.GetNetworkByte());
        }
    }
}
