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
  public class EmbeddedBlockchainUpgradeTransactionDTO : EmbeddedTransactionDTO {
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
      sb.Append("class EmbeddedBlockchainUpgradeTransactionDTO {\n");
      sb.Append("  UpgradePeriod: ").Append(UpgradePeriod).Append("\n");
      sb.Append("  NewBlockChainVersion: ").Append(NewBlockChainVersion).Append("\n");
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
