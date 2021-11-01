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
    public class AccountMetadataTransaction : Transaction
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
        public AccountMetadataTransaction(NetworkType networkType, int version, EntityType transactionType, Deadline deadline, ulong? maxFee, ulong scopedKey, PublicAccount targetPublicKey,
           string newValue, short valueSizeDelta, ushort valueSize,
           string signature = null, PublicAccount signer = null, TransactionInfo transactionInfo = null)
            : base(networkType, version, transactionType, deadline, maxFee, signature, signer, transactionInfo)
        {
            ScopedKey = scopedKey;
            TargetPublicKey = targetPublicKey;
            Value = newValue;
            ValueSizeDelta = valueSizeDelta;
            ValueSize = valueSize;
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
        ///     Account Metadata
        /// </summary>
        /// <param name="deadline"></param>
        /// <param name="account"></param>
        /// <param name="scopedKey"></param>
        /// <param name="newValue"></param>
        /// <param name="oldValue"></param>
        /// <param name="networkType"></param>
        /// <returns></returns>
        public static AccountMetadataTransaction Create(Deadline deadline, PublicAccount account, string scopedKey, string newValue, string oldValue, NetworkType networkType, ulong? maxFee = 0)
        {
            byte[] newV = Encoding.UTF8.GetBytes(newValue);
            byte[] oldV = Encoding.UTF8.GetBytes(oldValue);
            byte[] scope_key = Encoding.UTF8.GetBytes(scopedKey);
            var scopekey = CryptoUtils.Sha3_256_To_Ulong(scope_key);
            var ValueSizeDelta = Convert.ToInt16(newV.Length - oldV.Length);
            var newValueSize = Math.Max(newV.Length, oldV.Length);

            valueDifference_Bytes = Converter.CompareValues(newV, oldV);

            return new AccountMetadataTransaction(
                networkType,
                EntityVersion.METADATA_ACCOUNT.GetValue(),
                EntityType.ACCOUNT_METADATA_V2,
                deadline,
                maxFee,
                scopekey,
                account,
                newValue,
                ValueSizeDelta,
                (ushort)newValueSize
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
            return
             8  // scopedMetadataKey - uint64
             + 32 // targetPublicKey - publicKey
             + 2 // valueDeltaSize - int16
             + 2// value size - uint16;
             + value_size_different;
        }

        internal override byte[] GenerateBytes()
        {
            var builder = new FlatBufferBuilder(1);

            // create version
            var version = GetTxVersionSerialization();
            var signatureVector = MetadataAccountTransactionBuffer.CreateSignatureVector(builder, new byte[64]);
            var signerVector = MetadataAccountTransactionBuffer.CreateSignerVector(builder, GetSigner());
            var feeVector = MetadataAccountTransactionBuffer.CreateMaxFeeVector(builder, MaxFee?.ToUInt8Array());
            var deadlineVector = MetadataAccountTransactionBuffer.CreateDeadlineVector(builder, Deadline.Ticks.ToUInt8Array());

            var targetKeyVector = MetadataAccountTransactionBuffer.CreateTargetKeyVector(builder, TargetPublicKey.PublicKey.DecodeHexString());
            var scopedMetadataKeyVector = MetadataAccountTransactionBuffer.CreateScopedMetadataKeyVector(builder, ScopedKey.ToUInt8Array());

            var valueVector = MetadataAccountTransactionBuffer.CreateValueVector(builder, valueDifference_Bytes);
            // add size of stuff with constant size and size of metadata id
            // var fixedSize = HEADER_SIZE + 1 + metadataIdBytes.Length + totalSize;
            var totalSize = GetSerializedSize();

            MetadataAccountTransactionBuffer.StartMetadataAccountTransactionBuffer(builder);
            MetadataAccountTransactionBuffer.AddSize(builder, (uint)totalSize);
            MetadataAccountTransactionBuffer.AddSignature(builder, signatureVector);
            MetadataAccountTransactionBuffer.AddSigner(builder, signerVector);
            MetadataAccountTransactionBuffer.AddVersion(builder, (uint)version);
            MetadataAccountTransactionBuffer.AddType(builder, TransactionType.GetValue());
            MetadataAccountTransactionBuffer.AddMaxFee(builder, feeVector);
            MetadataAccountTransactionBuffer.AddDeadline(builder, deadlineVector);

            MetadataAccountTransactionBuffer.AddTargetKey(builder, targetKeyVector);
            MetadataAccountTransactionBuffer.AddScopedMetadataKey(builder, scopedMetadataKeyVector);
            MetadataAccountTransactionBuffer.AddValueSizeDelta(builder, ValueSizeDelta);
            MetadataAccountTransactionBuffer.AddValueSize(builder, ValueSize);
            MetadataAccountTransactionBuffer.AddValue(builder, valueVector);

            // end build
            var codedTransfer = MetadataAccountTransactionBuffer.EndMetadataAccountTransactionBuffer(builder);
            builder.Finish(codedTransfer.Value);

            var output = new MetadataAccountTransactionSchema().Serialize(builder.SizedByteArray());
            if (output.Length != totalSize) throw new SerializationException("Serialized form has incorrect length");

            return output;
        }
    }
}