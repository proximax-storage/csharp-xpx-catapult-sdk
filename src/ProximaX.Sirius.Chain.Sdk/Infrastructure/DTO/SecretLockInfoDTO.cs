
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
  public class SecretLockInfoDTO{
    /// <summary>
    /// Gets or Sets Account
    /// </summary>
    [DataMember(Name= "account", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "account")]
    public string Account { get; set; }

     /// <summary>
    /// Gets or Sets AccountAddress
    /// </summary>
    [DataMember(Name= "accountAddress", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "accountAddress")]
    public string AccountAddress { get; set; }

    /// <summary>
    /// Gets or Sets MosaicId
    /// </summary>
    [DataMember(Name= "mosaicId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "mosaicId")]
    public UInt64DTO MosaicId { get; set; }
    
    /// <summary>
    /// Gets or Sets Amount
    /// </summary>
    [DataMember(Name= "amount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "amount")]
    public UInt64DTO Amount { get; set; }

    /// <summary>
    /// Gets or Sets Height
    /// </summary>
    [DataMember(Name= "height", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "height")]
    public UInt64DTO Height { get; set; }

    /// <summary>
    /// Gets or Sets Status
    /// </summary>
    [DataMember(Name= "status", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "status")]
    public string Status { get; set; }

        /// <summary>
        /// Gets or Sets HashAlgorithm
        /// </summary>
    [DataMember(Name= "hashAlgorithm", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "hashAlgorithm")]
    public HashAlgorithmEnum HashAlgorithm { get; set; }

        /// <summary>
        /// Gets or Sets Secret
        /// </summary>
        [DataMember(Name = "secret", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "secret")]
        public string Secret { get; set; }

        /// <summary>
        /// Gets or Sets Recipient
        /// </summary>
        [DataMember(Name = "recipient", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "recipient")]
        public string Recipient { get; set; }

        /// <summary>
        /// Gets or Sets CompositeHash
        /// </summary>
        [DataMember(Name = "compositeHash", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "compositeHash")]
        public string CompositeHash { get; set; }
        /// <summary>
        /// Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class HashLockInfoDTO {\n");
      sb.Append("  Account: ").Append(Account).Append("\n");
      sb.Append("  AccountAddress: ").Append(AccountAddress).Append("\n");
      sb.Append("  MosaicId: ").Append(MosaicId).Append("\n");
      sb.Append("  Amount: ").Append(Amount).Append("\n");
      sb.Append("  Height: ").Append(Height).Append("\n");
      sb.Append("  Status: ").Append(Status).Append("\n");
      sb.Append("  HashAlgorithm: ").Append(HashAlgorithm).Append("\n");
            sb.Append("  Secret: ").Append(Secret).Append("\n");
            sb.Append("  Recipient: ").Append(Recipient).Append("\n");
            sb.Append("  CompositeHash: ").Append(CompositeHash).Append("\n");

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
