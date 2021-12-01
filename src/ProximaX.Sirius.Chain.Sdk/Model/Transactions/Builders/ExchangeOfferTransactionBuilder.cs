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
using ProximaX.Sirius.Chain.Sdk.Model.Exchange;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions.Builders
{
    public class ExchangeOfferTransactionBuilder : TransactionBuilder<ExchangeOfferTransactionBuilder, ExchangeOfferTransaction>
    {
        public List<ExchangeOffer> Offers { get; private set; }

        public ExchangeOfferTransactionBuilder() : base(EntityType.EXCHANGE_OFFER, EntityVersion.EXCHANGE_OFFER.GetValue())
        {
            // defaults
            Offers = new List<ExchangeOffer>();
        }

        public override ExchangeOfferTransaction Build()
        {
            var maxFee = MaxFee ?? GetMaxFeeCalculation(ExchangeOfferTransaction.CalculatePayloadSize(Offers.Count));

            return new ExchangeOfferTransaction(NetworkType, Version, Deadline, maxFee, Offers);
        }

        protected override ExchangeOfferTransactionBuilder Self()
        {
            return this;
        }

        public ExchangeOfferTransactionBuilder SetOffers(List<ExchangeOffer> offers)
        {
            Offers = offers;
            return this;
        }
    }
}