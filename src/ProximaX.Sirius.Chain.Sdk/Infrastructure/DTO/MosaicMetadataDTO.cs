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
  public class MosaicMetadataDTO : MetadataDTO {
    /// <summary>
    /// Gets or Sets MetadataType
    /// </summary>
    [DataMember(Name="metadataType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "metadataType")]
    public int? MetadataType { get; set; }

    /// <summary>
    /// Gets or Sets MetadataId
    /// </summary>
    [DataMember(Name="metadataId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "metadataId")]
    public UInt64DTO MetadataId { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class MosaicMetadataDTO {\n");
      sb.Append("  MetadataType: ").Append(MetadataType).Append("\n");
      sb.Append("  MetadataId: ").Append(MetadataId).Append("\n");
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
