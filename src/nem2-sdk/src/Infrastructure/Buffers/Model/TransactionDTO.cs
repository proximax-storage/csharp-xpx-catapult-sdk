// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="TransactionDTO.cs" company="Nem.io">   
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
    public class TransactionDTO
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
        public ushort Type { get; set; }

        /// <summary>
        /// Gets or sets the fee.
        /// </summary>
        /// <value>The fee.</value>
        [JsonProperty("fee")]
        [JsonConverter(typeof(UInt32ArrayToLong))]
        public ulong Fee { get; set; }

        /// <summary>
        /// Gets or sets the deadline.
        /// </summary>
        /// <value>The deadline.</value>
        [JsonProperty("deadline")]
        [JsonConverter(typeof(UInt32ArrayToLong))]
        public ulong Deadline { get; set; }
    }


    public class AggregateTransactionDTO : TransactionDTO
    {
        /// <summary>
        /// Gets or sets the cosignatures.
        /// </summary>
        /// <value>The cosignatures.</value>
        [JsonProperty("cosignatures")]
        public List<CosignatureDTO> Cosignatures { get; set; }

        /// <summary>
        /// Gets or sets the sub transactions.
        /// </summary>
        /// <value>The sub transactions.</value>
        [JsonProperty("transactions")]
        public List<AggregateTransactionInfoDTO> InnerTransactions { get; set; }
    }

    public class NamespaceCreationTransactionDTO : TransactionDTO
    {
        /// <summary>
        /// Gets or sets the type of the namespace.
        /// </summary>
        /// <value>The type of the namespace.</value>
        [JsonProperty("namespaceType")]
        public byte NamespaceType { get; set; }

        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        /// <value>The duration.</value>
        [JsonProperty("duration")]
        [JsonConverter(typeof(UInt32ArrayToLong))]
        public ulong Duration { get; set; }

        /// <summary>
        /// Gets or sets the namespace identifier.
        /// </summary>
        /// <value>The namespace identifier.</value>
        [JsonProperty("namespaceId")]
        [JsonConverter(typeof(UInt32ArrayToLong))]
        public ulong NamespaceId { get; set; }

        /// <summary>
        /// Gets or sets the parent identifier.
        /// </summary>
        /// <value>The parent identifier.</value>
        [JsonProperty("parentId")]
        [JsonConverter(typeof(UInt32ArrayToLong))]
        public ulong ParentId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("name")]
        public string Name { get; set; }

    }

    /// <summary>
    /// Class MosaicSupplyChangeTransactionDTO.
    /// </summary>
    /// <seealso cref="io.nem2.sdk.Infrastructure.Buffers.Model.TransactionDTO" />
    public class MosaicSupplyChangeTransactionDTO : TransactionDTO
    {
        /// <summary>
        /// Gets or sets the delta.
        /// </summary>
        /// <value>The delta.</value>
        [JsonProperty("delta")]
        [JsonConverter(typeof(UInt32ArrayToLong))]
        public ulong Delta { get; set; }

        /// <summary>
        /// Gets or sets the direction.
        /// </summary>
        /// <value>The direction.</value>
        [JsonProperty("direction")]
        public byte Direction { get; set; }

        /// <summary>
        /// Gets or sets the mosaic identifier.
        /// </summary>
        /// <value>The mosaic identifier.</value>
        [JsonProperty("mosaicId")]
        [JsonConverter(typeof(UInt32ArrayToLong))]
        public ulong MosaicId { get; set; }
    }

    /// <summary>
    /// Class LockFundsTransactionDTO.
    /// </summary>
    /// <seealso cref="io.nem2.sdk.Infrastructure.Buffers.Model.TransactionDTO" />
    public class LockFundsTransactionDTO : TransactionDTO
    {
        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        /// <value>The duration.</value>
        [JsonProperty("duration")]
        [JsonConverter(typeof(UInt32ArrayToLong))]
        public ulong Duration { get; set; }

        /// <summary>
        /// Gets or sets the mosaic identifier.
        /// </summary>
        /// <value>The mosaic identifier.</value>
        [JsonProperty("mosaicId")]
        [JsonConverter(typeof(UInt32ArrayToLong))]
        public ulong MosaicId { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
        [JsonProperty("amount")]
        [JsonConverter(typeof(UInt32ArrayToLong))]
        public ulong Amount { get; set; }

        /// <summary>
        /// Gets or sets the hash.
        /// </summary>
        /// <value>The hash.</value>
        [JsonProperty("hash")]
        public string Hash { get; set; }
    }

    public class SecretProofTransactionDTO : TransactionDTO
    {
        /// <summary>
        /// Gets or sets the hash algorithm.
        /// </summary>
        /// <value>The hash algorithm.</value>
        [JsonProperty("hashAlgorithm")]
        public byte HashAlgorithm { get; set; }

        /// <summary>
        /// Gets or sets the secret.
        /// </summary>
        /// <value>The secret.</value>
        [JsonProperty("secret")]
        public string Secret { get; set; }

        /// <summary>
        /// Gets or sets the proof.
        /// </summary>
        /// <value>The proof.</value>
        [JsonProperty("proof")]
        public string Proof { get; set; }
    }
    /// <summary>
    /// Class SecretLockTransactionDTO.
    /// </summary>
    /// <seealso cref="io.nem2.sdk.Infrastructure.Buffers.Model.TransactionDTO" />
    public class SecretLockTransactionDTO : TransactionDTO
    {
        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        /// <value>The duration.</value>
        [JsonProperty("duration")]
        [JsonConverter(typeof(UInt32ArrayToLong))]
        public ulong Duration { get; set; }

        /// <summary>
        /// Gets or sets the mosaic identifier.
        /// </summary>
        /// <value>The mosaic identifier.</value>
        [JsonProperty("mosaicId")]
        [JsonConverter(typeof(UInt32ArrayToLong))]
        public ulong MosaicId { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>The amount.</value>
        [JsonProperty("amount")]
        [JsonConverter(typeof(UInt32ArrayToLong))]
        public ulong Amount { get; set; }

        /// <summary>
        /// Gets or sets the secret.
        /// </summary>
        /// <value>The secret.</value>
        [JsonProperty("secret")]
        public string Secret { get; set; }

        /// <summary>
        /// Gets or sets the hash algorithm.
        /// </summary>
        /// <value>The hash algorithm.</value>
        [JsonProperty("hashAlgorithm")]
        public byte HashAlgorithm { get; set; }

        /// <summary>
        /// Gets or sets the recipient.
        /// </summary>
        /// <value>The recipient.</value>
        [JsonProperty("recipient")]
        public string Recipient { get; set; }
    }

    /// <summary>
    /// Class MosaicCreationTransactionDTO.
    /// </summary>
    /// <seealso cref="io.nem2.sdk.Infrastructure.Buffers.Model.TransactionDTO" />
    public class MosaicCreationTransactionDTO : TransactionDTO
    {
        /// <summary>
        /// Class Property.
        /// </summary>
        public class Property
        {
            /// <summary>
            /// Gets or sets the identifier.
            /// </summary>
            /// <value>The identifier.</value>
            [JsonProperty("id")]
            public int Id { get; set; }

            /// <summary>
            /// Gets or sets the value.
            /// </summary>
            /// <value>The value.</value>
            [JsonProperty("value")]
            [JsonConverter(typeof(UInt32ArrayToLong))]
            public ulong value { get; set; }
        }

        /// <summary>
        /// Gets or sets the properties.
        /// </summary>
        /// <value>The properties.</value>
        [JsonProperty("properties")]
        public List<Property> Properties { get; set; }

        /// <summary>
        /// Gets or sets the mosaic identifier.
        /// </summary>
        /// <value>The mosaic identifier.</value>
        [JsonProperty("mosaicId")]
        [JsonConverter(typeof(UInt32ArrayToLong))]
        public ulong MosaicId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>The name.</value>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the parent namespace identifier.
        /// </summary>
        /// <value>The parent identifier.</value>
        [JsonProperty("parentId")]
        [JsonConverter(typeof(UInt32ArrayToLong))]
        public ulong ParentId { get; set; }
    }

    /// <summary>
    /// Class TransactionDTO.
    /// </summary>
    public class TransferTransactionDTO : TransactionDTO
    {     
        /// <summary>
        /// Gets or sets the recipient.
        /// </summary>
        /// <value>The recipient.</value>
        [JsonProperty("recipient")]
        [JsonConverter(typeof(HexToBase32))]
        public string Recipient { get; set; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        [JsonProperty("message")]
        public Message Message { get; set; }

        /// <summary>
        /// Gets or sets the mosaics.
        /// </summary>
        /// <value>The mosaics.</value>
        [JsonProperty("mosaics")]
        public List<MosaicDTO> Mosaics { get; set; }
    }
}
