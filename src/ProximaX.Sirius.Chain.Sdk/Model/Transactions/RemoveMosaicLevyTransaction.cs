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
    public class RemoveMosaicLevyTransaction : Transaction
    {
        public RemoveMosaicLevyTransaction(NetworkType networkType, int version, Deadline deadline, MosaicId mosaidId, ulong? maxFee, string signature = null, PublicAccount signer = null, TransactionInfo transactionInfo = null) : base(networkType, version, EntityType.REMOVE_MOSAIC_LEVY, deadline, maxFee, signature, signer, transactionInfo)
        {
            MosaicId = mosaidId;
        }

        public MosaicId MosaicId { get; }

        public static RemoveMosaicLevyTransaction Create(Deadline deadline, MosaicId mosaicid, NetworkType networkType)
        {
            return new RemoveMosaicLevyTransaction(networkType, EntityVersion.REMOVE_MOSAIC_LEVY_VERSION.GetValue(), deadline, mosaicid, 0);
        }

        public static int CalculatePayloadSize()
        {
            //MosaicIdSize
            return 8;
        }

        protected override int GetPayloadSerializedSize()
        {
            return CalculatePayloadSize();
        }

        internal override byte[] GenerateBytes()
        {
            var builder = new FlatBufferBuilder(1);
            // create vectors
            var signatureVector = RemoveMosaicLevyTransactionBuffer.CreateSignatureVector(builder, new byte[64]);
            var signerVector = RemoveMosaicLevyTransactionBuffer.CreateSignerVector(builder, GetSigner());
            var feeVector = RemoveMosaicLevyTransactionBuffer.CreateMaxFeeVector(builder, MaxFee?.ToUInt8Array());
            var deadlineVector =
                RemoveMosaicLevyTransactionBuffer.CreateDeadlineVector(builder, Deadline.Ticks.ToUInt8Array());
            var mosaicIdVector =
                RemoveMosaicLevyTransactionBuffer.CreateMosaicIdVector(builder, MosaicId.Id.ToUInt8Array());

            // add size of the transaction
            int totalSize = GetSerializedSize();

            // create version
            var version = GetTxVersionSerialization();

            //  add vectors to buffer
            RemoveMosaicLevyTransactionBuffer.StartRemoveMosaicLevyTransactionBuffer(builder);
            RemoveMosaicLevyTransactionBuffer.AddSize(builder, (uint)totalSize);
            RemoveMosaicLevyTransactionBuffer.AddSignature(builder, signatureVector);
            RemoveMosaicLevyTransactionBuffer.AddSigner(builder, signerVector);
            RemoveMosaicLevyTransactionBuffer.AddVersion(builder, (uint)version);
            RemoveMosaicLevyTransactionBuffer.AddType(builder, TransactionType.GetValue());
            RemoveMosaicLevyTransactionBuffer.AddMaxFee(builder, feeVector);
            RemoveMosaicLevyTransactionBuffer.AddDeadline(builder, deadlineVector);

            RemoveMosaicLevyTransactionBuffer.AddMosaicId(builder, mosaicIdVector);

            // Calculate size
            var codedTransaction = RemoveMosaicLevyTransactionBuffer.EndRemoveMosaicLevyTransactionBuffer(builder);
            builder.Finish(codedTransaction.Value);

            var output = new MosaicLevyRemoveTransactionSchema().Serialize(builder.SizedByteArray());

            if (output.Length != totalSize) throw new SerializationException("Serialized form has incorrect length");

            return output;
        }
    }
}