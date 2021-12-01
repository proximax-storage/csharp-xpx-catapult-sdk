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
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO
{

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class OfferInfoDTO
    {
        /// <summary>
        /// Gets or Sets mosaicId
        /// </summary>
        [DataMember(Name = "mosaicId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "mosaicId")]
        public UInt64DTO MosaicId { get; set; }

        /// <summary>
        /// Gets or Sets amount
        /// </summary>
        [DataMember(Name = "amount", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "amount")]
        public UInt64DTO Amount { get; set; }

        /// <summary>
        /// Gets or Sets initial amount
        /// </summary>
        [DataMember(Name = "initialAmount", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "initialAmount")]
        public UInt64DTO InitialAmount { get; set; }
        /// <summary>
        /// Gets or Sets initial cost
        /// </summary>
        [DataMember(Name = "initialCost", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "initialCost")]
        public UInt64DTO InitialCost { get; set; }
        /// <summary>
        /// Gets or Sets deadline
        /// </summary>
        [DataMember(Name = "deadline", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "deadline")]
        public UInt64DTO Deadline { get; set; }
        /// <summary>
        /// Gets or Sets price
        /// </summary>
        [DataMember(Name = "price", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "price")]
        public int Price { get; set; }
        /// <summary>
        /// Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class OfferInfoDTO {\n");
            sb.Append("  MosaicId: ").Append(MosaicId).Append("\n");
            sb.Append("  Amount: ").Append(Amount).Append("\n");
            sb.Append("  InitialAmount: ").Append(InitialAmount).Append("\n");
            sb.Append("  InitialCost: ").Append(InitialCost).Append("\n");
            sb.Append("  Deadline: ").Append(Deadline).Append("\n");
            sb.Append("  Price: ").Append(Price).Append("\n");

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
