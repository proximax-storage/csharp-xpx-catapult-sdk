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

using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using FlatBuffers;
using ProximaX.Sirius.Chain.Sdk.Buffers;
using ProximaX.Sirius.Chain.Sdk.Buffers.Schema;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions
{
    /// <summary>
    ///     Class ModifyAccountPropertyTransaction&lt;T&gt;
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ModifyAccountPropertyTransaction<T> : Transaction
    {
        /// <summary>
        ///     The account property modification
        /// </summary>
        protected readonly IList<AccountPropertyModification<T>> Modifications;

        /// <summary>
        ///     The property type
        /// </summary>
        protected readonly PropertyType PropertyType;


        /// <summary>
        ///     Initializes a new instance of the <see cref="ModifyAccountPropertyTransaction&lt;T&gt;" /> class.
        /// </summary>
        /// <param name="networkType"></param>
        /// <param name="version"></param>
        /// <param name="transactionType"></param>
        /// <param name="deadline"></param>
        /// <param name="propertyType"></param>
        /// <param name="propertyModifications"></param>
        /// <param name="maxFee"></param>
        /// <param name="signature"></param>
        /// <param name="signer"></param>
        /// <param name="transactionInfo"></param>
        public ModifyAccountPropertyTransaction(NetworkType networkType, int version,
            TransactionType transactionType,
            Deadline deadline, PropertyType propertyType, IList<AccountPropertyModification<T>> propertyModifications,
            ulong? maxFee, string signature = null, PublicAccount signer = null,
            TransactionInfo transactionInfo = null) :
            base(networkType, version, transactionType, deadline, maxFee, signature, signer, transactionInfo)
        {
            PropertyType = propertyType;
            Modifications = propertyModifications;
        }


        /// <summary>
        ///     CreateForAddress
        /// </summary>
        /// <param name="deadline"></param>
        /// <param name="maxFee"></param>
        /// <param name="propertyType"></param>
        /// <param name="propertyModifications"></param>
        /// <param name="networkType"></param>
        /// <returns></returns>
        public static ModifyAccountPropertyTransaction<Address> CreateForAddress(Deadline deadline, ulong? maxFee,
            PropertyType propertyType,
            List<AccountPropertyModification<Address>> propertyModifications, NetworkType networkType)
        {
            return new AddressModification(networkType,
                TransactionVersion.MODIFY_ACCOUNT_PROPERTY_ADDRESS.GetValue(),
                deadline, propertyType, propertyModifications, maxFee);
        }

        /// <summary>
        ///     CreateForMosaic
        /// </summary>
        /// <param name="deadline"></param>
        /// <param name="maxFee"></param>
        /// <param name="propertyType"></param>
        /// <param name="propertyModifications"></param>
        /// <param name="networkType"></param>
        /// <returns></returns>
        public static ModifyAccountPropertyTransaction<IUInt64Id> CreateForMosaic(Deadline deadline, ulong? maxFee,
            PropertyType propertyType,
            List<AccountPropertyModification<IUInt64Id>> propertyModifications, NetworkType networkType)
        {
            return new MosaicModification(networkType,
                TransactionVersion.MODIFY_ACCOUNT_PROPERTY_MOSAIC.GetValue(),
                deadline, propertyType, propertyModifications, maxFee);
        }

        /// <summary>
        ///     CreateForEntityType
        /// </summary>
        /// <param name="deadline"></param>
        /// <param name="maxFee"></param>
        /// <param name="propertyType"></param>
        /// <param name="propertyModifications"></param>
        /// <param name="networkType"></param>
        /// <returns></returns>
        public static ModifyAccountPropertyTransaction<TransactionType> CreateForEntityType(Deadline deadline,
            ulong? maxFee,
            PropertyType propertyType,
            List<AccountPropertyModification<TransactionType>> propertyModifications, NetworkType networkType)
        {
            return new EntityTypeModification(networkType,
                TransactionVersion.MODIFY_ACCOUNT_PROPERTY_ENTITY_TYPE.GetValue(),
                deadline, propertyType, propertyModifications, maxFee);
        }

        /// <summary>
        ///     GenerateBytes
        /// </summary>
        /// <returns></returns>
        internal override byte[] GenerateBytes()
        {
            var builder = new FlatBufferBuilder(1);

            // track the size of the whole metadata modification
            var totalSize = 0;

            // load modifications
            var modificationVectors = new Offset<PropertyModificationBuffer>[Modifications.Count];

            for (var i = 0; i < modificationVectors.Length; i++)
            {
                var mod = Modifications[i];
                var modType = mod.Type;

                var valueBytes = GetValueBytesFromModification(mod);

                var valueVector = PropertyModificationBuffer.CreateValueVector(builder, valueBytes);

                // compute number of bytes: modType + value bytes
                var modSize = 1 + valueBytes.Length;

                // increase total size
                totalSize += modSize;

                PropertyModificationBuffer.StartPropertyModificationBuffer(builder);
                PropertyModificationBuffer.AddValue(builder, valueVector);
                PropertyModificationBuffer.AddModificationType(builder, modType.GetValueInByte());
                modificationVectors[i] = PropertyModificationBuffer.EndPropertyModificationBuffer(builder);
            }

            // create version
            var version = GetTxVersionSerialization();

            var signatureVector = AccountPropertiesTransactionBuffer.CreateSignatureVector(builder, new byte[64]);
            var signerVector = AccountPropertiesTransactionBuffer.CreateSignerVector(builder, GetSigner());
            var feeVector = AccountPropertiesTransactionBuffer.CreateMaxFeeVector(builder, MaxFee?.ToUInt8Array());
            var deadlineVector =
                AccountPropertiesTransactionBuffer.CreateDeadlineVector(builder, Deadline.Ticks.ToUInt8Array());
            var modificationsVector =
                AccountPropertiesTransactionBuffer.CreateModificationsVector(builder, modificationVectors);

            // add size of the header (120) + size of prop type (1) + size of mod count (1)
            var fixedSize = HEADER_SIZE + +1 + 1 + totalSize;

            AccountPropertiesTransactionBuffer.StartAccountPropertiesTransactionBuffer(builder);
            AccountPropertiesTransactionBuffer.AddSize(builder, (uint) fixedSize);
            AccountPropertiesTransactionBuffer.AddSignature(builder, signatureVector);
            AccountPropertiesTransactionBuffer.AddSigner(builder, signerVector);
            AccountPropertiesTransactionBuffer.AddVersion(builder,(uint)version);
            AccountPropertiesTransactionBuffer.AddType(builder, TransactionType.GetValue());
            AccountPropertiesTransactionBuffer.AddMaxFee(builder, feeVector);
            AccountPropertiesTransactionBuffer.AddDeadline(builder, deadlineVector);

            AccountPropertiesTransactionBuffer.AddPropertyType(builder, PropertyType.GetValueInByte());
            AccountPropertiesTransactionBuffer.AddModificationCount(builder, (byte) Modifications.Count);
            AccountPropertiesTransactionBuffer.AddModifications(builder, modificationsVector);

            // end build
            var codedTransfer = AccountPropertiesTransactionBuffer.EndAccountPropertiesTransactionBuffer(builder);
            builder.Finish(codedTransfer.Value);

            var output = new ModifyAccountPropertyTransactionSchema().Serialize(builder.SizedByteArray());

            if (output.Length != fixedSize) throw new SerializationException("Serialized form has incorrect length");

            return output;
        }

        protected abstract byte[] GetValueBytesFromModification(AccountPropertyModification<T> mod);
    }
}