using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO {

  /// <summary>
  /// A resolution statement keeps the relation between a namespace alias used in a transaction and the real address or mosaicId.
  /// </summary>
  [DataContract]
  public class ResolutionStatementDTO {
    /// <summary>
    /// Gets or Sets Height
    /// </summary>
    [DataMember(Name="height", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "height")]
    public UInt64DTO Height { get; set; }

    /// <summary>
    /// Gets or Sets Unresolved
    /// </summary>
    [DataMember(Name="unresolved", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "unresolved")]
    public UInt64DTO Unresolved { get; set; }

    /// <summary>
    /// The array of resolution entries linked to the unresolved namespaceId. It is an array instead of a single UInt64 field since within one block the resolution might change for different sources due to alias related transactions.
    /// </summary>
    /// <value>The array of resolution entries linked to the unresolved namespaceId. It is an array instead of a single UInt64 field since within one block the resolution might change for different sources due to alias related transactions.</value>
    [DataMember(Name="resolutionEntries", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "resolutionEntries")]
    public List<ResolutionEntryDTO> ResolutionEntries { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class ResolutionStatementDTO {\n");
      sb.Append("  Height: ").Append(Height).Append("\n");
      sb.Append("  Unresolved: ").Append(Unresolved).Append("\n");
      sb.Append("  ResolutionEntries: ").Append(ResolutionEntries).Append("\n");
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
