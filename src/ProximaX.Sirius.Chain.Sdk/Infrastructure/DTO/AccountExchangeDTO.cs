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
using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class AccountExchangeDTO
    {
        /// <summary>
        /// Gets or Sets owner
        /// </summary>
        [DataMember(Name = "owner", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "owner")]
        public string Owner { get; set; }

        /// <summary>
        /// Gets or Sets ownerAddress
        /// </summary>
        [DataMember(Name = "ownerAddress", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "ownerAddress")]
        public string OwnerAddress { get; set; }

        /// <summary>
        /// Gets or Sets buyOffers
        /// </summary>
        [DataMember(Name = "buyOffers", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "buyOffers")]
        public List<OfferInfoDTO> BuyOffers { get; set; }
        /// <summary>
        /// Gets or Sets sellOffers
        /// </summary>
        [DataMember(Name = "sellOffers", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "sellOffers")]
        public List<OfferInfoDTO> SellOffers { get; set; }
        /// <summary>
        /// Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class AccountExchangeDTO {\n");
            sb.Append("  Owner: ").Append(Owner).Append("\n");
            sb.Append("  OwnerAddress: ").Append(OwnerAddress).Append("\n");
            sb.Append("  BuyOffers: ").Append(BuyOffers).Append("\n");
            sb.Append("  SellOffers: ").Append(SellOffers).Append("\n");
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
