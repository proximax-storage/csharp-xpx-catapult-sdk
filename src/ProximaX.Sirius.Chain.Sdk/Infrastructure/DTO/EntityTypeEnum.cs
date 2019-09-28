using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO {

  /// <summary>
  /// The entity type: * 0x4158 (16728 decimal) - Blockchain Upgrade Transaction. * 0x4159 (16729 decimal) - Network Config Transaction. * 0x413D (16701 decimal) - Address Metadata Transaction. * 0x423D (16957 decimal) - Mosaic Metadata Transaction. * 0x433D (17213 decimal) - Namespace Metadata Transaction. * 0x414D (16717 decimal) - Mosaic Definition Transaction. * 0x424D (16973 decimal) - Mosaic Supply Change Transaction. * 0x414E (16718 decimal) - Register Namespace Transaction. * 0x424E (16974 decimal) - Address Alias Transaction. * 0x434E (17230 decimal) - Mosaic Alias Transaction. * 0x4154 (16724 decimal) - Transfer Transaction. * 0x4155 (16725 decimal) - Modify Multisig Account Transaction. * 0x4141 (16705 decimal) - Aggregate Complete Transaction. * 0x4241 (16961 decimal) - Aggregate Bonded Transaction. * 0x4148 (16712 decimal) - Hash Lock Transaction. * 0x4150 (16720 decimal) - Account Properties Address Transaction. * 0x4250 (16976 decimal) - Account Properties Mosaic Transaction. * 0x4350 (17232 decimal) - Account Properties Entity Type Transaction. * 0x4152 (16722 decimal) - Secret Lock Transaction. * 0x4252 (16978 decimal) - Secret Proof Transaction. * 0x414C (16716 decimal) - Account Link Transaction. * 0x8043 (32835 decimal) - Nemesis block. * 0x8143 (33091 decimal) - Regular block. 
  /// </summary>
  [DataContract]
  public class EntityTypeEnum {

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class EntityTypeEnum {\n");
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
