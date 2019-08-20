using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO {

  /// <summary>
  /// The transaction that triggered the receipt.
  /// </summary>
  [DataContract]
  public class SourceDTO {
    /// <summary>
    /// The transaction index within the block.
    /// </summary>
    /// <value>The transaction index within the block.</value>
    [DataMember(Name="primaryId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "primaryId")]
    public int? PrimaryId { get; set; }

    /// <summary>
    /// The transaction index inside within the aggregate transaction. If the transaction is not an inner transaction, then the secondary id is set to 0.
    /// </summary>
    /// <value>The transaction index inside within the aggregate transaction. If the transaction is not an inner transaction, then the secondary id is set to 0.</value>
    [DataMember(Name="secondaryId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "secondaryId")]
    public int? SecondaryId { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class SourceDTO {\n");
      sb.Append("  PrimaryId: ").Append(PrimaryId).Append("\n");
      sb.Append("  SecondaryId: ").Append(SecondaryId).Append("\n");
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
