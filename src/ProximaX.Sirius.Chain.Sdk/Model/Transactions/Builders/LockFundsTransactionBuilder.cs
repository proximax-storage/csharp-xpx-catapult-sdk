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


        public LockFundsTransactionBuilder() : base(EntityType.HASHLOCK, EntityVersion.HASHLOCK.GetValue())
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
