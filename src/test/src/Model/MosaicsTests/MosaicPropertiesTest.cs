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

using io.nem2.sdk.Model.Mosaics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test.Model.MosaicsTests
{
    [TestClass]
    public class MosaicPropertiesTest
    {
        [TestMethod]
        public void ShouldCreateMosaicPropertiesViaConstructor()
        {
            var mosaicProperties = new MosaicProperties(true, true, true, 1, 1000);
            Assert.IsTrue(mosaicProperties.IsSupplyMutable);
            Assert.IsTrue(mosaicProperties.IsTransferable);
            Assert.IsTrue(mosaicProperties.IsLevyMutable);
            Assert.IsTrue(1 == mosaicProperties.Divisibility);
            Assert.AreEqual((ulong)1000, mosaicProperties.Duration);
        }
        
        [TestMethod]
        public void ShouldCreateMosaicPropertiesViaBuilder()
        {
            var mosaicProperties = new MosaicProperties(true, true, true, 1, 1000);
            Assert.IsTrue(mosaicProperties.IsSupplyMutable);
            Assert.IsTrue(mosaicProperties.IsTransferable);
            Assert.IsTrue(mosaicProperties.IsLevyMutable);
            Assert.IsTrue(1 == mosaicProperties.Divisibility);
            Assert.AreEqual((ulong)1000, mosaicProperties.Duration);
        }
    }
}
