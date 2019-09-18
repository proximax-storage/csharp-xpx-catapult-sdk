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
using ProximaX.Sirius.Chain.Sdk.Model.Namespaces;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions
{
    public class AliasTransaction : Transaction
    {
        /// <summary>
        ///     The alias action type
        /// </summary>
        public AliasActionType AliasActionType;

        public AliasTransaction(NetworkType networkType, int version, Deadline deadline, ulong? maxFee,
            EntityType transactionType, NamespaceId namespaceId, AliasActionType actionType,
            MosaicId mosaicId = null, Address address = null, string signature = null, PublicAccount signer = null,
            TransactionInfo transactionInfo = null)
            : base(networkType, version, transactionType, deadline, maxFee, signature, signer, transactionInfo)
        {
            NamespaceId = namespaceId;
            MosaicId = mosaicId;
            AliasActionType = actionType;
            Address = address;
        }

        /// <summary>
        ///     The mosaic identifier.
        /// </summary>
        public MosaicId MosaicId { get; }

        /// <summary>
        ///     The namespace identifier
        /// </summary>
        public NamespaceId NamespaceId { get; }

        /// <summary>
        ///     The address
        /// </summary>
        public Address Address { get; }


        public static AliasTransaction CreateForMosaic(MosaicId mosaicId, NamespaceId namespaceId,
            AliasActionType actionType,
            Deadline deadline, NetworkType networkType)
        {
            if (namespaceId == null)
                throw new ArgumentNullException(nameof(namespaceId));

            if (mosaicId == null)
                throw new ArgumentNullException(nameof(mosaicId));

            return new AliasTransaction(networkType, EntityVersion.MOSAIC_ALIAS.GetValue(),
                deadline, 0, EntityType.MOSAIC_ALIAS, namespaceId, actionType, mosaicId);
        }

        public static int CalculatePayloadSize(bool isAddress)
        {
            // alias is either 25 bytes of address or 8 bytes of mosaic ID
            int aliasIdSize = isAddress ? 25 : 8;
            // alias ID + namespace ID + action
            return aliasIdSize + 8 + 1;
        }

        public static AliasTransaction CreateForAddress(Address address, NamespaceId namespaceId,
            AliasActionType actionType,
            Deadline deadline, NetworkType networkType)
        {
            if (namespaceId == null)
                throw new ArgumentNullException(nameof(namespaceId));

            if (address == null)
                throw new ArgumentNullException(nameof(address));

            return new AliasTransaction(networkType, EntityVersion.ADDRESS_ALIAS.GetValue(),
                deadline, 0, EntityType.ADDRESS_ALIAS, namespaceId, actionType, null, address);
        }

        internal override byte[] GenerateBytes()
        {
            var builder = new FlatBufferBuilder(1);

            byte[] aliasIdBytes;

            if (MosaicId != null)
                aliasIdBytes = BitConverter.GetBytes(MosaicId.Id);
            else if (Address != null)
                aliasIdBytes = Address.Plain.FromBase32String();
            else
                throw new Exception("Unsupported Alias");

            // create vectors
            var signatureVector = AliasTransactionBuffer.CreateSignatureVector(builder, new byte[64]);
            var signerVector = AliasTransactionBuffer.CreateSignerVector(builder, GetSigner());
            var feeVector = AliasTransactionBuffer.CreateMaxFeeVector(builder, MaxFee?.ToUInt8Array());
            var deadlineVector = AliasTransactionBuffer.CreateDeadlineVector(builder, Deadline.Ticks.ToUInt8Array());
            var namespaceIdVector =
                AliasTransactionBuffer.CreateNamespaceIdVector(builder, NamespaceId.Id.ToUInt8Array());
            var aliasIdVector = AliasTransactionBuffer.CreateAliasIdVector(builder, aliasIdBytes);

            //var fixedSize = HEADER_SIZE + aliasIdBytes.Length + 8 + 1;

            // header, 2 uint64 and int
            int totalSize = GetSerializedSize();

            // create version
            var version = GetTxVersionSerialization();


            AliasTransactionBuffer.StartAliasTransactionBuffer(builder);
            AliasTransactionBuffer.AddSize(builder, (uint)totalSize);
            AliasTransactionBuffer.AddSignature(builder, signatureVector);
            AliasTransactionBuffer.AddSigner(builder, signerVector);
            AliasTransactionBuffer.AddVersion(builder, (uint)version);
            AliasTransactionBuffer.AddType(builder, TransactionType.GetValue());
            AliasTransactionBuffer.AddMaxFee(builder, feeVector);
            AliasTransactionBuffer.AddDeadline(builder, deadlineVector);
            AliasTransactionBuffer.AddAliasId(builder, aliasIdVector);
            AliasTransactionBuffer.AddNamespaceId(builder, namespaceIdVector);
            AliasTransactionBuffer.AddActionType(builder, AliasActionType.GetValueInByte());

            // end build
            var codedTransaction = AliasTransactionBuffer.EndAliasTransactionBuffer(builder).Value;
            builder.Finish(codedTransaction);

            return new AliasTransactionSchema().Serialize(builder.SizedByteArray());
        }

        protected override int GetPayloadSerializedSize()
        {
              return  CalculatePayloadSize(Address != null);
        }
    }
}