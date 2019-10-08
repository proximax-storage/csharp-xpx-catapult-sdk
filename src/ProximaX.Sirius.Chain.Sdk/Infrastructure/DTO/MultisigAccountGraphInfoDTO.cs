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
  public class MultisigAccountGraphInfoDTO {
    /// <summary>
    /// The level of the multisig account.
    /// </summary>
    /// <value>The level of the multisig account.</value>
    [DataMember(Name="level", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "level")]
    public int? Level { get; set; }

    /// <summary>
    /// The array of multisig accounts for this level.
    /// </summary>
    /// <value>The array of multisig accounts for this level.</value>
    [DataMember(Name="multisigEntries", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "multisigEntries")]
    public List<MultisigAccountInfoDTO> MultisigEntries { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class MultisigAccountGraphInfoDTO {\n");
      sb.Append("  Level: ").Append(Level).Append("\n");
      sb.Append("  MultisigEntries: ").Append(MultisigEntries).Append("\n");
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
