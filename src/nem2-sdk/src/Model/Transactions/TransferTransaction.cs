// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 02-01-2018
// ***********************************************************************
// <copyright file="TransferTransaction.cs" company="Nem.io">
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
using io.nem2.sdk.Core.Utils;
using io.nem2.sdk.Infrastructure.Buffers;
using io.nem2.sdk.Infrastructure.Buffers.Schema;
using io.nem2.sdk.Infrastructure.Imported.FlatBuffers;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Blockchain;
using io.nem2.sdk.Model.Mosaics;
using io.nem2.sdk.Model.Transactions.Messages;

namespace io.nem2.sdk.Model.Transactions
{
    /// <summary>
    /// Class TransferTransaction.
    /// </summary>
    /// <seealso cref="Transaction" />
    public class TransferTransaction : Transaction
    {
        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>The address.</value>
        public Address Address { get; }

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public IMessage Message { get; private set; }

        /// <summary>
        /// Gets the mosaics.
        /// </summary>
        /// <value>The mosaics.</value>
        public List<Mosaic> Mosaics { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransferTransaction" /> class.
        /// </summary>
        /// <param name="networkType">Type of the network.</param>
        /// <param name="version">The version.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="fee">The fee.</param>
        /// <param name="recipient">The recipient.</param>
        /// <param name="mosaics">The mosaics.</param>
        /// <param name="message">The message.</param>
        /// <exception cref="ArgumentNullException">mosaics
        /// or
        /// recipient</exception>
        internal TransferTransaction(NetworkType.Types networkType, int version, Deadline deadline, ulong fee, Address recipient, List<Mosaic> mosaics, IMessage message)
        {
            if (mosaics == null) throw new ArgumentNullException(nameof(mosaics));
            Address = recipient ?? throw new ArgumentNullException(nameof(recipient));
            TransactionType = TransactionTypes.Types.Transfer;
            Version = version;
            mosaics.Sort((c1, c2) => string.CompareOrdinal(c1.MosaicId.MosaicName, c2.MosaicId.MosaicName));
            Deadline = deadline;
            Message = message ?? EmptyMessage.Create();
            Mosaics = mosaics;
            NetworkType = networkType;
            Fee = fee;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransferTransaction"/> class.
        /// </summary>
        /// <param name="networkType">Type of the network.</param>
        /// <param name="version">The version.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="fee">The fee.</param>
        /// <param name="recipient">The recipient.</param>
        /// <param name="mosaics">The mosaics.</param>
        /// <param name="message">The message.</param>
        /// <param name="signature">The signature.</param>
        /// <param name="signer">The signer.</param>
        /// <param name="transactionInfo">The transaction information.</param>
        /// <exception cref="ArgumentNullException">
        /// mosaics
        /// or
        /// recipient
        /// </exception>
        internal TransferTransaction(NetworkType.Types networkType, int version, Deadline deadline, ulong fee, Address recipient, List<Mosaic> mosaics, IMessage message, string signature, PublicAccount signer, TransactionInfo transactionInfo)
        {
            if (mosaics == null) throw new ArgumentNullException(nameof(mosaics));
            Address = recipient ?? throw new ArgumentNullException(nameof(recipient));
            mosaics.Sort((c1, c2) => string.CompareOrdinal(c1.MosaicId.MosaicName, c2.MosaicId.MosaicName));
            TransactionType = TransactionTypes.Types.Transfer;
            Version = version;
            Deadline = deadline;
            Message = message ?? EmptyMessage.Create();
            Mosaics = mosaics;
            NetworkType = networkType;
            Fee = fee;
            Signature = signature;
            Signer = signer;
            TransactionInfo = transactionInfo;
        }

        /// <summary>
        /// Statically creates an instance of <see cref="TransferTransaction" />.
        /// </summary>
        /// <param name="deadline">The deadline.</param>
        /// <param name="address">The address.</param>
        /// <param name="mosaics">The mosaics.</param>
        /// <param name="message">The message.</param>
        /// <param name="netowrkType">Type of the netowrk.</param>
        /// <param name="signer">The signer.</param>
        /// <returns><see cref="TransferTransaction" />.</returns>
        public static TransferTransaction Create(NetworkType.Types netowrkType, Deadline deadline, Address address, List<Mosaic> mosaics, IMessage message)
        {
            return new TransferTransaction(netowrkType, 3, deadline, 0, address, mosaics, message);
        }

        /// <summary>
        /// Generates the bytes.
        /// </summary>
        /// <returns>The transaction bytes.</returns>
        internal override byte[] GenerateBytes()
        {
            var builder = new FlatBufferBuilder(1);

            // create vectors
            var signatureVector = TransferTransactionBuffer.CreateSignatureVector(builder, new byte[64]);
            var signerVector = TransferTransactionBuffer.CreateSignerVector(builder, GetSigner());
            var feeVector = TransferTransactionBuffer.CreateFeeVector(builder, Fee.ToUInt8Array());
            var deadlineVector = TransferTransactionBuffer.CreateDeadlineVector(builder, Deadline.Ticks.ToUInt8Array());
            var recipientVector = TransferTransactionBuffer.CreateRecipientVector(builder, Address.Plain.FromBase32String());

            ushort version = ushort.Parse(NetworkType.GetNetworkByte().ToString("X") + "0" + Version.ToString("X"), System.Globalization.NumberStyles.HexNumber);

            if (Message == null) Message = EmptyMessage.Create();

            // create message vector
            var bytePayload = Message.GetPayload(); 
            var payload = MessageBuffer.CreatePayloadVector(builder, bytePayload);
            MessageBuffer.StartMessageBuffer(builder);
            if (bytePayload != null) MessageBuffer.AddType(builder, Message.GetMessageType());
            MessageBuffer.AddPayload(builder, payload);
            var message = MessageBuffer.EndMessageBuffer(builder);

            // create mosaics vector
            var mosaics = new Offset<MosaicBuffer>[Mosaics.Count];
            for (var index = 0; index < Mosaics.Count; index++)
            {
                var mosaic = Mosaics[index];
                var idPayload = MosaicBuffer.CreateIdVector(builder, mosaic.MosaicId.Id.ToUInt8Array());
                var amountVector = MosaicBuffer.CreateAmountVector(builder, mosaic.Amount.ToUInt8Array());
                MosaicBuffer.StartMosaicBuffer(builder);
                MosaicBuffer.AddId(builder, idPayload);
                MosaicBuffer.AddAmount(builder, amountVector);

                mosaics[index] = MosaicBuffer.EndMosaicBuffer(builder);
            }

            var mosaicsVector = TransferTransactionBuffer.CreateMosaicsVector(builder, mosaics);

            // add vectors
            TransferTransactionBuffer.StartTransferTransactionBuffer(builder);
            TransferTransactionBuffer.AddSize(builder, (uint)(/*fixed size*/148 + 16 * Mosaics.Count + Message.GetLength()));
            TransferTransactionBuffer.AddSignature(builder, signatureVector);
            TransferTransactionBuffer.AddSigner(builder, signerVector);
            TransferTransactionBuffer.AddVersion(builder, version);
            TransferTransactionBuffer.AddType(builder, TransactionTypes.Types.Transfer.GetValue());
            TransferTransactionBuffer.AddFee(builder, feeVector);
            TransferTransactionBuffer.AddDeadline(builder, deadlineVector);
            TransferTransactionBuffer.AddRecipient(builder, recipientVector);
            TransferTransactionBuffer.AddNumMosaics(builder, (byte)Mosaics.Count);
            TransferTransactionBuffer.AddMessageSize(builder, (byte)Message.GetLength());
            TransferTransactionBuffer.AddMessage(builder, message);
            TransferTransactionBuffer.AddMosaics(builder, mosaicsVector);

            // end build
            var codedTransfer = TransferTransactionBuffer.EndTransferTransactionBuffer(builder);
            builder.Finish(codedTransfer.Value);

            return new TransferTransactionSchema().Serialize(builder.SizedByteArray());
        }
    }  
}
