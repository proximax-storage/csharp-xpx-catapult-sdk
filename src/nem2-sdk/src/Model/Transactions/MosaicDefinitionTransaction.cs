// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 02-01-2018
// ***********************************************************************
// <copyright file="MosaicCreationTransaction.cs" company="Nem.io">
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

using System.Text;
using io.nem2.sdk.Core.Crypto.Chaso.NaCl;
using io.nem2.sdk.Core.Utils;
using io.nem2.sdk.Infrastructure.Buffers;
using io.nem2.sdk.Infrastructure.Buffers.Schema;
using io.nem2.sdk.Infrastructure.Imported.FlatBuffers;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Blockchain;
using io.nem2.sdk.Model.Mosaics;
using io.nem2.sdk.Model.Namespace;

namespace io.nem2.sdk.Model.Transactions
{
    /// <inheritdoc />
    /// <summary>
    /// Class MosaicDefinitionTransaction.
    /// </summary>
    /// <seealso cref="T:io.nem2.sdk.Model.Transactions.Transaction" />
    public class MosaicDefinitionTransaction : Transaction
    {
        /// <summary>
        /// Gets the name of the mosaic.
        /// </summary>
        /// <value>The name of the mosaic.</value>
        public  string MosaicName { get; }

        /// <summary>
        /// The parent id.
        /// </summary>
        /// <value>The namespace identifier.</value>
        public NamespaceId NamespaceId { get; }

        /// <summary>
        /// The mosaic name.
        /// </summary>
        /// <value>The mosaic identifier.</value>
        public MosaicId MosaicId { get; }

        /// <summary>
        /// The mosaic flags
        /// </summary>
        /// <value>The flags.</value>
        public MosaicProperties Properties { get; }

        /// <summary>
        /// Constructor.
        /// </summary>
        public MosaicDefinitionTransaction(NetworkType.Types networkType, int version, Deadline deadline, ulong fee, string mosaicName, NamespaceId namespaceId, MosaicId mosaicId, MosaicProperties properties)
            : this(networkType, version, deadline, fee, mosaicName, namespaceId, mosaicId, properties, null, null, null){}

        /// <summary>
        /// Constructor.
        /// </summary>
        public MosaicDefinitionTransaction(NetworkType.Types networkType, int version, Deadline deadline, ulong fee, string mosaicName, NamespaceId namespaceId, MosaicId mosaicId, MosaicProperties properties,  string signature, PublicAccount signer, TransactionInfo transactionInfo)
        {
            Deadline = deadline;
            NetworkType = networkType;
            Version = version;
            Properties = properties;
            MosaicId = mosaicId;
            NamespaceId = namespaceId;
            MosaicName = mosaicName;
            Fee = fee;
            TransactionType = TransactionTypes.Types.MosaicDefinition;
            Signature = signature;
            Signer = signer;
            TransactionInfo = transactionInfo;
        }

        public static MosaicDefinitionTransaction Create(NetworkType.Types networkType, Deadline deadline, string namespaceId,  string mosaicName, MosaicProperties properties)
        {
            return new MosaicDefinitionTransaction(
                networkType,
                3,
                deadline,
                0,             
                mosaicName,
                NamespaceId.Create(namespaceId),
                MosaicId.CreateFromMosaicIdentifier(namespaceId + ":" + mosaicName),              
                properties
            );
        }

        /// <summary>
        /// Generates the bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        internal override byte[] GenerateBytes()
        {
            byte flags = 0;

            if (Properties.IsSupplyMutable)
            {
                flags += 1;
            }

            if (Properties.IsTransferable)
            {
                flags += 2;
            }

            if (Properties.IsLevyMutable)
            {
                flags += 4;
            }

            var builder = new FlatBufferBuilder(1);

            // create vectors
            var signatureVector = MosaicCreationTransactionBuffer.CreateSignatureVector(builder, new byte[64]);
            var signerVector = MosaicCreationTransactionBuffer.CreateSignerVector(builder, GetSigner());
            var feeVector = MosaicCreationTransactionBuffer.CreateFeeVector(builder, Fee.ToUInt8Array());
            var deadlineVector = MosaicCreationTransactionBuffer.CreateDeadlineVector(builder, Deadline.Ticks.ToUInt8Array());
            var parentIdVector = MosaicCreationTransactionBuffer.CreateParentIdVector(builder, NamespaceId.Id.ToUInt8Array());
            var mosaicIdVector = MosaicCreationTransactionBuffer.CreateMosaicIdVector(builder, MosaicId.Id.ToUInt8Array());
            var mosaicNameVector = MosaicCreationTransactionBuffer.CreateMosaicNameVector(builder, Encoding.UTF8.GetBytes(MosaicId.MosaicName));
            var durationVector = MosaicCreationTransactionBuffer.CreateDurationVector(builder, Properties.Duration.ToUInt8Array());

            ushort version = ushort.Parse(NetworkType.GetNetworkByte().ToString("X") + "0" + Version.ToString("X"), System.Globalization.NumberStyles.HexNumber);


            // add vectors to buffer
            MosaicCreationTransactionBuffer.StartMosaicCreationTransactionBuffer(builder);
            MosaicCreationTransactionBuffer.AddSize(builder, (uint)(149 + Encoding.UTF8.GetBytes(MosaicId.MosaicName).ToHexLower().Length / 2));
            MosaicCreationTransactionBuffer.AddSignature(builder, signatureVector);
            MosaicCreationTransactionBuffer.AddSigner(builder, signerVector);
            MosaicCreationTransactionBuffer.AddVersion(builder, version);
            MosaicCreationTransactionBuffer.AddType(builder, TransactionTypes.Types.MosaicDefinition.GetValue());
            MosaicCreationTransactionBuffer.AddFee(builder, feeVector);
            MosaicCreationTransactionBuffer.AddDeadline(builder, deadlineVector);
            MosaicCreationTransactionBuffer.AddMosaicId(builder, mosaicIdVector);
            MosaicCreationTransactionBuffer.AddParentId(builder, parentIdVector);
            MosaicCreationTransactionBuffer.AddNameLength(builder, (byte)(Encoding.UTF8.GetBytes(MosaicId.MosaicName).ToHexLower().Length / 2));
            MosaicCreationTransactionBuffer.AddNumOptionalProperties(builder, 1);
            MosaicCreationTransactionBuffer.AddFlags(builder, flags);
            MosaicCreationTransactionBuffer.AddDivisibility(builder, (byte)Properties.Divisibility);
            MosaicCreationTransactionBuffer.AddMosaicName(builder, mosaicNameVector);
            MosaicCreationTransactionBuffer.AddIndicateDuration(builder, 2);
            MosaicCreationTransactionBuffer.AddDuration(builder, durationVector);

            // Calculate size
            var codedNamespace = NamespaceCreationTransactionBuffer.EndNamespaceCreationTransactionBuffer(builder);
            builder.Finish(codedNamespace.Value);

            return new MosaicCreationTransactionSchema().Serialize(builder.SizedByteArray());
        }
    }
}
