using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO {

  /// <summary>
  /// The collection of receipts related to a transaction.
  /// </summary>
  [DataContract]
  public class TransactionStatementDTO {
    /// <summary>
    /// Gets or Sets Height
    /// </summary>
    [DataMember(Name="height", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "height")]
    public UInt64DTO Height { get; set; }

    /// <summary>
    /// Gets or Sets Source
    /// </summary>
    [DataMember(Name="source", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "source")]
    public SourceDTO Source { get; set; }

    /// <summary>
    /// The array of receipts.
    /// </summary>
    /// <value>The array of receipts.</value>
    [DataMember(Name="receipts", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "receipts")]
    public List<object> Receipts { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class TransactionStatementDTO {\n");
      sb.Append("  Height: ").Append(Height).Append("\n");
      sb.Append("  Source: ").Append(Source).Append("\n");
      sb.Append("  Receipts: ").Append(Receipts).Append("\n");
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
