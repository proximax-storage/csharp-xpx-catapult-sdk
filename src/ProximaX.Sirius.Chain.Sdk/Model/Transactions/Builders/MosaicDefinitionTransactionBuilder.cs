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
