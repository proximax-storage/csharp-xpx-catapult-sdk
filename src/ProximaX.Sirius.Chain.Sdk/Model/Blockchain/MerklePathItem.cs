// Copyright 2019 ProximaX
// 
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

namespace ProximaX.Sirius.Chain.Sdk.Model.Blockchain
{
    /// <summary>
    /// MerklePathItem class
    /// </summary>
    public class MerklePathItem
    {
        public MerklePathItem(int position, string hash)
        {
            Position = position;
            Hash = hash;
        }

        public int Position { get; }
        public string Hash { get; }

        public static MerklePathItem FromDto(ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO.MerklePathItem dto)
        {
            return new MerklePathItem(dto.Position.Value, dto.Hash);
        }

    }
}
