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
