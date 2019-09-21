using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using System;
using System.Collections.Generic;
using System.Text;

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
