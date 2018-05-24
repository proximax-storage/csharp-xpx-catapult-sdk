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

using io.nem2.sdk.Infrastructure.Buffers.Model;
using io.nem2.sdk.Model.Namespace;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace test.Model.NamespaceTests
{
    [TestClass]
    public class NamespaceTest
    {
        [TestMethod]
        public void ShouldSerializeNamespace()
        {
            var data = "{\"meta\":{\"active\":true,\"index\":0,\"id\":\"5A6A7758FD331300012A108F\"},\"namespace\":{\"type\":0,\"depth\":1,\"level0\":[929036875,2226345261],\"parentId\":[0,0],\"owner\":\"B4F12E7C9F6946091E2CB8B6D3A12B50D17CCBBF646386EA27CE2946A7423DCF\",\"startHeight\":[1,0],\"endHeight\":[4294967295,4294967295]}}";

            var namespaceInfo = JsonConvert.DeserializeObject<NamespaceInfoDTO>(data);

            Assert.AreEqual(0, namespaceInfo.Meta.Index);
            Assert.IsTrue(namespaceInfo.Meta.Active);
            Assert.AreEqual("5A6A7758FD331300012A108F", namespaceInfo.Meta.Id);
            Assert.AreEqual(0, namespaceInfo.Namespace.Type);
            Assert.AreEqual(1, namespaceInfo.Namespace.Depth);
            Assert.AreEqual(9562080086528621131, namespaceInfo.Namespace.Level0);
            Assert.AreEqual((ulong)0, namespaceInfo.Namespace.ParentId);
            Assert.AreEqual("B4F12E7C9F6946091E2CB8B6D3A12B50D17CCBBF646386EA27CE2946A7423DCF", namespaceInfo.Namespace.Owner);
            Assert.AreEqual((ulong)1, namespaceInfo.Namespace.StartHeight);
            Assert.AreEqual(18446744073709551615, namespaceInfo.Namespace.EndHeight);
        }
    }

    [TestClass]
    public class NamespaceIdTest
    {
        [TestMethod]
        public void CreateANamespaceIdFromRootNamespaceNameViaConstructor()
        {
            NamespaceId namespaceId = new NamespaceId("nem");
            Assert.AreEqual(namespaceId.Id, 9562080086528621131);
            Assert.AreEqual(namespaceId.Name, "nem");
            Assert.AreEqual(namespaceId.HexId, "84B3552D375FFA4B");
        }



        [TestMethod]
        public void CreateANamespaceIdFromSubNamespaceNameViaConstructor()
        {
            NamespaceId namespaceId = new NamespaceId("nem.xem");
            Assert.AreEqual(namespaceId.Id, (ulong)6507831095446241869);
            Assert.AreEqual(namespaceId.Name, "nem.xem");
        }



        [TestMethod]
        public void CreateANamespaceIdFromIdViaConstructor()
        {
            NamespaceId namespaceId = new NamespaceId(9562080086528621131);
            Assert.AreEqual(namespaceId.Id, 9562080086528621131);
            
        }
    }
}
