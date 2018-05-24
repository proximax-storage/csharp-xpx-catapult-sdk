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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace test.Model.BlockchainTests
{
    [TestClass]
    public class BlockInfoTests
    {
        [TestMethod]
        public void CreateANewBlockInfo()
        {
            var data = "{\"meta\":{\"hash\":\"193F3C2FE8BB3A41A22CE455CBF469D1F198D4BBBB1E916EDBA17323DF7C065F\",\"generationHash\":\"57F7DA205008026C776CB6AED843393F04CD458E0AA2D9F1D5F31A402072B2D6\",\"totalFee\":[0,0],\"numTransactions\":14},\"block\":{\"signature\":\"AA78A4E0F89ACAA07E3132525F1F4455D4D84EAB686BC56BD8CC9D053CE5CCFC2DA62B7FFA9B9A0D195F54456C7C5C71278F516FE3DE098AEB47271BC1DFA604\",\"signer\":\"B4F12E7C9F6946091E2CB8B6D3A12B50D17CCBBF646386EA27CE2946A7423DCF\",\"version\":36867,\"type\":32835,\"height\":[1,0],\"timestamp\":[0,0],\"difficulty\":[276447232,23283],\"previousBlockHash\":\"0000000000000000000000000000000000000000000000000000000000000000\",\"blockTransactionsHash\":\"379C587C3BD3341A4877A78424CD5B64C2B9B95BA9AD26FA6CCC5AE98D4F6A7B\"}}";

            var blockData = JsonConvert.DeserializeObject<BlockInfoDTO>(data);

            Assert.AreEqual("193F3C2FE8BB3A41A22CE455CBF469D1F198D4BBBB1E916EDBA17323DF7C065F", blockData.Meta.Hash);
            Assert.AreEqual("57F7DA205008026C776CB6AED843393F04CD458E0AA2D9F1D5F31A402072B2D6", blockData.Meta.GenerationHash);
            Assert.AreEqual(14, blockData.Meta.NumTransactions);
            Assert.AreEqual((ulong)0, blockData.Meta.TotalFee);
            Assert.AreEqual("379C587C3BD3341A4877A78424CD5B64C2B9B95BA9AD26FA6CCC5AE98D4F6A7B", blockData.Block.BlockTransactionsHash);
            Assert.AreEqual("AA78A4E0F89ACAA07E3132525F1F4455D4D84EAB686BC56BD8CC9D053CE5CCFC2DA62B7FFA9B9A0D195F54456C7C5C71278F516FE3DE098AEB47271BC1DFA604", blockData.Block.Signature);
            Assert.AreEqual("B4F12E7C9F6946091E2CB8B6D3A12B50D17CCBBF646386EA27CE2946A7423DCF", blockData.Block.Signer);
            Assert.AreEqual(36867, blockData.Block.Version);
            Assert.AreEqual(32835, blockData.Block.Type);
        }
    }
}
