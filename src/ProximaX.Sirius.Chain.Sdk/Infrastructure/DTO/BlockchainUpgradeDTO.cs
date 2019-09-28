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
  public class BlockchainUpgradeDTO {
    /// <summary>
    /// Gets or Sets BlockchainUpgrade
    /// </summary>
    [DataMember(Name="blockchainUpgrade", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "blockchainUpgrade")]
    public UpgradeDTO BlockchainUpgrade { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class BlockchainUpgradeDTO {\n");
      sb.Append("  BlockchainUpgrade: ").Append(BlockchainUpgrade).Append("\n");
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
