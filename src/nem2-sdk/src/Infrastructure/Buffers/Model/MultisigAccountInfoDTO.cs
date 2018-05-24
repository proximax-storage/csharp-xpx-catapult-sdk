// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="MultisigDTO.cs" company="Nem.io">   
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
using Newtonsoft.Json;

namespace io.nem2.sdk.Infrastructure.Buffers.Model
{
    public class MultisigModificationTransactionDTO : TransactionDTO
    {
        /// <summary>
        /// Gets or sets the minimum approval.
        /// </summary>
        /// <value>The minimum approval.</value>
        [JsonProperty("minApprovalDelta")]
        public int MinApprovalDelta { get; set; }

        /// <summary>
        /// Gets or sets the minimum removal.
        /// </summary>
        /// <value>The minimum removal.</value>
        [JsonProperty("MinRemovalDelta")]
        public int MinRemovalDelta { get; set; }

        /// <summary>
        /// Gets or sets the modifications.
        /// </summary>
        /// <value>The modifications.</value>
        [JsonProperty("modifications")]
        public List<MultisigModification> Modifications { get; set; }
    }

    public class MultisigModification
    {
        /// <summary>
        /// Gets or sets the cosignatory public key.
        /// </summary>
        /// <value>The cosignatory public key.</value>
        [JsonProperty("cosignatoryPublicKey")]
        public string CosignatoryPublicKey { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>The type.</value>
        [JsonProperty("type")]
        public byte Type { get; set; }

    }
    /// <summary>
    /// Class MultisigDTO.
    /// </summary>
    public class MultisigDTO
    {
        /// <summary>
        /// Gets or sets the account.
        /// </summary>
        /// <value>The account.</value>
        [JsonProperty("account")]
        public string Account { get; set; }

        /// <summary>
        /// Gets or sets the minimum approval.
        /// </summary>
        /// <value>The minimum approval.</value>
        [JsonProperty("minApproval")]
        public int MinApproval { get; set; }

        /// <summary>
        /// Gets or sets the minimum removal.
        /// </summary>
        /// <value>The minimum removal.</value>
        [JsonProperty("minRemoval")]
        public int MinRemoval { get; set; }

        /// <summary>
        /// Gets or sets the cosignatories.
        /// </summary>
        /// <value>The cosignatories.</value>
        [JsonProperty("cosignatories")]
        public string[] Cosignatories { get; set; }

        /// <summary>
        /// Gets or sets the multisig accounts.
        /// </summary>
        /// <value>The multisig accounts.</value>
        [JsonProperty("multisigAccounts")]
        public string[] MultisigAccounts { get; set; }
    }
}
