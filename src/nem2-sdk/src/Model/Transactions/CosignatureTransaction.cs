// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-25-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="CosignatureTransaction.cs" company="Nem.io">
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
using io.nem2.sdk.Infrastructure.Buffers.Model;
using io.nem2.sdk.Model.Accounts;

namespace io.nem2.sdk.Model.Transactions
{
    /// <summary>
    /// The cosignature transaction is used to sign an aggregate transactions with missing cosignatures.
    /// </summary>
    public class CosignatureTransaction
    {
        /// <summary>
        /// The transaction hash to be signed.
        /// </summary>
        /// <value>The hash.</value>
        public string Hash { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CosignatureTransaction"/> class.
        /// </summary>
        /// <param name="hash">The hash.</param>
        /// <exception cref="ArgumentNullException">hash</exception>
        /// <exception cref="ArgumentException">
        /// invalid hash length
        /// or
        /// invalid hash not hex
        /// </exception>
        public CosignatureTransaction(string hash)
        {
            if (hash == null) throw new ArgumentNullException(nameof(hash));
            if (!Regex.IsMatch(hash, @"\A\b[0-9a-fA-F]+\b\Z")) throw new ArgumentException("invalid hash length");
            if (hash.Length != 64) throw new ArgumentException("invalid hash, the given String is not in hex format");

            Hash = hash;
        }

        /// <summary>
        /// Creates the specified CosignatureTransaction from the given hash.
        /// </summary>
        /// <param name="hash">The hash.</param>
        /// <returns>CosignatureTransaction.</returns>
        public static CosignatureTransaction Create(string hash)
        {
            return new CosignatureTransaction(hash);
        }

        /// <summary>
        /// Sign the transaction with a given account.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns>CosignatureSignedTransactionDTO.</returns>
        /// <exception cref="ArgumentNullException">account</exception>
        public CosignatureSignedTransactionDTO SignWith(KeyPair account)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));
            var bytes = Hash.FromHex();
            var signatureBytes = TransactionExtensions.SignHash(account, bytes);

            return new CosignatureSignedTransactionDTO{ ParentHash = Hash, Signature = signatureBytes.ToHexLower(), Signer = account.PublicKeyString};
        }      
    }
}
