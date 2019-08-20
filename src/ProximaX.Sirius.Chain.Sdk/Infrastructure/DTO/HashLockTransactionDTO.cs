using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO {

  /// <summary>
  /// Transaction to lock funds before sending an aggregate bonded transaction.
  /// </summary>
  [DataContract]
  public class HashLockTransactionDTO : TransactionDTO {
    /// <summary>
    /// Gets or Sets Mosaic
    /// </summary>
    [DataMember(Name="mosaic", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "mosaic")]
    public MosaicDTO Mosaic { get; set; }

    /// <summary>
    /// Gets or Sets Duration
    /// </summary>
    [DataMember(Name="duration", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "duration")]
    public MosaicDTO Duration { get; set; }

    /// <summary>
    /// The aggregate bonded transaction hash that has to be confirmed before unlocking the mosaics. 
    /// </summary>
    /// <value>The aggregate bonded transaction hash that has to be confirmed before unlocking the mosaics. </value>
    [DataMember(Name="hash", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "hash")]
    public string Hash { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class HashLockTransactionDTO {\n");
      sb.Append("  Mosaic: ").Append(Mosaic).Append("\n");
      sb.Append("  Duration: ").Append(Duration).Append("\n");
      sb.Append("  Hash: ").Append(Hash).Append("\n");
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
