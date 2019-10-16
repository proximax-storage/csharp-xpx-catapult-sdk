// Copyright 2019 ProximaX
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
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
  public class BlockMetaDTO {
    /// <summary>
    /// Gets or Sets Hash
    /// </summary>
    [DataMember(Name="hash", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "hash")]
    public string Hash { get; set; }

    /// <summary>
    /// Gets or Sets GenerationHash
    /// </summary>
    [DataMember(Name="generationHash", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "generationHash")]
    public string GenerationHash { get; set; }

    /// <summary>
    /// Gets or Sets SubCacheMerkleRoots
    /// </summary>
    [DataMember(Name="subCacheMerkleRoots", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "subCacheMerkleRoots")]
    public List<string> SubCacheMerkleRoots { get; set; }

    /// <summary>
    /// Gets or Sets TotalFee
    /// </summary>
    [DataMember(Name="totalFee", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "totalFee")]
    public UInt64DTO TotalFee { get; set; }

    /// <summary>
    /// Gets or Sets NumTransactions
    /// </summary>
    [DataMember(Name="numTransactions", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "numTransactions")]
    public int? NumTransactions { get; set; }

    /// <summary>
    /// Gets or Sets NumStatements
    /// </summary>
    [DataMember(Name="numStatements", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "numStatements")]
    public int? NumStatements { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class BlockMetaDTO {\n");
      sb.Append("  Hash: ").Append(Hash).Append("\n");
      sb.Append("  GenerationHash: ").Append(GenerationHash).Append("\n");
      sb.Append("  SubCacheMerkleRoots: ").Append(SubCacheMerkleRoots).Append("\n");
      sb.Append("  TotalFee: ").Append(TotalFee).Append("\n");
      sb.Append("  NumTransactions: ").Append(NumTransactions).Append("\n");
      sb.Append("  NumStatements: ").Append(NumStatements).Append("\n");
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
