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
using io.nem2.sdk.Model.Namespace;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test.Model.MosaicsTests
{
    [TestClass]
    public class MosaicNameTest
    {
        [TestMethod]
        public void CreateAMosaicName()
        {
            var mosaicName = new MosaicName(new MosaicId(9562080086528621131), "xem", new NamespaceId(15358872602548358953));

            Assert.AreEqual(15358872602548358953, mosaicName.ParentId.Id);
            Assert.AreEqual("xem", mosaicName.Name);
            Assert.AreEqual(9562080086528621131, mosaicName.MosaicId.Id);
        }
    }
}
