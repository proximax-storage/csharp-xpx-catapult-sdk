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

using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO
{
    /// <summary>
    /// </summary>
    public class AccountDTO
    {
        /// <summary>
        ///     Gets or Sets Address
        /// </summary>

        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }

        /// <summary>
        ///     Gets or Sets AddressHeight
        /// </summary>

        [JsonProperty(PropertyName = "addressHeight")]
        public UInt64DTO AddressHeight { get; set; }

        /// <summary>
        ///     Gets or Sets PublicKey
        /// </summary>

        [JsonProperty(PropertyName = "publicKey")]
        public string PublicKey { get; set; }

        /// <summary>
        ///     Gets or Sets PublicKeyHeight
        /// </summary>

        [JsonProperty(PropertyName = "publicKeyHeight")]
        public UInt64DTO PublicKeyHeight { get; set; }

        /// <summary>
        ///     Gets or Sets Mosaics
        /// </summary>

        [JsonProperty(PropertyName = "mosaics")]
        public List<MosaicDTO> Mosaics { get; set; }


        /// <summary>
        ///     Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class AccountDTO {\n");
            sb.Append("  Address: ").Append(Address).Append("\n");
            sb.Append("  AddressHeight: ").Append(AddressHeight).Append("\n");
            sb.Append("  PublicKey: ").Append(PublicKey).Append("\n");
            sb.Append("  PublicKeyHeight: ").Append(PublicKeyHeight).Append("\n");
            sb.Append("  Mosaics: ").Append(Mosaics).Append("\n");
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