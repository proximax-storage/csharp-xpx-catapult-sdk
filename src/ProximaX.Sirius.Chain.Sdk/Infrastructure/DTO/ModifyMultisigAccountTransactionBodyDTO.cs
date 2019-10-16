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
  public class ModifyMultisigAccountTransactionBodyDTO {
    /// <summary>
    /// The number of signatures needed to remove a cosignatory. If we are modifying an existing multisig account, this indicates the relative change of the minimum cosignatories. 
    /// </summary>
    /// <value>The number of signatures needed to remove a cosignatory. If we are modifying an existing multisig account, this indicates the relative change of the minimum cosignatories. </value>
    [DataMember(Name="minRemovalDelta", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "minRemovalDelta")]
    public int? MinRemovalDelta { get; set; }

    /// <summary>
    /// The number of signatures needed to approve a transaction. If we are modifying an existing multisig account, this indicates the relative change of the minimum cosignatories. 
    /// </summary>
    /// <value>The number of signatures needed to approve a transaction. If we are modifying an existing multisig account, this indicates the relative change of the minimum cosignatories. </value>
    [DataMember(Name="minApprovalDelta", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "minApprovalDelta")]
    public int? MinApprovalDelta { get; set; }

    /// <summary>
    /// The array of cosignatory accounts to add or delete.
    /// </summary>
    /// <value>The array of cosignatory accounts to add or delete.</value>
    [DataMember(Name="modifications", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "modifications")]
    public List<CosignatoryModificationDTO> Modifications { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class ModifyMultisigAccountTransactionBodyDTO {\n");
      sb.Append("  MinRemovalDelta: ").Append(MinRemovalDelta).Append("\n");
      sb.Append("  MinApprovalDelta: ").Append(MinApprovalDelta).Append("\n");
      sb.Append("  Modifications: ").Append(Modifications).Append("\n");
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
