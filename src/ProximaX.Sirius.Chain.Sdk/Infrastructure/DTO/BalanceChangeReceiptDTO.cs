using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO {

  /// <summary>
  /// The invisible state change changed an account balance.
  /// </summary>
  [DataContract]
  public class BalanceChangeReceiptDTO : ReceiptDTO {
    /// <summary>
    /// The target account public key.
    /// </summary>
    /// <value>The target account public key.</value>
    [DataMember(Name="account", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "account")]
    public string Account { get; set; }

    /// <summary>
    /// Gets or Sets MosaicId
    /// </summary>
    [DataMember(Name="mosaicId", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "mosaicId")]
    public UInt64DTO MosaicId { get; set; }

    /// <summary>
    /// Gets or Sets Amount
    /// </summary>
    [DataMember(Name="amount", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "amount")]
    public UInt64DTO Amount { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class BalanceChangeReceiptDTO {\n");
      sb.Append("  Account: ").Append(Account).Append("\n");
      sb.Append("  MosaicId: ").Append(MosaicId).Append("\n");
      sb.Append("  Amount: ").Append(Amount).Append("\n");
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
