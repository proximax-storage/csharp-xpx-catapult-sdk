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
  public class AccountPropertyDTO {
    /// <summary>
    /// Gets or Sets PropertyType
    /// </summary>
    [DataMember(Name="propertyType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "propertyType")]
    public AccountPropertyTypeEnum PropertyType { get; set; }

    /// <summary>
    /// The address, transaction type or mosaic id to filter.
    /// </summary>
    /// <value>The address, transaction type or mosaic id to filter.</value>
    [DataMember(Name="values", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "values")]
    public List<object> Values { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class AccountPropertyDTO {\n");
      sb.Append("  PropertyType: ").Append(PropertyType).Append("\n");
      sb.Append("  Values: ").Append(Values).Append("\n");
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
