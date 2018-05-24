using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Blockchain;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test.Model.AccountTest
{
    [TestClass]
    public class CreateAccount
    {
        [TestMethod]
        public void CreateNewAccount()
        {
            var acc = Account.GenerateNewAccount(NetworkType.Types.MIJIN_TEST);

            Assert.AreEqual(64, acc.PublicAccount.PublicKey.Length);

        }

        [TestMethod]
        public void CreateNewAccountFromKey()
        {
            var acc = Account.CreateFromPrivateKey("52b62ec8fafe1d5b7dc2896749f979d5c9ec852a4d37cff9f10656629f4efbf7", NetworkType.Types.MIJIN_TEST);

            Assert.AreEqual(64, acc.PublicAccount.PublicKey.Length);

        }
    }
}
