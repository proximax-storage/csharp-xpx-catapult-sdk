using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO {

  /// <summary>
  /// Transaction that creates or renew a namespace.
  /// </summary>
  [DataContract]
  public class RegisterNamespaceTransactionDTO : TransactionDTO {
    /// <summary>
    /// Gets or Sets NamespaceType
    /// </summary>
    [DataMember(Name="namespaceType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "namespaceType")]
    public NamespaceTypeEnum NamespaceType { get; set; }

    /// <summary>
    /// Gets or Sets Duration
    /// </summary>
    [DataMember(Name="duration", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "duration")]
    public UInt64DTO Duration { get; set; }

    /// <summary>
    /// Gets or Sets NamespaceId
    /// </summary>
    [DataMember(Name="namespaceId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "namespaceId")]
    public UInt64DTO NamespaceId { get; set; }

    /// <summary>
    /// The unique namespace name.
    /// </summary>
    /// <value>The unique namespace name.</value>
    [DataMember(Name="name", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "name")]
    public string Name { get; set; }

    /// <summary>
    /// Gets or Sets ParentId
    /// </summary>
    [DataMember(Name="parentId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "parentId")]
    public UInt64DTO ParentId { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class RegisterNamespaceTransactionDTO {\n");
      sb.Append("  NamespaceType: ").Append(NamespaceType).Append("\n");
      sb.Append("  Duration: ").Append(Duration).Append("\n");
      sb.Append("  NamespaceId: ").Append(NamespaceId).Append("\n");
      sb.Append("  Name: ").Append(Name).Append("\n");
      sb.Append("  ParentId: ").Append(ParentId).Append("\n");
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
