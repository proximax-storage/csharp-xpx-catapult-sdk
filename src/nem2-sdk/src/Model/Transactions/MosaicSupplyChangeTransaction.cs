// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 02-01-2018
// ***********************************************************************
// <copyright file="MosaicSupplyChangeTransaction.cs" company="Nem.io">
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
using System.ComponentModel;
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
    /// Class MosaicSupplyChangeTransaction.
    /// </summary>
    /// <seealso cref="T:io.nem2.sdk.Model.Transactions.Transaction" />
    public class MosaicSupplyChangeTransaction : Transaction
    {
        /// <summary>
        /// The delta. ie. The quantity by which the supply should be augmented.
        /// </summary>
        /// <value>The delta.</value>
        public ulong Delta { get; }

        /// <summary>
        /// The mosaic id.
        /// </summary>
        /// <value>The mosaic identifier.</value>
        public MosaicId MosaicId { get; }

        /// <summary>
        /// The direction. ie. If the supply should be increase or decreased by the given delta.
        /// </summary>
        /// <value>The direction.</value>
        public MosaicSupplyType.Type SupplyType { get; }

        /// <summary>
        /// Creates the specified MosaicSupplyChangeTransaction.
        /// </summary>
        /// <param name="mosaicId">The mosaic identifier.</param>
        /// <param name="delta">The delta.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="networkType">Type of the network.</param>
        /// <returns>MosaicSupplyChangeTransaction.</returns>
        public static MosaicSupplyChangeTransaction Create(NetworkType.Types networkType, Deadline deadline, MosaicId mosaicId,  MosaicSupplyType.Type direction, ulong delta)
        {
            return new MosaicSupplyChangeTransaction(networkType, 3, deadline, 0, mosaicId, direction, delta);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MosaicSupplyChangeTransaction"/> class.
        /// </summary>
        /// <param name="mosaicId">The mosaic identifier.</param>
        /// <param name="delta">The delta.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="version">The transaction version.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="fee">The fee.</param>
        /// <param name="networkType">Type of the network.</param>
        /// <exception cref="ArgumentOutOfRangeException">direction</exception>
        /// <exception cref="InvalidEnumArgumentException">networkType</exception>
        /// <exception cref="ArgumentNullException">mosaicId</exception>
        public MosaicSupplyChangeTransaction(NetworkType.Types networkType, int version, Deadline deadline, ulong fee, MosaicId mosaicId, MosaicSupplyType.Type direction, ulong delta) 
            : this(networkType, version, deadline, fee, mosaicId, direction, delta, null, null, null) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="MosaicSupplyChangeTransaction"/> class.
        /// </summary>
        /// <param name="networkType">Type of the network.</param>
        /// <param name="version">The transaction version.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="fee">The fee.</param>
        /// <param name="mosaicId">The mosaic identifier.</param>
        /// <param name="direction">The direction.</param>
        /// <param name="delta">The delta.</param>
        /// <param name="signature">The signature.</param>
        /// <param name="signer">The signer.</param>
        /// <param name="transactionInfo">The transaction information.</param>
        /// <exception cref="ArgumentOutOfRangeException">direction</exception>
        /// <exception cref="InvalidEnumArgumentException">networkType</exception>
        /// <exception cref="ArgumentNullException">mosaicId</exception>
        public MosaicSupplyChangeTransaction(NetworkType.Types networkType, int version, Deadline deadline, ulong fee, MosaicId mosaicId, MosaicSupplyType.Type direction, ulong delta, string signature, PublicAccount signer, TransactionInfo transactionInfo)
        {
            if (direction.GetValue() >= 2) throw new ArgumentOutOfRangeException(nameof(direction));
            if (!Enum.IsDefined(typeof(NetworkType.Types), networkType))
                throw new InvalidEnumArgumentException(nameof(networkType), (int)networkType,
                    typeof(NetworkType.Types));

            MosaicId = mosaicId ?? throw new ArgumentNullException(nameof(mosaicId));
            Delta = delta;
            SupplyType = direction;
            Version = version;
            Deadline = deadline;
            Fee = fee;
            NetworkType = networkType;
            TransactionType = TransactionTypes.Types.MosaicSupplyChange;
            Signer = signer;
            Signature = signature;
            TransactionInfo = transactionInfo;
        }

        /// <summary>
        /// Generates the bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        internal override byte[] GenerateBytes()
        {
            var builder = new FlatBufferBuilder(1);

            // create vectors
            var signatureVector = MosaicSupplyChangeTransactionBuffer.CreateSignatureVector(builder, new byte[64]);
            var signerVector = MosaicSupplyChangeTransactionBuffer.CreateSignerVector(builder, GetSigner());
            var feeVector = MosaicSupplyChangeTransactionBuffer.CreateFeeVector(builder, Fee.ToUInt8Array());
            var deadlineVector = MosaicSupplyChangeTransactionBuffer.CreateDeadlineVector(builder, Deadline.Ticks.ToUInt8Array());
            var mosaicIdVector = MosaicSupplyChangeTransactionBuffer.CreateMosaicIdVector(builder,MosaicId.Id.ToUInt8Array());
            var deltaVector = MosaicSupplyChangeTransactionBuffer.CreateFeeVector(builder, Delta.ToUInt8Array());

            ushort version = ushort.Parse(NetworkType.GetNetworkByte().ToString("X") + "0" + Version.ToString("X"), System.Globalization.NumberStyles.HexNumber);

            // add vectors to buffer
            MosaicSupplyChangeTransactionBuffer.StartMosaicSupplyChangeTransactionBuffer(builder);
            MosaicSupplyChangeTransactionBuffer.AddSize(builder, 137);
            MosaicSupplyChangeTransactionBuffer.AddSignature(builder, signatureVector);
            MosaicSupplyChangeTransactionBuffer.AddSigner(builder, signerVector);
            MosaicSupplyChangeTransactionBuffer.AddVersion(builder, version);
            MosaicSupplyChangeTransactionBuffer.AddType(builder, TransactionTypes.Types.MosaicSupplyChange.GetValue());
            MosaicSupplyChangeTransactionBuffer.AddFee(builder, feeVector);
            MosaicSupplyChangeTransactionBuffer.AddDeadline(builder, deadlineVector);
            MosaicSupplyChangeTransactionBuffer.AddMosaicId(builder, mosaicIdVector);
            MosaicSupplyChangeTransactionBuffer.AddDirection(builder, SupplyType.GetValue());
            MosaicSupplyChangeTransactionBuffer.AddDelta(builder, deltaVector);


            // end build
            var codedTransfer = TransferTransactionBuffer.EndTransferTransactionBuffer(builder);
            builder.Finish(codedTransfer.Value);

            return new MosaicSupplyChangeTransactionSchema().Serialize(builder.SizedByteArray());
        }

       
    }
}
