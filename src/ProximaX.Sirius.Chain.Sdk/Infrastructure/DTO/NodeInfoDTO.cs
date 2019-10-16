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

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO {

  /// <summary>
  /// 
  /// </summary>
  [DataContract]
  public class NodeInfoDTO {
    /// <summary>
    /// The public key used to identify the node.
    /// </summary>
    /// <value>The public key used to identify the node.</value>
    [DataMember(Name="publicKey", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "publicKey")]
    public string PublicKey { get; set; }

    /// <summary>
    /// The port used for the communication.
    /// </summary>
    /// <value>The port used for the communication.</value>
    [DataMember(Name="port", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "port")]
    public int? Port { get; set; }

    /// <summary>
    /// Gets or Sets NetworkIdentifier
    /// </summary>
    [DataMember(Name="networkIdentifier", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "networkIdentifier")]
    public int? NetworkIdentifier { get; set; }

    /// <summary>
    /// The version of the application.
    /// </summary>
    /// <value>The version of the application.</value>
    [DataMember(Name="version", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "version")]
    public int? Version { get; set; }

    /// <summary>
    /// Gets or Sets Roles
    /// </summary>
    [DataMember(Name="roles", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "roles")]
    public RolesTypeEnum Roles { get; set; }

    /// <summary>
    /// The IP address of the endpoint.
    /// </summary>
    /// <value>The IP address of the endpoint.</value>
    [DataMember(Name="host", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "host")]
    public string Host { get; set; }

    /// <summary>
    /// The name of the node.
    /// </summary>
    /// <value>The name of the node.</value>
    [DataMember(Name="friendlyName", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "friendlyName")]
    public string FriendlyName { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
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
    /// Get the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public string ToJson() {
      return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

}
}
