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

using Newtonsoft.Json.Linq;
using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Model.Exchange;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.Mapping
{
    public class ExchangeOfferRemoveTransactionMapping: TransactionMapping
    {
        public new ExchangeOfferRemoveTransaction Apply(JObject input)
        {
            return ToExchangeOfferRemoveTransaction(input, TransactionMappingHelper.CreateTransactionInfo(input));
        }

        private ExchangeOfferRemoveTransaction ToExchangeOfferRemoveTransaction(JObject tx, TransactionInfo txInfo)
        {
            var transaction = tx["transaction"].ToObject<JObject>();
            var version = transaction["version"];


            //Bug - It seems the dotnetcore does not 
            //understand the Integer.
            //The workaround it to double cast the version
            int versionValue;
            try
            {
                versionValue = (int)((uint)version);
            }
            catch (Exception)
            {
                versionValue = (int)version;
            }

            var network = TransactionMappingUtils.ExtractNetworkType(versionValue);
            var txVersion = TransactionMappingUtils.ExtractTransactionVersion(versionValue);

            var deadline = new Deadline(transaction["deadline"].ToObject<UInt64DTO>().ToUInt64());
            var maxFee = transaction["maxFee"]?.ToObject<UInt64DTO>().ToUInt64();
            var signature = transaction["signature"].ToObject<string>();

            return new ExchangeOfferRemoveTransaction(network, txVersion, deadline, maxFee, LoadOffers(transaction),
              signature);
        }

        private List<RemoveExchangeOffer> LoadOffers(JObject tx)
        {
            List<RemoveExchangeOffer> list = new List<RemoveExchangeOffer>();

            if (tx["offers"] != null)
            {
                list = tx["offers"].Select(i => new RemoveExchangeOffer(
                    new MosaicId(i["mosaicId"].ToObject<UInt64DTO>().ToUInt64()),
                     ExchangeOfferTypeExtension.GetRawValue(i["type"].ToObject<int>())
                
                    )).ToList();
            }

            return list;
        }
    }
}