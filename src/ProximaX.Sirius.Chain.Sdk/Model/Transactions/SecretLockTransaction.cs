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
using System.Globalization;
using FlatBuffers;
using GuardNet;
using ProximaX.Sirius.Chain.Sdk.Buffers;
using ProximaX.Sirius.Chain.Sdk.Buffers.Schema;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions
{
    public class SecretLockTransaction : Transaction
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="networkType"></param>
        /// <param name="version"></param>
        /// <param name="deadline"></param>
        /// <param name="maxFee"></param>
        /// <param name="mosaic"></param>
        /// <param name="duration"></param>
        /// <param name="hashType"></param>
        /// <param name="secret"></param>
        /// <param name="recipient"></param>
        /// <param name="signature"></param>
        /// <param name="signer"></param>
        /// <param name="transactionInfo"></param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public SecretLockTransaction(NetworkType networkType, int version, Deadline deadline, ulong? maxFee,
            Mosaic mosaic, ulong duration, HashType hashType, string secret, Address recipient,
            string signature = null, PublicAccount signer = null, TransactionInfo transactionInfo = null)
            : base(networkType, version, TransactionType.SECRET_LOCK, deadline, maxFee, signature, signer,
                transactionInfo)
        {
            Guard.NotNull(mosaic, "Mosaic must not be null");
            Guard.NotNull(secret, "Secret must not be null");
            Guard.NotNull(recipient, "Recipient must not be null");
            if (!hashType.Validate(secret))
                throw new ArgumentOutOfRangeException(
                    "HashType and Secret have incompatible length or not hexadecimal string");

            Mosaic = mosaic;
            Duration = duration;
            HashType = hashType;
            Secret = secret;
            Recipient = recipient;
        }

        /// <summary>
        ///     Gets or sets the mosaic.
        /// </summary>
        /// <value>The mosaic.</value>
        public Mosaic Mosaic { get; }

        /// <summary>
        ///     Gets or sets the duration.
        /// </summary>
        /// <value>The duration.</value>
        public ulong Duration { get; }


        /// <summary>
        ///     The secret string in hex
        /// </summary>
        public string Secret { get; }

        /// <summary>
        ///     Gets or sets the hash type.
        /// </summary>
        /// <value>The hash type.</value>
        public HashType HashType { get; }

        /// <summary>
        ///     Gets or sets the recipient.
        /// </summary>
        /// <value>The recipient.</value>
        public Address Recipient { get; }

        /// <summary>
        ///     Create SecretLockTransaction
        /// </summary>
        /// <param name="deadline"></param>
        /// <param name="mosaic"></param>
        /// <param name="duration"></param>
        /// <param name="hashType"></param>
        /// <param name="secret"></param>
        /// <param name="recipient"></param>
        /// <param name="networkType"></param>
        /// <returns></returns>
        public static SecretLockTransaction Create(Deadline deadline, Mosaic mosaic, ulong duration, HashType hashType,
            string secret, Address recipient, NetworkType networkType)
        {
            return new SecretLockTransaction(networkType, TransactionVersion.SECRET_LOCK.GetValue(),
                deadline, 0, mosaic, duration, hashType, secret, recipient);
        }

        internal override byte[] GenerateBytes()
        {
            var builder = new FlatBufferBuilder(1);


            // create vectors
            var signatureVector = SecretLockTransactionBuffer.CreateSignatureVector(builder, new byte[64]);
            var signerVector = SecretLockTransactionBuffer.CreateSignerVector(builder, GetSigner());
            var feeVector = SecretLockTransactionBuffer.CreateMaxFeeVector(builder, MaxFee?.ToUInt8Array());
            var deadlineVector =
                SecretLockTransactionBuffer.CreateDeadlineVector(builder, Deadline.Ticks.ToUInt8Array());
            var mosaicIdVector = SecretLockTransactionBuffer.CreateMosaicIdVector(builder, Mosaic.Id.ToUInt8Array());
            var mosaicAmountVector =
                SecretLockTransactionBuffer.CreateMosaicAmountVector(builder, Mosaic.Amount.ToUInt8Array());
            var durationVector = SecretLockTransactionBuffer.CreateDurationVector(builder, Duration.ToUInt8Array());
            var secretVector = SecretLockTransactionBuffer.CreateSecretVector(builder, Secret.DecodeHexString());
            var recipientVector =
                SecretLockTransactionBuffer.CreateRecipientVector(builder, Recipient.Plain.FromBase32String());

            var version = ushort.Parse(NetworkType.GetValueInByte().ToString("X") + "0" + Version.ToString("X"),
                NumberStyles.HexNumber);

            const int fixedSize = 202;

            SecretLockTransactionBuffer.StartSecretLockTransactionBuffer(builder);
            SecretLockTransactionBuffer.AddSize(builder, fixedSize);
            SecretLockTransactionBuffer.AddSignature(builder, signatureVector);
            SecretLockTransactionBuffer.AddSigner(builder, signerVector);
            SecretLockTransactionBuffer.AddVersion(builder, version);
            SecretLockTransactionBuffer.AddType(builder, TransactionType.GetValue());
            SecretLockTransactionBuffer.AddMaxFee(builder, feeVector);
            SecretLockTransactionBuffer.AddDeadline(builder, deadlineVector);
            SecretLockTransactionBuffer.AddMosaicId(builder, mosaicIdVector);
            SecretLockTransactionBuffer.AddMosaicAmount(builder, mosaicAmountVector);
            SecretLockTransactionBuffer.AddDuration(builder, durationVector);
            SecretLockTransactionBuffer.AddHashAlgorithm(builder, HashType.GetValueInByte());
            SecretLockTransactionBuffer.AddSecret(builder, secretVector);
            SecretLockTransactionBuffer.AddRecipient(builder, recipientVector);

            // end build
            var codedTransfer = SecretLockTransactionBuffer.EndSecretLockTransactionBuffer(builder);
            builder.Finish(codedTransfer.Value);

            return new SecretLockTransactionSchema().Serialize(builder.SizedByteArray());
        }
    }
}