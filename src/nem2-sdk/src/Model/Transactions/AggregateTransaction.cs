// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 02-01-2018
// ***********************************************************************
// <copyright file="AggregateTransaction.cs" company="Nem.io">
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
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using io.nem2.sdk.Core.Crypto.Chaso.NaCl;
using io.nem2.sdk.Core.Utils;
using io.nem2.sdk.Infrastructure.Buffers;
using io.nem2.sdk.Infrastructure.Buffers.Schema;
using io.nem2.sdk.Infrastructure.Imported.FlatBuffers;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Blockchain;

namespace io.nem2.sdk.Model.Transactions
{
    /// <summary>
    /// Class AggregateTransaction.
    /// </summary>
    /// <seealso cref="Transaction" />
    public class AggregateTransaction : Transaction
    {
        /// <summary>
        /// Gets or sets the inner transactions.
        /// </summary>
        /// <value>The inner transactions.</value>
        public List<Transaction> InnerTransactions { get; }

        /// <summary>
        /// Gets the cosignatures.
        /// </summary>
        /// <value>The cosignatures.</value>
        public List<AggregateTransactionCosignature> Cosignatures { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateTransaction"/> class.
        /// </summary>
        /// <param name="networkType">Type of the network.</param>
        /// <param name="version">The transaction version.</param>
        /// <param name="transactionType">Type of the transaction.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="fee">The fee.</param>
        /// <param name="innerTransactions">The inner transactions.</param>
        /// <param name="cosignatures">The cosignatures.</param>
        public AggregateTransaction(NetworkType.Types networkType, int version, TransactionTypes.Types transactionType, Deadline deadline, ulong fee,  List<Transaction> innerTransactions, List<AggregateTransactionCosignature> cosignatures)
         : this(networkType, version, transactionType, deadline, fee,innerTransactions, cosignatures, null, null, null){

        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AggregateTransaction"/> class.
        /// </summary>
        /// <param name="networkType">Type of the network.</param>
        /// <param name="version">The transaction version.</param>
        /// <param name="transactionType">Type of the transaction.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="fee">The fee.</param>
        /// <param name="innerTransactions">The inner transactions.</param>
        /// <param name="cosignatures">The cosignatures.</param>
        /// <param name="signature">The signature.</param>
        /// <param name="signer">The signer.</param>
        /// <param name="transactionInfo">The transaction information.</param>
        public AggregateTransaction(NetworkType.Types networkType, int version, TransactionTypes.Types transactionType, Deadline deadline, ulong fee, List<Transaction> innerTransactions, List<AggregateTransactionCosignature> cosignatures, string signature, PublicAccount signer, TransactionInfo transactionInfo)
        {
            InnerTransactions = innerTransactions;
            Cosignatures = cosignatures;
            Deadline = deadline;
            NetworkType = networkType;
            Fee = fee;
            TransactionType = transactionType;
            Version = version;
            Signature = signature;
            Signer = signer;
            TransactionInfo = transactionInfo;
        }
        /// <summary>
        /// Creates the complete.
        /// </summary>
        /// <param name="networkType">Type of the network.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="innerTransactions">The inner transactions.</param>
        /// <param name="cosignatures">The cosignatures.</param>
        /// <returns>AggregateTransaction.</returns>
        /// <exception cref="InvalidEnumArgumentException">networkType</exception>
        public static AggregateTransaction CreateComplete(NetworkType.Types networkType, Deadline deadline, List<Transaction> innerTransactions)
        {
            if (!Enum.IsDefined(typeof(NetworkType.Types), networkType))
                throw new InvalidEnumArgumentException(nameof(networkType), (int)networkType, typeof(NetworkType.Types));

            return new AggregateTransaction(networkType, 2, TransactionTypes.Types.AggregateComplete, deadline, 0, innerTransactions, new List<AggregateTransactionCosignature>());
        }

        /// <summary>
        /// Creates the bonded.
        /// </summary>
        /// <param name="networkType">Type of the network.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="innerTransactions">The inner transactions.</param>
        /// <param name="cosignatures">The cosignatures.</param>
        /// <returns>AggregateTransaction.</returns>
        /// <exception cref="InvalidEnumArgumentException">networkType</exception>
        public static AggregateTransaction CreateBonded(NetworkType.Types networkType, Deadline deadline, List<Transaction> innerTransactions, List<AggregateTransactionCosignature> cosignatures)
        {
            if (!Enum.IsDefined(typeof(NetworkType.Types), networkType))
                throw new InvalidEnumArgumentException(nameof(networkType), (int)networkType, typeof(NetworkType.Types));

           
            return new AggregateTransaction(networkType, 2, TransactionTypes.Types.AggregateBonded, deadline, 0, innerTransactions, cosignatures);
        }

        /// <summary>
        /// Signs the aggregate transaction with cosigners.
        /// </summary>
        /// <param name="initiatorAccount">The initiator account.</param>
        /// <param name="cosignatories">The cosignatories.</param>
        /// <returns>SignedTransaction.</returns>
        /// <exception cref="ArgumentNullException">
        /// initiatorAccount
        /// or
        /// cosignatories
        /// </exception>
        public SignedTransaction SignWithAggregateCosigners(KeyPair initiatorAccount, List<Account> cosignatories)
        {
            if (initiatorAccount == null) throw new ArgumentNullException(nameof(initiatorAccount));
            if (cosignatories == null) throw new ArgumentNullException(nameof(cosignatories));

            var signedTransaction = SignWith(initiatorAccount);
            var payload = signedTransaction.Payload.FromHex();

            foreach (var cosignatory in cosignatories)
            {
                var bytes = signedTransaction.Hash.FromHex();

                var signatureBytes = TransactionExtensions.SignHash(cosignatory.KeyPair, bytes);

                payload = payload.Concat(cosignatory.KeyPair.PublicKey.Concat(signatureBytes)).ToArray();

                Cosignatures.Add(new AggregateTransactionCosignature(signatureBytes.ToHexLower(), new PublicAccount(cosignatory.KeyPair.PublicKey.ToHexLower(), Blockchain.NetworkType.Types.MIJIN_TEST)));  
            }

            payload = BitConverter.GetBytes(payload.Length).Concat(payload.Take(4, payload.Length - 4).ToArray()).ToArray();

            return SignedTransaction.Create(payload, signedTransaction.Hash.FromHex(), initiatorAccount.PublicKey, TransactionType);
        }

        /// <summary>
        /// Checks if the aggregate is either initiated or signed the by account as a cosignature.
        /// </summary>
        /// <param name="publicAccount">The public account.</param>
        /// <returns><c>true</c> if signed by the account, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">publicAccount</exception>
        public bool SignedByAccount(PublicAccount publicAccount)
        {
            if (publicAccount == null) throw new ArgumentNullException(nameof(publicAccount));

            return Signer.PublicKey == publicAccount.PublicKey || Cosignatures.Any(e => e.Signer.PublicKey == publicAccount.PublicKey);
        }

        internal override byte[] GenerateBytes()
        {
            var builder = new FlatBufferBuilder(1);

            var transactionsBytes = new byte[0];

            transactionsBytes = InnerTransactions.Aggregate(transactionsBytes, (current, innerTransaction) => current.Concat(innerTransaction.ToAggregate()).ToArray());

            // Create Vectors
            var signatureVector = AggregateTransactionBuffer.CreateSignatureVector(builder, new byte[64]);
            var signerVector = AggregateTransactionBuffer.CreateSignerVector(builder, GetSigner());
            var deadlineVector = AggregateTransactionBuffer.CreateDeadlineVector(builder, Deadline.Ticks.ToUInt8Array());
            var feeVector = AggregateTransactionBuffer.CreateFeeVector(builder, Fee.ToUInt8Array());
            var transactionsVector = AggregateTransactionBuffer.CreateTransactionsVector(builder, transactionsBytes);

            ushort version = ushort.Parse(NetworkType.GetNetworkByte().ToString("X") + "0" + Version.ToString("X"), System.Globalization.NumberStyles.HexNumber);
            // add vectors
            AggregateTransactionBuffer.StartAggregateTransactionBuffer(builder);
            AggregateTransactionBuffer.AddSize(builder, (uint)(120 + 4 + transactionsBytes.Length));
            AggregateTransactionBuffer.AddSignature(builder, signatureVector);
            AggregateTransactionBuffer.AddSigner(builder, signerVector);
            AggregateTransactionBuffer.AddVersion(builder, version);
            AggregateTransactionBuffer.AddType(builder, TransactionType.GetValue());
            AggregateTransactionBuffer.AddFee(builder, feeVector);
            AggregateTransactionBuffer.AddDeadline(builder, deadlineVector);
            AggregateTransactionBuffer.AddTransactionsSize(builder, (uint)transactionsBytes.Length);
            AggregateTransactionBuffer.AddTransactions(builder, transactionsVector);

            // end build
            var codedTransaction = AggregateTransactionBuffer.EndAggregateTransactionBuffer(builder).Value;
            builder.Finish(codedTransaction);

            return new AggregateTransactionSchema().Serialize(builder.SizedByteArray());
        }
    }
}
