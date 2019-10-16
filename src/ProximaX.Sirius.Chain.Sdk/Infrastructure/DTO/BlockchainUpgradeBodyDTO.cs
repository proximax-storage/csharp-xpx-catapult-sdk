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
  public class BlockchainUpgradeBodyDTO {
    /// <summary>
    /// Gets or Sets UpgradePeriod
    /// </summary>
    [DataMember(Name="upgradePeriod", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "upgradePeriod")]
    public UInt64DTO UpgradePeriod { get; set; }

    /// <summary>
    /// Gets or Sets NewBlockChainVersion
    /// </summary>
    [DataMember(Name="newBlockChainVersion", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "newBlockChainVersion")]
    public UInt64DTO NewBlockChainVersion { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class BlockchainUpgradeBodyDTO {\n");
      sb.Append("  UpgradePeriod: ").Append(UpgradePeriod).Append("\n");
      sb.Append("  NewBlockChainVersion: ").Append(NewBlockChainVersion).Append("\n");
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
