// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="BlockDTO.cs" company="Nem.io">   
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
    /// Class BlockDTO.
    /// </summary>
    public class BlockDTO
    {
        /// <summary>
        /// Gets or sets the signature.
        /// </summary>
        /// <value>The signature.</value>
        [JsonProperty("signature")]
        public string Signature { get; set; }

        /// <summary>
        /// Gets or sets the signer.
        /// </summary>
        /// <value>The signer.</value>
        [JsonProperty("signer")]
        public string Signer { get; set; }

        /// <summary>
        /// Gets or sets the version.
        /// </summary>
        /// <value>The version.</value>
        [JsonProperty("version")]
        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [JsonProperty("type")]
        public int Type { get; set; }

        /// <summary>
        /// Gets or sets the height.
        /// </summary>
        /// <value>The height.</value>
        [JsonProperty("height")]
        [JsonConverter(typeof(UInt32ArrayToLong))]
        public ulong Height { get; set; }

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        /// <value>The timestamp.</value>
        [JsonProperty("timestamp")]
        [JsonConverter(typeof(UInt32ArrayToLong))]
        public ulong Timestamp { get; set; }

        /// <summary>
        /// Gets or sets the difficulty.
        /// </summary>
        /// <value>The difficulty.</value>
        [JsonProperty("difficulty")]
        [JsonConverter(typeof(UInt32ArrayToLong))]
        public ulong Difficulty { get; set; }

        /// <summary>
        /// Gets or sets the previous block hash.
        /// </summary>
        /// <value>The previous block hash.</value>
        [JsonProperty("previousBlockHash")]
        public string PreviousBlockHash { get; set; }

        /// <summary>
        /// Gets or sets the block transactions hash.
        /// </summary>
        /// <value>The block transactions hash.</value>
        [JsonProperty("blockTransactionsHash")]
        public string BlockTransactionsHash { get; set; }
    }
}
