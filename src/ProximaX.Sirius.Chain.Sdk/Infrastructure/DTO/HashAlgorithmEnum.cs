using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO {

  /// <summary>
  /// The hash algorithm used to hash te proof: * 0 (Op_Sha3_256)  - The proof is hashed using sha3 256. * 1 (Op_Keccak_256)  - The proof is hashed using Keccak (ETH compatibility). * 2 (Op_Hash_160)  - The proof is hashed twice: first with Sha-256 and then with RIPEMD-160 (bitcoin’s OP_HASH160). * 3 (Op_Hash_256)  - The proof is hashed twice with Sha-256 (bitcoin’s OP_HASH256). 
  /// </summary>
  [DataContract]
  public class HashAlgorithmEnum {

    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class HashAlgorithmEnum {\n");
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
