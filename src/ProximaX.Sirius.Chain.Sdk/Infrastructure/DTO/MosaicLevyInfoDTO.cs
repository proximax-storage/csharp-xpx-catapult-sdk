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
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO
{
    /// <summary>
    ///
    /// </summary>
    [DataContract]
    public class MosaicLevyInfoDTO
    {
        /// <summary>
        /// Gets or Sets type
        /// </summary>
        [DataMember(Name = "type", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "type")]
        public MosaicLevyTypeEnum Type { get; set; }

        /// <summary>
        /// Gets or Sets recipient
        /// </summary>
        [DataMember(Name = "recipient", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "recipient")]
        public string Recipient { get; set; }

        /// <summary>
        /// Gets or Sets mosaicId
        /// </summary>
        [DataMember(Name = "mosaicId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "mosaicId")]
        public UInt64DTO mosaic { get; set; }

        /// <summary>
        /// Gets or Sets fee
        /// </summary>
        [DataMember(Name = "fee", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "fee")]
        public UInt64DTO Fee { get; set; }

        /// <summary>
        /// Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class MosaicLevyInfoDTO {\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  Recipient: ").Append(Recipient).Append("\n");
            sb.Append("  MosaicId: ").Append(mosaic).Append("\n");
            sb.Append("  Fee: ").Append(Fee).Append("\n");
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