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
  public class NamespaceNameDTO {
    /// <summary>
    /// Gets or Sets NamespaceId
    /// </summary>
    [DataMember(Name="namespaceId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "namespaceId")]
    public UInt64DTO NamespaceId { get; set; }

    /// <summary>
    /// The full name of the namespace.
    /// </summary>
    /// <value>The full name of the namespace.</value>
    [DataMember(Name="name", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class NamespaceNameDTO {\n");
      sb.Append("  NamespaceId: ").Append(NamespaceId).Append("\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
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
