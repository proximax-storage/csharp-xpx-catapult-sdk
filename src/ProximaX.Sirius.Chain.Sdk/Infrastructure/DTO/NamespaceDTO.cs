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
    public class NamespaceDTO
    {
        /// <summary>
        ///     Gets or Sets Owner
        /// </summary>
        [DataMember(Name = "owner", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "owner")]
        public string Owner { get; set; }

        /// <summary>
        ///     Gets or Sets OwnerAddress
        /// </summary>
        [DataMember(Name = "ownerAddress", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "ownerAddress")]
        public string OwnerAddress { get; set; }

        /// <summary>
        ///     Gets or Sets StartHeight
        /// </summary>
        [DataMember(Name = "startHeight", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "startHeight")]
        public UInt64DTO StartHeight { get; set; }

        /// <summary>
        ///     Gets or Sets EndHeight
        /// </summary>
        [DataMember(Name = "endHeight", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "endHeight")]
        public UInt64DTO EndHeight { get; set; }

        /// <summary>
        ///     Gets or Sets Depth
        /// </summary>
        [DataMember(Name = "depth", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "depth")]
        public int? Depth { get; set; }

        /// <summary>
        ///     Gets or Sets Level0
        /// </summary>
        [DataMember(Name = "level0", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "level0")]
        public UInt64DTO Level0 { get; set; }

        /// <summary>
        ///     Gets or Sets Level1
        /// </summary>
        [DataMember(Name = "level1", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "level1")]
        public UInt64DTO Level1 { get; set; }

        /// <summary>
        ///     Gets or Sets Level2
        /// </summary>
        [DataMember(Name = "level2", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "level2")]
        public UInt64DTO Level2 { get; set; }

        /// <summary>
        ///     Gets or Sets Type
        /// </summary>
        [DataMember(Name = "type", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "type")]
        public int? Type { get; set; }

        /// <summary>
        ///     Gets or Sets Alias
        /// </summary>
        [DataMember(Name = "alias", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "alias")]
        public AliasDTO Alias { get; set; }

        /// <summary>
        ///     Gets or Sets ParentId
        /// </summary>
        [DataMember(Name = "parentId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "parentId")]
        public UInt64DTO ParentId { get; set; }


        /// <summary>
        ///     Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class NamespaceDTO {\n");
            sb.Append("  Owner: ").Append(Owner).Append("\n");
            sb.Append("  OwnerAddress: ").Append(OwnerAddress).Append("\n");
            sb.Append("  StartHeight: ").Append(StartHeight).Append("\n");
            sb.Append("  EndHeight: ").Append(EndHeight).Append("\n");
            sb.Append("  Depth: ").Append(Depth).Append("\n");
            sb.Append("  Level0: ").Append(Level0).Append("\n");
            sb.Append("  Level1: ").Append(Level1).Append("\n");
            sb.Append("  Level2: ").Append(Level2).Append("\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  Alias: ").Append(Alias).Append("\n");
            sb.Append("  ParentId: ").Append(ParentId).Append("\n");
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