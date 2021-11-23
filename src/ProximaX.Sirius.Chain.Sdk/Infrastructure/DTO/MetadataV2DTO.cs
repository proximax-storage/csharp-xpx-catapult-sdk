// Copyright 2021 ProximaX
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
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO
{
    /// <summary>
    /// Class of MetadataV2DTO
    /// </summary>
    [DataContract]
    public class MetadataV2DTO
    {
        /// <summary>
        /// Gets or Sets Version
        /// </summary>
        [DataMember(Name = "version", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "version")]
        public int Version { get; set; }

        /// <summary>
        /// Gets or Sets Composite Hash
        /// </summary>
        [DataMember(Name = "compositeHash", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "compositeHash")]
        public string CompositeHash { get; set; }

        /// <summary>
        /// Gets or Sets Source Address
        /// </summary>
        [DataMember(Name = "sourceAddress", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "sourceAddress")]
        public string SourceAddress { get; set; }

        /// <summary>
        /// Gets or Sets Target Key
        /// </summary>
        [DataMember(Name = "targetKey", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "targetKey")]
        public string TargetKey { get; set; }

        /// <summary>
        /// Gets or Sets Scoped Metadata Key
        /// </summary>
        [DataMember(Name = "scopedMetadataKey", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "scopedMetadataKey")]
        public UInt64DTO ScopedMetadataKey { get; set; }

        /// <summary>
        /// Gets or Sets Target Id
        /// </summary>
        [DataMember(Name = "targetId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "targetId")]
        public UInt64DTO TargetId { get; set; }

        /// <summary>
        /// Gets or Sets Metadata Type
        /// </summary>
        [DataMember(Name = "metadataType", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "metadataType")]
        public int MetadataType { get; set; }

        /// <summary>
        /// Gets or Sets Value Size
        /// </summary>
        [DataMember(Name = "valueSize", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "valueSize")]
        public int ValueSize { get; set; }

        /// <summary>
        /// Gets or Sets Value
        /// </summary>
        [DataMember(Name = "value", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "value")]
        public string Value { get; set; }

        /// <summary>
        /// Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class MetadataV2DTO {\n");
            sb.Append("  Version: ").Append(Version).Append("\n");
            sb.Append("  CompositeHash: ").Append(CompositeHash).Append("\n");
            sb.Append("  SourceAddress: ").Append(SourceAddress).Append("\n");
            sb.Append("  TargetKey: ").Append(TargetKey).Append("\n");
            sb.Append("  ScopedMetadataKey: ").Append(ScopedMetadataKey).Append("\n");
            sb.Append("  TargetId: ").Append(TargetId).Append("\n");
            sb.Append("  MetadataType: ").Append(MetadataType).Append("\n");
            sb.Append("  ValueSize: ").Append(ValueSize).Append("\n");
            sb.Append("  Value: ").Append(Value).Append("\n");
            sb.Append("  Version: ").Append(Version).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Get the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}