using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions.Builders
{
    public class LockFundsTransactionBuilder : TransactionBuilder<LockFundsTransactionBuilder, LockFundsTransaction>
    {

        public Mosaic Mosaic { get; private set; }
   
        public SignedTransaction SignedTransaction { get; private set; }


        public LockFundsTransactionBuilder() : base(EntityType.LOCK, EntityVersion.LOCK.GetValue())
        {
        }

        public override LockFundsTransaction Build()
        {
            var maxFee = MaxFee ?? GetMaxFeeCalculation(LockFundsTransaction.CalculatePayloadSize());

            return new LockFundsTransaction(NetworkType, Version, Deadline, maxFee, Mosaic, Duration, SignedTransaction);
        }

        protected override LockFundsTransactionBuilder Self()
        {
            return this;
        }

        public LockFundsTransactionBuilder SetMosaic(Mosaic mosaic)
        {
            Mosaic = mosaic;
            return Self();
        }

        public LockFundsTransactionBuilder SetSignedTransaction(SignedTransaction transaction)
        {
            SignedTransaction = transaction;
            return Self();
        }
    }
}
