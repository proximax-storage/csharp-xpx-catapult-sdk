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
  public class AddressAliasTransactionBodyDTO {
    /// <summary>
    /// Gets or Sets AliasAction
    /// </summary>
    [DataMember(Name="aliasAction", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "aliasAction")]
    public AliasActionEnum AliasAction { get; set; }

    /// <summary>
    /// Gets or Sets NamespaceId
    /// </summary>
    [DataMember(Name="namespaceId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "namespaceId")]
    public UInt64DTO NamespaceId { get; set; }

    /// <summary>
    /// The aliased address in hexadecimal.
    /// </summary>
    /// <value>The aliased address in hexadecimal.</value>
    [DataMember(Name="address", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "address")]
    public string Address { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class AddressAliasTransactionBodyDTO {\n");
      sb.Append("  AliasAction: ").Append(AliasAction).Append("\n");
      sb.Append("  NamespaceId: ").Append(NamespaceId).Append("\n");
      sb.Append("  Address: ").Append(Address).Append("\n");
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
