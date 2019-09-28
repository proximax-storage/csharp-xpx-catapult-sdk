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
  public class AccountIds {
    /// <summary>
    /// The array of public keys.
    /// </summary>
    /// <value>The array of public keys.</value>
    [DataMember(Name="publicKeys", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "publicKeys")]
    public List<string> PublicKeys { get; set; }

    /// <summary>
    /// The array of addresses.
    /// </summary>
    /// <value>The array of addresses.</value>
    [DataMember(Name="addresses", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "addresses")]
    public List<string> Addresses { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class AccountIds {\n");
      sb.Append("  PublicKeys: ").Append(PublicKeys).Append("\n");
      sb.Append("  Addresses: ").Append(Addresses).Append("\n");
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
