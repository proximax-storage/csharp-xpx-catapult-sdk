// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="MultisigAccountGraphInfoDTO.cs" company="Nem.io">   
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
    /// Class MultisigAccountGraphInfoDTO.
    /// </summary>
    public class MultisigAccountGraphInfoDTO
    {
        /// <summary>
        /// Gets or sets the level.
        /// </summary>
        /// <value>The level.</value>
        [JsonProperty("level")]
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets the multisig entries.
        /// </summary>
        /// <value>The multisig entries.</value>
        [JsonProperty("multisigEntries")]
        public MultisigEntryDTO[] MultisigEntries { get; set; }
    }

    /// <summary>
    /// Class MultisigEntryDTO.
    /// </summary>
    public class MultisigEntryDTO
    {
        /// <summary>
        /// Gets or sets the multisig.
        /// </summary>
        /// <value>The multisig.</value>
        [JsonProperty("multisig")]
        public MultisigDTO Multisig { get; set; }
    }

    
}
