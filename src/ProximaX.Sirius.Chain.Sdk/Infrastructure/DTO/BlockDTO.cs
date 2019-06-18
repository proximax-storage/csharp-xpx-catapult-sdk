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

using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO
{
    /// <summary>
    /// </summary>
    [DataContract]
    public class BlockDTO
    {
        /// <summary>
        ///     Gets or Sets Signature
        /// </summary>
        [DataMember(Name = "signature", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "signature")]
        public string Signature { get; set; }

        /// <summary>
        ///     Gets or Sets Signer
        /// </summary>
        [DataMember(Name = "signer", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "signer")]
        public string Signer { get; set; }

        /// <summary>
        ///     Gets or Sets Version
        /// </summary>
        [DataMember(Name = "version", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "version")]
        public decimal? Version { get; set; }

        /// <summary>
        ///     Gets or Sets Type
        /// </summary>
        [DataMember(Name = "type", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "type")]
        public decimal? Type { get; set; }

        /// <summary>
        ///     Gets or Sets Height
        /// </summary>
        [DataMember(Name = "height", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "height")]
        public UInt64DTO Height { get; set; }

        /// <summary>
        ///     Gets or Sets Timestamp
        /// </summary>
        [DataMember(Name = "timestamp", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "timestamp")]
        public UInt64DTO Timestamp { get; set; }

        /// <summary>
        ///     Gets or Sets Difficulty
        /// </summary>
        [DataMember(Name = "difficulty", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "difficulty")]
        public UInt64DTO Difficulty { get; set; }

        /// <summary>
        ///     Gets or Sets FeeMultiplier
        /// </summary>
        [DataMember(Name = "feeMultiplier", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "feeMultiplier")]
        public int? FeeMultiplier { get; set; }

        /// <summary>
        ///     Gets or Sets PreviousBlockHash
        /// </summary>
        [DataMember(Name = "previousBlockHash", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "previousBlockHash")]
        public string PreviousBlockHash { get; set; }

        /// <summary>
        ///     Gets or Sets BlockTransactionsHash
        /// </summary>
        [DataMember(Name = "blockTransactionsHash", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "blockTransactionsHash")]
        public string BlockTransactionsHash { get; set; }

        /// <summary>
        ///     Gets or Sets BlockReceiptsHash
        /// </summary>
        [DataMember(Name = "blockReceiptsHash", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "blockReceiptsHash")]
        public string BlockReceiptsHash { get; set; }

        /// <summary>
        ///     Gets or Sets StateHash
        /// </summary>
        [DataMember(Name = "stateHash", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "stateHash")]
        public string StateHash { get; set; }

        /// <summary>
        ///     Gets or Sets BeneficiaryPublicKey
        /// </summary>
        [DataMember(Name = "beneficiaryPublicKey", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "beneficiaryPublicKey")]
        public string BeneficiaryPublicKey { get; set; }


        /// <summary>
        ///     Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class BlockDTO {\n");
            sb.Append("  Signature: ").Append(Signature).Append("\n");
            sb.Append("  Signer: ").Append(Signer).Append("\n");
            sb.Append("  Version: ").Append(Version).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  Height: ").Append(Height).Append("\n");
            sb.Append("  Timestamp: ").Append(Timestamp).Append("\n");
            sb.Append("  Difficulty: ").Append(Difficulty).Append("\n");
            sb.Append("  FeeMultiplier: ").Append(FeeMultiplier).Append("\n");
            sb.Append("  PreviousBlockHash: ").Append(PreviousBlockHash).Append("\n");
            sb.Append("  BlockTransactionsHash: ").Append(BlockTransactionsHash).Append("\n");
            sb.Append("  BlockReceiptsHash: ").Append(BlockReceiptsHash).Append("\n");
            sb.Append("  StateHash: ").Append(StateHash).Append("\n");
            sb.Append("  BeneficiaryPublicKey: ").Append(BeneficiaryPublicKey).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        ///     Get the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}