using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions.Builders
{
    internal class ModifyMosaicLevyTransactionBuilder : TransactionBuilder<ModifyMosaicLevyTransactionBuilder, ModifyMosaicLevyTransaction>
    {
        public MosaicId MosaicId { get; private set; }
        public MosaicLevyInfo MosaicLevy { get; private set; }

        public ModifyMosaicLevyTransactionBuilder() : base(EntityType.MODIFY_MOSAIC_LEVY, EntityVersion.MODIFY_MOSAIC_LEVY_VERSION.GetValue())
        {
        }

        public override ModifyMosaicLevyTransaction Build()
        {
            var maxFee = MaxFee ?? GetMaxFeeCalculation(ModifyMosaicLevyTransaction.CalculatePayloadSize());

            return new ModifyMosaicLevyTransaction(NetworkType, Version, Deadline, MosaicId, MosaicLevy, maxFee);
        }

        protected override ModifyMosaicLevyTransactionBuilder Self()
        {
            return this;
        }

        public ModifyMosaicLevyTransactionBuilder SetMosaicId(MosaicId mosaicId)
        {
            MosaicId = mosaicId;
            return Self();
        }

        public ModifyMosaicLevyTransactionBuilder SetMosaicLevy(MosaicLevyInfo mosaicLevy)
        {
            MosaicLevy = mosaicLevy;
            return Self();
        }
    }
}