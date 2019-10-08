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
  public class BlockDTO : VerifiableEntityDTO {
    /// <summary>
    /// The public key of the entity signer formatted as hexadecimal.
    /// </summary>
    /// <value>The public key of the entity signer formatted as hexadecimal.</value>
    [DataMember(Name="signer", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "signer")]
    public string Signer { get; set; }

    /// <summary>
    /// Gets or Sets Version
    /// </summary>
    [DataMember(Name="version", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "version")]
    public NetworkTypeEnum Version { get; set; }

    /// <summary>
    /// Gets or Sets Type
    /// </summary>
    [DataMember(Name="type", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "type")]
    public EntityTypeEnum Type { get; set; }

    /// <summary>
    /// Gets or Sets Height
    /// </summary>
    [DataMember(Name="height", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "height")]
    public UInt64DTO Height { get; set; }

    /// <summary>
    /// Gets or Sets Timestamp
    /// </summary>
    [DataMember(Name="timestamp", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "timestamp")]
    public UInt64DTO Timestamp { get; set; }

    /// <summary>
    /// Gets or Sets Difficulty
    /// </summary>
    [DataMember(Name="difficulty", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "difficulty")]
    public UInt64DTO Difficulty { get; set; }

    /// <summary>
    /// The fee multiplier applied to transactions contained in block.
    /// </summary>
    /// <value>The fee multiplier applied to transactions contained in block.</value>
    [DataMember(Name="feeMultiplier", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "feeMultiplier")]
    public int? FeeMultiplier { get; set; }

    /// <summary>
    /// The hash of the previous block.
    /// </summary>
    /// <value>The hash of the previous block.</value>
    [DataMember(Name="previousBlockHash", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "previousBlockHash")]
    public string PreviousBlockHash { get; set; }

    /// <summary>
    /// The transactions included in a block are hashed forming a merkle tree. The root of the tree summarizes them. 
    /// </summary>
    /// <value>The transactions included in a block are hashed forming a merkle tree. The root of the tree summarizes them. </value>
    [DataMember(Name="blockTransactionsHash", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "blockTransactionsHash")]
    public string BlockTransactionsHash { get; set; }

    /// <summary>
    /// The collection of receipts  are hashed into a merkle tree and linked  to a block. The block header stores the root hash. 
    /// </summary>
    /// <value>The collection of receipts  are hashed into a merkle tree and linked  to a block. The block header stores the root hash. </value>
    [DataMember(Name="blockReceiptsHash", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "blockReceiptsHash")]
    public string BlockReceiptsHash { get; set; }

    /// <summary>
    /// For each block, the state of the blockchain is stored in RocksDB,  forming a patricia tree. The root of the tree summarizes the state of the blockchain for the given block. 
    /// </summary>
    /// <value>For each block, the state of the blockchain is stored in RocksDB,  forming a patricia tree. The root of the tree summarizes the state of the blockchain for the given block. </value>
    [DataMember(Name="stateHash", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "stateHash")]
    public string StateHash { get; set; }

    /// <summary>
    /// The public key of the optional beneficiary designated by harvester.
    /// </summary>
    /// <value>The public key of the optional beneficiary designated by harvester.</value>
    [DataMember(Name="beneficiary", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "beneficiary")]
    public string Beneficiary { get; set; }

    /// <summary>
    /// The part of the transaction fee harvester is willing to get. From 0 up to FeeInterestDenominator. The customer gets (FeeInterest / FeeInterestDenominator)'th part of the maximum transaction fee.
    /// </summary>
    /// <value>The part of the transaction fee harvester is willing to get. From 0 up to FeeInterestDenominator. The customer gets (FeeInterest / FeeInterestDenominator)'th part of the maximum transaction fee.</value>
    [DataMember(Name="feeInterest", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "feeInterest")]
    public int? FeeInterest { get; set; }

    /// <summary>
    /// Denominator of the transaction fee.
    /// </summary>
    /// <value>Denominator of the transaction fee.</value>
    [DataMember(Name="feeInterestDenominator", EmitDefaultValue=false)]
    [JsonProperty(PropertyName = "feeInterestDenominator")]
    public int? FeeInterestDenominator { get; set; }


    /// <summary>
    /// Get the string presentation of the object
    /// </summary>
    /// <returns>String presentation of the object</returns>
    public override string ToString()  {
      var sb = new StringBuilder();
      sb.Append("class BlockDTO {\n");
      sb.Append("  Signer: ").Append(Signer).Append("\n");
      sb.Append("  Version: ").Append(Version).Append("\n");
      sb.Append("  Type: ").Append(Type).Append("\n");
      sb.Append("  Height: ").Append(Height).Append("\n");
      sb.Append("  Timestamp: ").Append(Timestamp).Append("\n");
      sb.Append("  Difficulty: ").Append(Difficulty).Append("\n");
      sb.Append("  FeeMultiplier: ").Append(FeeMultiplier).Append("\n");
      sb.Append("  PreviousBlockHash: ").Append(PreviousBlockHash).Append("\n");
      sb.Append("  BlockTransactionsHash: ").Append(BlockTransactionsHash).Append("\n");
      sb.Append("  BlockReceiptsHash: ").Append(BlockReceiptsHash).Append("\n");
      sb.Append("  StateHash: ").Append(StateHash).Append("\n");
      sb.Append("  Beneficiary: ").Append(Beneficiary).Append("\n");
      sb.Append("  FeeInterest: ").Append(FeeInterest).Append("\n");
      sb.Append("  FeeInterestDenominator: ").Append(FeeInterestDenominator).Append("\n");
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
