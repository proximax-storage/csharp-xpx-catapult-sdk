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
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions
{
    public class BlockchainConfigTransaction : Transaction
    {


        public BlockchainConfigTransaction(NetworkType networkType, int version, EntityType transactionType,
           Deadline deadline, ulong? maxFee,
           ulong applyHeightDelta, string blockchainConfig, string supportedEntityVersions,
           string signature = null, PublicAccount signer = null, TransactionInfo transactionInfo = null)
           : base(networkType, version, transactionType, deadline, maxFee, signature, signer, transactionInfo)
        {
            ApplyHeightDelta = applyHeightDelta;
            BlockchainConfig = blockchainConfig;
            SupportedEntityVersions = supportedEntityVersions;
           
        }

        public ulong ApplyHeightDelta { get; }
        public string BlockchainConfig { get; }
        public string SupportedEntityVersions { get; }

        public static BlockchainConfigTransaction Create(Deadline deadline, ulong applyHeightDelta, string blockchainConfig, string supportedEntityVersions, NetworkType networkType)
        {
            return new BlockchainConfigTransaction(networkType,
                EntityVersion.BLOCKCHAIN_CONFIG.GetValue(),
                EntityType.BLOCKCHAIN_CONFIG,
                deadline,
                0,
                applyHeightDelta,
                blockchainConfig,
                supportedEntityVersions);

        }

        internal override byte[] GenerateBytes()
        {
            var builder = new FlatBufferBuilder(1);

            // create version
            var version = GetTxVersionSerialization();

            var signatureVector = CatapultConfigTransactionBuffer.CreateSignatureVector(builder, new byte[64]);
            var signerVector = CatapultConfigTransactionBuffer.CreateSignerVector(builder, GetSigner());
            var feeVector = CatapultConfigTransactionBuffer.CreateMaxFeeVector(builder, MaxFee?.ToUInt8Array());
            var deadlineVector =
                CatapultConfigTransactionBuffer.CreateDeadlineVector(builder, Deadline.Ticks.ToUInt8Array());

            var configBytes = Encoding.UTF8.GetBytes(BlockchainConfig);
            var entityBytes = Encoding.UTF8.GetBytes(SupportedEntityVersions);
            var applyHeightVector = CatapultConfigTransactionBuffer.CreateApplyHeightDeltaVector(builder,ApplyHeightDelta.ToUInt8Array());
            var configVector = CatapultConfigTransactionBuffer.CreateBlockChainConfigVector(builder, configBytes);
            var entityVector = CatapultConfigTransactionBuffer.CreateBlockChainConfigVector(builder, entityBytes);

            // header, 2 uint64 and int
            var fixedSize = HEADER_SIZE + 8 + 2 + 2 + entityBytes.Length + configBytes.Length;

            CatapultConfigTransactionBuffer.StartCatapultConfigTransactionBuffer(builder);
            CatapultConfigTransactionBuffer.AddSize(builder, (uint)fixedSize);
            CatapultConfigTransactionBuffer.AddSignature(builder, signatureVector);
            CatapultConfigTransactionBuffer.AddSigner(builder, signerVector);
            CatapultConfigTransactionBuffer.AddVersion(builder,(uint)version);
            CatapultConfigTransactionBuffer.AddType(builder, TransactionType.GetValue());
            CatapultConfigTransactionBuffer.AddMaxFee(builder, feeVector);
            CatapultConfigTransactionBuffer.AddDeadline(builder, deadlineVector);
            CatapultConfigTransactionBuffer.AddApplyHeightDelta(builder, applyHeightVector);
            CatapultConfigTransactionBuffer.AddBlockChainConfig(builder, configVector);
            CatapultConfigTransactionBuffer.AddBlockChainConfigSize(builder, (ushort)configBytes.Length);
            CatapultConfigTransactionBuffer.AddSupportedEntityVersionsSize(builder, (ushort)entityBytes.Length);
            CatapultConfigTransactionBuffer.AddSupportedEntityVersions(builder, entityVector);

            // end build
            var codedTransfer = CatapultConfigTransactionBuffer.EndCatapultConfigTransactionBuffer(builder);
            builder.Finish(codedTransfer.Value);

            var output = new BlockchainConfigTransactionSchema().Serialize(builder.SizedByteArray());
            if (output.Length != fixedSize) throw new SerializationException("Serialized form has incorrect length");

            return output;

        }
    }
}
