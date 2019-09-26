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
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Utils;


namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.Mapping
{
    public class AggregateTransactionMapping : TransactionMapping
    {
        public new AggregateTransaction Apply(JObject input)
        {
            return ToAggregateTransactionTransaction(input, TransactionMappingHelper.CreateTransactionInfo(input));
        }

        private AggregateTransaction ToAggregateTransactionTransaction(JObject tx, TransactionInfo txInfo)
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

            var type = EntityTypeExtension.GetRawValue(transaction["type"].ToObject<int>());
            var deadline = new Deadline(transaction["deadline"].ToObject<UInt64DTO>().ToUInt64());
            var maxFee = transaction["maxFee"]?.ToObject<UInt64DTO>().ToUInt64();
            var innerTransactions = MapInnerTransactions(tx);
            var cosignatures = MapCosignatures(tx);
            var signature = transaction["signature"].ToObject<string>();
            var signer = new PublicAccount(transaction["signer"].ToObject<string>(), network);

            return new AggregateTransaction(
                network, TransactionMappingUtils.ExtractTransactionVersion(versionValue), type, deadline, maxFee,
                innerTransactions, cosignatures, signature, signer, txInfo);
        }

        private static List<Transaction> MapInnerTransactions(JObject transaction)
        {
          
            var txs = new List<Transaction>();

            for (var i = 0; i < transaction["transaction"]["transactions"].ToList().Count; i++)
            {
                var innerTransaction = transaction["transaction"]["transactions"].ToList()[i] as JObject;

                if (innerTransaction != null)
                {
                    var innerInnerTransaction = innerTransaction["transaction"].ToObject<JObject>();
                    innerInnerTransaction.Add("deadline", transaction["transaction"]["deadline"]);
                    innerInnerTransaction.Add("fee", transaction["transaction"]["fee"]);
                    innerInnerTransaction.Add("signature", transaction["signature"]);
                    innerTransaction["transaction"] = innerInnerTransaction;
                }

                if (innerTransaction != null && innerTransaction["meta"] == null)
                    innerTransaction.Add("meta", transaction["meta"]);

                if (innerTransaction != null) txs.Add(new TransactionMapping().Apply(innerTransaction));
            }

            return txs;
        }

        private static List<AggregateTransactionCosignature> MapCosignatures(JObject tx)
        {
            var cosignatures = new List<AggregateTransactionCosignature>();

            if (tx["cosignatures"] != null)
                cosignatures = tx["cosignatures"]
                    .Select(i => new AggregateTransactionCosignature(
                        i["signature"].ToString(),
                        new PublicAccount(i["signer"].ToString(),
                            int.Parse(tx["version"].ToString()).ExtractNetworkType())
                    )).ToList();

            return cosignatures;
        }
    }
}