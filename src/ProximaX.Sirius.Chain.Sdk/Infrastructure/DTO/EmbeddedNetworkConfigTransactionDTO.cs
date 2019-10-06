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
  public class EmbeddedNetworkConfigTransactionDTO : EmbeddedTransactionDTO {
    /// <summary>
    /// Gets or Sets ApplyHeightDelta
    /// </summary>
    [DataMember(Name="applyHeightDelta", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "applyHeightDelta")]
    public UInt64DTO ApplyHeightDelta { get; set; }

    /// <summary>
    /// Network config like a raw text.
    /// </summary>
    /// <value>Network config like a raw text.</value>
    [DataMember(Name="networkConfig", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "networkConfig")]
    public string NetworkConfig { get; set; }

    /// <summary>
    /// Allowed versions of transaction in json format.
    /// </summary>
    /// <value>Allowed versions of transaction in json format.</value>
    [DataMember(Name="supportedEntityVersions", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "supportedEntityVersions")]
    public string SupportedEntityVersions { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class EmbeddedNetworkConfigTransactionDTO {\n");
      sb.Append("  ApplyHeightDelta: ").Append(ApplyHeightDelta).Append("\n");
      sb.Append("  NetworkConfig: ").Append(NetworkConfig).Append("\n");
      sb.Append("  SupportedEntityVersions: ").Append(SupportedEntityVersions).Append("\n");
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
