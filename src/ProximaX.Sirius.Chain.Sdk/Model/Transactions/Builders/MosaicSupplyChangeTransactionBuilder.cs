
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

using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions.Builders
{
    public class MosaicSupplyChangeTransactionBuilder : TransactionBuilder<MosaicSupplyChangeTransactionBuilder, MosaicSupplyChangeTransaction>
    {

        public MosaicId MosaicId { get; private set; }
        public MosaicSupplyType MosaicSupplyType { get; private set; }
        public ulong Delta { get; private set; }

        public MosaicSupplyChangeTransactionBuilder() : base(EntityType.MOSAIC_SUPPLY_CHANGE, EntityVersion.MOSAIC_SUPPLY_CHANGE.GetValue())
        {
        }

        public override MosaicSupplyChangeTransaction Build()
        {
            var maxFee = MaxFee ?? GetMaxFeeCalculation(MosaicSupplyChangeTransaction.CalculatePayloadSize());

            return new MosaicSupplyChangeTransaction(NetworkType, Version, Deadline, maxFee,  MosaicId, MosaicSupplyType, Delta);

        }

        protected override MosaicSupplyChangeTransactionBuilder Self()
        {
            return this;
        }

        public MosaicSupplyChangeTransactionBuilder SetMosaicId(MosaicId mosaicId)
        {
            MosaicId = mosaicId;
            return Self();
        }

        public MosaicSupplyChangeTransactionBuilder SetMosaicSupplyType(MosaicSupplyType mosaicSupplyType)
        {
            MosaicSupplyType = mosaicSupplyType;
            return Self();
        }

        public MosaicSupplyChangeTransactionBuilder SetDelta(ulong delta)
        {
            Delta = delta;
            return Self();
        }

        public MosaicSupplyChangeTransactionBuilder IncreaseSupplyFor(MosaicId mosaicId)
        {
            SetMosaicSupplyType(MosaicSupplyType.INCREASE);
            SetMosaicId(mosaicId);
            return Self();
        }

        public MosaicSupplyChangeTransactionBuilder DecreaseSupplyFor(MosaicId mosaicId)
        {
            SetMosaicSupplyType(MosaicSupplyType.DECREASE);
            SetMosaicId(mosaicId);
            return Self();
        }
    }
}
