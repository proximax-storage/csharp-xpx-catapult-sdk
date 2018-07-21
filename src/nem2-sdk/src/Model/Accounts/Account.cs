// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="Account.cs" company="Nem.io">   
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
using System.Security.Cryptography;
using io.nem2.sdk.Core.Crypto.Chaso.NaCl;
using io.nem2.sdk.Infrastructure.Buffers.Model;
using io.nem2.sdk.Model.Blockchain;
using io.nem2.sdk.Model.Transactions;
using Org.BouncyCastle.Crypto.Digests;

namespace io.nem2.sdk.Model.Accounts
{
    /// <summary>
    /// Class Account.
    /// </summary>
    public class Account
    {
        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>The address.</value>
        public Address Address { get; }

        /// <summary>
        /// Gets or sets the key pair.
        /// </summary>
        /// <value>The key pair.</value>
        public KeyPair KeyPair { get; }

        /// <summary>
        /// Gets the private key.
        /// </summary>
        /// <value>The private key.</value>
        public string PrivateKey => KeyPair.PrivateKeyString;

        /// <summary>
        /// Gets the public key.
        /// </summary>
        /// <value>The public key.</value>
        public string PublicKey => KeyPair.PublicKeyString;

        /// <summary>
        /// Gets or sets the public account.
        /// </summary>
        /// <value>The public account.</value>
        public PublicAccount PublicAccount { get; set; }

        /// <summary>
        /// Creates from private key.
        /// </summary>
        /// <param name="privateKey">The private key.</param>
        /// <param name="networkType">Type of the network.</param>
        /// <returns>Account.</returns>
        public static Account CreateFromPrivateKey(string privateKey, NetworkType.Types networkType)
        {
            var keyPair = KeyPair.CreateFromPrivateKey(privateKey);
            var address = Address.CreateFromPublicKey(keyPair.PublicKeyString, networkType);
            
            return new Account(address, keyPair);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Account" /> class.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="keyPair">The key pair.</param>
        public Account(Address address, KeyPair keyPair)
        {
            Address = address;
            KeyPair = keyPair;
            PublicAccount = new PublicAccount(keyPair.PublicKeyString, address.NetworkByte);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Account" /> class.
        /// </summary>
        /// <param name="privateKey">The private key.</param>
        /// <param name="networkType">Type of the network.</param>
        public Account(string privateKey, NetworkType.Types networkType)
        {
            KeyPair = KeyPair.CreateFromPrivateKey(privateKey);
            Address = Address.CreateFromPublicKey(KeyPair.PublicKeyString, networkType);
            PublicAccount = new PublicAccount(KeyPair.PublicKeyString, networkType);
        }

        /// <summary>
        /// Signs the specified transaction.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <returns>SignedTransaction.</returns>
        public SignedTransaction Sign(Transaction transaction)
        {
            return transaction.SignWith(KeyPair);
        }

        /// <summary>
        /// Signs the cosignature transaction.
        /// </summary>
        /// <param name="cosignatureTransaction">The cosignature transaction.</param>
        /// <returns>CosignatureSignedTransaction.</returns>
        public CosignatureSignedTransaction SignCosignatureTransaction(CosignatureTransaction cosignatureTransaction)
        {
            return cosignatureTransaction.SignWith(KeyPair);
        }

        /// <summary>
        /// Signs the transaction with cosignatories.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="cosignatories">The cosignatories.</param>
        /// <returns>SignedTransaction.</returns>
        public SignedTransaction SignTransactionWithCosignatories(AggregateTransaction transaction, List<Account> cosignatories)
        {
            return transaction.SignWithAggregateCosigners(KeyPair, cosignatories);
        }

        public static Account GenerateNewAccount(NetworkType.Types networkType)
        {
            using (var ng = RandomNumberGenerator.Create())
            {
                var bytes = new byte[2048];
                ng.GetNonZeroBytes(bytes);

                var digestSha3 = new Sha3Digest(256);
                var stepOne = new byte[32];
                digestSha3.BlockUpdate(bytes, 0, 32);
                digestSha3.DoFinal(stepOne, 0);

                var keyPair = KeyPair.CreateFromPrivateKey(stepOne.ToHexLower());

                return new Account(Address.CreateFromPublicKey(keyPair.PublicKeyString, networkType), keyPair);
            }         
        }
    }
}
