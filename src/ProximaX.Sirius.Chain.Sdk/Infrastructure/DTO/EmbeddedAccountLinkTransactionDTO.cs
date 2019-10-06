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
  public class EmbeddedAccountLinkTransactionDTO : EmbeddedTransactionDTO {
    /// <summary>
    /// The public key of the remote account.
    /// </summary>
    /// <value>The public key of the remote account.</value>
    [DataMember(Name="remoteAccountKey", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "remoteAccountKey")]
    public string RemoteAccountKey { get; set; }

    /// <summary>
    /// Gets or Sets Action
    /// </summary>
    [DataMember(Name="action", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "action")]
    public LinkActionEnum Action { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class EmbeddedAccountLinkTransactionDTO {\n");
      sb.Append("  RemoteAccountKey: ").Append(RemoteAccountKey).Append("\n");
      sb.Append("  Action: ").Append(Action).Append("\n");
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
