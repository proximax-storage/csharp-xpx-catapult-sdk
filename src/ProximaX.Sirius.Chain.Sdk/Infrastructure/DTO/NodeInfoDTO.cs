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
    public class NodeInfoDTO
    {
        /// <summary>
        ///     Gets or Sets PublicKey
        /// </summary>
        [DataMember(Name = "publicKey", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "publicKey")]
        public string PublicKey { get; set; }

        /// <summary>
        ///     Gets or Sets Port
        /// </summary>
        [DataMember(Name = "port", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "port")]
        public int? Port { get; set; }

        /// <summary>
        ///     Gets or Sets NetworkIdentifier
        /// </summary>
        [DataMember(Name = "networkIdentifier", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "networkIdentifier")]
        public int? NetworkIdentifier { get; set; }

        /// <summary>
        ///     Gets or Sets Version
        /// </summary>
        [DataMember(Name = "version", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "version")]
        public int? Version { get; set; }

        /// <summary>
        ///     Gets or Sets Roles
        /// </summary>
        [DataMember(Name = "roles", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "roles")]
        public int? Roles { get; set; }

        /// <summary>
        ///     Gets or Sets Host
        /// </summary>
        [DataMember(Name = "host", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "host")]
        public string Host { get; set; }

        /// <summary>
        ///     Gets or Sets FriendlyName
        /// </summary>
        [DataMember(Name = "friendlyName", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "friendlyName")]
        public string FriendlyName { get; set; }


        /// <summary>
        ///     Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class NodeInfoDTO {\n");
            sb.Append("  PublicKey: ").Append(PublicKey).Append("\n");
            sb.Append("  Port: ").Append(Port).Append("\n");
            sb.Append("  NetworkIdentifier: ").Append(NetworkIdentifier).Append("\n");
            sb.Append("  Version: ").Append(Version).Append("\n");
            sb.Append("  Roles: ").Append(Roles).Append("\n");
            sb.Append("  Host: ").Append(Host).Append("\n");
            sb.Append("  FriendlyName: ").Append(FriendlyName).Append("\n");
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