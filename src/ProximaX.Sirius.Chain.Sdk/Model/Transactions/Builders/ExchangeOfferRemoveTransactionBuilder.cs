using ProximaX.Sirius.Chain.Sdk.Model.Exchange;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions.Builders
{
    public class ExchangeOfferRemoveTransactionBuilder : TransactionBuilder<ExchangeOfferRemoveTransactionBuilder, ExchangeOfferRemoveTransaction>
    {
        public List<RemoveExchangeOffer> Offers { get; private set; }
        public ExchangeOfferRemoveTransactionBuilder() :base(EntityType.EXCHANGE_OFFER_REMOVE, EntityVersion.EXCHANGE_OFFER_REMOVE.GetValue())
        {

            // defaults
            Offers = new List<RemoveExchangeOffer>();

        }

        public override ExchangeOfferRemoveTransaction Build()
        {
            var maxFee = MaxFee ?? GetMaxFeeCalculation(ExchangeOfferRemoveTransaction.CalculatePayloadSize(Offers.Count));

            return new ExchangeOfferRemoveTransaction(NetworkType, Version, Deadline, maxFee, Offers);
        }

        protected override ExchangeOfferRemoveTransactionBuilder Self()
        {
            return this;
        }

        public ExchangeOfferRemoveTransactionBuilder SetOffers(List<RemoveExchangeOffer> offers)
        {
            Offers = offers;
            return this;
        }


    }
}
