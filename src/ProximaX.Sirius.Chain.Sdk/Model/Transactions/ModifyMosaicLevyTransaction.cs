// Copyright 2021 ProximaX
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
using FlatBuffers;
using ProximaX.Sirius.Chain.Sdk.Buffers;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Exchange;
using System;
using System.Collections.Generic;
using System.Text;
using ProximaX.Sirius.Chain.Sdk.Utils;
using ProximaX.Sirius.Chain.Sdk.Buffers.Schema;
using System.Runtime.Serialization;
using System.Linq;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions
{
    /// <summary>
    ///     Class ModifyMosaicLevyTransaction
    /// </summary>
    public class ModifyMosaicLevyTransaction : Transaction
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="networkType"></param>
        /// <param name="version"></param>
        /// <param name="deadline"></param>
        /// <param name="mosaidId"></param>
        /// <param name="levy"></param>
        /// <param name="maxFee"></param>
        /// <param name="signature"></param>
        /// <param name="signer"></param>
        /// <param name="transactionInfo"></param>
        public ModifyMosaicLevyTransaction(NetworkType networkType, int version, Deadline deadline, MosaicId mosaidId, Mosaics.MosaicLevy levy, ulong? maxFee, string signature = null, PublicAccount signer = null, TransactionInfo transactionInfo = null) : base(networkType, version, EntityType.MODIFY_MOSAIC_LEVY, deadline, maxFee, signature, signer, transactionInfo)
        {
            LevyInfo = levy;
            MosaicId = mosaidId;
        }

        /// <summary>
        ///     The LevyInfo
        /// </summary>
        public Mosaics.MosaicLevy LevyInfo { get; }

        /// <summary>
        ///     The MosaicId
        /// </summary>
        public MosaicId MosaicId { get; }

        /// <summary>
        ///     Modifies mosaic levy
        /// </summary>
        /// <param name="deadline"></param>
        /// <param name="mosaicid"></param>
        /// <param name="LevyInfo"></param>
        /// <param name="networkType"></param>
        /// <returns></returns>
        public static ModifyMosaicLevyTransaction Create(Deadline deadline, MosaicId mosaicid, Mosaics.MosaicLevy LevyInfo, NetworkType networkType)
        {
            return new ModifyMosaicLevyTransaction(networkType, EntityVersion.MODIFY_MOSAIC_LEVY_VERSION.GetValue(), deadline, mosaicid, LevyInfo, 0);
        }

        public static int CalculatePayloadSize()
        {
            //mosaicid + levy type + recipent(levy) + mosaid id(levy) + fee (levy)
            return 8 + 1 + 25 + 8 + 8;
        }

        protected override int GetPayloadSerializedSize()
        {
            return CalculatePayloadSize();
        }

        internal override byte[] GenerateBytes()
        {
            var builder = new FlatBufferBuilder(1);

            // create vectors
            var mosaicidVector = Buffers.MosaicLevy.CreateMosaicIdVector(builder, LevyInfo.Mosaic.Id.ToUInt8Array());
            var levyFeeVector = Buffers.MosaicLevy.CreateFeeVector(builder, LevyInfo.Amount.ToUInt8Array());
            var levyRecipentVector = Buffers.MosaicLevy.CreateRecipientVector(builder, LevyInfo.Recipent.GetBytes());

            Buffers.MosaicLevy.StartMosaicLevy(builder);
            Buffers.MosaicLevy.AddFee(builder, levyFeeVector);
            Buffers.MosaicLevy.AddMosaicId(builder, mosaicidVector);
            Buffers.MosaicLevy.AddRecipient(builder, levyRecipentVector);
            Buffers.MosaicLevy.AddType(builder, LevyInfo.Levytype.GetValueInByte());

            var Mosaic = Buffers.MosaicLevy.EndMosaicLevy(builder);

            // create vectors
            var signatureVector = ModifyMosaicLevyTransactionBuffer.CreateSignatureVector(builder, new byte[64]);
            var signerVector = ModifyMosaicLevyTransactionBuffer.CreateSignerVector(builder, GetSigner());
            var feeVector = ModifyMosaicLevyTransactionBuffer.CreateMaxFeeVector(builder, MaxFee?.ToUInt8Array());
            var deadlineVector =
                ModifyMosaicLevyTransactionBuffer.CreateDeadlineVector(builder, Deadline.Ticks.ToUInt8Array());
            var mosaicIdVector =
                ModifyMosaicLevyTransactionBuffer.CreateMosaicIdVector(builder, MosaicId.Id.ToUInt8Array());

            // add size of the transaction
            int totalSize = GetSerializedSize();

            // create version
            var version = GetTxVersionSerialization();

            //  add vectors to buffer
            ModifyMosaicLevyTransactionBuffer.StartModifyMosaicLevyTransactionBuffer(builder);
            ModifyMosaicLevyTransactionBuffer.AddSize(builder, (uint)totalSize);
            ModifyMosaicLevyTransactionBuffer.AddSignature(builder, signatureVector);
            ModifyMosaicLevyTransactionBuffer.AddSigner(builder, signerVector);
            ModifyMosaicLevyTransactionBuffer.AddVersion(builder, (uint)version);
            ModifyMosaicLevyTransactionBuffer.AddType(builder, TransactionType.GetValue());
            ModifyMosaicLevyTransactionBuffer.AddMaxFee(builder, feeVector);
            ModifyMosaicLevyTransactionBuffer.AddDeadline(builder, deadlineVector);

            ModifyMosaicLevyTransactionBuffer.AddLevy(builder, Mosaic);
            ModifyMosaicLevyTransactionBuffer.AddMosaicId(builder, mosaicIdVector);

            // Calculate size
            var codedTransaction = ModifyMosaicLevyTransactionBuffer.EndModifyMosaicLevyTransactionBuffer(builder);
            builder.Finish(codedTransaction.Value);

            var output = new ModifyMosaicLevyTransactionSchema().Serialize(builder.SizedByteArray());
            if (output.Length != totalSize) throw new SerializationException("Serialized form has incorrect length");

            return output;
        }
    }
}