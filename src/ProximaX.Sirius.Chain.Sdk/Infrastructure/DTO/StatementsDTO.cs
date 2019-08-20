using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO {

  /// <summary>
  /// The collection of transaction statements and resolutions triggered for the block requested.
  /// </summary>
  [DataContract]
  public class StatementsDTO {
    /// <summary>
    /// The array of transaction statements for the block requested.
    /// </summary>
    /// <value>The array of transaction statements for the block requested.</value>
    [DataMember(Name="transactionStatements", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "transactionStatements")]
    public List<TransactionStatementDTO> TransactionStatements { get; set; }

    /// <summary>
    /// The array of address resolutions for the block requested.
    /// </summary>
    /// <value>The array of address resolutions for the block requested.</value>
    [DataMember(Name="addressResolutionStatements", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "addressResolutionStatements")]
    public List<ResolutionStatementDTO> AddressResolutionStatements { get; set; }

    /// <summary>
    /// The array of mosaic resolutions for the block requested.
    /// </summary>
    /// <value>The array of mosaic resolutions for the block requested.</value>
    [DataMember(Name="mosaicResolutionStatements", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "mosaicResolutionStatements")]
    public List<ResolutionStatementDTO> MosaicResolutionStatements { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class StatementsDTO {\n");
      sb.Append("  TransactionStatements: ").Append(TransactionStatements).Append("\n");
      sb.Append("  AddressResolutionStatements: ").Append(AddressResolutionStatements).Append("\n");
      sb.Append("  MosaicResolutionStatements: ").Append(MosaicResolutionStatements).Append("\n");
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
