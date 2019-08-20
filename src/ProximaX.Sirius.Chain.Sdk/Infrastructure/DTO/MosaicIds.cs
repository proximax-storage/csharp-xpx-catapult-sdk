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
  public class MosaicIds {
    /// <summary>
    /// The array of mosaic identifiers.
    /// </summary>
    /// <value>The array of mosaic identifiers.</value>
    [DataMember(Name="mosaicIds", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "mosaicIds")]
    public List<string> _MosaicIds { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class MosaicIds {\n");
      sb.Append("  _MosaicIds: ").Append(_MosaicIds).Append("\n");
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
