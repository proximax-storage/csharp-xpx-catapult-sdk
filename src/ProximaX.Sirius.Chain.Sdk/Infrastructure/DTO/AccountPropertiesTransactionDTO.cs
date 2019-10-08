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

using System.Text;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO
{

    /// <summary>
    /// Transaction that prevents receiving transactions from undesired addresses, mosaics or sending certain transaction types.
    /// </summary>
    [DataContract]
  public class AccountPropertiesTransactionDTO : TransactionDTO {
    /// <summary>
    /// Gets or Sets PropertyType
    /// </summary>
    [DataMember(Name="propertyType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "propertyType")]
    public AccountPropertyTypeEnum PropertyType { get; set; }

    /// <summary>
    /// Gets or Sets Modifications
    /// </summary>
    [DataMember(Name="modifications", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "modifications")]
    public List<AccountPropertiesModificationDTO> Modifications { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class AccountPropertiesTransactionDTO {\n");
      sb.Append("  PropertyType: ").Append(PropertyType).Append("\n");
      sb.Append("  Modifications: ").Append(Modifications).Append("\n");
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
