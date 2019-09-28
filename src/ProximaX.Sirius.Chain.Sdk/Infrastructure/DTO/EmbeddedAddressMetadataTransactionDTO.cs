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
  public class EmbeddedAddressMetadataTransactionDTO : EmbeddedTransactionDTO {
    /// <summary>
    /// The address in hexadecimal.
    /// </summary>
    /// <value>The address in hexadecimal.</value>
    [DataMember(Name="metadataId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "metadataId")]
    public string MetadataId { get; set; }

    /// <summary>
    /// Gets or Sets MetadataType
    /// </summary>
    [DataMember(Name="metadataType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "metadataType")]
    public MetadataTypeEnum MetadataType { get; set; }

    /// <summary>
    /// The array of metadata modifications.
    /// </summary>
    /// <value>The array of metadata modifications.</value>
    [DataMember(Name="modifications", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "modifications")]
    public List<MetadataModificationDTO> Modifications { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class EmbeddedAddressMetadataTransactionDTO {\n");
      sb.Append("  MetadataId: ").Append(MetadataId).Append("\n");
      sb.Append("  MetadataType: ").Append(MetadataType).Append("\n");
      sb.Append("  Modifications: ").Append(Modifications).Append("\n");
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
