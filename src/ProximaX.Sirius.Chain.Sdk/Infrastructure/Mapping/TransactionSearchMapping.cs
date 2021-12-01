// Copyright 2021 ProximaX
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
    public class TransactionSearchMapping
    {
        public static TransactionSearch Apply(JObject input)
        {
            var txs = new List<Transaction>();

            var pagination = input["pagination"] as JObject;
            int TotalEntries = pagination["totalEntries"].ToObject<int>();
            int PageNumber = pagination["pageNumber"].ToObject<int>();
            int PageSize = pagination["pageSize"].ToObject<int>();
            int TotalPages = pagination["totalPages"].ToObject<int>();
            if (input["data"] != null)
            {
                for (var i = 0; i < input["data"].ToList().Count; i++)
                {
                    var transaction = input["data"].ToList()[i] as JObject;
                    //var meta = transaction["meta"].ToObject<JObject>();
                    var tx = transaction["transaction"].ToObject<JObject>();
                    if (transaction != null) txs.Add(new TransactionMapping().Apply(transaction));
                }
            }
            return new TransactionSearch(txs, new Pagination(TotalEntries, PageNumber, PageSize, TotalPages));
        }
    }
}