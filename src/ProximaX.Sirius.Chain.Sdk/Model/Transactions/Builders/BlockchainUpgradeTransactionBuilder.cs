using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions.Builders
{
    public class BlockchainUpgradeTransactionBuilder : TransactionBuilder<BlockchainUpgradeTransactionBuilder, BlockchainUpgradeTransaction>
    {

        public ulong UpgradePeriod { get; private set; }

        public BlockchainVersion NewVersion { get; private set; }

        public BlockchainUpgradeTransactionBuilder() : base(EntityType.BLOCKCHAIN_UPGRADE, EntityVersion.BLOCKCHAIN_UPGRADE.GetValue())
        {
        }

        public override BlockchainUpgradeTransaction Build()
        {
            // calculate max fee
            var maxFee = MaxFee ?? GetMaxFeeCalculation(BlockchainUpgradeTransaction.CalculatePayloadSize());


            // create transaction instance
            return new BlockchainUpgradeTransaction(NetworkType, Version, EntityType, Deadline, maxFee, UpgradePeriod, NewVersion);
        }

        protected override BlockchainUpgradeTransactionBuilder Self()
        {
            return this;
        }

        public BlockchainUpgradeTransactionBuilder SetUpgradePeriod(ulong upgradePeriod)
        {
            UpgradePeriod = upgradePeriod;
            return Self();
        }


        public BlockchainUpgradeTransactionBuilder SetNewVersion(BlockchainVersion newVersion)
        {
            NewVersion = newVersion;
            return Self();
        }
    }
}
