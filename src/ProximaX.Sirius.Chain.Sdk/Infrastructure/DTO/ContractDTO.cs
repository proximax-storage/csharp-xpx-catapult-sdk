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
using System.Runtime.Serialization;
using System.Text;
using Newtonsoft.Json;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO
{
    /// <summary>
    /// </summary>
    [DataContract]
    public class ContractDTO
    {
        /// <summary>
        ///     Gets or Sets Multisig
        /// </summary>
        [DataMember(Name = "multisig", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "multisig")]
        public string Multisig { get; set; }

        /// <summary>
        ///     Gets or Sets MultisigAddress
        /// </summary>
        [DataMember(Name = "multisigAddress", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "multisigAddress")]
        public string MultisigAddress { get; set; }

        /// <summary>
        ///     Gets or Sets Start
        /// </summary>
        [DataMember(Name = "start", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "start")]
        public UInt64DTO Start { get; set; }

        /// <summary>
        ///     Gets or Sets Duration
        /// </summary>
        [DataMember(Name = "duration", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "duration")]
        public UInt64DTO Duration { get; set; }

        /// <summary>
        ///     Gets or Sets Hash
        /// </summary>
        [DataMember(Name = "hash", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "hash")]
        public string Hash { get; set; }

        /// <summary>
        ///     Gets or Sets Customers
        /// </summary>
        [DataMember(Name = "customers", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "customers")]
        public List<string> Customers { get; set; }

        /// <summary>
        ///     Gets or Sets Executors
        /// </summary>
        [DataMember(Name = "executors", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "executors")]
        public List<string> Executors { get; set; }

        /// <summary>
        ///     Gets or Sets Verifiers
        /// </summary>
        [DataMember(Name = "verifiers", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "verifiers")]
        public List<string> Verifiers { get; set; }


        /// <summary>
        ///     Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ContractDTO {\n");
            sb.Append("  Multisig: ").Append(Multisig).Append("\n");
            sb.Append("  MultisigAddress: ").Append(MultisigAddress).Append("\n");
            sb.Append("  Start: ").Append(Start).Append("\n");
            sb.Append("  Duration: ").Append(Duration).Append("\n");
            sb.Append("  Hash: ").Append(Hash).Append("\n");
            sb.Append("  Customers: ").Append(Customers).Append("\n");
            sb.Append("  Executors: ").Append(Executors).Append("\n");
            sb.Append("  Verifiers: ").Append(Verifiers).Append("\n");
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