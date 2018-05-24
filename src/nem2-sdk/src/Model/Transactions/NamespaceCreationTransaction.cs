// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 02-01-2018
// ***********************************************************************
// <copyright file="NamespaceCreationTransaction.cs" company="Nem.io">
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
using System.Text;
using io.nem2.sdk.Core.Utils;
using io.nem2.sdk.Infrastructure.Buffers;
using io.nem2.sdk.Infrastructure.Buffers.Schema;
using io.nem2.sdk.Infrastructure.Imported.FlatBuffers;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Blockchain;
using io.nem2.sdk.Model.Namespace;

namespace io.nem2.sdk.Model.Transactions
{
    /// <inheritdoc />
    /// <summary>
    /// Class RegisterNamespaceTransaction.
    /// </summary>
    /// <seealso cref="T:io.nem2.sdk.Model.Transactions.Transaction" />
    public class RegisterNamespaceTransaction : Transaction
    {
        /// <summary>
        /// The namespace type.
        /// </summary>
        /// <value>The type of the namespace.</value>
        public NamespaceTypes.Types NamespaceType { get; set; }

        /// <summary>
        /// The duration in blocks before the namespace expires.
        /// </summary>
        /// <value>The duration.</value>
        public ulong Duration { get; }

        /// <summary>
        /// The namespace name.
        /// </summary>
        /// <value>The name of the namespace.</value>
        public NamespaceId NamespaceId { get; }

        /// <summary>
        /// The parent namespace id, if applicable.
        /// </summary>
        /// <value>The parent namespace id.</value>
        /// <remarks>Null if the namespace to be rented is not a sub namespace.</remarks>
        public NamespaceId ParentId { get; }


        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterNamespaceTransaction"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="version">The version.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="fee">The fee.</param>
        /// <param name="namespaceType">Type of the namespace.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <param name="namespaceName">Name of the namespace.</param>
        public RegisterNamespaceTransaction(NetworkType.Types type, int version, Deadline deadline, ulong fee, byte namespaceType, ulong duration, NamespaceId parentId, NamespaceId namespaceName)
           : this(type, version, deadline, fee, namespaceType, duration, parentId, namespaceName, null, null, null){}

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterNamespaceTransaction"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="version">The version.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="fee">The fee.</param>
        /// <param name="namespaceType">Type of the namespace.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <param name="namespaceName">Name of the namespace.</param>
        /// <param name="signer">The signer.</param>
        /// <param name="signature">The signature.</param>
        /// <param name="transactionInfo">The transaction information.</param>
        /// <exception cref="ArgumentNullException">parentId</exception>
        /// <exception cref="InvalidEnumArgumentException">type</exception>
        /// <exception cref="ArgumentOutOfRangeException">namespaceType</exception>
        public RegisterNamespaceTransaction(NetworkType.Types type, int version, Deadline deadline, ulong fee, byte namespaceType, ulong duration, NamespaceId parentId, NamespaceId namespaceName, PublicAccount signer, string signature, TransactionInfo transactionInfo)
        {
            if (parentId == null && namespaceName == null) throw new ArgumentNullException(nameof(parentId) + " and " + nameof(namespaceName) + " cannot both be null");
            if (!Enum.IsDefined(typeof(NetworkType.Types), type)) throw new InvalidEnumArgumentException(nameof(type), (int)type, typeof(NetworkType.Types));
            if (namespaceType != 0 && namespaceType != 1) throw new ArgumentOutOfRangeException(nameof(namespaceType));
            
            NetworkType = type;
            Version = version;
            Deadline = deadline;
            Fee = fee;
            NamespaceType = NamespaceTypes.GetRawValue(namespaceType);
            TransactionType = TransactionTypes.Types.RegisterNamespace;
            Duration = duration;
            ParentId = parentId;
            NamespaceId = namespaceName;
            Signer = signer;
            Signature = signature;
            TransactionInfo = transactionInfo;
        }

        /// <summary>
        /// Creates the root namespace.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="namespaceName">Name of the namespace.</param>
        /// <param name="duration">The duration.</param>
        /// <returns>RegisterNamespaceTransaction.</returns>
        public static RegisterNamespaceTransaction CreateRootNamespace(NetworkType.Types type, Deadline deadline, string namespaceName, ulong duration)
        {
            return new RegisterNamespaceTransaction(type, 3, deadline, 0, 0x00, duration, null, NamespaceId.Create(namespaceName));
        }

        /// <summary>
        /// Creates the sub namespace.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="parentId">The parent identifier.</param>
        /// <param name="namespaceName">Name of the namespace.</param>
        /// <returns>RegisterNamespaceTransaction.</returns>
        public static RegisterNamespaceTransaction CreateSubNamespace(NetworkType.Types type, Deadline deadline, string parentId, string namespaceName)
        {
            return new RegisterNamespaceTransaction(type, 3, deadline, 0, 0x01, 0, NamespaceId.Create(parentId), NamespaceId.Create(namespaceName));
        }

        /// <summary>
        /// Generates the bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        internal override byte[] GenerateBytes()
        {
            var builder = new FlatBufferBuilder(1);

            var namespaceNameLength = (uint)Encoding.UTF8.GetBytes(NamespaceId.Name).Length;

            // create vectors
            var signatureVector = NamespaceCreationTransactionBuffer.CreateSignatureVector(builder, new byte[64]);
            var signerVector = NamespaceCreationTransactionBuffer.CreateSignerVector(builder, GetSigner());
            var feeVector = NamespaceCreationTransactionBuffer.CreateFeeVector(builder, Fee.ToUInt8Array());
            var deadlineVector = NamespaceCreationTransactionBuffer.CreateDeadlineVector(builder, Deadline.Ticks.ToUInt8Array());
            var durationParentId = ParentId == null ? Duration.ToUInt8Array() : ParentId.Id.ToUInt8Array();
            var durationParentIdVector = NamespaceCreationTransactionBuffer.CreateDurationParentIdVector(builder, durationParentId);
            var namespaceIdVector = NamespaceCreationTransactionBuffer.CreateNamespaceIdVector(builder, IdGenerator.GenerateId(NamespaceType == 0x00 ? 0 : IdGenerator.GenerateId(0, ParentId.Name), NamespaceId.Name).ToUInt8Array());

            var name = NamespaceCreationTransactionBuffer.CreateNamespaceNameVector(builder, Encoding.UTF8.GetBytes(NamespaceId.Name));
            ushort version = ushort.Parse(NetworkType.GetNetworkByte().ToString("X") + "0" + Version.ToString("X"), System.Globalization.NumberStyles.HexNumber);

            // Add to buffer
            NamespaceCreationTransactionBuffer.StartNamespaceCreationTransactionBuffer(builder);
            NamespaceCreationTransactionBuffer.AddSize(builder, 138 + namespaceNameLength);
            NamespaceCreationTransactionBuffer.AddSignature(builder, signatureVector);
            NamespaceCreationTransactionBuffer.AddSigner(builder, signerVector);
            NamespaceCreationTransactionBuffer.AddVersion(builder, version);
            NamespaceCreationTransactionBuffer.AddType(builder, TransactionTypes.Types.RegisterNamespace.GetValue());
            NamespaceCreationTransactionBuffer.AddFee(builder, feeVector);
            NamespaceCreationTransactionBuffer.AddDeadline(builder, deadlineVector);
            NamespaceCreationTransactionBuffer.AddNamespaceType(builder, NamespaceType.GetValue());
            NamespaceCreationTransactionBuffer.AddDurationParentId(builder, durationParentIdVector);
            NamespaceCreationTransactionBuffer.AddNamespaceId(builder, namespaceIdVector);
            NamespaceCreationTransactionBuffer.AddNamespaceNameSize(builder, (byte)namespaceNameLength);
            NamespaceCreationTransactionBuffer.AddNamespaceName(builder, name);

            // Calculate size
            var codedNamespace = NamespaceCreationTransactionBuffer.EndNamespaceCreationTransactionBuffer(builder);
            builder.Finish(codedNamespace.Value);

            return new NamespaceCreateionTransactionSchema().Serialize(builder.SizedByteArray());      
        }
    }
}
