// Copyright 2019 ProximaX
// 
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

using System;
using System.Linq;
using System.Text;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions
{
    public abstract class Transaction
    {
        protected Transaction(NetworkType networkType, int version, TransactionType transactionType, Deadline deadline,
            ulong? maxFee, string signature = null, PublicAccount signer = null, TransactionInfo transactionInfo = null)
        {
            TransactionType = transactionType;
            NetworkType = networkType;
            Version = version;
            Deadline = deadline;
            MaxFee = maxFee;
            Signature = signature;
            Signer = signer;
            TransactionInfo = transactionInfo;
        }

        /// <summary>
        ///     The transaction type
        /// </summary>
        public TransactionType TransactionType { get; set; }

        /// <summary>
        ///     The network type
        /// </summary>
        public NetworkType NetworkType { get; set; }

        /// <summary>
        ///     Transaction version
        /// </summary>
        public int Version { get; set; }

        /// <summary>
        ///     The deadline to include the transaction.
        /// </summary>
        public Deadline Deadline { get; set; }

        /// <summary>
        ///     A sender of a transaction must specify during the transaction definition a max_fee,
        ///     meaning the maximum fee the account allows to spend for this transaction.
        /// </summary>
        public ulong? MaxFee { get; set; }

        /// <summary>
        ///     The transaction signature (missing if part of an aggregate transaction).
        /// </summary>
        public string Signature { get; set; }

        /// <summary>
        ///     The account of the transaction creator.
        /// </summary>
        public PublicAccount Signer { get; set; }

        /// <summary>
        ///     Transactions meta data object contains additional information about the transaction.
        /// </summary>
        public TransactionInfo TransactionInfo { get; set; }

        /// <summary>
        ///     Gets or sets the bytes.
        /// </summary>
        /// <value>The bytes.</value>
        private byte[] Bytes { get; set; }

        /// <summary>
        ///     Gets the signer.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        internal byte[] GetSigner()
        {
            return Signer == null ? new byte[32] : Signer.PublicKey.DecodeHexString();
        }

        /// <summary>
        ///     Signs the transaction with the given <see cref="Account" />.
        /// </summary>
        /// <param name="account"></param>
        /// <returns></returns>
        public SignedTransaction SignWith(Account account,string generationHash)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));

            Signer = PublicAccount.CreateFromPublicKey(account.KeyPair.PublicKeyString, NetworkType);

            var generationHashBytes = Encoding.UTF8.GetBytes(generationHash);

            Bytes = generationHashBytes.Concat(GenerateBytes()).ToArray();

            var sig = TransactionExtensions.SignTransaction(account.KeyPair, Bytes);

            var signedBuffer = Bytes.Take(4)
                .Concat(sig)
                .Concat(account.KeyPair.PublicKey)
                .Concat(
                    Bytes.Take(4 + 64 + 32, Bytes.Length - (4 + 64 + 32))
                ).ToArray();

            return SignedTransaction.Create(signedBuffer, TransactionExtensions.Hasher(signedBuffer),
                account.KeyPair.PublicKey, TransactionType, NetworkType);
        }

        /// <summary>
        ///     Convert an aggregate transaction to an inner transaction including transaction signer.
        /// </summary>
        /// <param name="signer">Transaction signer</param>
        /// <returns>Transaction</returns>
        public Transaction ToAggregate(PublicAccount signer)
        {
            Signer = PublicAccount.CreateFromPublicKey(signer.PublicKey, NetworkType);

            return this;
        }

        /// <summary>
        ///     Transaction pending to be included in a block
        /// </summary>
        /// <returns>bool</returns>
        public bool IsUnconfirmed()
        {
            return TransactionInfo != null && TransactionInfo.Height == 0 && TransactionInfo.Hash ==
                   TransactionInfo.MerkleComponentHash;
        }

        /// <summary>
        ///     Transaction included in a block
        /// </summary>
        /// <returns>bool</returns>
        public bool IsConfirmed()
        {
            return TransactionInfo != null && TransactionInfo.Height > 0;
        }

        /// <summary>
        ///     Transaction is not known by the network
        /// </summary>
        /// <returns>bool</returns>
        public bool IsUnannounced()
        {
            return TransactionInfo == null;
        }

        /// <summary>
        ///     Takes a transaction and formats the bytes to be included in an aggregate transaction
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

        /// <summary>
        ///     Generates the bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        internal abstract byte[] GenerateBytes();
    }
}