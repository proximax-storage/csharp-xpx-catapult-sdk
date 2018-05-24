// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-30-2018
// ***********************************************************************
// <copyright file="Transaction.cs" company="Nem.io">
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
using System.Linq;
using io.nem2.sdk.Core.Crypto.Chaso.NaCl;
using io.nem2.sdk.Core.Utils;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Blockchain;

namespace io.nem2.sdk.Model.Transactions
{
    /// <summary>
    /// An abstract transaction class that serves as the base class of all NEM transactions.
    /// </summary>
    public abstract class Transaction
    {
        /// <summary>
        /// Gets or sets the fee.
        /// </summary>
        /// <value>The fee.</value>
        public ulong Fee { get; internal set; }

        /// <summary>
        /// Gets or sets the deadline.
        /// </summary>
        /// <value>The deadline.</value>
        public Deadline Deadline { get; internal set; }

        /// <summary>
        /// Gets or sets the type of the network.
        /// </summary>
        /// <value>The type of the network.</value>
        public NetworkType.Types NetworkType { get; internal set; }

        public int Version { get; set; }

        /// <summary>
        /// Gets or sets the type of the transaction.
        /// </summary>
        /// <value>The type of the transaction.</value>
        public TransactionTypes.Types TransactionType { get; internal set; }

        /// <summary>
        /// Gets or sets the signer.
        /// </summary>
        /// <value>The signer.</value>
        public PublicAccount Signer { get; internal set; }

        /// <summary>
        /// Gets the signature.
        /// </summary>
        /// <value>The signature.</value>
        public string Signature { get; internal set; }

        /// <summary>
        /// Gets the transaction information.
        /// </summary>
        /// <value>The transaction information.</value>
        public TransactionInfo TransactionInfo { get; internal set; }

        /// <summary>
        /// Gets or sets the bytes.
        /// </summary>
        /// <value>The bytes.</value>
        private byte[] Bytes { get; set; }

        /// <summary>
        /// Gets the signer.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        internal byte[] GetSigner()
        {
            return Signer == null ? new byte[32] : Signer.PublicKey.DecodeHexString();
        }

        /// <summary>
        /// Signs the transaction with the given <see cref="KeyPair"/>.
        /// </summary>
        /// <param name="keyPair">The <see cref="KeyPair"/>.</param>
        /// <returns><see cref="SignedTransaction"/>.</returns>
        /// <exception cref="ArgumentNullException">keyPair</exception>
        public SignedTransaction SignWith(KeyPair keyPair)
        {
            if (keyPair == null) throw new ArgumentNullException(nameof(keyPair));

            Signer = PublicAccount.CreateFromPublicKey(keyPair.PublicKeyString, NetworkType);

            Bytes = GenerateBytes();

            var sig = TransactionExtensions.SignTransaction(keyPair, Bytes);
          
            var signedBuffer = Bytes.Take(4)
                                    .Concat(sig)
                                    .Concat(keyPair.PublicKey)
                                    .Concat(
                                        Bytes.Take(4 + 64 + 32, Bytes.Length - (4 + 64 + 32))
                                    ).ToArray();

            return SignedTransaction.Create(signedBuffer, TransactionExtensions.Hasher(signedBuffer), keyPair.PublicKey, TransactionType);
        }

        /// <summary>
        /// Generates the hash for a serialized transaction payload.
        /// </summary>
        /// <param name="transactionPayload">The transaction payload in hex format.</param>
        /// <returns>The hash in hex format.</returns>
        public static string CreateTransactionHash(string transactionPayload)
        {
            if (transactionPayload == null) throw new ArgumentNullException(nameof(transactionPayload));

            return TransactionExtensions.Hasher(transactionPayload.FromHex()).ToHexUpper();
        }

        /// <summary>
        /// Takes a transaction and formats the bytes to be included in an aggregate transaction
        /// </summary>
        /// <returns>System.Byte[].</returns>
        internal byte[] ToAggregate()
        {
            var bytes = GenerateBytes();

            var aggregate = bytes.Take(4 + 64, 32 + 2 + 2)
                                 .Concat( 
                                        bytes.Take(4 + 64 + 32 + 2 + 2 + 8 + 8, bytes.Length - (4 + 64 + 32 + 2 + 2 + 8 + 8))
                                 ).ToArray();

            return BitConverter.GetBytes(aggregate.Length + 4).Concat(aggregate).ToArray();
        }

        public Transaction ToAggregate(PublicAccount signer)
        {
            Signer = PublicAccount.CreateFromPublicKey(signer.PublicKey, NetworkType);

            return this;
        }

        /// <summary>
        /// Generates the bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        internal abstract byte[] GenerateBytes();
    }
}
