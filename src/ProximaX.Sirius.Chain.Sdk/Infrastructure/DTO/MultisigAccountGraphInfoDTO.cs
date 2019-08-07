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
  public class MultisigAccountGraphInfoDTO {
    /// <summary>
    /// The level of the multisig account.
    /// </summary>
    /// <value>The level of the multisig account.</value>
    [DataMember(Name="level", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "level")]
    public int? Level { get; set; }

    /// <summary>
    /// The array of multisig accounts for this level.
    /// </summary>
    /// <value>The array of multisig accounts for this level.</value>
    [DataMember(Name="multisigEntries", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "multisigEntries")]
    public List<MultisigAccountInfoDTO> MultisigEntries { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class MultisigAccountGraphInfoDTO {\n");
      sb.Append("  Level: ").Append(Level).Append("\n");
      sb.Append("  MultisigEntries: ").Append(MultisigEntries).Append("\n");
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
