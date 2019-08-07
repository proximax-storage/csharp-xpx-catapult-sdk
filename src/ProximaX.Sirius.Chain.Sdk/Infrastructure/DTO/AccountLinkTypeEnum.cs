using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO {

  /// <summary>
  /// The account link types: * 0 -  Unlinked. Account is not linked to another account. * 1 -  Main. Account is a balance-holding account that is linked to a remote harvester account. * 2 -  Remote. Account is a remote harvester account that is linked to a balance-holding account. * 3 -  Remote_Unlinked. Account is a remote harvester eligible account that is unlinked. 
  /// </summary>
  [DataContract]
  public class AccountLinkTypeEnum {

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class AccountLinkTypeEnum {\n");
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
