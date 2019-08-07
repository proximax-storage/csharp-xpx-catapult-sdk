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
  public class EmbeddedAccountPropertiesTransactionDTO : EmbeddedTransactionDTO {
    /// <summary>
    /// Gets or Sets PropertyType
    /// </summary>
    [DataMember(Name="propertyType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "propertyType")]
    public AccountPropertyTypeEnum PropertyType { get; set; }

    /// <summary>
    /// Gets or Sets Modifications
    /// </summary>
    [DataMember(Name="modifications", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "modifications")]
    public List<AccountPropertiesModificationDTO> Modifications { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class EmbeddedAccountPropertiesTransactionDTO {\n");
      sb.Append("  PropertyType: ").Append(PropertyType).Append("\n");
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
