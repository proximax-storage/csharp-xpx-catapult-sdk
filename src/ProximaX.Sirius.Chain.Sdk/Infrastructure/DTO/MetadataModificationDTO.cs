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
  public class MetadataModificationDTO {
    /// <summary>
    /// Gets or Sets ModificationType
    /// </summary>
    [DataMember(Name="modificationType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "modificationType")]
    public MetadataModificationTypeEnum ModificationType { get; set; }

    /// <summary>
    /// The key of metadata modification.
    /// </summary>
    /// <value>The key of metadata modification.</value>
    [DataMember(Name="key", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "key")]
    public string Key { get; set; }

    /// <summary>
    /// The value of metadata modification.
    /// </summary>
    /// <value>The value of metadata modification.</value>
    [DataMember(Name="value", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "value")]
    public string Value { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class MetadataModificationDTO {\n");
      sb.Append("  ModificationType: ").Append(ModificationType).Append("\n");
      sb.Append("  Key: ").Append(Key).Append("\n");
      sb.Append("  Value: ").Append(Value).Append("\n");
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
