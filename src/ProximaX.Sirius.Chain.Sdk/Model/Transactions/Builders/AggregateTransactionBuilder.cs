using System;
using System.Collections.Generic;
using System.Text;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions.Builders
{
    public class AggregateTransactionBuilder: TransactionBuilder<AggregateTransactionBuilder, AggregateTransaction>
    {
        public List<Transaction> InnerTransactions { get; private set; }
        public List<AggregateTransactionCosignature> Cosignatures { get; private set; }

        public AggregateTransactionBuilder(EntityType entityType, int version) : base(entityType, version)
        {
            InnerTransactions = new List<Transaction>();
            Cosignatures = new List<AggregateTransactionCosignature>();
        }

        public static AggregateTransactionBuilder CreateBonded()
        {
            return new AggregateTransactionBuilder(EntityType.AGGREGATE_BONDED,
                  EntityVersion.AGGREGATE_BONDED.GetValue());
        }

        public static AggregateTransactionBuilder CreateComplete()
        {
            return new AggregateTransactionBuilder(EntityType.AGGREGATE_COMPLETE,
                  EntityVersion.AGGREGATE_COMPLETE.GetValue());
        }

        protected override AggregateTransactionBuilder Self()
        {
            return this;
        }

        public override AggregateTransaction Build()
        {
            var maxFee = MaxFee ?? GetMaxFeeCalculation(AggregateTransaction.CalculatePayloadSize(InnerTransactions));

            return new AggregateTransaction(NetworkType, Version, EntityType, Deadline, maxFee, InnerTransactions, Cosignatures);
            
        }

        public AggregateTransactionBuilder SetInnerTransactions(List<Transaction> innerTransactions)
        {
            InnerTransactions = innerTransactions;
            return Self();
        }

        public AggregateTransactionBuilder SetCosignatures(List<AggregateTransactionCosignature> cosignatures)
        {
            Cosignatures = cosignatures;
            return Self();
        }
    }
}
