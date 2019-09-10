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
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using FlatBuffers;
using ProximaX.Sirius.Chain.Sdk.Buffers;
using ProximaX.Sirius.Chain.Sdk.Crypto.Core.Chaso.NaCl;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Buffers.Schema;
using ProximaX.Sirius.Chain.Sdk.Utils;
using GuardNet;
using System.Runtime.Serialization;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions
{
    /// <summary>
    ///     Class AggregateTransaction.
    /// </summary>
    /// <seealso cref="Transaction" />
    public class AggregateTransaction : Transaction
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AggregateTransaction" /> class.
        /// </summary>
        /// <param name="networkType">Type of the network.</param>
        /// <param name="version">The transaction version.</param>
        /// <param name="transactionType">Type of the transaction.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="fee">The fee.</param>
        /// <param name="innerTransactions">The inner transactions.</param>
        /// <param name="cosignatures">The cosignatures.</param>
        public AggregateTransaction(NetworkType networkType, int version, EntityType transactionType,
            Deadline deadline, ulong fee, List<Transaction> innerTransactions,
            List<AggregateTransactionCosignature> cosignatures)
            : this(networkType, version, transactionType, deadline, fee, innerTransactions, cosignatures, null, null,
                null)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AggregateTransaction" /> class.
        /// </summary>
        /// <param name="networkType">Type of the network.</param>
        /// <param name="version">The transaction version.</param>
        /// <param name="transactionType">Type of the transaction.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="maxFee">The fee.</param>
        /// <param name="innerTransactions">The inner transactions.</param>
        /// <param name="cosignatures">The cosignatures.</param>
        /// <param name="signature">The signature.</param>
        /// <param name="signer">The signer.</param>
        /// <param name="transactionInfo">The transaction information.</param>
        public AggregateTransaction(NetworkType networkType, int version, EntityType transactionType,
            Deadline deadline, ulong? maxFee, List<Transaction> innerTransactions,
            List<AggregateTransactionCosignature> cosignatures, string signature, PublicAccount signer,
            TransactionInfo transactionInfo)
            : base(networkType, version, transactionType, deadline, maxFee, signature, signer, transactionInfo)
        {
            Guard.NotNull(innerTransactions,nameof(innerTransactions), "InnerTransactions must not be null");
            Guard.NotNull(cosignatures, nameof(cosignatures), "Cosignatures must not be null");
            InnerTransactions = innerTransactions;
            Cosignatures = cosignatures;
        }

        /// <summary>
        ///     Gets or sets the inner transactions.
        /// </summary>
        /// <value>The inner transactions.</value>
        public List<Transaction> InnerTransactions { get; }

        /// <summary>
        ///     Gets the cosignatures.
        /// </summary>
        /// <value>The cosignatures.</value>
        public List<AggregateTransactionCosignature> Cosignatures { get; }

        /// <summary>
        ///     Creates the complete.
        /// </summary>
        /// <param name="networkType">Type of the network.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="innerTransactions">The inner transactions.</param>
        /// <returns>AggregateTransaction.</returns>
        /// <exception cref="InvalidEnumArgumentException">networkType</exception>
        public static AggregateTransaction CreateComplete(Deadline deadline, List<Transaction> innerTransactions,
            NetworkType networkType)
        {
      
            return new AggregateTransaction(networkType, EntityVersion.AGGREGATE_COMPLETE.GetValue(),
                EntityType.AGGREGATE_COMPLETE, deadline, 0, innerTransactions,
                new List<AggregateTransactionCosignature>());
        }

        /// <summary>
        ///     Creates the bonded.
        /// </summary>
        /// <param name="networkType">Type of the network.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="innerTransactions">The inner transactions.</param>
        /// <param name="cosignatures">The cosignatures.</param>
        /// <returns>AggregateTransaction.</returns>
        /// <exception cref="InvalidEnumArgumentException">networkType</exception>
        public static AggregateTransaction CreateBonded(Deadline deadline, List<Transaction> innerTransactions,
            NetworkType networkType)
        {
        
            return new AggregateTransaction(networkType, EntityVersion.AGGREGATE_BONDED.GetValue(),
                EntityType.AGGREGATE_BONDED, deadline, 0, innerTransactions,
                new List<AggregateTransactionCosignature>());
        }

        /// <summary>
        ///     Signs the aggregate transaction with cosigners.
        /// </summary>
        /// <param name="initiatorAccount">The initiator account.</param>
        /// <param name="cosignatories">The cosignatories.</param>
        /// <returns>SignedTransaction.</returns>
        /// <exception cref="ArgumentNullException">
        ///     initiatorAccount
        ///     or
        ///     cosignatories
        /// </exception>
        public SignedTransaction SignTransactionWithCosigners(Account initiatorAccount, List<Account> cosignatories,string generationHash)
        {
            if (initiatorAccount == null) throw new ArgumentNullException(nameof(initiatorAccount));
            if (cosignatories == null) throw new ArgumentNullException(nameof(cosignatories));

            var signedTransaction = SignWith(initiatorAccount, generationHash);
            var payload = signedTransaction.Payload.FromHex();

            foreach (var cosignatory in cosignatories)
            {
                var bytes = signedTransaction.Hash.FromHex();

                var signatureBytes = TransactionExtensions.SignHash(cosignatory.KeyPair, bytes);

                payload = payload.Concat(cosignatory.KeyPair.PublicKey.Concat(signatureBytes)).ToArray();

                Cosignatures.Add(new AggregateTransactionCosignature(signatureBytes.ToHexLower(),
                    new PublicAccount(cosignatory.KeyPair.PublicKey.ToHexLower(),
                        initiatorAccount.Address.NetworkType)));
            }

            payload = BitConverter.GetBytes(payload.Length).Concat(payload.Take(4, payload.Length - 4).ToArray())
                .ToArray();

            return SignedTransaction.Create(payload, signedTransaction.Hash.FromHex(),
                initiatorAccount.PublicKey.FromHex(), TransactionType, initiatorAccount.Address.NetworkType);
        }

        /// <summary>
        ///     Checks if the aggregate is either initiated or signed the by account as a cosignature.
        /// </summary>
        /// <param name="publicAccount">The public account.</param>
        /// <returns><c>true</c> if signed by the account, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">publicAccount</exception>
        public bool IsSignedByAccount(PublicAccount publicAccount)
        {
            Guard.NotNull(publicAccount, nameof(publicAccount), "Public account should not be null");
          
            return Signer.PublicKey == publicAccount.PublicKey ||
                   Cosignatures.Any(e => e.Signer.PublicKey == publicAccount.PublicKey);
        }

        internal override byte[] GenerateBytes()
        {
            var builder = new FlatBufferBuilder(1);

            // create transaction bytes
            var transactionsBytes = new byte[0];

            transactionsBytes = InnerTransactions.Aggregate(transactionsBytes,
                (current, innerTransaction) => current.Concat(innerTransaction.ToAggregate()).ToArray());

            // Create Vectors
            var signatureVector = AggregateTransactionBuffer.CreateSignatureVector(builder, new byte[64]);
            var signerVector = AggregateTransactionBuffer.CreateSignerVector(builder, GetSigner());
            var deadlineVector =
                AggregateTransactionBuffer.CreateDeadlineVector(builder, Deadline.Ticks.ToUInt8Array());
            var feeVector = AggregateTransactionBuffer.CreateMaxFeeVector(builder, MaxFee?.ToUInt8Array());
            var transactionsVector = AggregateTransactionBuffer.CreateTransactionsVector(builder, transactionsBytes);

            // total size of transaction
            /*var totalSize = HEADER_SIZE
                + 4
                + transactionsBytes.Length;*/
            var totalSize = GetSerializedSize();

            // create version
            var version = GetTxVersionSerialization();


            // add vectors
            AggregateTransactionBuffer.StartAggregateTransactionBuffer(builder);
            AggregateTransactionBuffer.AddSize(builder,(uint)totalSize);
            AggregateTransactionBuffer.AddSignature(builder, signatureVector);
            AggregateTransactionBuffer.AddSigner(builder, signerVector);
            AggregateTransactionBuffer.AddVersion(builder, (uint)version);
            AggregateTransactionBuffer.AddType(builder, TransactionType.GetValue());
            AggregateTransactionBuffer.AddMaxFee(builder, feeVector);
            AggregateTransactionBuffer.AddDeadline(builder, deadlineVector);

            AggregateTransactionBuffer.AddTransactionsSize(builder, (uint) transactionsBytes.Length);
            AggregateTransactionBuffer.AddTransactions(builder, transactionsVector);

            // end build
            var codedTransaction = AggregateTransactionBuffer.EndAggregateTransactionBuffer(builder).Value;
            builder.Finish(codedTransaction);

            var output = new AggregateTransactionSchema().Serialize(builder.SizedByteArray());

            if (output.Length != totalSize) throw new SerializationException("Serialized form has incorrect length");

            return output;

        }

        protected override int GetPayloadSerializedSize()
        {
            return CalculatePayloadSize(InnerTransactions);
        }

        public static int CalculatePayloadSize(List<Transaction> innerTransactions)
        {
            // sum sizes of inner transactions, subtract 80 as toAggregateTransactionBytes leaves out 80 bytes of header
            var transactionsBytes = new byte[0];

            transactionsBytes = innerTransactions.Aggregate(transactionsBytes,
                (current, innerTransaction) => current.Concat(innerTransaction.ToAggregate()).ToArray());

            int innerSize = transactionsBytes.Length - 80;

            // transactions size + transactions
            return 4 + innerSize;
        }
    }
}