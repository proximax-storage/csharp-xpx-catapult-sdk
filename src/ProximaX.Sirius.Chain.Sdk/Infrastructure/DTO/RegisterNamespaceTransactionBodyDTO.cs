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
  public class RegisterNamespaceTransactionBodyDTO {
    /// <summary>
    /// Gets or Sets NamespaceType
    /// </summary>
    [DataMember(Name="namespaceType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "namespaceType")]
    public NamespaceTypeEnum NamespaceType { get; set; }

    /// <summary>
    /// Gets or Sets Duration
    /// </summary>
    [DataMember(Name="duration", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "duration")]
    public UInt64DTO Duration { get; set; }

    /// <summary>
    /// Gets or Sets NamespaceId
    /// </summary>
    [DataMember(Name="namespaceId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "namespaceId")]
    public UInt64DTO NamespaceId { get; set; }

    /// <summary>
    /// The unique namespace name.
    /// </summary>
    /// <value>The unique namespace name.</value>
    [DataMember(Name="name", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or Sets ParentId
    /// </summary>
    [DataMember(Name="parentId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "parentId")]
    public UInt64DTO ParentId { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class RegisterNamespaceTransactionBodyDTO {\n");
      sb.Append("  NamespaceType: ").Append(NamespaceType).Append("\n");
      sb.Append("  Duration: ").Append(Duration).Append("\n");
      sb.Append("  NamespaceId: ").Append(NamespaceId).Append("\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  ParentId: ").Append(ParentId).Append("\n");
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
