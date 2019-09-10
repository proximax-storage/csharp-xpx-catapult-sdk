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
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using System.Text;
using FlatBuffers;
using ProximaX.Sirius.Chain.Sdk.Buffers;
using ProximaX.Sirius.Chain.Sdk.Buffers.Schema;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Metadata;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Namespaces;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions
{
    /// <summary>
    ///     Class ModifyMetadataTransaction
    /// </summary>
    public class ModifyMetadataTransaction : Transaction
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="networkType"></param>
        /// <param name="version"></param>
        /// <param name="transactionType"></param>
        /// <param name="deadline"></param>
        /// <param name="maxFee"></param>
        /// <param name="metadataType"></param>
        /// <param name="metadataId"></param>
        /// <param name="address"></param>
        /// <param name="modifications"></param>
        /// <param name="signature"></param>
        /// <param name="signer"></param>
        /// <param name="transactionInfo"></param>
        public ModifyMetadataTransaction(NetworkType networkType, int version, EntityType transactionType,
            Deadline deadline, ulong? maxFee,
            MetadataType metadataType, ulong? metadataId, Address address = null,
            IList<MetadataModification> modifications = null,
            string signature = null, PublicAccount signer = null, TransactionInfo transactionInfo = null)
            : base(networkType, version, transactionType, deadline, maxFee, signature, signer, transactionInfo)
        {
            MetadataType = metadataType;
            MetadataId = metadataId;
            Address = address;
            Modifications = modifications;
        }

        /// <summary>
        ///     The metadata type
        /// </summary>
        public MetadataType MetadataType { get; }

        /// <summary>
        ///     The metadata id
        /// </summary>
        public ulong? MetadataId { get; }

        /// <summary>
        ///     The address
        /// </summary>
        public Address Address { get; }

        /// <summary>
        ///     The modifications
        /// </summary>
        public IList<MetadataModification> Modifications { get; }

        /// <summary>
        ///     Modifies mosaic metadata
        /// </summary>
        /// <param name="deadline"></param>
        /// <param name="mosaicId"></param>
        /// <param name="modifications"></param>
        /// <param name="networkType"></param>
        /// <returns></returns>
        public static ModifyMetadataTransaction CreateForMosaic(Deadline deadline, MosaicId mosaicId,
            List<MetadataModification> modifications, NetworkType networkType)
        {
            return new ModifyMetadataTransaction(networkType,
                EntityVersion.MODIFY_METADATA.GetValue(),
                EntityType.MODIFY_MOSAIC_METADATA,
                deadline,
                0,
                MetadataType.MOSAIC,
                mosaicId.Id,
                null,
                modifications);
        }

        /// <summary>
        ///     Modifies namespace metadata
        /// </summary>
        /// <param name="deadline"></param>
        /// <param name="namespaceId"></param>
        /// <param name="modifications"></param>
        /// <param name="networkType"></param>
        /// <returns></returns>
        public static ModifyMetadataTransaction CreateForNamespace(Deadline deadline, NamespaceId namespaceId,
            List<MetadataModification> modifications, NetworkType networkType)
        {
            return new ModifyMetadataTransaction(networkType,
                EntityVersion.MODIFY_METADATA.GetValue(),
                EntityType.MODIFY_NAMESPACE_METADATA,
                deadline,
                0,
                MetadataType.NAMESPACE,
                namespaceId.Id,
                null,
                modifications);
        }

        /// <summary>
        ///     Modifies address metadata
        /// </summary>
        /// <param name="deadline"></param>
        /// <param name="address"></param>
        /// <param name="modifications"></param>
        /// <param name="networkType"></param>
        /// <returns></returns>
        public static ModifyMetadataTransaction CreateForAddress(Deadline deadline, Address address,
            List<MetadataModification> modifications, NetworkType networkType)
        {
            return new ModifyMetadataTransaction(networkType,
                EntityVersion.MODIFY_METADATA.GetValue(),
                EntityType.MODIFY_ADDRESS_METADATA,
                deadline,
                0,
                MetadataType.ADDRESS,
                null,
                address,
                modifications);
        }

        internal override byte[] GenerateBytes()
        {
            var builder = new FlatBufferBuilder(1);

            // track the size of the whole metadata modification
            var totalSize = 0;

            // load modifications
            var modificationVectors = new Offset<MetadataModificationBuffer>[Modifications.Count];

            for (var i = 0; i < modificationVectors.Length; i++)
            {
                var mod = Modifications[i];
                var modField = mod.Field;
                var modType = mod.Type;

                // prepare intermediate data
                var keyBytes = Encoding.UTF8.GetBytes(modField.Key);
                var valueBytes = modType == MetadataModificationType.REMOVE
                    ? new byte[0]
                    : Encoding.UTF8.GetBytes(modField.Value);

                // 2 bytes
                var valueSizeBytes = BitConverter.GetBytes((short) valueBytes.Length);

                // prepare vectors for collections
                var keyVector = MetadataModificationBuffer.CreateKeyVector(builder, keyBytes);
                var valueVector = MetadataModificationBuffer.CreateValueVector(builder, valueBytes);
                var valueSizeVector = MetadataModificationBuffer.CreateValueSizeVector(builder, valueSizeBytes);

                // compute number of bytes: size + modType + keySize + valueSize + key + value
                var modSize = 4 + 1 + 1 + 2 + keyBytes.Length + valueBytes.Length;

                // increase total size
                totalSize += modSize;

                // populate flat-buffer
                MetadataModificationBuffer.StartMetadataModificationBuffer(builder);
                MetadataModificationBuffer.AddSize(builder, (uint) modSize);
                MetadataModificationBuffer.AddKeySize(builder, (byte) keyBytes.Length);
                MetadataModificationBuffer.AddKey(builder, keyVector);
                MetadataModificationBuffer.AddValueSize(builder, valueSizeVector);
                MetadataModificationBuffer.AddValue(builder, valueVector);
                MetadataModificationBuffer.AddModificationType(builder, modType.GetValueInByte());
                modificationVectors[i] = MetadataModificationBuffer.EndMetadataModificationBuffer(builder);
            }

            // create version
            var version = GetTxVersionSerialization();

            var signatureVector = ModifyMetadataTransactionBuffer.CreateSignatureVector(builder, new byte[64]);
            var signerVector = ModifyMetadataTransactionBuffer.CreateSignerVector(builder, GetSigner());
            var feeVector = ModifyMetadataTransactionBuffer.CreateMaxFeeVector(builder, MaxFee?.ToUInt8Array());
            var deadlineVector =
                ModifyMetadataTransactionBuffer.CreateDeadlineVector(builder, Deadline.Ticks.ToUInt8Array());

            var metadataIdBytes = new byte[0];
            switch (MetadataType)
            {
                case MetadataType.ADDRESS:
                    metadataIdBytes = Address.Plain.FromBase32String();
                    break;
                case MetadataType.MOSAIC:
                case MetadataType.NAMESPACE:
                    metadataIdBytes = MetadataId.HasValue ? BitConverter.GetBytes(MetadataId.Value) : new byte[0];
                    break;
            }


            var metadataIdVector = ModifyMetadataTransactionBuffer.CreateMetadataIdVector(builder, metadataIdBytes);
            var modificationsVector =
                ModifyMetadataTransactionBuffer.CreateModificationsVector(builder, modificationVectors);

            // add size of stuff with constant size and size of metadata id
            var fixedSize = HEADER_SIZE + 1 + metadataIdBytes.Length + totalSize;

            ModifyMetadataTransactionBuffer.StartModifyMetadataTransactionBuffer(builder);
            ModifyMetadataTransactionBuffer.AddSize(builder, (uint) fixedSize);
            ModifyMetadataTransactionBuffer.AddSignature(builder, signatureVector);
            ModifyMetadataTransactionBuffer.AddSigner(builder, signerVector);
            ModifyMetadataTransactionBuffer.AddVersion(builder, (uint)version);
            ModifyMetadataTransactionBuffer.AddType(builder, TransactionType.GetValue());
            ModifyMetadataTransactionBuffer.AddMaxFee(builder, feeVector);
            ModifyMetadataTransactionBuffer.AddDeadline(builder, deadlineVector);
            ModifyMetadataTransactionBuffer.AddMetadataId(builder, metadataIdVector);
            ModifyMetadataTransactionBuffer.AddMetadataType(builder, MetadataType.GetValueInByte());
            ModifyMetadataTransactionBuffer.AddModifications(builder, modificationsVector);

            // end build
            var codedTransfer = ModifyMetadataTransactionBuffer.EndModifyMetadataTransactionBuffer(builder);
            builder.Finish(codedTransfer.Value);

            var output = new ModifyMetadataTransactionSchema().Serialize(builder.SizedByteArray());
            if (output.Length != fixedSize) throw new SerializationException("Serialized form has incorrect length");

            return output;
        }

        protected override int GetPayloadSerializedSize()
        {
            throw new NotImplementedException();
        }
    }
}