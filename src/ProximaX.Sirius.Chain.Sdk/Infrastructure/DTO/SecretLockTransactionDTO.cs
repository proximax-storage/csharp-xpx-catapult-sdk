using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO {

  /// <summary>
  /// Transaction that sends mosaics to a recipient if the proof used is revealed. If the duration is reached, the locked funds go back to the sender of the transaction.
  /// </summary>
  [DataContract]
  public class SecretLockTransactionDTO : TransactionDTO {
    /// <summary>
    /// Gets or Sets Duration
    /// </summary>
    [DataMember(Name="duration", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "duration")]
    public UInt64DTO Duration { get; set; }

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
    /// The address in hexadecimal that will receive the funds once the transaction is unlocked.
    /// </summary>
    /// <value>The address in hexadecimal that will receive the funds once the transaction is unlocked.</value>
    [DataMember(Name="recipient", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "recipient")]
    public string Recipient { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class SecretLockTransactionDTO {\n");
      sb.Append("  Duration: ").Append(Duration).Append("\n");
      sb.Append("  MosaicId: ").Append(MosaicId).Append("\n");
      sb.Append("  Amount: ").Append(Amount).Append("\n");
      sb.Append("  HashAlgorithm: ").Append(HashAlgorithm).Append("\n");
      sb.Append("  Secret: ").Append(Secret).Append("\n");
      sb.Append("  Recipient: ").Append(Recipient).Append("\n");
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
