// Copyright 2021 ProximaX
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions.Builders
{
    public class RemoveMosaicLevyTransactionBuilder : TransactionBuilder<RemoveMosaicLevyTransactionBuilder, RemoveMosaicLevyTransaction>
    {
        public MosaicId MosaicId { get; private set; }

        public RemoveMosaicLevyTransactionBuilder() : base(EntityType.REMOVE_MOSAIC_LEVY, EntityVersion.REMOVE_MOSAIC_LEVY_VERSION.GetValue())
        {
        }

        public override RemoveMosaicLevyTransaction Build()
        {
            var maxFee = MaxFee ?? GetMaxFeeCalculation(RemoveMosaicLevyTransaction.CalculatePayloadSize());

            return new RemoveMosaicLevyTransaction(NetworkType, Version, Deadline, MosaicId, maxFee);
        }

        protected override RemoveMosaicLevyTransactionBuilder Self()
        {
            return this;
        }

        public RemoveMosaicLevyTransactionBuilder SetMosaicId(MosaicId mosaicId)
        {
            MosaicId = mosaicId;
            return Self();
        }
    }
}