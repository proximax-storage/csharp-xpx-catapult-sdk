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
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;

namespace ProximaX.Sirius.Chain.Sdk.Model.Exchange
{
    /// <summary>
    /// Class of AddExchangeOffer
    /// </summary>
    public class AddExchangeOffer
    {
        /// <summary>
        /// Get and set MosaicId
        /// </summary>
        public IUInt64Id MosaicId { get; private set; }

        /// <summary>
        /// Get and set MosaicAmount
        /// </summary>
        public ulong MosaicAmount { get; private set; }

        /// <summary>
        /// Get and set Cost
        /// </summary>
        public ulong Cost { get; private set; }

        /// <summary>
        /// Get and set Type
        /// </summary>
        public ExchangeOfferType Type { get; private set; }

        /// <summary>
        /// Get and set Duration
        /// </summary>
        public ulong Duration { get; private set; }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="mosaicId"></param>
        /// <param name="mosaicAmount"></param>
        /// <param name="cost"></param>
        /// <param name="type"></param>
        /// <param name="duration"></param>
        public AddExchangeOffer(IUInt64Id mosaicId, ulong mosaicAmount, ulong cost, ExchangeOfferType type, ulong duration)
        {
            MosaicId = mosaicId;
            MosaicAmount = mosaicAmount;
            Cost = cost;
            Type = type;
            Duration = duration;
        }

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="mosaic"></param>
        /// <param name="cost"></param>
        /// <param name="type"></param>
        /// <param name="duration"></param>
        public AddExchangeOffer(Mosaic mosaic, ulong cost, ExchangeOfferType type, ulong duration)
        {
            MosaicId = mosaic.Id;
            MosaicAmount = mosaic.Amount;
            Cost = cost;
            Type = type;
            Duration = duration;
        }
    }
}