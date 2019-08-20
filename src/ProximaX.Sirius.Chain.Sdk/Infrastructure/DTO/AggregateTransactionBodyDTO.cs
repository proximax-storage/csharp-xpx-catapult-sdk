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
  public class AggregateTransactionBodyDTO {
    /// <summary>
    /// An array of transaction cosignatures.
    /// </summary>
    /// <value>An array of transaction cosignatures.</value>
    [DataMember(Name="cosignatures", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "cosignatures")]
    public List<CosignatureDTO> Cosignatures { get; set; }

    /// <summary>
    /// The array of transactions initiated by different accounts.
    /// </summary>
    /// <value>The array of transactions initiated by different accounts.</value>
    [DataMember(Name="transactions", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "transactions")]
    public List<EmbeddedTransactionInfoDTO> Transactions { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class AggregateTransactionBodyDTO {\n");
      sb.Append("  Cosignatures: ").Append(Cosignatures).Append("\n");
      sb.Append("  Transactions: ").Append(Transactions).Append("\n");
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
