// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="NamespaceDTO.cs" company="Nem.io">   
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
    /// Class NamespaceDTO.
    /// </summary>
    public class NamespaceDTO
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [JsonProperty("type")]
        public byte Type { get; set; }

        /// <summary>
        /// Gets or sets the depth.
        /// </summary>
        /// <value>The depth.</value>
        [JsonProperty("depth")]
        public int Depth { get; set; }

        /// <summary>
        /// Gets or sets the level0.
        /// </summary>
        /// <value>The level0.</value>
        [JsonProperty("level0")]
        [JsonConverter(typeof(UInt32ArrayToLong))]
        public ulong Level0 { get; set; }

        /// <summary>
        /// Gets or sets the level1.
        /// </summary>
        /// <value>The level1.</value>
        [JsonProperty("level1")]
        [JsonConverter(typeof(UInt32ArrayToLong))]
        public ulong Level1 { get; set; }

        /// <summary>
        /// Gets or sets the level2.
        /// </summary>
        /// <value>The level2.</value>
        [JsonProperty("level2")]
        [JsonConverter(typeof(UInt32ArrayToLong))]
        public ulong Level2 { get; set; }

        /// <summary>
        /// Gets or sets the parent identifier.
        /// </summary>
        /// <value>The parent identifier.</value>
        [JsonProperty("parentId")]
        [JsonConverter(typeof(UInt32ArrayToLong))]
        public ulong ParentId { get; set; }

        /// <summary>
        /// Gets or sets the owner.
        /// </summary>
        /// <value>The owner.</value>
        [JsonProperty("owner")]
        public string Owner { get; set; }

        /// <summary>
        /// Gets or sets the start height.
        /// </summary>
        /// <value>The start height.</value>
        [JsonProperty("startHeight")]
        [JsonConverter(typeof(UInt32ArrayToLong))]
        public ulong StartHeight { get; set; }

        /// <summary>
        /// Gets or sets the end height.
        /// </summary>
        /// <value>The end height.</value>
        [JsonProperty("endHeight")]
        [JsonConverter(typeof(UInt32ArrayToLong))]
        public ulong EndHeight { get; set; }
    }
}
