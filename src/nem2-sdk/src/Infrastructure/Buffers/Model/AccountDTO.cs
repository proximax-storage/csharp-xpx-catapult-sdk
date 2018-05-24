// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="AccountDTO.cs" company="Nem.io">   
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

using System.Collections.Generic;
using io.nem2.sdk.Infrastructure.Buffers.Model.JsonConverters;
using Newtonsoft.Json;

namespace io.nem2.sdk.Infrastructure.Buffers.Model
{
    /// <summary>
    /// Class AccountDTO.
    /// </summary>
    public class AccountDTO
    {
        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>The address.</value>
        [JsonProperty("address")]
        [JsonConverter(typeof(HexToBase32))]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the height of the address.
        /// </summary>
        /// <value>The height of the address.</value>
        [JsonProperty("addressHeight")]
        [JsonConverter(typeof(UInt32ArrayToLong))]
        public ulong AddressHeight { get; set; }

        /// <summary>
        /// Gets or sets the public key.
        /// </summary>
        /// <value>The public key.</value>
        [JsonProperty("publicKey")]
        public string PublicKey { get; set; }

        /// <summary>
        /// Gets or sets the height of the public key.
        /// </summary>
        /// <value>The height of the public key.</value>
        [JsonProperty("publicKeyHeight")]
        [JsonConverter(typeof(UInt32ArrayToLong))]
        public ulong PublicKeyHeight { get; set; }

        /// <summary>
        /// Gets or sets the mosaics.
        /// </summary>
        /// <value>The mosaics.</value>
        [JsonProperty("mosaics")]
        public List<MosaicDTO> Mosaics { get; set; }

        /// <summary>
        /// Gets or sets the importance.
        /// </summary>
        /// <value>The importance.</value>
        [JsonProperty("importance")]
        [JsonConverter(typeof(UInt32ArrayToLong))]
        public ulong Importance { get; set; }

        /// <summary>
        /// Gets or sets the height of the importance.
        /// </summary>
        /// <value>The height of the importance.</value>
        [JsonProperty("importanceHeight")]
        [JsonConverter(typeof(UInt32ArrayToLong))]
        public ulong ImportanceHeight { get; set; }
    }
}
