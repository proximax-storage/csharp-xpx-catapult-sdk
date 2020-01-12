using ProximaX.Sirius.Chain.Sdk.Model.Exchange;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions.Builders
{
    public class ExchangeOfferTransactionBuilder : TransactionBuilder<ExchangeOfferTransactionBuilder, ExchangeOfferTransaction>
    {
        public List<ExchangeOffer> Offers { get; private set; }
        public ExchangeOfferTransactionBuilder() :base(EntityType.EXCHANGE_OFFER, EntityVersion.EXCHANGE_OFFER.GetValue())
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
