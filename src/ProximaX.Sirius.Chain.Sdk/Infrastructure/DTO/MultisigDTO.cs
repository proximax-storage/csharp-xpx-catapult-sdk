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
  public class MultisigDTO {
    /// <summary>
    /// The account public key.
    /// </summary>
    /// <value>The account public key.</value>
    [DataMember(Name="account", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "account")]
    public string Account { get; set; }

    /// <summary>
    /// The account address in hexadecimal.
    /// </summary>
    /// <value>The account address in hexadecimal.</value>
    [DataMember(Name="accountAddress", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "accountAddress")]
    public string AccountAddress { get; set; }

    /// <summary>
    /// The number of signatures needed to approve a transaction.
    /// </summary>
    /// <value>The number of signatures needed to approve a transaction.</value>
    [DataMember(Name="minApproval", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "minApproval")]
    public int? MinApproval { get; set; }

    /// <summary>
    /// The number of signatures needed to remove a cosignatory.
    /// </summary>
    /// <value>The number of signatures needed to remove a cosignatory.</value>
    [DataMember(Name="minRemoval", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "minRemoval")]
    public int? MinRemoval { get; set; }

    /// <summary>
    /// The array of public keys of the cosignatory accounts.
    /// </summary>
    /// <value>The array of public keys of the cosignatory accounts.</value>
    [DataMember(Name="cosignatories", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "cosignatories")]
    public List<string> Cosignatories { get; set; }

    /// <summary>
    /// The array of multisig accounts where the account is cosignatory.
    /// </summary>
    /// <value>The array of multisig accounts where the account is cosignatory.</value>
    [DataMember(Name="multisigAccounts", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "multisigAccounts")]
    public List<string> MultisigAccounts { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class MultisigDTO {\n");
      sb.Append("  Account: ").Append(Account).Append("\n");
      sb.Append("  AccountAddress: ").Append(AccountAddress).Append("\n");
      sb.Append("  MinApproval: ").Append(MinApproval).Append("\n");
      sb.Append("  MinRemoval: ").Append(MinRemoval).Append("\n");
      sb.Append("  Cosignatories: ").Append(Cosignatories).Append("\n");
      sb.Append("  MultisigAccounts: ").Append(MultisigAccounts).Append("\n");
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
