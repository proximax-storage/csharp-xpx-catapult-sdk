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
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO
{

    /// <summary>
    /// Delegates the account importance score to a proxy account.
    /// </summary>
    [DataContract]
  public class AccountLinkTransactionDTO : TransactionDTO {
    /// <summary>
    /// The public key of the remote account.
    /// </summary>
    /// <value>The public key of the remote account.</value>
    [DataMember(Name="remoteAccountKey", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "remoteAccountKey")]
    public string RemoteAccountKey { get; set; }

    /// <summary>
    /// Gets or Sets Action
    /// </summary>
    [DataMember(Name="action", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "action")]
    public LinkActionEnum Action { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class AccountLinkTransactionDTO {\n");
      sb.Append("  RemoteAccountKey: ").Append(RemoteAccountKey).Append("\n");
      sb.Append("  Action: ").Append(Action).Append("\n");
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
