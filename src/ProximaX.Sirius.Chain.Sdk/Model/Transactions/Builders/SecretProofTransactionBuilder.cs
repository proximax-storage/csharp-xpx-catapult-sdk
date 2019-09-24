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
