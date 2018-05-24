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
    public class PublicAccountTest
    {
        private readonly string publicKey = "b4f12e7c9f6946091e2cb8b6d3a12b50d17ccbbf646386ea27ce2946a7423dcf";

        [TestMethod]
        public void ShouldCreatePublicAccountViaConstructor()
        {
            var publicAccount = new PublicAccount(publicKey, NetworkType.Types.MIJIN_TEST);
            Assert.AreEqual(publicKey, publicAccount.PublicKey);
            Assert.AreEqual("SARNASAS2BIAB6LMFA3FPMGBPGIJGK6IJETM3ZSP", publicAccount.Address.Plain);
        }

        [TestMethod]
        public void ShouldCreatePublicAccountViaStaticConstructor()
        {
            var publicAccount = PublicAccount.CreateFromPublicKey(publicKey, NetworkType.Types.MIJIN_TEST);
            Assert.AreEqual(publicKey, publicAccount.PublicKey);
            Assert.AreEqual("SARNASAS2BIAB6LMFA3FPMGBPGIJGK6IJETM3ZSP", publicAccount.Address.Plain);
        }

        [TestMethod]
        public void EqualityIsBasedOnPublicKeyAndNetwork()
        {
            var publicAccount = new PublicAccount(publicKey, NetworkType.Types.MIJIN_TEST);
            var publicAccount2 = new PublicAccount(publicKey, NetworkType.Types.MIJIN_TEST);
            Assert.AreEqual(publicAccount.Address.Pretty, publicAccount2.Address.Pretty);
        }

        [TestMethod]
        public void EqualityReturnsFalseIfNetworkIsDifferent()
        {
            var publicAccount = new PublicAccount(publicKey, NetworkType.Types.MIJIN_TEST);
            var publicAccount2 = new PublicAccount(publicKey, NetworkType.Types.MAIN_NET);
            Assert.AreNotEqual(publicAccount.Address.Plain, publicAccount2.Address.Plain);
            Assert.AreNotEqual(publicAccount.Address.Pretty, publicAccount2.Address.Pretty);
            Assert.AreNotEqual(publicAccount.Address.NetworkByte, publicAccount2.Address.NetworkByte);
        }
    }
}
