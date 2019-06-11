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

namespace ProximaX.Sirius.Sdk.Infrastructure.DTO
{
    /// <summary>
    /// </summary>
    [DataContract]
    public class BlockchainStorageInfoDTO
    {
        /// <summary>
        ///     Gets or Sets NumBlocks
        /// </summary>
        [DataMember(Name = "numBlocks", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "numBlocks")]
        public int? NumBlocks { get; set; }

        /// <summary>
        ///     Gets or Sets NumTransactions
        /// </summary>
        [DataMember(Name = "numTransactions", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "numTransactions")]
        public int? NumTransactions { get; set; }

        /// <summary>
        ///     Gets or Sets NumAccounts
        /// </summary>
        [DataMember(Name = "numAccounts", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "numAccounts")]
        public int? NumAccounts { get; set; }


        /// <summary>
        ///     Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class BlockchainStorageInfoDTO {\n");
            sb.Append("  NumBlocks: ").Append(NumBlocks).Append("\n");
            sb.Append("  NumTransactions: ").Append(NumTransactions).Append("\n");
            sb.Append("  NumAccounts: ").Append(NumAccounts).Append("\n");
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