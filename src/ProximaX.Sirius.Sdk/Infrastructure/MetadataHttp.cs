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
using System.Reactive.Linq;
using Flurl.Http;
using GuardNet;
using Newtonsoft.Json.Linq;
using ProximaX.Sirius.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Sdk.Infrastructure.Mapping;
using ProximaX.Sirius.Sdk.Model.Accounts;
using ProximaX.Sirius.Sdk.Model.Metadata;
using ProximaX.Sirius.Sdk.Model.Mosaics;
using ProximaX.Sirius.Sdk.Model.Namespaces;
using ProximaX.Sirius.Sdk.Utils;

namespace ProximaX.Sirius.Sdk.Infrastructure
{
    /// <summary>
    ///     Class MetadataHttp
    /// </summary>
    public class MetadataHttp : BaseHttp
    {
        /// <summary>
        ///     GetMetadataFromAddress
        /// </summary>
        /// <param name="address">The account address</param>
        /// <returns>IObservable&lt;AddressMetadata&gt;</returns>
        public IObservable<AddressMetadata> GetMetadataFromAddress(Address address)
        {
            Guard.NotNull(address, nameof(address), "Account address should not be null");

            var route = $"{BasePath}/account/{address.Plain}/metadata";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<AddressMetadataInfoDTO>())
                .Select(info => new AddressMetadata(
                    info.Metadata.Fields.Select(f => new Field(f.Key, f.Value)).ToList(),
                    Address.CreateFromHex(info.Metadata.MetadataId)
                ));
        }

        /// <summary>
        ///     GetMetadataFromMosaic
        /// </summary>
        /// <param name="mosaicId">The mosaic Id</param>
        /// <returns> IObservable&lt;MosaicMetadata&gt;</returns>
        public IObservable<MosaicMetadata> GetMetadataFromMosaic(MosaicId mosaicId)
        {
            Guard.NotNull(mosaicId, nameof(mosaicId), "Mosaic id should not be null");

            var route = $"{BasePath}/mosaic/{mosaicId.HexId}/metadata";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<MosaicMetadataInfoDTO>())
                .Select(info => new MosaicMetadata(
                    info.Metadata.Fields.Select(f => new Field(f.Key, f.Value)).ToList(),
                    new MosaicId(info.Metadata.MetadataId.ToUInt64())
                ));
        }

        /// <summary>
        ///     GetMetadataFromNamespace
        /// </summary>
        /// <param name="namespaceId">The namespace Id</param>
        /// <returns> IObservable&lt;MosaicMetadata&gt;</returns>
        public IObservable<NamespaceMetadata> GetMetadataFromNamespace(NamespaceId namespaceId)
        {
            Guard.NotNull(namespaceId, nameof(namespaceId), "Namespace id should not be null");

            var route = $"{BasePath}/namespace/{namespaceId.HexId}/metadata";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<NamespaceMetadataInfoDTO>())
                .Select(info => new NamespaceMetadata(
                    info.Metadata.Fields.Select(f => new Field(f.Key, f.Value)).ToList(),
                    new NamespaceId(info.Metadata.MetadataId.ToUInt64())
                ));
        }

        /// <summary>
        ///     GetMetadata
        /// </summary>
        /// <param name="metadataId">the metadata id</param>
        /// <returns>IObservable&lt;Metadata&gt;</returns>
        public IObservable<Metadata> GetMetadata(string metadataId)
        {
            Guard.NotNullOrEmpty(metadataId, nameof(metadataId), "Metadata id should not be null");

            var route = $"{BasePath}/metadata/{metadataId}";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<JObject>())
                .Select(m => new MetadataMapping().Apply(m));
        }

        /// <summary>
        ///     Gets metadata list given an array of metadata ids
        /// </summary>
        /// <param name="metadataIds">The array of metadata ids</param>
        /// <returns>IObservable&lt;IList&lt;Metadata&gt;&gt;</returns>
        public IObservable<IList<Metadata>> GetMetadata(List<string> metadataIds)
        {
            Guard.NotLessThanOrEqualTo(metadataIds.Count, 0, "Metadata list should not be empty");

            var route = $"{BasePath}/metadata";

            var metadataList = new MetadataIds
            {
                _MetadataIds = metadataIds
            };

            return Observable
                .FromAsync(async ar => await route.PostJsonAsync(metadataList).ReceiveJson<List<JObject>>())
                .Select(l => l.Select(m => new MetadataMapping().Apply(m)).ToList());
        }

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

        #endregion
    }
}