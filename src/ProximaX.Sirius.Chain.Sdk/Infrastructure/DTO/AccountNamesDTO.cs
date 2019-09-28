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
  public class AccountNamesDTO {
    /// <summary>
    /// The address of the account in hexadecimal.
    /// </summary>
    /// <value>The address of the account in hexadecimal.</value>
    [DataMember(Name="address", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "address")]
    public string Address { get; set; }

    /// <summary>
    /// The mosaic linked namespace names.
    /// </summary>
    /// <value>The mosaic linked namespace names.</value>
    [DataMember(Name="names", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "names")]
    public List<string> Names { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class AccountNamesDTO {\n");
      sb.Append("  Address: ").Append(Address).Append("\n");
      sb.Append("  Names: ").Append(Names).Append("\n");
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
