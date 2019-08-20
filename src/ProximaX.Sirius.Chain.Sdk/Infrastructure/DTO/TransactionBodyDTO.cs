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
  public class TransactionBodyDTO {
    /// <summary>
    /// Gets or Sets MaxFee
    /// </summary>
    [DataMember(Name="max_fee", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "max_fee")]
    public UInt64DTO MaxFee { get; set; }

    /// <summary>
    /// Gets or Sets Deadline
    /// </summary>
    [DataMember(Name="deadline", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "deadline")]
    public UInt64DTO Deadline { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class TransactionBodyDTO {\n");
      sb.Append("  MaxFee: ").Append(MaxFee).Append("\n");
      sb.Append("  Deadline: ").Append(Deadline).Append("\n");
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
