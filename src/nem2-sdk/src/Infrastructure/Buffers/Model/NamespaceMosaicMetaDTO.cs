// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="NamespaceMosaicMetaDTO.cs" company="Nem.io">   
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

using Newtonsoft.Json;

namespace io.nem2.sdk.Infrastructure.Buffers.Model
{
    /// <summary>
    /// Class NamespaceMosaicMetaDTO.
    /// </summary>
    public class NamespaceMosaicMetaDTO
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="NamespaceMosaicMetaDTO"/> is active.
        /// </summary>
        /// <value><c>true</c> if active; otherwise, <c>false</c>.</value>
        [JsonProperty("active")]
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the index.
        /// </summary>
        /// <value>The index.</value>
        [JsonProperty("index")]
        public int Index { get; set; }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        [JsonProperty("id")]
        public string Id { get; set; }
    }
}
