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
using System.Reactive.Linq;
using Flurl;
using Flurl.Http;
using GuardNet;
using Newtonsoft.Json.Linq;
using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Infrastructure.Mapping;
using ProximaX.Sirius.Chain.Sdk.Model;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Metadata;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure
{
    /// <summary>
    ///     Class MetadataHttp
    /// </summary>
    public class MetadataHttp : BaseHttp
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="MetadataHttp" /> class.
        /// </summary>
        /// <param name="host">The host</param>
        public MetadataHttp(string host) : this(host, new NetworkHttp(host))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MetadataHttp" /> class.
        /// </summary>
        /// <param name="host">The host</param>
        /// <param name="networkHttp">The network http</param>
        public MetadataHttp(string host, NetworkHttp networkHttp) : base(host, networkHttp)
        {
        }

        #endregion Constructors

        #region GetMetadata

        /// <summary>
        ///    Get metadatas(namespace/mosaic/account) by list of compositeHashes
        /// </summary>
        /// <param name="compositeHash">The array of composite Hash</param>
        /// <returns>IObservable&lt;IList&lt;MetadataEntry&gt;&gt;</returns>
        public IObservable<List<MetadataEntry>> GetMetadatas(List<string> compositeHash)
        {
            if (compositeHash.Count < 0) throw new ArgumentNullException(nameof(compositeHash));

            var compositeHashes = new CompositeHashes
            {
                _CompositeHash = compositeHash
            };

            var route = $"{BasePath}/metadata_v2";

            return Observable
                .FromAsync(async ar => await route.PostJsonAsync(compositeHashes).ReceiveJson<List<MetadataV2InfoDTO>>())
                .Select(h => h.Select(metadataEntry => new MetadataEntry(
                    metadataEntry.MetadataEntry.Version,
                    metadataEntry.MetadataEntry.CompositeHash,
                    Address.CreateFromHex(metadataEntry.MetadataEntry.SourceAddress),
                    metadataEntry.MetadataEntry.TargetKey,
                    metadataEntry.MetadataEntry.ScopedMetadataKey.ToUInt64(),
                    metadataEntry.MetadataEntry.TargetId.ToUInt64(),
                    metadataEntry.MetadataEntry.MetadataType,
                    metadataEntry.MetadataEntry.ValueSize,
                    metadataEntry.MetadataEntry.Value,
                    metadataEntry.Id)).ToList());
        }

        /// <summary>
        ///     Gets metadatas(namespace/mosaic/account) by composite hash
        /// </summary>
        /// <param name="compositeHash">The composite hash</param>
        /// <returns>IObservable&lt;MetadataEntry&gt;&gt;</returns>
        public IObservable<MetadataEntry> GetMetadata(string compositeHash)
        {
            var route = $"{BasePath}/metadata_v2/{compositeHash}";
            Guard.NotNull(compositeHash, nameof(compositeHash), "compositeHash type should not be null");

            return Observable
                .FromAsync(async ar => await route.GetJsonAsync<MetadataV2InfoDTO>())
                .Select(metadataEntry => new MetadataEntry(
                    metadataEntry.MetadataEntry.Version,
                    metadataEntry.MetadataEntry.CompositeHash,
                    Address.CreateFromHex(metadataEntry.MetadataEntry.SourceAddress),
                    metadataEntry.MetadataEntry.TargetKey,
                    metadataEntry.MetadataEntry.ScopedMetadataKey.ToUInt64(),
                    metadataEntry.MetadataEntry.TargetId.ToUInt64(),
                    metadataEntry.MetadataEntry.MetadataType,
                    metadataEntry.MetadataEntry.ValueSize,
                    metadataEntry.MetadataEntry.Value,
                    metadataEntry.Id));
        }

        #endregion GetMetadata

        /// <summary>
        ///     Search metadata
        /// </summary>
        /// <param name="query">The query parameters</param>
        /// <returns>IObservable&lt;MetadataTransactionSearch&gt;&gt;</returns>
        public IObservable<MetadataTransactionSearch> SearchMetadata(MetadataQueryParams query = null)
        {
            var route = $"{BasePath}/metadata_v2";
            if (query != null)
            {
                if (query.PageSize > 0)
                {
                    if (query.PageSize < 10)
                    {
                        route = route.SetQueryParam("pageSize", 10);
                    }
                    else if (query.PageSize > 100)
                    {
                        route = route.SetQueryParam("pageSize", 100);
                    }
                }

                if (query.PageNumber <= 0)
                {
                    route = route.SetQueryParam("pageNumber", 1);
                }
                if (query.SourceAddress != null) route = route.SetQueryParam("sourceAddress", query.SourceAddress.Plain);

                if (query.TargetKey != null) route = route.SetQueryParam("targetKey", query.TargetKey);
                if (!string.IsNullOrEmpty(query.ScopedMetadataKey)) route = route.SetQueryParam("scopeMetadataKey", query.ScopedMetadataKey);
                if (!string.IsNullOrEmpty(query.TargetId)) route = route.SetQueryParam("targetId", query.TargetId);
                switch (query.Order)
                  {
                      case Order.ASC:
                          route = route.SetQueryParam("ordering", "asc");
                          break;

                      case Order.DESC:
                          route = route.SetQueryParam("ordering", "desc");
                          break;

                      default:
                          route = route.SetQueryParam("ordering", "desc");
                          break;
                  }

                switch (query.SortField)
                 {
                     case MetadataSortingField.VALUE:
                         route = route.SetQueryParam("ordering", "metadataEntry.value");
                         break;

                     case MetadataSortingField.VALUE_SIZE:
                         route = route.SetQueryParam("ordering", "metadataEntry.valueSize");
                         break;

                     default:
                         route = route.SetQueryParam("ordering", "metadataEntry.value");
                         break;
                 }
            }
            return Observable
                .FromAsync(async ar => await route.GetJsonAsync<JObject>())
                .Select(t => MetadataSearchMapping.Apply(t));
        }
    }
}