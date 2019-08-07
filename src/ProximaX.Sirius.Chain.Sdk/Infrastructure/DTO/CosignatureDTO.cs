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
  public class CosignatureDTO : VerifiableEntityDTO {
    /// <summary>
    /// The public key of the transaction signer.
    /// </summary>
    /// <value>The public key of the transaction signer.</value>
    [DataMember(Name="signer", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "signer")]
    public string Signer { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class CosignatureDTO {\n");
      sb.Append("  Signer: ").Append(Signer).Append("\n");
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
