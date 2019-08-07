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
  public class BlockchainScoreDTO {
    /// <summary>
    /// Gets or Sets ScoreHigh
    /// </summary>
    [DataMember(Name="scoreHigh", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "scoreHigh")]
    public UInt64DTO ScoreHigh { get; set; }

    /// <summary>
    /// Gets or Sets ScoreLow
    /// </summary>
    [DataMember(Name="scoreLow", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "scoreLow")]
    public UInt64DTO ScoreLow { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class BlockchainScoreDTO {\n");
      sb.Append("  ScoreHigh: ").Append(ScoreHigh).Append("\n");
      sb.Append("  ScoreLow: ").Append(ScoreLow).Append("\n");
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
