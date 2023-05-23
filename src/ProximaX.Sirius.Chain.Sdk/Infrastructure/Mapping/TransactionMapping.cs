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

            switch (EntityTypeExtension.GetRawValue(transType))
            {
                case EntityType.TRANSFER:
                    return new TransferTransactionMapping().Apply(input);

                case EntityType.REGISTER_NAMESPACE:
                    return new RegisterNamespaceTransactionMapping().Apply(input);

                case EntityType.MOSAIC_DEFINITION:
                    return new MosaicDefinitionTransactionMapping().Apply(input);

                case EntityType.MOSAIC_SUPPLY_CHANGE:
                    return new MosaicSupplyChangeTransactionMapping().Apply(input);

                case EntityType.AGGREGATE_COMPLETE:
                case EntityType.AGGREGATE_BONDED:
                    return new AggregateTransactionMapping().Apply(input);

                case EntityType.ADDRESS_ALIAS:
                case EntityType.MOSAIC_ALIAS:
                    return new AliasTransactionMapping().Apply(input);

                case EntityType.MODIFY_MULTISIG_ACCOUNT:
                    return new ModifyMultisigAccountTransactionMapping().Apply(input);

                case EntityType.HASH_LOCK:
                    return new LockFundsTransactionMapping().Apply(input);

                case EntityType.SECRET_LOCK:
                    return new SecretLockTransactionMapping().Apply(input);

                case EntityType.SECRET_PROOF:
                    return new SecretProofTransactionMapping().Apply(input);

                case EntityType.MODIFY_ADDRESS_METADATA:
                case EntityType.MODIFY_MOSAIC_METADATA:
                case EntityType.MODIFY_NAMESPACE_METADATA:
                    return new ModifyMetadataTransactionMapping().Apply(input);

                case EntityType.MODIFY_ACCOUNT_PROPERTY_ADDRESS:
                    return new ModifyAccountPropertyTransactionAddressMapping().Apply(input);

                case EntityType.MODIFY_ACCOUNT_PROPERTY_MOSAIC:
                    return new ModifyAccountPropertyTransactionMosaicMapping().Apply(input);

                case EntityType.MODIFY_ACCOUNT_PROPERTY_ENTITY_TYPE:
                    return new ModifyAccountPropertyTransactionEntityTypeMapping().Apply(input);

                case EntityType.LINK_ACCOUNT:
                    return new AccountLinkTransactionMapping().Apply(input);

                case EntityType.EXCHANGE_OFFER_ADD:
                    return new ExchangeOfferAddTransactionMapping().Apply(input);

                case EntityType.EXCHANGE_OFFER_REMOVE:
                    return new ExchangeOfferRemoveTransactionMapping().Apply(input);

                case EntityType.EXCHANGE_OFFER:
                    return new ExchangeOfferTransactionMapping().Apply(input);

                case EntityType.MODIFY_MOSAIC_LEVY:
                    return new ModifyMosaicLevyTransactionMapping().Apply(input);

                case EntityType.REMOVE_MOSAIC_LEVY:
                    return new RemoveMosaicLevyTransactionMapping().Apply(input);

                case EntityType.MOSAIC_METADATA_V2:
                    return new MosaicMetadataV2TransactionMapping().Apply(input);

                case EntityType.NAMESPACE_METADATA_V2:
                    return new NamespaceMetadataV2TransactionMapping().Apply(input);

                case EntityType.ACCOUNT_METADATA_V2:
                    return new AccountMetadataV2TransactionMapping().Apply(input);

                default:
                    throw new Exception("Unsupported transaction");
            }
        }
    }
}