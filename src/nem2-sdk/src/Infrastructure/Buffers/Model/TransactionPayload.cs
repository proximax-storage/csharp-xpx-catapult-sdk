// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="TransactionPayload.cs" company="Nem.io">   
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
    /// Class TransactionPayload.
    /// </summary>
    public class TransactionPayload
    {
        /// <summary>
        /// Gets or sets the payload.
        /// </summary>
        /// <value>The payload.</value>
        [JsonProperty("payload")]
        public string Payload { get; set; }

        /// <summary>
        /// Gets or sets the hash.
        /// </summary>
        /// <value>The hash.</value>
        [JsonProperty("hash")]
        public string Hash { get; set; }

        /// <summary>
        /// Gets or sets the signer.
        /// </summary>
        /// <value>The signer.</value>
        [JsonProperty("signer")]
        public string Signer { get; set; }
    }
}
