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
  /// Transaction that transfers mosaics and messages to another account.
  /// </summary>
  [DataContract]
  public class TransferTransactionDTO : TransactionDTO {
    /// <summary>
    /// If the bit 0 of byte 0 is not set (like in 0x90), then it is a regular address. Else (e.g. 0x91) it represents a namespace id which starts at byte 1.
    /// </summary>
    /// <value>If the bit 0 of byte 0 is not set (like in 0x90), then it is a regular address. Else (e.g. 0x91) it represents a namespace id which starts at byte 1.</value>
    [DataMember(Name="recipient", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "recipient")]
    public string Recipient { get; set; }

    /// <summary>
    /// The array of mosaics sent to the recipient. If the most significant bit of byte 0 is set, a namespaceId (alias) is used instead of a instead of a mosaicId corresponds to a mosaicId.
    /// </summary>
    /// <value>The array of mosaics sent to the recipient. If the most significant bit of byte 0 is set, a namespaceId (alias) is used instead of a instead of a mosaicId corresponds to a mosaicId.</value>
    [DataMember(Name="mosaics", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "mosaics")]
    public List<MosaicDTO> Mosaics { get; set; }

    /// <summary>
    /// Gets or Sets Message
    /// </summary>
    [DataMember(Name="message", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "message")]
    public MessageDTO Message { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class TransferTransactionDTO {\n");
      sb.Append("  Recipient: ").Append(Recipient).Append("\n");
      sb.Append("  Mosaics: ").Append(Mosaics).Append("\n");
      sb.Append("  Message: ").Append(Message).Append("\n");
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
