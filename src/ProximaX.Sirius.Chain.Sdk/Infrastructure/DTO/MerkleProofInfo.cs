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
  public class MerkleProofInfo {
    /// <summary>
    /// The complementary data needed to calculate the merkle root.
    /// </summary>
    /// <value>The complementary data needed to calculate the merkle root.</value>
    [DataMember(Name="merklePath", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "merklePath")]
    public List<MerklePathItem> MerklePath { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class MerkleProofInfo {\n");
      sb.Append("  MerklePath: ").Append(MerklePath).Append("\n");
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
