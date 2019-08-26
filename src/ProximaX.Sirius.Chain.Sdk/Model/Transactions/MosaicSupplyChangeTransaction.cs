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
    public class MosaicSupplyChangeTransaction : Transaction
    {
        /// <summary>
        /// </summary>
        /// <param name="networkType"></param>
        /// <param name="version"></param>
        /// <param name="deadline"></param>
        /// <param name="maxFee"></param>
        /// <param name="mosaicId"></param>
        /// <param name="mosaicSupplyType"></param>
        /// <param name="delta"></param>
        /// <param name="signature"></param>
        /// <param name="signer"></param>
        /// <param name="transactionInfo"></param>
        public MosaicSupplyChangeTransaction(NetworkType networkType, int version, Deadline deadline, ulong maxFee,
            MosaicId mosaicId, MosaicSupplyType mosaicSupplyType, ulong delta, string signature = null,
            PublicAccount signer = null, TransactionInfo transactionInfo = null)
            : base(networkType, version, TransactionType.MOSAIC_SUPPLY_CHANGE, deadline, maxFee, signature, signer,
                transactionInfo)
        {
            MosaicId = mosaicId;
            Delta = delta;
            MosaicSupplyType = mosaicSupplyType;
        }

        /// <summary>
        ///     The delta. ie. The quantity by which the supply should be augmented.
        /// </summary>
        /// <value>The delta.</value>
        public ulong Delta { get; }

        /// <summary>
        ///     The mosaic id.
        /// </summary>
        /// <value>The mosaic identifier.</value>
        public MosaicId MosaicId { get; }

        /// <summary>
        ///     The direction. ie. If the supply should be increase or decreased by the given delta.
        /// </summary>
        /// <value>The direction.</value>
        public MosaicSupplyType MosaicSupplyType { get; }

        /// <summary>
        /// </summary>
        /// <param name="deadline"></param>
        /// <param name="mosaicId"></param>
        /// <param name="mosaicSupplyType"></param>
        /// <param name="delta"></param>
        /// <param name="networkType"></param>
        /// <returns></returns>
        public static MosaicSupplyChangeTransaction Create(Deadline deadline, MosaicId mosaicId,
            MosaicSupplyType mosaicSupplyType, ulong delta, NetworkType networkType)
        {
            return new MosaicSupplyChangeTransaction(networkType, TransactionVersion.MOSAIC_SUPPLY_CHANGE.GetValue(),
                deadline, 0, mosaicId, mosaicSupplyType, delta);
        }

        internal override byte[] GenerateBytes()
        {
            var builder = new FlatBufferBuilder(1);
            var signatureVector = MosaicSupplyChangeTransactionBuffer.CreateSignatureVector(builder, new byte[64]);
            var signerVector = MosaicSupplyChangeTransactionBuffer.CreateSignerVector(builder, GetSigner());
            var feeVector = MosaicSupplyChangeTransactionBuffer.CreateMaxFeeVector(builder, MaxFee?.ToUInt8Array());
            var deadlineVector =
                MosaicSupplyChangeTransactionBuffer.CreateDeadlineVector(builder, Deadline.Ticks.ToUInt8Array());
            var mosaicIdVector =
                MosaicSupplyChangeTransactionBuffer.CreateMosaicIdVector(builder, MosaicId.Id.ToUInt8Array());
            var deltaVector = MosaicSupplyChangeTransactionBuffer.CreateDeltaVector(builder, Delta.ToUInt8Array());

            var version = int.Parse(NetworkType.GetValueInByte().ToString("X") + "0" + Version.ToString("X"),
                NumberStyles.HexNumber);


            int fixedSize = HEADER_SIZE
                + 8 //mosaic id, 
                + 1 //supply type
                + 8; //delta

            MosaicSupplyChangeTransactionBuffer.StartMosaicSupplyChangeTransactionBuffer(builder);
            MosaicSupplyChangeTransactionBuffer.AddSize(builder, (uint)fixedSize);
            MosaicSupplyChangeTransactionBuffer.AddSignature(builder, signatureVector);
            MosaicSupplyChangeTransactionBuffer.AddSigner(builder, signerVector);
            MosaicSupplyChangeTransactionBuffer.AddVersion(builder, (uint)version);
            MosaicSupplyChangeTransactionBuffer.AddType(builder, TransactionType.GetValue());
            MosaicSupplyChangeTransactionBuffer.AddMaxFee(builder, feeVector);
            MosaicSupplyChangeTransactionBuffer.AddDeadline(builder, deadlineVector);
            MosaicSupplyChangeTransactionBuffer.AddMosaicId(builder, mosaicIdVector);
            MosaicSupplyChangeTransactionBuffer.AddDirection(builder, MosaicSupplyType.GetValueInByte());
            MosaicSupplyChangeTransactionBuffer.AddDelta(builder, deltaVector);

            var codedTransaction = MosaicSupplyChangeTransactionBuffer.EndMosaicSupplyChangeTransactionBuffer(builder);
            builder.Finish(codedTransaction.Value);

            return new MosaicSupplyChangeTransactionSchema().Serialize(builder.SizedByteArray());
        }
    }
}