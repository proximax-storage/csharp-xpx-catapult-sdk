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
using Newtonsoft.Json.Linq;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.Mapping
{
    /// <summary>
    ///     TransactionMapping
    /// </summary>
    public class TransactionMapping
    {
        /// <summary>
        ///     Apply mapping for transaction
        /// </summary>
        /// <param name="input">The return transaction</param>
        /// <returns>Transaction</returns>
        public virtual Transaction Apply(JObject input)
        {
            var transType = 0;

            input.TryGetValue("transaction", out var transToken);

            if (transToken != null) transType = transToken["type"].ToObject<int>();

            switch (TransactionTypeExtension.GetRawValue(transType))
            {
                case TransactionType.TRANSFER:
                    return new TransferTransactionMapping().Apply(input);

                case TransactionType.REGISTER_NAMESPACE:
                    return new RegisterNamespaceTransactionMapping().Apply(input);

                case TransactionType.MOSAIC_DEFINITION:
                    return new MosaicDefinitionTransactionMapping().Apply(input);

                case TransactionType.MOSAIC_SUPPLY_CHANGE:
                    return new MosaicSupplyChangeTransactionMapping().Apply(input);

                case TransactionType.AGGREGATE_COMPLETE:
                case TransactionType.AGGREGATE_BONDED:
                    return new AggregateTransactionMapping().Apply(input);

                case TransactionType.ADDRESS_ALIAS:
                case TransactionType.MOSAIC_ALIAS:
                    return new AliasTransactionMapping().Apply(input);

                case TransactionType.MODIFY_MULTISIG_ACCOUNT:
                    return new ModifyMultisigAccountTransactionMapping().Apply(input);

                case TransactionType.LOCK:
                    return new LockFundsTransactionMapping().Apply(input);

                case TransactionType.SECRET_LOCK:
                    return new SecretLockTransactionMapping().Apply(input);

                case TransactionType.SECRET_PROOF:
                    return new SecretProofTransactionMapping().Apply(input);

                case TransactionType.MODIFY_ADDRESS_METADATA:
                case TransactionType.MODIFY_MOSAIC_METADATA:
                case TransactionType.MODIFY_NAMESPACE_METADATA:
                    return new ModifyMetadataTransactionMapping().Apply(input);

                case TransactionType.MODIFY_ACCOUNT_PROPERTY_ADDRESS:
                    return new ModifyAccountPropertyTransactionAddressMapping().Apply(input);
                case TransactionType.MODIFY_ACCOUNT_PROPERTY_MOSAIC:
                    return new ModifyAccountPropertyTransactionMosaicMapping().Apply(input);
                case TransactionType.MODIFY_ACCOUNT_PROPERTY_ENTITY_TYPE:
                    return new ModifyAccountPropertyTransactionEntityTypeMapping().Apply(input);

                case TransactionType.LINK_ACCOUNT:
                    return new AccountLinkTransactionMapping().Apply(input);

                default:
                    throw new Exception("Unsupported transaction");
            }
        }
    }
}