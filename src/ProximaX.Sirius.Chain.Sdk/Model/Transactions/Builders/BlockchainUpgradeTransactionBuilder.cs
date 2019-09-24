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

using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;

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
