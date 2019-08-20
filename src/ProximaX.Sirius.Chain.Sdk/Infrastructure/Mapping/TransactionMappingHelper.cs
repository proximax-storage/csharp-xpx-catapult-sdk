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
using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Namespaces;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.Mapping
{
    public static class TransactionMappingHelper
    {
        /// <summary>
        ///     Create transaction info based on the provided transaction meta json object
        /// </summary>
        /// <param name="jsonObject">The json object</param>
        /// <returns>TransactionInfo</returns>
        public static TransactionInfo CreateTransactionInfo(JObject jsonObject)
        {
            var meta = jsonObject["meta"].ToObject<JObject>();
            var heightOjb = meta["height"].ToObject<UInt64DTO>().ToUInt64();

            var indexObj = meta["index"]?.ToObject<int>();

            //standard transaction info based on hash + id
            if (meta.ContainsKey("hash") && meta.ContainsKey("id"))
                return TransactionInfo.Create(heightOjb,
                    indexObj,
                    meta["id"].ToObject<string>(),
                    meta["hash"].ToObject<string>(),
                    meta["merkleComponentHash"].ToObject<string>()
                );
            // aggregateHash and id indicate aggregate transaction
            if (meta.ContainsKey("aggregateHash") && meta.ContainsKey("id"))
                return TransactionInfo.CreateAggregate(heightOjb,
                    indexObj,
                    meta["id"].ToObject<string>(),
                    meta["aggregateHash"].ToObject<string>(),
                    meta["aggregateId"].ToObject<string>()
                );

            // missing id
            return TransactionInfo.Create(heightOjb, meta["hash"].ToObject<string>(),
                meta["merkleComponentHash"].ToObject<string>());
        }

        public static MosaicNonce ExtractMosaicNonce(JObject tx)
        {
            var transaction = tx["transaction"].ToObject<JObject>();

            if (transaction["mosaicNonce"] != null)
                return MosaicNonce.Create(transaction["mosaicNonce"].ToObject<uint>());
            if (transaction["nonce"] != null)
                return MosaicNonce.Create(transaction["nonce"].ToObject<uint>());

            throw new ArgumentOutOfRangeException($"Unsupported mosaic properties");
        }

        public static AliasActionType ExtractActionType(JObject tx)
        {
            var transaction = tx["transaction"].ToObject<JObject>();

            if (transaction["action"] != null)
                return AliasActionTypeExtension.GetRawValue(transaction["action"].ToObject<int>());

            if (transaction["aliasAction"] != null)
                return AliasActionTypeExtension.GetRawValue(transaction["aliasAction"].ToObject<int>());

            throw new ArgumentOutOfRangeException($"Unsupported mosaic properties");
        }
    }
}