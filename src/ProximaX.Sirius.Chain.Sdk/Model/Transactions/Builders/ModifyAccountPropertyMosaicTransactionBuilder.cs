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
    public class ModifyAccountPropertyMosaicTransactionBuilder : ModifyAccountPropertyTransactionBuilder<IUInt64Id>
    {
        public ModifyAccountPropertyMosaicTransactionBuilder() : base(EntityType.MODIFY_ACCOUNT_PROPERTY_MOSAIC, EntityVersion.MODIFY_ACCOUNT_PROPERTY_MOSAIC.GetValue())
        {
        }

        public override ModifyAccountPropertyTransaction<IUInt64Id> Build()
        {
            // use or calculate maxFee
            var maxFee = MaxFee ?? GetMaxFeeCalculation(MosaicModification.CalculatePayloadSize(Modifications.Count));

            // create transaction instance
            return new MosaicModification(NetworkType, Version, Deadline, PropertyType, Modifications, maxFee);
        }
    }
}
