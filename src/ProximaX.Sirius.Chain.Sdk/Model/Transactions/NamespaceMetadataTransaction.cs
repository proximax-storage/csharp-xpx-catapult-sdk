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
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using FlatBuffers;
using ProximaX.Sirius.Chain.Sdk.Buffers;
using ProximaX.Sirius.Chain.Sdk.Buffers.Schema;
using ProximaX.Sirius.Chain.Sdk.Crypto.Core;
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
    public class NamespaceMetadataTransaction : Transaction
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
        public NamespaceMetadataTransaction(NetworkType networkType, int version, EntityType transactionType, Deadline deadline, ulong? maxFee, ulong scopedKey, PublicAccount targetPublicKey, NamespaceId targetId, string newValue, short valueSizeDelta, ushort valueSize,
           string signature = null, PublicAccount signer = null, TransactionInfo transactionInfo = null)
            : base(networkType, version, transactionType, deadline, maxFee, signature, signer, transactionInfo)
        {
            ScopedKey = scopedKey;
            TargetPublicKey = targetPublicKey;
            TargetId = targetId;
            Value = newValue;
            ValueSizeDelta = valueSizeDelta;
            ValueSize = valueSize;
            // oldValue = oldValue;
        }

        /// <summary>
        ///     The Scoped Key
        /// </summary>
        public ulong ScopedKey { get; }

        /// <summary>
        ///     The Target Public Key
        /// </summary>
        public PublicAccount TargetPublicKey { get; }

        /// <summary>
        ///     The target id
        /// </summary>
        public NamespaceId TargetId { get; }

        /// <summary>
        ///     The new value
        /// </summary>
        public string Value { get; }

        /// <summary>
        ///     The Value Size Delta
        /// </summary>
        public short ValueSizeDelta { get; }

        /// <summary>
        ///     The value size
        /// </summary>
        public ushort ValueSize { get; }

        private static byte[] valueDifference_Bytes;

        /// <summary>
        ///     Modifies namespace metadata
        /// </summary>
        /// <param name="deadline"></param>
        /// <param name="namespaceId"></param>
        /// <param name="modifications"></param>
        /// <param name="networkType"></param>
        /// <returns></returns>
        public static NamespaceMetadataTransaction Create(Deadline deadline, PublicAccount account, NamespaceId targetNamespaceId, string scopedKey, string newValue, string oldValue, NetworkType networkType, ulong? maxFee = 0)
        {
            byte[] newV = Encoding.UTF8.GetBytes(newValue);
            byte[] oldV = Encoding.UTF8.GetBytes(oldValue);

            byte[] scope_key_byte = Encoding.UTF8.GetBytes(scopedKey);
            var scopekey = CryptoUtils.Sha3_256_To_Ulong(scope_key_byte);

            var deltasize = (newV.Length - oldV.Length);
            var ValueSizeDelta = Convert.ToInt16(deltasize);
            var max_Value_Size_Bytes = Math.Max(newV.Length, oldV.Length);

            valueDifference_Bytes = Converter.CompareValues(newV, oldV);
            return new NamespaceMetadataTransaction(
                 networkType,
                 EntityVersion.METADATA_NAMESPACE.GetValue(),
                 EntityType.NAMESPACE_METADATA_V2,
                 deadline,
                 maxFee,
                 scopekey,
                 account,
                 targetNamespaceId,
                 newValue,
                 ValueSizeDelta,
                 (ushort)max_Value_Size_Bytes
                 );
        }

        protected override int GetPayloadSerializedSize()
        {
            int byte_Adjust;
            if (ValueSizeDelta == 0)
            {
                byte_Adjust = ValueSize - 1;
            }
            else
            {
                byte_Adjust = ValueSize;
            }
            return CalculatePayloadSize(byte_Adjust);
        }

        public static int CalculatePayloadSize(int value_size_different)
        {
            //MetadataTypeSize
            return
             8  // scopedMetadataKey
             + 32 // targetPublicKey - pk
             + 8 // NamespaceId
             + 2 // valueDeltaSize
             + 2 // value size
             + value_size_different;
        }

        internal override byte[] GenerateBytes()
        {
            var builder = new FlatBufferBuilder(1);

            // create version
            var version = GetTxVersionSerialization();
            var signatureVector = MetadataTransactionBuffer.CreateSignatureVector(builder, new byte[64]);
            var signerVector = MetadataTransactionBuffer.CreateSignerVector(builder, GetSigner());
            var feeVector = MetadataTransactionBuffer.CreateMaxFeeVector(builder, MaxFee?.ToUInt8Array());
            var deadlineVector = MetadataTransactionBuffer.CreateDeadlineVector(builder, Deadline.Ticks.ToUInt8Array());
            var TargetIdVector = MetadataTransactionBuffer.CreateTargetIdVector(builder, TargetId.Id.ToUInt8Array());
            var targetKeyVector = MetadataTransactionBuffer.CreateTargetKeyVector(builder, TargetPublicKey.PublicKey.DecodeHexString());
            var scopedMetadataKeyVector = MetadataTransactionBuffer.CreateScopedMetadataKeyVector(builder, ScopedKey.ToUInt8Array());
            var valueVector = MetadataTransactionBuffer.CreateValueVector(builder, valueDifference_Bytes);
            // add size of stuff with constant size and size of metadata id
            var totalSize = GetSerializedSize();

            MetadataTransactionBuffer.StartMetadataTransactionBuffer(builder);
            MetadataTransactionBuffer.AddSize(builder, (uint)totalSize);
            MetadataTransactionBuffer.AddSignature(builder, signatureVector);
            MetadataTransactionBuffer.AddSigner(builder, signerVector);
            MetadataTransactionBuffer.AddVersion(builder, (uint)version);
            MetadataTransactionBuffer.AddType(builder, EntityType.NAMESPACE_METADATA_V2.GetValue());
            MetadataTransactionBuffer.AddMaxFee(builder, feeVector);
            MetadataTransactionBuffer.AddDeadline(builder, deadlineVector);

            MetadataTransactionBuffer.AddTargetKey(builder, targetKeyVector);
            MetadataTransactionBuffer.AddScopedMetadataKey(builder, scopedMetadataKeyVector);
            MetadataTransactionBuffer.AddTargetId(builder, TargetIdVector);
            MetadataTransactionBuffer.AddValueSizeDelta(builder, ValueSizeDelta);
            MetadataTransactionBuffer.AddValueSize(builder, ValueSize);
            MetadataTransactionBuffer.AddValue(builder, valueVector);

            // end build
            var codedTransfer = MetadataTransactionBuffer.EndMetadataTransactionBuffer(builder);
            builder.Finish(codedTransfer.Value);

            var output = new MetadataTransactionSchema().Serialize(builder.SizedByteArray());
            if (output.Length != totalSize) throw new SerializationException("Serialized form has incorrect length");

            return output;
        }
    }
}