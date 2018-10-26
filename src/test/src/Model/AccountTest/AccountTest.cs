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

using System.Collections.Generic;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Blockchain;
using io.nem2.sdk.Model.Mosaics;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.Model.Transactions.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test.Model.AccountTest
{
    [TestClass]
    public class AccountTest
    {
        [TestMethod]
        public void ShouldCreateAccountViaConstructor()
        {
            Account account = new Account("787225aaff3d2c71f4ffa32d4f19ec4922f3cd869747f267378f81f8e3fcb12d", NetworkType.Types.MIJIN_TEST);
            Assert.AreEqual("SDRDGFTDLLCB67D4HPGIMIHPNSRYRJRT7DOBGWZY", account.Address.Plain);
            Assert.AreEqual("1026D70E1954775749C6811084D6450A3184D977383F0E4282CD47118AF37755", account.PublicKey);
            Assert.AreEqual("787225AAFF3D2C71F4FFA32D4F19EC4922F3CD869747F267378F81F8E3FCB12D", account.PrivateKey);
        }

        [TestMethod]
        public void ShouldCreateAccountViaStaticConstructor()
        {
            Account account = Account.CreateFromPrivateKey("787225AAFF3D2C71F4FFA32D4F19EC4922F3CD869747F267378F81F8E3FCB12D", NetworkType.Types.MIJIN_TEST);
            Assert.AreEqual("787225AAFF3D2C71F4FFA32D4F19EC4922F3CD869747F267378F81F8E3FCB12D", account.PrivateKey);
            Assert.AreEqual("SDRDGFTDLLCB67D4HPGIMIHPNSRYRJRT7DOBGWZY", account.Address.Plain);
            Assert.AreEqual("1026D70E1954775749C6811084D6450A3184D977383F0E4282CD47118AF37755", account.PublicKey);
        }

        [TestMethod]
        public void ShouldCreateAccountViaStaticConstructor2()
        {
            Account account = Account.CreateFromPrivateKey("9FC72A5116EB37C343E0B6CA907A3CFFEE14FC625CFDCBD77DDECD96E783C1AF", NetworkType.Types.MIJIN_TEST);
            Assert.AreEqual("9FC72A5116EB37C343E0B6CA907A3CFFEE14FC625CFDCBD77DDECD96E783C1AF", account.PrivateKey);
            Assert.AreEqual("14A239D2ADB96753CFC160BB262F27B01BCCC8C74599F51771BC6BD39980F4E7", account.PublicKey);
            Assert.AreEqual("SDBDG4IT43MPCW2W4CBBCSJJT42AYALQN7A4VVWL", account.Address.Plain);
        }

        [TestMethod]
        public void ShouldCreateAccountViaStaticConstructor3()
        {
            Account account = Account.CreateFromPrivateKey("B8AFAE6F4AD13A1B8AAD047B488E0738A437C7389D4FF30C359AC068910C1D59", NetworkType.Types.MIJIN_TEST);
            Assert.AreEqual("B8AFAE6F4AD13A1B8AAD047B488E0738A437C7389D4FF30C359AC068910C1D59", account.PrivateKey);
            Assert.AreEqual("68B3FBB18729C1FDE225C57F8CE080FA828F0067E451A3FD81FA628842B0B763", account.PublicKey);
            Assert.AreEqual("SBE6CS7LZKJXLDVTNAC3VZ3AUVZDTF3PACNFIXFN", account.Address.Plain);
        }

       // [TestMethod]
        public void ShouldSignTransaction()
        {
            Account account = new Account("787225aaff3d2c71f4ffa32d4f19ec4922f3cd869747f267378f81f8e3fcb12d", NetworkType.Types.MIJIN_TEST);
            TransferTransaction transferTransaction = TransferTransaction.Create(
                    NetworkType.Types.MIJIN_TEST,
                    Deadline.CreateHours(0), 
                    Address.CreateFromRawAddress("SDUP5PLHDXKBX3UU5Q52LAY4WYEKGEWC6IB3VBFM"),
                    new List<Mosaic>() {
                        new Mosaic(new MosaicId("nem:xem"), 100)
                    },
                    EmptyMessage.Create()
            );

            SignedTransaction signedTransaction = account.Sign(transferTransaction);
            Assert.AreEqual("A50000003400097BF12505E4138374A1B5AFB01E183F53F5EF1416C0D1E84BBE8CA29819722063922E8006B7738080DF1BDA819BF4D937765E941D025BE1C61B345975061026D70E1954775749C6811084D6450A3184D977383F0E4282CD47118AF37755039054410000000000000000000000000000000090E8FEBD671DD41BEE94EC3BA5831CB608A312C2F203BA84AC0100010029CF5FD941AD25D56400000000000000", signedTransaction.Payload);
            Assert.AreEqual("4877AFCC92D1CCA47FDB65F1E7B70BB25D196287E8C9177BCF46F644887B7BA3", signedTransaction.Hash);
        }
    }
}
