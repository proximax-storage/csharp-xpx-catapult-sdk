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
  /// Transaction that updates config.
  /// </summary>
  [DataContract]
  public class NetworkConfigTransactionDTO : TransactionDTO {
    /// <summary>
    /// Gets or Sets ApplyHeightDelta
    /// </summary>
    [DataMember(Name="applyHeightDelta", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "applyHeightDelta")]
    public UInt64DTO ApplyHeightDelta { get; set; }

    /// <summary>
    /// Network config like a raw text.
    /// </summary>
    /// <value>Network config like a raw text.</value>
    [DataMember(Name="networkConfig", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "networkConfig")]
    public string NetworkConfig { get; set; }

    /// <summary>
    /// Allowed versions of transaction in json format.
    /// </summary>
    /// <value>Allowed versions of transaction in json format.</value>
    [DataMember(Name="supportedEntityVersions", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "supportedEntityVersions")]
    public string SupportedEntityVersions { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class NetworkConfigTransactionDTO {\n");
      sb.Append("  ApplyHeightDelta: ").Append(ApplyHeightDelta).Append("\n");
      sb.Append("  NetworkConfig: ").Append(NetworkConfig).Append("\n");
      sb.Append("  SupportedEntityVersions: ").Append(SupportedEntityVersions).Append("\n");
      sb.Append("}\n");
      return sb.ToString();
    }

    /// <summary>
    /// Get the JSON string presentation of the object
    /// </summary>
    /// <returns>JSON string presentation of the object</returns>
    public  new string ToJson() {
      return JsonConvert.SerializeObject(this, Formatting.Indented);
    }

}
}
