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

using io.nem2.sdk.Model.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using io.nem2.sdk.Core.Crypto.Chaso.NaCl;
namespace test.Model.Transactions
{
    [TestClass]
    public class TransactionExtensionsTest
    {
        [TestMethod]
        public void Hasher()
        {
            var payloadString = "A6000000642AC34E5DD986B6CD3817F2CFAA5A5525447A010F1C08EE62109BCF515AE1F735484478216BEC3DD1B53FE3BCEBCDC3AC182EDC323F860F2736EF4C868EDB0610CC07742437C205D9A0BC0434DC5B4879E002114753DE70CDC4C4BD0D93A64A0390014100000000000000004378F8F40B0000009039404AFFA3D5BB337FE9FA21182210EE40CE08662CF93B77020001000029CF5FD941AD25D50010A5D4E8000000";

            var expected = "E7D31348244723DFDFF54A24BC48BA08623A3660F472EE231D8A93F9C082E360";

            var hash = TransactionExtensions.Hasher(payloadString.FromHex());
            
            Assert.AreEqual(expected, hash.ToHexLower().ToUpper());


        }
    }
}
