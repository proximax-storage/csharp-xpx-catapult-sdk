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

using System.Text;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions.Builders
{
    public class BlockchainConfigTransactionBuilder : TransactionBuilder<BlockchainConfigTransactionBuilder, BlockchainConfigTransaction>
    {

        public ulong ApplyHeightDelta { get; private set; }
        public string BlockchainConfig { get; private set; }
        public string SupportedEntityVersions { get; private set; }

        public BlockchainConfigTransactionBuilder() : base(EntityType.BLOCKCHAIN_CONFIG, EntityVersion.BLOCKCHAIN_CONFIG.GetValue())
        {
        }

        public override BlockchainConfigTransaction Build()
        {
         
            var configBytes = Encoding.UTF8.GetBytes(BlockchainConfig);
            var entityBytes = Encoding.UTF8.GetBytes(SupportedEntityVersions);

            // calculate max fee
            var maxFee = MaxFee ?? GetMaxFeeCalculation(BlockchainConfigTransaction.CalculatePayloadSize(configBytes.Length, entityBytes.Length));

            // create transaction instance
            return new BlockchainConfigTransaction(NetworkType, Version, EntityType, Deadline,
                maxFee, ApplyHeightDelta, BlockchainConfig, SupportedEntityVersions);
 
        }

        protected override BlockchainConfigTransactionBuilder Self()
        {
            return this;
        }

        public BlockchainConfigTransactionBuilder SetApplyHeightDelta(ulong applyHeightDelta)
        {
            ApplyHeightDelta = applyHeightDelta;
            return Self();
        }

        public BlockchainConfigTransactionBuilder SetBlockchainConfig(string blockchainConfig)
        {
            BlockchainConfig = blockchainConfig;
            return Self();
        }

        public BlockchainConfigTransactionBuilder SetSupportedEntityVersions(string supportedEntityVersions)
        {
            SupportedEntityVersions = supportedEntityVersions;
            return Self();
        }

    }
}
