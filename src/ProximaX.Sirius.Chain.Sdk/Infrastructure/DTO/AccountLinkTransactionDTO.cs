using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO {

  /// <summary>
  /// Delegates the account importance score to a proxy account.
  /// </summary>
  [DataContract]
  public class AccountLinkTransactionDTO : TransactionDTO {
    /// <summary>
    /// The public key of the remote account.
    /// </summary>
    /// <value>The public key of the remote account.</value>
    [DataMember(Name="remoteAccountKey", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "remoteAccountKey")]
    public string RemoteAccountKey { get; set; }

    /// <summary>
    /// Gets or Sets LinkAction
    /// </summary>
    [DataMember(Name="linkAction", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "linkAction")]
    public LinkActionEnum LinkAction { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class AccountLinkTransactionDTO {\n");
      sb.Append("  RemoteAccountKey: ").Append(RemoteAccountKey).Append("\n");
      sb.Append("  LinkAction: ").Append(LinkAction).Append("\n");
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
