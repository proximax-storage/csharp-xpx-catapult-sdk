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
    public class BlockchainUpgradeTransaction : Transaction
    {


        public BlockchainUpgradeTransaction(NetworkType networkType, int version, EntityType transactionType,
           Deadline deadline, ulong? maxFee,
           ulong upgradePeriod, BlockchainVersion newVersion,
           string signature = null, PublicAccount signer = null, TransactionInfo transactionInfo = null)
           : base(networkType, version, transactionType, deadline, maxFee, signature, signer, transactionInfo)
        {
            UpgradePeriod = upgradePeriod;
            NewVersion = newVersion;
        
           
        }

        public static int CalculatePayloadSize()
        {
            // period offset + version
            return 8 + 8;

        }

        public ulong UpgradePeriod { get; }
        public BlockchainVersion NewVersion { get; }
     

        public static BlockchainUpgradeTransaction Create(Deadline deadline, ulong upgradePeriod, BlockchainVersion newVersion, NetworkType networkType)
        {
            return new BlockchainUpgradeTransaction(networkType,
                EntityVersion.BLOCKCHAIN_CONFIG.GetValue(),
                EntityType.BLOCKCHAIN_CONFIG,
                deadline,
                0,
                upgradePeriod,
                newVersion
                );

        }

        protected override int GetPayloadSerializedSize()
        {
            return CalculatePayloadSize();
        }

        internal override byte[] GenerateBytes()
        {
            var builder = new FlatBufferBuilder(1);

            // create version
            var version = GetTxVersionSerialization();

            var signatureVector = CatapultUpgradeTransactionBuffer.CreateSignatureVector(builder, new byte[64]);
            var signerVector = CatapultUpgradeTransactionBuffer.CreateSignerVector(builder, GetSigner());
            var feeVector = CatapultUpgradeTransactionBuffer.CreateMaxFeeVector(builder, MaxFee?.ToUInt8Array());
            var deadlineVector =
                CatapultUpgradeTransactionBuffer.CreateDeadlineVector(builder, Deadline.Ticks.ToUInt8Array());

        
            var upgradePeriodVector = CatapultUpgradeTransactionBuffer.CreateUpgradePeriodVector(builder,UpgradePeriod.ToUInt8Array());
            var newCatapultVersionVector = CatapultUpgradeTransactionBuffer.CreateNewCatapultVersionVector(builder, NewVersion.GetVersionValue().ToUInt8Array());

            // header, 2 uint64
            var totalSize = GetPayloadSerializedSize();

            CatapultUpgradeTransactionBuffer.StartCatapultUpgradeTransactionBuffer(builder);
            CatapultUpgradeTransactionBuffer.AddSize(builder, (uint)totalSize);
            CatapultUpgradeTransactionBuffer.AddSignature(builder, signatureVector);
            CatapultUpgradeTransactionBuffer.AddSigner(builder, signerVector);
            CatapultUpgradeTransactionBuffer.AddVersion(builder, (uint)version);
            CatapultUpgradeTransactionBuffer.AddType(builder, TransactionType.GetValue());
            CatapultUpgradeTransactionBuffer.AddMaxFee(builder, feeVector);
            CatapultUpgradeTransactionBuffer.AddDeadline(builder, deadlineVector);
            CatapultUpgradeTransactionBuffer.AddNewCatapultVersion(builder, newCatapultVersionVector);
            CatapultUpgradeTransactionBuffer.AddUpgradePeriod(builder, upgradePeriodVector);

            // end build
            var codedTransfer = CatapultUpgradeTransactionBuffer.EndCatapultUpgradeTransactionBuffer(builder);
            builder.Finish(codedTransfer.Value);

            var output = new BlockchainUpgradeTransactionSchema().Serialize(builder.SizedByteArray());
            if (output.Length != totalSize) throw new SerializationException("Serialized form has incorrect length");

            return output;

        }
    }
}
