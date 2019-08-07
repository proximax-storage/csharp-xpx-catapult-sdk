using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO {

  /// <summary>
  /// Transaction that revealed a proof.
  /// </summary>
  [DataContract]
  public class SecretProofTransactionDTO : TransactionDTO {
    /// <summary>
    /// Gets or Sets HashAlgorithm
    /// </summary>
    [DataMember(Name="hashAlgorithm", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "hashAlgorithm")]
    public HashAlgorithmEnum HashAlgorithm { get; set; }

    /// <summary>
    /// The proof hashed.
    /// </summary>
    /// <value>The proof hashed.</value>
    [DataMember(Name="secret", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "secret")]
    public string Secret { get; set; }

    /// <summary>
    /// The address in hexadecimal that received the funds.
    /// </summary>
    /// <value>The address in hexadecimal that received the funds.</value>
    [DataMember(Name="recipient", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "recipient")]
    public string Recipient { get; set; }

    /// <summary>
    /// The original random set of bytes.
    /// </summary>
    /// <value>The original random set of bytes.</value>
    [DataMember(Name="proof", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "proof")]
    public string Proof { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class SecretProofTransactionDTO {\n");
      sb.Append("  HashAlgorithm: ").Append(HashAlgorithm).Append("\n");
      sb.Append("  Secret: ").Append(Secret).Append("\n");
      sb.Append("  Recipient: ").Append(Recipient).Append("\n");
      sb.Append("  Proof: ").Append(Proof).Append("\n");
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
