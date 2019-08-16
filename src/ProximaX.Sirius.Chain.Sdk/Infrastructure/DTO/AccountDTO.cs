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
    /// 
    /// </summary>
    [DataContract]
  public class AccountDTO {
    /// <summary>
    /// The account unique address in hexadecimal. 
    /// </summary>
    /// <value>The account unique address in hexadecimal. </value>
    [DataMember(Name="address", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "address")]
    public string Address { get; set; }

    /// <summary>
    /// Gets or Sets AddressHeight
    /// </summary>
    [DataMember(Name="addressHeight", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "addressHeight")]
    public UInt64DTO AddressHeight { get; set; }

    /// <summary>
    /// The public key of an account can be used to verify signatures of the account. Only accounts that have already published a transaction have a public key assigned to the account. Otherwise, the field is null. 
    /// </summary>
    /// <value>The public key of an account can be used to verify signatures of the account. Only accounts that have already published a transaction have a public key assigned to the account. Otherwise, the field is null. </value>
    [DataMember(Name="publicKey", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "publicKey")]
    public string PublicKey { get; set; }

    /// <summary>
    /// Gets or Sets PublicKeyHeight
    /// </summary>
    [DataMember(Name="publicKeyHeight", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "publicKeyHeight")]
    public UInt64DTO PublicKeyHeight { get; set; }

    /// <summary>
    /// The list of mosaics the account owns. The amount is represented in absolute amount. Thus a balance of 123456789 for a mosaic with divisibility 6 (absolute) means the account owns 123.456789 instead. 
    /// </summary>
    /// <value>The list of mosaics the account owns. The amount is represented in absolute amount. Thus a balance of 123456789 for a mosaic with divisibility 6 (absolute) means the account owns 123.456789 instead. </value>
    [DataMember(Name="mosaics", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "mosaics")]
    public List<MosaicDTO> Mosaics { get; set; }

    /// <summary>
    /// Gets or Sets AccountType
    /// </summary>
    [DataMember(Name="accountType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "accountType")]
    public AccountLinkTypeEnum AccountType { get; set; }

    /// <summary>
    /// The public key of a linked account. The linked account can use|provide balance for delegated harvesting. 
    /// </summary>
    /// <value>The public key of a linked account. The linked account can use|provide balance for delegated harvesting. </value>
    [DataMember(Name="linkedAccountKey", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "linkedAccountKey")]
    public string LinkedAccountKey { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class AccountDTO {\n");
      sb.Append("  Address: ").Append(Address).Append("\n");
      sb.Append("  AddressHeight: ").Append(AddressHeight).Append("\n");
      sb.Append("  PublicKey: ").Append(PublicKey).Append("\n");
      sb.Append("  PublicKeyHeight: ").Append(PublicKeyHeight).Append("\n");
      sb.Append("  Mosaics: ").Append(Mosaics).Append("\n");
      sb.Append("  AccountType: ").Append(AccountType).Append("\n");
      sb.Append("  LinkedAccountKey: ").Append(LinkedAccountKey).Append("\n");
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
