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
  public class MetadataDTO {
    /// <summary>
    /// Gets or Sets MetadataType
    /// </summary>
    [DataMember(Name="metadataType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "metadataType")]
    public int? MetadataType { get; set; }

    /// <summary>
    /// Gets or Sets Fields
    /// </summary>
    [DataMember(Name="fields", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "fields")]
    public List<FieldDTO> Fields { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class MetadataDTO {\n");
      sb.Append("  MetadataType: ").Append(MetadataType).Append("\n");
      sb.Append("  Fields: ").Append(Fields).Append("\n");
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
