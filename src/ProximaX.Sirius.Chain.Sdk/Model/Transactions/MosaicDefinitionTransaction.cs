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
using ProximaX.Sirius.Chain.Sdk.Buffers;
using ProximaX.Sirius.Chain.Sdk.Buffers.Schema;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions
{
    public class MosaicDefinitionTransaction : Transaction
    {
        /// <summary>
        ///     Constructor.
        /// </summary>
        public MosaicDefinitionTransaction(NetworkType networkType, int version, Deadline deadline, ulong maxFee,
            MosaicNonce nonce, MosaicId mosaicId, MosaicProperties properties, string signature = null,
            PublicAccount signer = null, TransactionInfo transactionInfo = null)
            : base(networkType, version, TransactionType.MOSAIC_DEFINITION, deadline, maxFee, signature, signer,
                transactionInfo)
        {
            Properties = properties;
            MosaicId = mosaicId;
            MosaicNonce = nonce;
        }

        /// <summary>
        ///     Gets the mosaic nonce
        /// </summary>
        /// <value>The the mosaic nonce.</value>
        public MosaicNonce MosaicNonce { get; }

        /// <summary>
        ///     The mosaic name.
        /// </summary>
        /// <value>The mosaic identifier.</value>
        public MosaicId MosaicId { get; }

        /// <summary>
        ///     The mosaic flags
        /// </summary>
        /// <value>The flags.</value>
        public MosaicProperties Properties { get; }

        public static MosaicDefinitionTransaction Create(MosaicNonce nonce, MosaicId mosaicId, Deadline deadline,
            MosaicProperties properties, NetworkType networkType)
        {
            if (mosaicId == null)
                throw new ArgumentNullException(nameof(mosaicId));

            return new MosaicDefinitionTransaction(networkType, TransactionVersion.MOSAIC_DEFINITION.GetValue(),
                deadline, 0, nonce, mosaicId, properties);
        }

        /// <summary>
        ///     Generates the bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        internal override byte[] GenerateBytes()
        {
            byte flags = 0;

            if (Properties.IsSupplyMutable) flags += 1;

            if (Properties.IsTransferable) flags += 2;

            if (Properties.IsLevyMutable) flags += 4;

            var builder = new FlatBufferBuilder(1);

            // create vectors
            var signatureVector = RegisterNamespaceTransactionBuffer.CreateSignatureVector(builder, new byte[64]);
            var signerVector = RegisterNamespaceTransactionBuffer.CreateSignerVector(builder, GetSigner());
            var feeVector = TransferTransactionBuffer.CreateFeeVector(builder, MaxFee?.ToUInt8Array());
            var deadlineVector =
                RegisterNamespaceTransactionBuffer.CreateDeadlineVector(builder, Deadline.Ticks.ToUInt8Array());
            var mosaicIdVector =
                MosaicDefinitionTransactionBuffer.CreateMosaicIdVector(builder, MosaicId.Id.ToUInt8Array());
            var durationVector =
                MosaicDefinitionTransactionBuffer.CreateDurationVector(builder, Properties.Duration.ToUInt8Array());

            var version = ushort.Parse(NetworkType.GetValueInByte().ToString("X") + "0" + Version.ToString("X"),
                NumberStyles.HexNumber);

            const int fixSize = 144;
            // add vectors to buffer
            MosaicDefinitionTransactionBuffer.StartMosaicDefinitionTransactionBuffer(builder);
            MosaicDefinitionTransactionBuffer.AddSize(builder, fixSize);
            MosaicDefinitionTransactionBuffer.AddSignature(builder, signatureVector);
            MosaicDefinitionTransactionBuffer.AddSigner(builder, signerVector);
            MosaicDefinitionTransactionBuffer.AddVersion(builder, version);
            MosaicDefinitionTransactionBuffer.AddType(builder, TransactionType.GetValue());
            MosaicDefinitionTransactionBuffer.AddFee(builder, feeVector);
            MosaicDefinitionTransactionBuffer.AddDeadline(builder, deadlineVector);
            MosaicDefinitionTransactionBuffer.AddMosaicNonce(builder, BitConverter.ToUInt32(MosaicNonce.Nonce, 0));
            MosaicDefinitionTransactionBuffer.AddMosaicId(builder, mosaicIdVector);
            MosaicDefinitionTransactionBuffer.AddNumOptionalProperties(builder, 1);
            MosaicDefinitionTransactionBuffer.AddFlags(builder, flags);
            MosaicDefinitionTransactionBuffer.AddDivisibility(builder, (byte) Properties.Divisibility);
            MosaicDefinitionTransactionBuffer.AddIndicateDuration(builder, 2);
            MosaicDefinitionTransactionBuffer.AddDuration(builder, durationVector);

            var t = BitConverter.ToUInt32(MosaicNonce.Nonce, 0);
            // Calculate size
            var codedMosaicDefinition = MosaicDefinitionTransactionBuffer.EndMosaicDefinitionTransactionBuffer(builder);
            builder.Finish(codedMosaicDefinition.Value);

            return new MosaicDefinitionTransactionSchema().Serialize(builder.SizedByteArray());
        }
    }
}