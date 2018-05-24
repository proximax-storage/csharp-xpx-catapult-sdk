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

using io.nem2.sdk.Core.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test.Crypto
{
    [TestClass]
    public class IdGeneratorTests
    {
        [TestMethod]
        public void CreateMosaicId()
        {      
            Assert.AreEqual("D525AD41D95FCF29", IdGenerator.GenerateId(IdGenerator.GenerateId(0, "nem"), "xem").ToString("X"));
        }

        [TestMethod]
        public void CreateNemId()
        {
            Assert.AreEqual("84B3552D375FFA4B", IdGenerator.GenerateId(0, "nem").ToString("X"));
        }
    }
}
