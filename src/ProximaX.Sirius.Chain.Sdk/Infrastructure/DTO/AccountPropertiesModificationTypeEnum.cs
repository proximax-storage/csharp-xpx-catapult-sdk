using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO {

  /// <summary>
  /// The account properties modification type: * 0 - Add property. * 1 - Remove property. 
  /// </summary>
  [DataContract]
  public class AccountPropertiesModificationTypeEnum {

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class AccountPropertiesModificationTypeEnum {\n");
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