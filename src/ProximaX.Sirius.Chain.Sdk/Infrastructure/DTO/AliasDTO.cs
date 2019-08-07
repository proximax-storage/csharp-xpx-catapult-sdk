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
  public class AliasDTO {
    /// <summary>
    /// Gets or Sets Type
    /// </summary>
    [DataMember(Name="type", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "type")]
    public AliasTypeEnum Type { get; set; }

    /// <summary>
    /// Gets or Sets MosaicId
    /// </summary>
    [DataMember(Name="mosaicId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "mosaicId")]
    public UInt64DTO MosaicId { get; set; }

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
      sb.Append("class AliasDTO {\n");
      sb.Append("  Type: ").Append(Type).Append("\n");
      sb.Append("  MosaicId: ").Append(MosaicId).Append("\n");
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
