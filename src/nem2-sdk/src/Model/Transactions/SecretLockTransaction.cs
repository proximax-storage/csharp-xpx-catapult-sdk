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
    /// Class SecretLockTransaction.
    /// </summary>
    /// <seealso cref="T:io.nem2.sdk.Model.Transactions.Transaction" />
    public class SecretLockTransaction : Transaction
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
        /// Gets or sets the secret.
        /// </summary>
        /// <value>The secret.</value>
        internal byte[] Secret { get; }

        public string SecretString() => Secret.ToHexUpper();

        /// <summary>
        /// Gets or sets the hash algo.
        /// </summary>
        /// <value>The hash algo.</value>
        public HashType.Types HashAlgo { get; }

        /// <summary>
        /// Gets or sets the recipient.
        /// </summary>
        /// <value>The recipient.</value>
        public Address Recipient { get; }

        /// <summary>
        /// Creates the specified deadline.
        /// </summary>
        /// <param name="deadline">The deadline.</param>
        /// <param name="mosaic">The mosaic.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="fee">The fee.</param>
        /// <param name="hashAlgo">The hash algo.</param>
        /// <param name="secret">The secret.</param>
        /// <param name="recipient">The recipient.</param>
        /// <param name="netowrkType">Type of the netowrk.</param>
        /// <returns>SecretLockTransaction.</returns>
        public static SecretLockTransaction Create(NetworkType.Types netowrkType, int version, Deadline deadline, ulong fee, Mosaic mosaic, ulong duration, HashType.Types hashAlgo, string secret, Address recipient)
        {
            return new SecretLockTransaction(netowrkType, version, deadline,fee, mosaic, duration, hashAlgo, secret, recipient, null, null, null);
        }

        /// <summary>
        /// Creates the specified deadline.
        /// </summary>
        /// <param name="deadline">The deadline.</param>
        /// <param name="mosaic">The mosaic.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="fee">The fee.</param>
        /// <param name="hashAlgo">The hash algo.</param>
        /// <param name="secret">The secret.</param>
        /// <param name="recipient">The recipient.</param>
        /// <param name="netowrkType">Type of the netowrk.</param>
        /// <returns>SecretLockTransaction.</returns>
        public static SecretLockTransaction Create(NetworkType.Types netowrkType, Deadline deadline, Mosaic mosaic, ulong duration, HashType.Types hashAlgo, string secret, Address recipient)
        {
            return new SecretLockTransaction(netowrkType, 3, deadline, 0, mosaic, duration, hashAlgo, secret, recipient, null, null, null);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecretLockTransaction"/> class.
        /// </summary>
        /// <param name="networkType">Type of the network.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="fee">The fee.</param>
        /// <param name="mosaic">The mosaic.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="hashAlgo">The hash algo.</param>
        /// <param name="secret">The secret.</param>
        /// <param name="recipient">The recipient.</param>
        public SecretLockTransaction(NetworkType.Types networkType, int version, Deadline deadline, ulong fee, Mosaic mosaic, ulong duration, HashType.Types hashAlgo, string secret, Address recipient)
            : this(networkType, version, deadline, fee, mosaic, duration, hashAlgo, secret, recipient, null, null, null) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="SecretLockTransaction"/> class.
        /// </summary>
        /// <param name="networkType">Type of the network.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="fee">The fee.</param>
        /// <param name="mosaic">The mosaic.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="hashAlgo">The hash algo.</param>
        /// <param name="secret">The secret.</param>
        /// <param name="recipient">The recipient.</param>
        /// <param name="signature">The signature.</param>
        /// <param name="signer">The signer.</param>
        /// <param name="transactionInfo">The transaction information.</param>
        /// <exception cref="ArgumentException">invalid secret</exception>
        public SecretLockTransaction(NetworkType.Types networkType, int version, Deadline deadline, ulong fee, Mosaic mosaic, ulong duration, HashType.Types hashAlgo, string secret, Address recipient, string signature, PublicAccount signer, TransactionInfo transactionInfo)
        {
            if (hashAlgo.GetHashTypeValue() == 0 && secret.Length != 128) throw new ArgumentException("invalid secret");

            Deadline = deadline;
            Version = version;
            Fee = fee;
            Duration = duration;
            Mosaic = mosaic;
            NetworkType = networkType;
            HashAlgo = hashAlgo;
            Secret = secret.FromHex();
            Recipient = recipient;
            TransactionType = TransactionTypes.Types.SecretLock;
            Signer = signer;
            Signature = signature;
            TransactionInfo = transactionInfo;
        }

        internal override byte[] GenerateBytes()
        {

            var builder = new FlatBufferBuilder(1);

            // create vectors
            var signatureVector = SecretLockTransactionBuffer.CreateSignatureVector(builder, new byte[64]);
            var signerVector = SecretLockTransactionBuffer.CreateSignerVector(builder, GetSigner());
            var feeVector = SecretLockTransactionBuffer.CreateFeeVector(builder, Fee.ToUInt8Array());
            var deadlineVector = SecretLockTransactionBuffer.CreateDeadlineVector(builder, Deadline.Ticks.ToUInt8Array());
            var mosaicId = SecretLockTransactionBuffer.CreateMosaicIdVector(builder, Mosaic.MosaicId.Id.ToUInt8Array());
            var mosaicAmount = SecretLockTransactionBuffer.CreateMosaicAmountVector(builder, Mosaic.Amount.ToUInt8Array());
            var duration = SecretLockTransactionBuffer.CreateDurationVector(builder, Duration.ToUInt8Array());
            var secretVector = SecretLockTransactionBuffer.CreateSecretVector(builder, Secret);
           
            var recipientVector = SecretLockTransactionBuffer.CreateRecipientVector(builder, Recipient.Plain.FromBase32String());
            ushort version = ushort.Parse(NetworkType.GetNetworkByte().ToString("X") + "0" + Version.ToString("X"), System.Globalization.NumberStyles.HexNumber);

            // add vectors
            SecretLockTransactionBuffer.StartSecretLockTransactionBuffer(builder);
            SecretLockTransactionBuffer.AddSize(builder, 234);
            SecretLockTransactionBuffer.AddSignature(builder, signatureVector);
            SecretLockTransactionBuffer.AddSigner(builder, signerVector);
            SecretLockTransactionBuffer.AddVersion(builder, version);
            SecretLockTransactionBuffer.AddType(builder, TransactionTypes.Types.SecretLock.GetValue());
            SecretLockTransactionBuffer.AddFee(builder, feeVector);
            SecretLockTransactionBuffer.AddDeadline(builder, deadlineVector);
            SecretLockTransactionBuffer.AddMosaicId(builder, mosaicId);
            SecretLockTransactionBuffer.AddMosaicAmount(builder, mosaicAmount);
            SecretLockTransactionBuffer.AddDuration(builder, duration);
            SecretLockTransactionBuffer.AddHashAlgorithm(builder, HashAlgo.GetHashTypeValue());
            SecretLockTransactionBuffer.AddSecret(builder, secretVector);
            SecretLockTransactionBuffer.AddRecipient(builder, recipientVector);

            // end build
            var codedTransfer = SecretLockTransactionBuffer.EndSecretLockTransactionBuffer(builder);
            builder.Finish(codedTransfer.Value);

            return new SecretLockTransactionSchema().Serialize(builder.SizedByteArray());
        }
    }
}
