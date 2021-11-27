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
using Newtonsoft.Json.Linq;
using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Metadata;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.Mapping
{
    public class MetadataSearchMapping
    {
        public static MetadataTransactionSearch Apply(JObject input)
        {
            var metadataList = new List<MetadataEntry>();

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

                    int version = transaction["metadataEntry"]["version"].ToObject<int>();
                    string compositeHash = transaction["metadataEntry"]["compositeHash"].ToObject<string>();
                    Address sourceAddress = Address.CreateFromHex(transaction["metadataEntry"]["sourceAddress"].ToObject<string>());
                    string targetKey = transaction["metadataEntry"]["targetKey"].ToObject<string>();
                    ulong scopedMetadataKey = transaction["metadataEntry"]["scopedMetadataKey"].ToObject<UInt64DTO>().ToUInt64();
                    ulong targetId = transaction["metadataEntry"]["targetId"].ToObject<UInt64DTO>().ToUInt64();
                    int metadataType = transaction["metadataEntry"]["metadataType"].ToObject<int>();
                    int valueSize = transaction["metadataEntry"]["valueSize"].ToObject<int>();
                    string value = transaction["metadataEntry"]["value"].ToObject<string>();
                    string id = transaction["meta"]["id"].ToObject<string>();
                    metadataList.Add(new MetadataEntry(version, compositeHash, sourceAddress, targetKey, scopedMetadataKey, targetId, metadataType, valueSize, value, id));
                }
            }
            return new MetadataTransactionSearch(metadataList, new Pagination(TotalEntries, PageNumber, PageSize, TotalPages));
        }
    }
}