using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions.Builders
{
    public class MosaicDefinitionTransactionBuilder : TransactionBuilder<MosaicDefinitionTransactionBuilder, MosaicDefinitionTransaction>
    {
        public MosaicNonce Nonce { get; private set; }
        public MosaicId MosaicId { get; private set; }
        public MosaicProperties MosaicProperties { get; private set; }

        public MosaicDefinitionTransactionBuilder() :
            base(EntityType.MOSAIC_DEFINITION, EntityVersion.MOSAIC_DEFINITION.GetValue())
        {
        }

        public override MosaicDefinitionTransaction Build()
        {
            var maxFee = MaxFee ?? GetMaxFeeCalculation(MosaicDefinitionTransaction.CalculatePayloadSize(MosaicProperties.Duration > 0 ? 1: 0));

            return new MosaicDefinitionTransaction(NetworkType, Version, Deadline, maxFee,Nonce, MosaicId, MosaicProperties);
        }

        protected override MosaicDefinitionTransactionBuilder Self()
        {
            return this;
        }

        public MosaicDefinitionTransactionBuilder SetNonce(MosaicNonce nonce)
        {
            Nonce = nonce;
            return Self();
        }

        public MosaicDefinitionTransactionBuilder SetMosaicId(MosaicId mosaicId)
        {
            MosaicId = mosaicId;
            return Self();
        }

        public MosaicDefinitionTransactionBuilder SetMosaicProperties(MosaicProperties mosaicProperties)
        {
            MosaicProperties = mosaicProperties;
            return Self();
        }
    }
}
