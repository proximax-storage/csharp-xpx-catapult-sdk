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
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.Mapping
{
    /// <summary>
    ///     Class ModifyAccountPropertyTransactionAddressMapping
    /// </summary>
    public class ModifyAccountPropertyTransactionMosaicMapping : TransactionMapping
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ModifyAccountPropertyTransaction&lt;MosaicId&gt;" /> class.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public new ModifyAccountPropertyTransaction<IUInt64Id> Apply(JObject input)
        {
            return ToModifyAccountPropertyTransaction(input, TransactionMappingHelper.CreateTransactionInfo(input));
        }

        /// <summary>
        ///     ToModifyAccountPropertyTransaction
        /// </summary>
        /// <param name="tx"></param>
        /// <param name="txInfo"></param>
        /// <returns></returns>
        private static ModifyAccountPropertyTransaction<IUInt64Id> ToModifyAccountPropertyTransaction(JObject tx,
            TransactionInfo txInfo)
        {
            var transaction = tx["transaction"].ToObject<JObject>();
            var version = transaction["version"].ToObject<int>();
            var network = version.ExtractNetworkType();
            var deadline = new Deadline(transaction["deadline"].ToObject<UInt64DTO>().ToUInt64());
            var maxFee = transaction["maxFee"]?.ToObject<UInt64DTO>().ToUInt64();
            var signature = transaction["signature"].ToObject<string>();
            var signer = new PublicAccount(transaction["signer"].ToObject<string>(), network);
            var type = TransactionTypeExtension.GetRawValue(transaction["type"].ToObject<int>());
            var propertyType = PropertyTypeExtension.GetRawValue(transaction["propertyType"].ToObject<int>());

            var modifications = transaction["modifications"];
            var modificationList = modifications == null
                ? new List<AccountPropertyModification<IUInt64Id>>()
                : modifications.Select(e =>
                {
                    var modificationType =
                        PropertyModificationTypeExtension.GetRawValue(e["modificationType"].ToObject<int>());
                    var mosaicId = new MosaicId(e["value"].ToObject<UInt64DTO>().ToUInt64());
                    var modification = new AccountPropertyModification<IUInt64Id>(modificationType,
                        mosaicId);
                    return modification;
                }).ToList();

            if (type == TransactionType.MODIFY_ACCOUNT_PROPERTY_MOSAIC)
                return new MosaicModification(
                    network,
                    version,
                    deadline,
                    propertyType,
                    modificationList,
                    maxFee,
                    signature,
                    signer,
                    txInfo
                );

            throw new Exception("Unsupported transaction type");
        }
    }
}