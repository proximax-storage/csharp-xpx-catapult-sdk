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

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions.Builders
{
    public class SecretLockTransactionBuilder : TransactionBuilder<SecretLockTransactionBuilder, SecretLockTransaction>
    {

        public Mosaic Mosaic { get; private set; }

        public HashType HashType { get; private set; }
        public string Secret { get; private set; }
        public Address Recipient { get; private set; }

        public SecretLockTransactionBuilder() : base(EntityType.SECRET_LOCK, EntityVersion.SECRET_LOCK.GetValue())
        {
        }

        public override SecretLockTransaction Build()
        {
            var maxFee = MaxFee ?? GetMaxFeeCalculation(SecretLockTransaction.CalculatePayloadSize());

            return new SecretLockTransaction(NetworkType, Version, Deadline, maxFee, Mosaic, Duration, HashType, Secret, Recipient);
        }

        protected override SecretLockTransactionBuilder Self()
        {
            return this;
        }

        public SecretLockTransactionBuilder SetMosaic(Mosaic mosaic)
        {
            Mosaic = mosaic;
            return Self();
        }


        public SecretLockTransactionBuilder SetHashType(HashType hashType)
        {
            HashType = hashType;
            return Self();
        }


        public SecretLockTransactionBuilder SetSecret(string secret)
        {
            Secret = secret;
            return Self();
        }

        public SecretLockTransactionBuilder SetRecipient(Address recipient)
        {
            Recipient = recipient;
            return Self();
        }

        public SecretLockTransactionBuilder SetSecret(HashType hashType, string secret)
        {
            return SetHashType(hashType).SetSecret(secret);
        }
    }
}
