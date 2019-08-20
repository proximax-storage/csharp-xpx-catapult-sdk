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
  public class NamespaceIds {
    /// <summary>
    /// The array of namespace identifiers.
    /// </summary>
    /// <value>The array of namespace identifiers.</value>
    [DataMember(Name="namespaceIds", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "namespaceIds")]
    public List<string> _NamespaceIds { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class NamespaceIds {\n");
      sb.Append("  _NamespaceIds: ").Append(_NamespaceIds).Append("\n");
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
