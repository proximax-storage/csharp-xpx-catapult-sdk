using System;
using System.Collections.Generic;
using System.Text;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions.Builders
{
    public class SecretProofTransactionBuilder : TransactionBuilder<SecretProofTransactionBuilder, SecretProofTransaction>
    {
        public HashType HashType;
        public string Secret;
        public string Proof;
        public Recipient Recipient;


        public SecretProofTransactionBuilder() : base(EntityType.SECRET_PROOF, EntityVersion.SECRET_PROOF.GetValue())
        {
        }

        public override SecretProofTransaction Build()
        {
            var maxFee = MaxFee ?? GetMaxFeeCalculation(SecretProofTransaction.CalculatePayloadSize(Recipient,Proof));

            return new SecretProofTransaction(NetworkType, Version, Deadline, maxFee,HashType,Recipient,Secret,Proof);
        }

        protected override SecretProofTransactionBuilder Self()
        {
            return this;
        }

        public SecretProofTransactionBuilder SetHashType(HashType hashType)
        {
            HashType = hashType;
            return Self();
        }

        public SecretProofTransactionBuilder SetSecret(string secret)
        {
            Secret = secret;
            return Self();
        }

        public SecretProofTransactionBuilder SetRecipient(Recipient recipient)
        {
            Recipient = recipient;
            return Self();
        }

        public SecretProofTransactionBuilder SetProof(string proof)
        {
            Proof = proof;
            return Self();
        }


        public SecretProofTransactionBuilder SetSecret(HashType hashType, string secret)
        {
            return SetHashType(hashType).SetSecret(secret);
        }
    }
}
