using io.nem2.sdk.Model.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test.Model.Transactions
{
    [TestClass]
    public class HashTypeTests
    {
        [TestMethod]
        public void HashTypeIsSha3()
        {
            var hashType = HashType.Types.SHA3_512;

            Assert.AreEqual(hashType.GetHashTypeValue(), 0x00);
        }
    }
}
