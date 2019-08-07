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
  public class ServerInfoDTO {
    /// <summary>
    /// The catapult-rest component version.
    /// </summary>
    /// <value>The catapult-rest component version.</value>
    [DataMember(Name="restVersion", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "restVersion")]
    public string RestVersion { get; set; }

    /// <summary>
    /// The catapult-sdk component version.
    /// </summary>
    /// <value>The catapult-sdk component version.</value>
    [DataMember(Name="sdkVersion", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "sdkVersion")]
    public string SdkVersion { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class ServerInfoDTO {\n");
      sb.Append("  RestVersion: ").Append(RestVersion).Append("\n");
      sb.Append("  SdkVersion: ").Append(SdkVersion).Append("\n");
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
