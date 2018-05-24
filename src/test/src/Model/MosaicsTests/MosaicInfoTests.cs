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
using io.nem2.sdk.Model.Mosaics;
using io.nem2.sdk.Model.Namespace;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test.Model.MosaicsTests
{
    [TestClass]
    public class MosaicInfoTest
    {
        [TestMethod]
        public void CreateAMosaicInfoViaConstructor()
        {
            var mosaicProperties = new MosaicProperties(true, true, true, 3, 10);

            var mosaicInfo = new MosaicInfo(true,
                    1,
                    "5A3CD9B09CD1E8000159249B",
                    new NamespaceId( 9562080086528621131),
                    new MosaicId( 15358872602548358953),
                    100,
                    1,
                    new PublicAccount("B4F12E7C9F6946091E2CB8B6D3A12B50D17CCBBF646386EA27CE2946A7423DCF", NetworkType.Types.MIJIN_TEST),
                    mosaicProperties);

            Assert.AreEqual(true, mosaicInfo.IsActive);
            Assert.IsTrue(mosaicInfo.Index == 1);
            Assert.AreEqual("5A3CD9B09CD1E8000159249B", mosaicInfo.MetaId);
            Assert.AreEqual(9562080086528621131, mosaicInfo.NamespaceId.Id);
            Assert.AreEqual(15358872602548358953, mosaicInfo.MosaicId.Id);
            Assert.AreEqual((ulong)100, mosaicInfo.Supply);
            Assert.AreEqual((ulong)1, mosaicInfo.Height);
            Assert.AreEqual(new PublicAccount("B4F12E7C9F6946091E2CB8B6D3A12B50D17CCBBF646386EA27CE2946A7423DCF", NetworkType.Types.MIJIN_TEST).Address.Plain, mosaicInfo.Owner.Address.Plain);
            Assert.IsTrue(mosaicInfo.IsSupplyMutable);
            Assert.IsTrue(mosaicInfo.IsTransferable);
            Assert.IsTrue(mosaicInfo.IsLevyMutable);
            Assert.AreEqual(3, mosaicInfo.Divisibility);
            Assert.AreEqual((ulong)10, mosaicInfo.Duration);
        }

        [TestMethod]
        public void ShouldReturnIsSupplyMutableWhenIsMutable()
        {
            var mosaicProperties = new MosaicProperties(true, true, true, 3, 10);

            var mosaicInfo = new MosaicInfo(true,
                    1,
                    "5A3CD9B09CD1E8000159249B",
                    new NamespaceId(9562080086528621131),
                    new MosaicId(15358872602548358953),
                    100,
                    1,
                    new PublicAccount("B4F12E7C9F6946091E2CB8B6D3A12B50D17CCBBF646386EA27CE2946A7423DCF", NetworkType.Types.MIJIN_TEST),
                    mosaicProperties);

            Assert.IsTrue(mosaicInfo.IsSupplyMutable);
        }

        [TestMethod]
        public void ShouldReturnIsSupplyMutableWhenIsImmutable()
        {
            var mosaicProperties = new MosaicProperties(false, true, true, 3, 10);

            var mosaicInfo = new MosaicInfo(true,
                    1,
                    "5A3CD9B09CD1E8000159249B",
                    new NamespaceId(9562080086528621131),
                    new MosaicId(15358872602548358953),
                    100,
                    1,
                    new PublicAccount("B4F12E7C9F6946091E2CB8B6D3A12B50D17CCBBF646386EA27CE2946A7423DCF", NetworkType.Types.MIJIN_TEST),
                    mosaicProperties);

            Assert.IsTrue(!mosaicInfo.IsSupplyMutable);
        }

        [TestMethod]
        public void ShouldReturnIsTransferableWhenItsTransferable()
        {
            var mosaicProperties = new MosaicProperties(true, true, true, 3, 10);

            var mosaicInfo = new MosaicInfo(true,
                    1,
                    "5A3CD9B09CD1E8000159249B",
                    new NamespaceId(9562080086528621131),
                    new MosaicId(15358872602548358953),
                    100,
                    1,
                    new PublicAccount("B4F12E7C9F6946091E2CB8B6D3A12B50D17CCBBF646386EA27CE2946A7423DCF", NetworkType.Types.MIJIN_TEST),
                    mosaicProperties);

            Assert.IsTrue(mosaicInfo.IsTransferable);
        }

        [TestMethod]
        public void ShouldReturnIsTransferableWhenItsNotTransferable()
        {
            var mosaicProperties = new MosaicProperties(true, false, true, 3, 10);

            var mosaicInfo = new MosaicInfo(true,
                    1,
                    "5A3CD9B09CD1E8000159249B",
                    new NamespaceId(9562080086528621131),
                    new MosaicId(15358872602548358953),
                    100,
                    1,
                    new PublicAccount("B4F12E7C9F6946091E2CB8B6D3A12B50D17CCBBF646386EA27CE2946A7423DCF", NetworkType.Types.MIJIN_TEST),
                    mosaicProperties);

            Assert.IsTrue(!mosaicInfo.IsTransferable);
        }

        [TestMethod]
        public void ShouldReturnIsTransferableWhenLevyIsMutable()
        {
            var mosaicProperties = new MosaicProperties(true, true, true, 3, 10);

            var mosaicInfo = new MosaicInfo(true,
                    1,
                    "5A3CD9B09CD1E8000159249B",
                    new NamespaceId(9562080086528621131),
                    new MosaicId(15358872602548358953),
                    100,
                    1,
                    new PublicAccount("B4F12E7C9F6946091E2CB8B6D3A12B50D17CCBBF646386EA27CE2946A7423DCF", NetworkType.Types.MIJIN_TEST),
                    mosaicProperties);

            Assert.IsTrue(mosaicInfo.IsLevyMutable);
        }

        [TestMethod]
        public void ShouldReturnIsTransferableWhenLevyIsImmutable()
        {
            var mosaicProperties = new MosaicProperties(true, true, false, 3, 10);

            var mosaicInfo = new MosaicInfo(true,
                    1,
                    "5A3CD9B09CD1E8000159249B",
                    new NamespaceId(9562080086528621131),
                    new MosaicId(15358872602548358953),
                    100,
                    1,
                    new PublicAccount("B4F12E7C9F6946091E2CB8B6D3A12B50D17CCBBF646386EA27CE2946A7423DCF", NetworkType.Types.MIJIN_TEST),
                    mosaicProperties);

            Assert.IsTrue(!mosaicInfo.IsLevyMutable);
        }
    }
}
