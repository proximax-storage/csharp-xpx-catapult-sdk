// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="TransactionTypes.cs" company="Nem.io">
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
using io.nem2.sdk.Core.Crypto.Chaso.NaCl;
using io.nem2.sdk.Core.Utils;
using io.nem2.sdk.Infrastructure.Buffers;
using io.nem2.sdk.Infrastructure.Buffers.Schema;
using io.nem2.sdk.Infrastructure.Imported.FlatBuffers;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Blockchain;
using io.nem2.sdk.Model.Mosaics;

namespace io.nem2.sdk.Model.Transactions
{
    /// <inheritdoc />
    /// <summary>
    /// Class HashLockTransaction.
    /// </summary>
    /// <seealso cref="T:Transaction" />
    public class LockFundsTransaction : Transaction
    {
        /// <summary>
        /// Gets or sets the mosaic.
        /// </summary>
        /// <value>The mosaic.</value>
        public Mosaic Mosaic { get; }

        /// <summary>
        /// Gets or sets the duration.
        /// </summary>
        /// <value>The duration.</value>
        public ulong Duration { get; }

        /// <summary>
        /// Gets or sets the transaction.
        /// </summary>
        /// <value>The transactions.</value>
        public SignedTransaction Transaction { get; }

        /// <summary>
        /// Creates the specified HashLockTransaction.
        /// </summary>
        /// <param name="deadline">The deadline.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="fee">The transaction fee.</param>
        /// <param name="mosaic">The mosaic.</param>
        /// <param name="transaction">The transaction.</param>
        /// <param name="netowrkType">Type of the netowrk.</param>
        /// <returns><see cref="LockFundsTransaction"/>.</returns>
        public static LockFundsTransaction Create(NetworkType.Types netowrkType, Deadline deadline, ulong fee, Mosaic mosaic, ulong duration, SignedTransaction transaction)
        {
            return new LockFundsTransaction(netowrkType, 3, deadline, fee, mosaic, duration, transaction);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LockFundsTransaction"/> class.
        /// </summary>
        /// <param name="version">The transaction version.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="fee">The transaction fee.</param>
        /// <param name="mosaic">The mosaic.</param>
        /// <param name="transaction">The transaction.</param>
        /// <param name="networkType">Type of the network.</param>
        public LockFundsTransaction(NetworkType.Types networkType, int version, Deadline deadline, ulong fee, Mosaic mosaic, ulong duration, SignedTransaction transaction )
            : this(networkType, version, deadline, fee, mosaic, duration, transaction, null, null, null) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="LockFundsTransaction"/> class.
        /// </summary>
        /// <param name="version">The transaction version.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="fee">The transaction fee.</param>
        /// <param name="mosaic">The mosaic.</param>
        /// <param name="transaction">The transaction.</param>
        /// <param name="networkType">Type of the network.</param>
        /// <param name="signature">The signature.</param>
        /// <param name="signer">The signer.</param>
        /// <param name="transactionInfo">The transaction information.</param>
        public LockFundsTransaction(NetworkType.Types networkType, int version, Deadline deadline, ulong fee,  Mosaic mosaic, ulong duration, SignedTransaction transaction, string signature, PublicAccount signer, TransactionInfo transactionInfo)
        {
            if (transaction.TransactionType != TransactionTypes.Types.AggregateBonded) throw new ArgumentException("Cannot lock non-aggregate-bonded transaction");
            Deadline = deadline;
            Version = version;
            Duration = duration;
            Mosaic = mosaic;
            NetworkType = networkType;
            Transaction = transaction;
            TransactionType = TransactionTypes.Types.LockFunds;
            Signer = signer;
            Signature = signature;
            TransactionInfo = transactionInfo;
            Fee = fee;
        }

        /// <summary>
        /// Generates the bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        internal override byte[] GenerateBytes()
        {
            var builder = new FlatBufferBuilder(1);

            // create vectors
            var signatureVector = HashLockTransactionBuffer.CreateSignatureVector(builder, new byte[64]);
            var signerVector = HashLockTransactionBuffer.CreateSignerVector(builder, GetSigner());
            var feeVector = HashLockTransactionBuffer.CreateFeeVector(builder, Fee.ToUInt8Array());
            var deadlineVector = HashLockTransactionBuffer.CreateDeadlineVector(builder, Deadline.Ticks.ToUInt8Array());
            var mosaicId = HashLockTransactionBuffer.CreateIdVector(builder, Mosaic.MosaicId.Id.ToUInt8Array());
            var mosaicAmount = HashLockTransactionBuffer.CreateAmountVector(builder, Mosaic.Amount.ToUInt8Array());
            var duration = HashLockTransactionBuffer.CreateDurationVector(builder, Duration.ToUInt8Array());
            var hash = HashLockTransactionBuffer.CreateHashVector(builder, Transaction.Hash.FromHex());

            ushort version = ushort.Parse(NetworkType.GetNetworkByte().ToString("X") + "0" + Version.ToString("X"), System.Globalization.NumberStyles.HexNumber);

            // add vectors
            HashLockTransactionBuffer.StartTestBuffer(builder);
            HashLockTransactionBuffer.AddSize(builder, 176 );
            HashLockTransactionBuffer.AddSignature(builder, signatureVector);
            HashLockTransactionBuffer.AddSigner(builder, signerVector);
            HashLockTransactionBuffer.AddVersion(builder, version);
            HashLockTransactionBuffer.AddType(builder, TransactionTypes.Types.LockFunds.GetValue());
            HashLockTransactionBuffer.AddFee(builder, feeVector);
            HashLockTransactionBuffer.AddDeadline(builder, deadlineVector);
            HashLockTransactionBuffer.AddId(builder, mosaicId);
            HashLockTransactionBuffer.AddAmount(builder, mosaicAmount);
            HashLockTransactionBuffer.AddDuration(builder, duration);
            HashLockTransactionBuffer.AddHash(builder, hash);

            // end build
            var codedTransfer = HashLockTransactionBuffer.EndTestBuffer(builder);
            builder.Finish(codedTransfer.Value);

            return new HashLockTransactionSchema().Serialize(builder.SizedByteArray());
        }
    }
}
