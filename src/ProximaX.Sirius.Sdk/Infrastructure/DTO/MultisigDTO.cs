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

namespace ProximaX.Sirius.Sdk.Infrastructure.DTO
{
    /// <summary>
    /// </summary>
    [DataContract]
    public class MultisigDTO
    {
        /// <summary>
        ///     Gets or Sets Account
        /// </summary>
        [DataMember(Name = "account", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "account")]
        public string Account { get; set; }

        /// <summary>
        ///     Gets or Sets AccountAddress
        /// </summary>
        [DataMember(Name = "accountAddress", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "accountAddress")]
        public string AccountAddress { get; set; }

        /// <summary>
        ///     Gets or Sets MinApproval
        /// </summary>
        [DataMember(Name = "minApproval", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "minApproval")]
        public int MinApproval { get; set; }

        /// <summary>
        ///     Gets or Sets MinRemoval
        /// </summary>
        [DataMember(Name = "minRemoval", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "minRemoval")]
        public int MinRemoval { get; set; }

        /// <summary>
        ///     Gets or Sets Cosignatories
        /// </summary>
        [DataMember(Name = "cosignatories", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "cosignatories")]
        public List<string> Cosignatories { get; set; }

        /// <summary>
        ///     Gets or Sets MultisigAccounts
        /// </summary>
        [DataMember(Name = "multisigAccounts", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "multisigAccounts")]
        public List<string> MultisigAccounts { get; set; }


        /// <summary>
        ///     Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class MultisigDTO {\n");
            sb.Append("  Account: ").Append(Account).Append("\n");
            sb.Append("  AccountAddress: ").Append(AccountAddress).Append("\n");
            sb.Append("  MinApproval: ").Append(MinApproval).Append("\n");
            sb.Append("  MinRemoval: ").Append(MinRemoval).Append("\n");
            sb.Append("  Cosignatories: ").Append(Cosignatories).Append("\n");
            sb.Append("  MultisigAccounts: ").Append(MultisigAccounts).Append("\n");
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