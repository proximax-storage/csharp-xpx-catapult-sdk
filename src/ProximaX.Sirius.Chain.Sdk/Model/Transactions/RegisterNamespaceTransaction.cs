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
using System.ComponentModel;
using System.Globalization;
using FlatBuffers;
using ProximaX.Sirius.Chain.Sdk.Buffers;
using ProximaX.Sirius.Chain.Sdk.Buffers.Schema;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Namespaces;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions
{
    /// <summary>
    ///     Accounts can rent a namespace for an amount of blocks and after a this renew the contract.
    ///     This is done via a RegisterNamespaceTransaction.
    /// </summary>
    public class RegisterNamespaceTransaction : Transaction
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="RegisterNamespaceTransaction" /> class.
        /// </summary>
        /// <param name="networkType"></param>
        /// <param name="version"></param>
        /// <param name="deadline"></param>
        /// <param name="maxFee"></param>
        /// <param name="namespaceName"></param>
        /// <param name="namespaceId"></param>
        /// <param name="namespaceType"></param>
        /// <param name="duration"></param>
        /// <param name="parentId"></param>
        /// <param name="signature"></param>
        /// <param name="signer"></param>
        /// <param name="transactionInfo"></param>
        public RegisterNamespaceTransaction(NetworkType networkType, int version, Deadline deadline, ulong? maxFee,
            string namespaceName, NamespaceId namespaceId, NamespaceType namespaceType, ulong? duration,
            NamespaceId parentId = null, string signature = null, PublicAccount signer = null,
            TransactionInfo transactionInfo = null)
            : base(networkType, version, EntityType.REGISTER_NAMESPACE, deadline, maxFee, signature, signer,
                transactionInfo)
        {
            if (string.IsNullOrEmpty(namespaceName))
                throw new ArgumentNullException(nameof(namespaceName));

            if (!Enum.IsDefined(typeof(NetworkType), networkType))
                throw new InvalidEnumArgumentException(nameof(networkType), networkType.GetValue(),
                    typeof(NetworkType));
            if (namespaceType != NamespaceType.ROOT_NAMESPACE && namespaceType != NamespaceType.SUB_NAMESPACE)
                throw new ArgumentOutOfRangeException(nameof(namespaceType));
            if (namespaceType == NamespaceType.ROOT_NAMESPACE)
            {
                if (!duration.HasValue)
                    throw new ArgumentNullException(nameof(duration));
            }
            else
            {
                if (parentId == null)
                    throw new ArgumentNullException(nameof(parentId));
            }


            Duration = duration;
            NamespaceName = namespaceName;
            NamespaceType = namespaceType;
            NamespaceId = namespaceId;
            ParentId = parentId;
        }

        /// <summary>
        ///     The namespace name
        /// </summary>
        public string NamespaceName { get; }

        /// <summary>
        ///     The namespace id
        /// </summary>
        public NamespaceId NamespaceId { get; }

        /// <summary>
        ///     The rent duration
        /// </summary>
        public ulong? Duration { get; }

        /// <summary>
        ///     The parent namespace
        /// </summary>
        public NamespaceId ParentId { get; }

        /// <summary>
        ///     The namespace type
        /// </summary>
        public NamespaceType NamespaceType { get; }

        /// <summary>
        ///     Create the root namespace
        /// </summary>
        /// <param name="deadline">The transaction deadline</param>
        /// <param name="namespaceName">The namespace name</param>
        /// <param name="duration">The namespace rent duration</param>
        /// <param name="networkType">The network type</param>
        /// <param name="maxFee">The network fee</param>
        /// <returns>RegisterNamespaceTransaction</returns>
        public static RegisterNamespaceTransaction CreateRootNamespace(Deadline deadline,
            string namespaceName, ulong duration, NetworkType networkType, ulong maxFee = 0)
        {
            return new RegisterNamespaceTransaction(networkType, EntityVersion.REGISTER_NAMESPACE.GetValue(),
                deadline, maxFee, namespaceName, new NamespaceId(namespaceName),
                NamespaceType.ROOT_NAMESPACE, duration);
        }

        /// <summary>
        ///     Create the sub namespace
        /// </summary>
        /// <param name="deadline">The transaction deadline</param>
        /// <param name="namespaceName">The namespace name</param>
        /// <param name="parentId">The parent namespace id</param>
        /// <param name="networkType">The network type</param>
        /// <param name="maxFee">The network fee</param>
        /// <returns>RegisterNamespaceTransaction</returns>
        public static RegisterNamespaceTransaction CreateSubNamespace(Deadline deadline,
            string namespaceName, NamespaceId parentId, NetworkType networkType, ulong maxFee = 0)
        {
            var subNamespaceId = IdGenerator.GenerateSubNamespaceIdFromParentId(parentId.Id, namespaceName);

            return new RegisterNamespaceTransaction(networkType, EntityVersion.REGISTER_NAMESPACE.GetValue(),
                deadline, maxFee, namespaceName, new NamespaceId(subNamespaceId),
                NamespaceType.SUB_NAMESPACE, null, parentId);
        }

        /// <summary>
        ///     Generate Bytes
        /// </summary>
        /// <returns>byte[]</returns>
        internal override byte[] GenerateBytes()
        {
            var builder = new FlatBufferBuilder(1);

            // create vectors
            var signatureVector = RegisterNamespaceTransactionBuffer.CreateSignatureVector(builder, new byte[64]);
            var signerVector = RegisterNamespaceTransactionBuffer.CreateSignerVector(builder, GetSigner());
            var feeVector = TransferTransactionBuffer.CreateMaxFeeVector(builder, MaxFee?.ToUInt8Array());
            var deadlineVector =
                RegisterNamespaceTransactionBuffer.CreateDeadlineVector(builder, Deadline.Ticks.ToUInt8Array());
            var namespaceIdVector =
                RegisterNamespaceTransactionBuffer.CreateNamespaceIdVector(builder, NamespaceId.Id.ToUInt8Array());
            var durationParentId = NamespaceType == NamespaceType.ROOT_NAMESPACE
                ? Duration?.ToUInt8Array()
                : ParentId.Id.ToUInt8Array();
            var durationParentIdVector =
                RegisterNamespaceTransactionBuffer.CreateDurationParentIdVector(builder, durationParentId);

        

            // header, ns type, duration, ns id, name size, name
            int fixedSize = HEADER_SIZE + 1 + 8 + 8 + 1 + NamespaceName.Length;
           
            // create version
            var version = GetTxVersionSerialization();

            var name = builder.CreateString(NamespaceName);

            // ADD to buffer
            RegisterNamespaceTransactionBuffer.StartRegisterNamespaceTransactionBuffer(builder);
            RegisterNamespaceTransactionBuffer.AddSize(builder, (uint) fixedSize);
            RegisterNamespaceTransactionBuffer.AddSignature(builder, signatureVector);
            RegisterNamespaceTransactionBuffer.AddSigner(builder, signerVector);
            RegisterNamespaceTransactionBuffer.AddVersion(builder, (uint)version);
            RegisterNamespaceTransactionBuffer.AddType(builder, TransactionType.GetValue());
            RegisterNamespaceTransactionBuffer.AddMaxFee(builder, feeVector);
            RegisterNamespaceTransactionBuffer.AddDeadline(builder, deadlineVector);
            RegisterNamespaceTransactionBuffer.AddNamespaceType(builder, NamespaceType.GetValueInByte());
            RegisterNamespaceTransactionBuffer.AddDurationParentId(builder, durationParentIdVector);
            RegisterNamespaceTransactionBuffer.AddNamespaceId(builder, namespaceIdVector);
            RegisterNamespaceTransactionBuffer.AddNamespaceNameSize(builder, (byte) NamespaceName.Length);
            RegisterNamespaceTransactionBuffer.AddNamespaceName(builder, name);

            // Calculate size
            var codedNamespace = RegisterNamespaceTransactionBuffer.EndRegisterNamespaceTransactionBuffer(builder);
            builder.Finish(codedNamespace.Value);

            return new RegisterNamespaceTransactionSchema().Serialize(builder.SizedByteArray());
        }
    }
}