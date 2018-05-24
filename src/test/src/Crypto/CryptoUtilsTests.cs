using System.Text;
using io.nem2.sdk.Core.Crypto;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test.Crypto
{
    [TestClass]
    public class CryptoUtilsTests
    {
        [TestMethod]
        public void ToMobileKey()
        {
            var password = "TestTest";
            var encrypted = CryptoUtils.ToMobileKey(password, "2A91E1D5C110A8D0105AAD4683F962C2A56663A3CAD46666B16D243174673D90");
            
            var key = CryptoUtils.FromMobileKey(encrypted, "TestTest");

            Assert.AreEqual(key, "2A91E1D5C110A8D0105AAD4683F962C2A56663A3CAD46666B16D243174673D90");

        }

        [TestMethod]
        public void DeriveKeyFromPasswordSha()
        {
            var password = "TestTest";
            var count = 20;
            var expectedKey = "8CD87BC513857A7079D182A6E19B370E907107D97BD3F81A85BCEBCC4B5BD3B5";

            var key = CryptoUtils.DerivePassSha(Encoding.UTF8.GetBytes(password), count);

            Assert.AreEqual(expectedKey, key);
        }
    }
}
