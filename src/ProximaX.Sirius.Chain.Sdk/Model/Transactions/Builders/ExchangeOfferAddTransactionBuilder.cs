using ProximaX.Sirius.Chain.Sdk.Model.Exchange;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions.Builders
{
    public class ExchangeOfferAddTransactionBuilder : TransactionBuilder<ExchangeOfferAddTransactionBuilder, ExchangeOfferAddTransaction>
    {
        public List<AddExchangeOffer> Offers { get; private set; }
        public ExchangeOfferAddTransactionBuilder():base(EntityType.EXCHANGE_OFFER_ADD, EntityVersion.EXCHANGE_OFFER_ADD.GetValue())
        {

            // defaults
            Offers = new List<AddExchangeOffer>();

        }

        public override ExchangeOfferAddTransaction Build()
        {
            var maxFee = MaxFee ?? GetMaxFeeCalculation(ExchangeOfferAddTransaction.CalculatePayloadSize(Offers.Count));

            return new ExchangeOfferAddTransaction(NetworkType, Version, Deadline, maxFee, Offers);
        }

        protected override ExchangeOfferAddTransactionBuilder Self()
        {
            return this;
        }

        public ExchangeOfferAddTransactionBuilder SetOffers(List<AddExchangeOffer> offers)
        {
            Offers = offers;
            return this;
        }
    }
}
