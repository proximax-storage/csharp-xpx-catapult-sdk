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
  public class StorageInfoDTO {
    /// <summary>
    /// The number of blocks stored.
    /// </summary>
    /// <value>The number of blocks stored.</value>
    [DataMember(Name="numBlocks", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "numBlocks")]
    public int? NumBlocks { get; set; }

    /// <summary>
    /// The number of transactions stored.
    /// </summary>
    /// <value>The number of transactions stored.</value>
    [DataMember(Name="numTransactions", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "numTransactions")]
    public int? NumTransactions { get; set; }

    /// <summary>
    /// The number of accounts created.
    /// </summary>
    /// <value>The number of accounts created.</value>
    [DataMember(Name="numAccounts", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "numAccounts")]
    public int? NumAccounts { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class StorageInfoDTO {\n");
      sb.Append("  NumBlocks: ").Append(NumBlocks).Append("\n");
      sb.Append("  NumTransactions: ").Append(NumTransactions).Append("\n");
      sb.Append("  NumAccounts: ").Append(NumAccounts).Append("\n");
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
