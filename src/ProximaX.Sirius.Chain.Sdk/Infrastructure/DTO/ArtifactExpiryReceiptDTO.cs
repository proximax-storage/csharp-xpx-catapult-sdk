using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO {

  /// <summary>
  /// An artifact namespace or mosaic expired.
  /// </summary>
  [DataContract]
  public class ArtifactExpiryReceiptDTO : ReceiptDTO {
    /// <summary>
    /// Gets or Sets ArtifactId
    /// </summary>
    [DataMember(Name="artifactId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "artifactId")]
    public UInt64DTO ArtifactId { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class ArtifactExpiryReceiptDTO {\n");
      sb.Append("  ArtifactId: ").Append(ArtifactId).Append("\n");
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
