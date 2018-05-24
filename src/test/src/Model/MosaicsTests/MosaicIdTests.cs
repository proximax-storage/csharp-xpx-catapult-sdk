using io.nem2.sdk.Model.Mosaics;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test.Model.MosaicsTests
{
    [TestClass]
    public class MosaicIdTest
    {

        [TestMethod]
        public void CreateAMosaicIdFromMosaicNameViaConstructor()
        {
            MosaicId mosaicId = new MosaicId("nem:xem");
            Assert.AreEqual(mosaicId.Id, 15358872602548358953);
            Assert.AreEqual(mosaicId.FullName, "nem:xem");
            Assert.AreEqual(mosaicId.HexId, Xem.Id.HexId);
            Assert.AreEqual(mosaicId.MosaicName, "xem");

        }

        [TestMethod]
        public void CreateAMosaicIdFromIdViaConstructor()
        {
            MosaicId mosaicId = new MosaicId(15358872602548358953);
            Assert.AreEqual(mosaicId.Id, 15358872602548358953);
            Assert.IsFalse(mosaicId.IsNamePresent);
            Assert.IsFalse(mosaicId.IsFullNamePresent);
        }

        [TestMethod]
        public void ShouldCompareMosaicIdsForEquality()
        {
            MosaicId mosaicId = new MosaicId(15358872602548358953);
            MosaicId mosaicId2 = new MosaicId(15358872602548358953);
            Assert.IsTrue(mosaicId.Equals(mosaicId2));
        }
    }
}
