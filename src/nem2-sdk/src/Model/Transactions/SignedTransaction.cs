// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-30-2018
// ***********************************************************************
// <copyright file="SignedTransaction.cs" company="Nem.io">
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

using System;
using System.Text.RegularExpressions;
using io.nem2.sdk.Core.Crypto.Chaso.NaCl;
using Newtonsoft.Json;

namespace io.nem2.sdk.Model.Transactions
{
    /// <summary>
    /// Class SignedTransaction.
    /// </summary>
    public class SignedTransaction
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

        /// <summary>
        /// Gets the type of the transaction.
        /// </summary>
        /// <value>The type of the transaction.</value>
        public TransactionTypes.Types TransactionType { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SignedTransaction"/> class.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <param name="hash">The hash.</param>
        /// <param name="signer">The signer.</param>
        /// <param name="transactionType">The transaction type.</param>
        /// <exception cref="ArgumentException">
        /// Value cannot be null or empty. - payload
        /// or
        /// Invalid hash.
        /// or
        /// Invalid signer key.
        /// </exception>
        internal SignedTransaction(string payload, string hash, string signer, TransactionTypes.Types transactionType)
        {          
            if (hash.Length != 64 || !Regex.IsMatch(hash, @"\A\b[0-9a-fA-F]+\b\Z")) throw new ArgumentException("Invalid hash.");
            TransactionType = transactionType;
            Payload = payload;
            Hash = hash;
            Signer = signer;
        }

        /// <summary>
        /// Static creates a new instance of the <see cref="SignedTransaction"/> class.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <param name="hash">The hash.</param>
        /// <param name="signer">The signer.</param>
        /// <param name="transactionType">The transaction type.</param>
        /// <returns><see cref="SignedTransaction"/>.</returns>
        /// <exception cref="ArgumentNullException">
        /// payload
        /// or
        /// hash
        /// or
        /// signer
        /// </exception>
        /// <exception cref="ArgumentException">
        /// invalid hash length
        /// or
        /// invalid signer length
        /// </exception>
        public static SignedTransaction Create(byte[] payload, byte[] hash, byte[] signer, TransactionTypes.Types transactionType)
        {
            if (payload == null) throw new ArgumentNullException(nameof(payload));
            if (hash == null) throw new ArgumentNullException(nameof(hash));
            if(hash.Length != 32) throw new ArgumentException("invalid hash length");
            if (signer == null) throw new ArgumentNullException(nameof(signer));
            if (signer.Length != 32) throw new ArgumentException("invalid signer length");
            
            return new SignedTransaction(payload.ToHexLower().ToUpper(), hash.ToHexLower().ToUpper(), signer.ToHexLower().ToUpper(), transactionType);
        }
    }
}
