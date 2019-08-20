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
  public class CosignatoryModificationDTO {
    /// <summary>
    /// Gets or Sets ModificationType
    /// </summary>
    [DataMember(Name="modificationType", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "modificationType")]
    public MultisigModificationTypeEnum ModificationType { get; set; }

    /// <summary>
    /// The public key of the cosignatory account.
    /// </summary>
    /// <value>The public key of the cosignatory account.</value>
    [DataMember(Name="cosignatoryPublicKey", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "cosignatoryPublicKey")]
    public string CosignatoryPublicKey { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class CosignatoryModificationDTO {\n");
      sb.Append("  ModificationType: ").Append(ModificationType).Append("\n");
      sb.Append("  CosignatoryPublicKey: ").Append(CosignatoryPublicKey).Append("\n");
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
