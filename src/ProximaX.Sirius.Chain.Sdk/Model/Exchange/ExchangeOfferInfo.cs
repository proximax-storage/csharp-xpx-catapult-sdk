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

using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Model.Exchange
{
    /// <summary>
    ///     Exchange Offer Info
    /// </summary>
    public class ExchangeOfferInfo
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="mosaicId"></param>
        /// <param name="amount"></param>
        /// <param name="initialAmount"></param>
        /// <param name="initialCost"></param>
        /// <param name="deadline"></param>
        /// <param name="price"></param>
        /// <param name="owner"></param>
        /// <param name="type"></param>
        public ExchangeOfferInfo(ulong mosaicId, ulong amount, ulong initialAmount, ulong initialCost, ulong deadline, decimal price, string owner, int type)
        {
            MosaicId = mosaicId;
            Amount = amount;
            InitialAmount = initialAmount;
            InitialCost = initialCost;
            Deadline = deadline;
            Price = price;
            Owner = owner;
            Type = type;
        }

        /// <summary>
        ///     The mosaic id.
        /// </summary>
        /// <value>The identifier.</value>
        public ulong MosaicId { get; private set; }

        /// <summary>
        ///     The amount.
        /// </summary>
        /// <value>The identifier.</value>
        public ulong Amount { get; private set; }

        /// <summary>
        ///     The initial amount.
        /// </summary>
        /// <value>The identifier.</value>
        public ulong InitialAmount { get; private set; }

        /// <summary>
        ///     The initial cost.
        /// </summary>
        /// <value>The identifier.</value>
        public ulong InitialCost { get; private set; }

        /// <summary>
        ///     The deadline.
        /// </summary>
        /// <value>The identifier.</value>
        public ulong Deadline { get; private set; }

        /// <summary>
        ///     The owner.
        /// </summary>
        /// <value>The identifier.</value>
        public string Owner { get; private set; }

        /// <summary>
        ///     The price.
        /// </summary>
        /// <value>The identifier.</value>
        public decimal Price { get; private set; }

        /// <summary>
        ///     The type.
        /// </summary>
        /// <value>The identifier.</value>
        public int Type { get; private set; }

        /// <summary>
        ///     ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{nameof(MosaicId)}: {MosaicId}, {nameof(Amount)}: {Amount}, {nameof(InitialAmount)}: {InitialAmount}, {nameof(InitialCost)}: {InitialCost},{nameof(Deadline)}: {Deadline}, {nameof(Price)}: {Price},{nameof(Owner)}: {Owner}, {nameof(Type)}: {Type}";
        }
    }
}