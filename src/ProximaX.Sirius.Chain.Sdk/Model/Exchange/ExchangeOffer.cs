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

using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProximaX.Sirius.Chain.Sdk.Model.Exchange
{
    public class ExchangeOffer
    {
        public IUInt64Id MosaicId { get; private set; }
        public ulong MosaicAmount { get; private set; }
        public ulong Cost { get; private set; }
        public ExchangeOfferType Type { get; private set; }
        public PublicAccount Owner { get; private set; }

        public ExchangeOffer(IUInt64Id mosaicId, ulong mosaicAmount, ulong cost, ExchangeOfferType type,
             PublicAccount owner)
        {
            MosaicId = mosaicId;
            MosaicAmount = mosaicAmount;
            Cost = cost;
            Type = type;
            Owner = owner;
        }

        public ExchangeOffer(Mosaic mosaic, ulong cost, ExchangeOfferType type,  PublicAccount owner)
        {
            MosaicId = mosaic.Id;
            MosaicAmount = mosaic.Amount;
            Cost = cost;
            Type = type;
            Owner = owner;
        }
    }
}
