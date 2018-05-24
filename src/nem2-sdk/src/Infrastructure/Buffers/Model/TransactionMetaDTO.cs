// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="TransactionMetaDTO.cs" company="Nem.io">   
// Copyright 2018 NEM
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
// </copyright>
// <summary></summary>
// ***********************************************************************

using io.nem2.sdk.Infrastructure.Buffers.Model.JsonConverters;
using Newtonsoft.Json;

namespace io.nem2.sdk.Infrastructure.Buffers.Model
{
    /// <summary>
    /// Class TransactionMeta.
    /// </summary>
    public class TransactionMetaDTO
    {
        /// <summary>
        /// Gets or sets the hash.
        /// </summary>
        /// <value>The hash.</value>
        [JsonProperty("hash")]
        public string Hash { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        [JsonProperty("height")]
        [JsonConverter(typeof(UInt32ArrayToLong))]
        public ulong Height { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>The index.</value>
        [JsonProperty("index")]
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the merkle component hash.
        /// </summary>
        /// <value>The merkle component hash.</value>
        [JsonProperty("merkleComponentHash")]
        public string MerkleComponentHash { get; set; }

    }

    /// <summary>
    /// Class TransactionMeta.
    /// </summary>
    public class AggregateTransactionMetaDTO : TransactionMetaDTO
    {
        /// <summary>
        /// Gets or sets the aggregate hash.
        /// </summary>
        /// <value>The aggregate hash.</value>
        [JsonProperty("aggregateHash")]
        public string AggregateHash { get; set; }

        /// <summary>
        /// Gets or sets the aggregate identifier.
        /// </summary>
        /// <value>The aggregate identifier.</value>
        [JsonProperty("aggregateId")]
        public string AggregateId { get; set; }  
    }
}
