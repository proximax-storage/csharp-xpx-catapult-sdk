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

using System;
using ProximaX.Sirius.Sdk.Crypto.Core.Chaso.NaCl;
using ProximaX.Sirius.Sdk.Model.Accounts;
using ProximaX.Sirius.Sdk.Model.Transactions;

namespace ProximaX.Sirius.Sdk.Model.Transactions
{
    /// <summary>
    ///     CosignatureTransaction
    /// </summary>
    public class CosignatureTransaction
    {
        public CosignatureTransaction(AggregateTransaction transactionToCosign)
        {
            if (transactionToCosign.IsUnannounced())
                throw new ArgumentException("transaction to cosign should be announced first");

            TransactionToCosign = transactionToCosign;
        }

        public AggregateTransaction TransactionToCosign { get; }


        public static CosignatureTransaction Create(AggregateTransaction transactionToCosign)
        {
            return new CosignatureTransaction(transactionToCosign);
        }

        public AggregateTransaction GetTransactionToCosign()
        {
            return TransactionToCosign;
        }

        public CosignatureSignedTransaction SignWith(Account account)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));
            var bytes = TransactionToCosign.TransactionInfo.Hash.FromHex();
            var signatureBytes = TransactionExtensions.SignHash(account.KeyPair, bytes);

            return new CosignatureSignedTransaction(TransactionToCosign.TransactionInfo.Hash,
                signatureBytes.ToHexLower(), account.PublicKey);
        }
    }
}