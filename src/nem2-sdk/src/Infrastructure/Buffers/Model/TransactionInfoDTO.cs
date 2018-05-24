// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="TransactionInfoDTO.cs" company="Nem.io">   
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
    /// Class TransactionInfoDTO.
    /// </summary>
    public class TransferTransactionInfoDTO
    {
        /// <summary>
        /// Gets or sets the meta.
        /// </summary>
        /// <value>The meta.</value>
        [JsonProperty("meta")]
        public TransactionMetaDTO Meta { get; set; }

        /// <summary>
        /// Gets or sets the transaction.
        /// </summary>
        /// <value>The transaction.</value>
        [JsonProperty("transaction")]
        public TransferTransactionDTO Transaction { get; set; }
    }

    public class MultisigModificationTransactionInfoDTO
    {
        /// <summary>
        /// Gets or sets the meta.
        /// </summary>
        /// <value>The meta.</value>
        [JsonProperty("meta")]
        public TransactionMetaDTO Meta { get; set; }

        /// <summary>
        /// Gets or sets the transaction.
        /// </summary>
        /// <value>The transaction.</value>
        [JsonProperty("transaction")]
        public MultisigModificationTransactionDTO Transaction { get; set; }
    }
    /// <summary>
    /// Class TransactionInfoDTO.
    /// </summary>
    public class NamespaceTransactionInfoDTO
    {
        /// <summary>
        /// Gets or sets the meta.
        /// </summary>
        /// <value>The meta.</value>
        [JsonProperty("meta")]
        public TransactionMetaDTO Meta { get; set; }

        /// <summary>
        /// Gets or sets the transaction.
        /// </summary>
        /// <value>The transaction.</value>
        [JsonProperty("transaction")]
        public NamespaceCreationTransactionDTO Transaction { get; set; }
    }

    /// <summary>
    /// Class MosaicCreationTransactionInfoDTO.
    /// </summary>
    public class MosaicCreationTransactionInfoDTO
    {
        /// <summary>
        /// Gets or sets the meta.
        /// </summary>
        /// <value>The meta.</value>
        [JsonProperty("meta")]
        public TransactionMetaDTO Meta { get; set; }

        /// <summary>
        /// Gets or sets the transaction.
        /// </summary>
        /// <value>The transaction.</value>
        [JsonProperty("transaction")]
        public MosaicCreationTransactionDTO Transaction { get; set; }       
    }

    /// <summary>
    /// Class LockFundsTransactionInfoDTO.
    /// </summary>
    public class LockFundsTransactionInfoDTO
    {
        /// <summary>
        /// Gets or sets the meta.
        /// </summary>
        /// <value>The meta.</value>
        [JsonProperty("meta")]
        public TransactionMetaDTO Meta { get; set; }

        /// <summary>
        /// Gets or sets the transaction.
        /// </summary>
        /// <value>The transaction.</value>
        [JsonProperty("transaction")]
        public LockFundsTransactionDTO Transaction { get; set; }
    }

    /// <summary>
    /// Class SecretLockTransactionInfoDTO.
    /// </summary>
    public class SecretLockTransactionInfoDTO
    {
        /// <summary>
        /// Gets or sets the meta data.
        /// </summary>
        /// <value>The meta data.</value>
        [JsonProperty("meta")]
        public TransactionMetaDTO Meta { get; set; }

        /// <summary>
        /// Gets or sets the transaction.
        /// </summary>
        /// <value>The transaction.</value>
        [JsonProperty("transaction")]
        public SecretLockTransactionDTO Transaction { get; set; }
    }

    /// <summary>
    /// Class SecretLockTransactionInfoDTO.
    /// </summary>
    public class SecretProofTransactionInfoDTO
    {
        /// <summary>
        /// Gets or sets the meta data.
        /// </summary>
        /// <value>The meta data.</value>
        [JsonProperty("meta")]
        public TransactionMetaDTO Meta { get; set; }

        /// <summary>
        /// Gets or sets the transaction.
        /// </summary>
        /// <value>The transaction.</value>
        [JsonProperty("transaction")]
        public SecretProofTransactionDTO Transaction { get; set; }
    }

    /// <summary>
    /// Class MosaicSupplyChangeTransactionInfoDTO.
    /// </summary>
    public class MosaicSupplyChangeTransactionInfoDTO
    {
        /// <summary>
        /// Gets or sets the meta data.
        /// </summary>
        /// <value>The meta data.</value>
        [JsonProperty("meta")]
        public TransactionMetaDTO Meta { get; set; }

        /// <summary>
        /// Gets or sets the transaction.
        /// </summary>
        /// <value>The transaction.</value>
        [JsonProperty("transaction")]
        public MosaicSupplyChangeTransactionDTO Transaction { get; set; }
    }

    /// <summary>
    /// Class TransactionInfoDTO.
    /// </summary>
    public class TransactionInfoDTO
    {
        /// <summary>
        /// Gets or sets the meta.
        /// </summary>
        /// <value>The meta.</value>
        [JsonProperty("meta")]
        public TransactionMetaDTO Meta { get; set; }

        /// <summary>
        /// Gets or sets the transaction.
        /// </summary>
        /// <value>The transaction.</value>
        [JsonProperty("transaction")]
        public TransactionDTO Transaction { get; set; }
    }

    public class AggregateTransactionInfoDTO
    {
        /// <summary>
        /// Gets or sets the meta.
        /// </summary>
        /// <value>The meta.</value>
        [JsonProperty("meta")]
        public AggregateTransactionMetaDTO Meta { get; set; }

        /// <summary>
        /// Gets or sets the transaction.
        /// </summary>
        /// <value>The transaction.</value>
        [JsonProperty("transaction")]
        public AggregateTransactionDTO Transaction { get; set; }
    }
}
