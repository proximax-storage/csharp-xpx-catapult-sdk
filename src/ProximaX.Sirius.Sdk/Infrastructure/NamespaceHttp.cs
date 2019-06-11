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
using Flurl;
using Flurl.Http;
using ProximaX.Sirius.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Sdk.Model.Accounts;
using ProximaX.Sirius.Sdk.Model.Mosaics;
using ProximaX.Sirius.Sdk.Model.Namespaces;
using ProximaX.Sirius.Sdk.Utils;

namespace ProximaX.Sirius.Sdk.Infrastructure
{
    /// <summary>
    ///     NamespaceHttp
    /// </summary>
    public class NamespaceHttp : BaseHttp
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="NamespaceHttp" /> class.
        /// </summary>
        /// <param name="host">The host</param>
        public NamespaceHttp(string host) : this(host, new NetworkHttp(host))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="NamespaceHttp" /> class.
        /// </summary>
        /// <param name="host">The host</param>
        /// <param name="networkHttp">The network http</param>
        public NamespaceHttp(string host, NetworkHttp networkHttp) : base(host, networkHttp)
        {
        }


        /// <summary>
        ///     Gets the namespace.
        /// </summary>
        /// <param name="namespaceId">The namespace Id</param>
        /// <returns>IObservable&lt;NamespaceInfo&gt;</returns>
        public IObservable<NamespaceInfo> GetNamespace(NamespaceId namespaceId)
        {
            var route = $"{BasePath}/namespace/{namespaceId.HexId}";

            if (namespaceId == null) throw new ArgumentNullException(nameof(namespaceId));

            var networkType = GetNetworkTypeObservable().Take(1);

            return Observable.FromAsync(async ar => await route.GetJsonAsync<NamespaceInfoDTO>())
                .Select(info => new NamespaceInfo(
                    info.Meta.Active,
                    info.Meta.Index,
                    info.Meta.Id,
                    NamespaceTypeExtension.GetRawValue(info.Namespace.Type),
                    info.Namespace.Depth,
                    ExtractLevels(info.Namespace.Level0, info.Namespace.Level1, info.Namespace.Level2),
                    info.Namespace.ParentId.ToUInt64() == 0
                        ? null
                        : new NamespaceId(info.Namespace.ParentId.ToUInt64()),
                    PublicAccount.CreateFromPublicKey(info.Namespace.Owner, networkType.Wait()),
                    info.Namespace.StartHeight.ToUInt64(),
                    info.Namespace.EndHeight.ToUInt64(),
                    new Alias(AliasTypeExtension.GetRawValue(info.Namespace.Alias.Type),
                        info.Namespace.Alias.Address != null
                            ? Address.CreateFromHex(info.Namespace.Alias.Address)
                            : null,
                        info.Namespace.Alias.MosaicId != null
                            ? new MosaicId(info.Namespace.Alias.MosaicId.ToUInt64())
                            : null
                    )
                ));
        }

        /// <summary>
        ///     Gets an array of namespaces for a given account address
        /// </summary>
        /// <param name="account">The address the account.</param>
        /// <param name="query">The query parameters</param>
        /// <returns></returns>
        public IObservable<List<NamespaceInfo>> GetNamespacesFromAccount(Address account, QueryParams query)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));

            var route = $"{BasePath}/account/{account.Plain}/namespaces";

            if (query != null)
            {
                if (query.PageSize > 0) route.SetQueryParam("pageSize", query.PageSize);

                if (!string.IsNullOrEmpty(query.Id)) route.SetQueryParam("id", query.Id);
            }

            var networkType = GetNetworkTypeObservable().Take(1);

            return Observable.FromAsync(async ar => await route.GetJsonAsync<List<NamespaceInfoDTO>>())
                .Select(i => i.Select(info => new NamespaceInfo(
                    info.Meta.Active,
                    info.Meta.Index,
                    info.Meta.Id,
                    NamespaceTypeExtension.GetRawValue(info.Namespace.Type),
                    info.Namespace.Depth,
                    ExtractLevels(info.Namespace.Level0, info.Namespace.Level1, info.Namespace.Level2),
                    new NamespaceId(info.Namespace.ParentId.ToUInt64()),
                    PublicAccount.CreateFromPublicKey(info.Namespace.Owner, networkType.Wait()),
                    info.Namespace.StartHeight.ToUInt64(),
                    info.Namespace.EndHeight.ToUInt64(),
                    new Alias(AliasTypeExtension.GetRawValue(info.Namespace.Alias.Type),
                        info.Namespace.Alias.Address != null
                            ? Address.CreateFromRawAddress(info.Namespace.Alias.Address)
                            : null,
                        info.Namespace.Alias.MosaicId != null
                            ? new MosaicId(info.Namespace.Alias.MosaicId.ToUInt64())
                            : null
                    )
                )).ToList());
        }

        /// <summary>
        /// </summary>
        /// <param name="accounts"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public IObservable<List<NamespaceInfo>> GetNamespacesFromAccount(List<Address> accounts, QueryParams query)
        {
            if (accounts.Count < 0) throw new ArgumentNullException(nameof(accounts));


            var addresses = new Addresses
            {
                _Addresses = accounts.Select(a => a.Plain).ToList()
            };

            var route = $"{BasePath}/account/namespaces";

            if (query != null)
            {
                if (query.PageSize > 0) route.SetQueryParam("pageSize", query.PageSize);

                if (!string.IsNullOrEmpty(query.Id)) route.SetQueryParam("id", query.Id);
            }

            var networkType = GetNetworkTypeObservable().Take(1);

            return Observable.FromAsync(async ar =>
                    await route.PostJsonAsync(addresses).ReceiveJson<List<NamespaceInfoDTO>>())
                .Select(i => i.Select(info => new NamespaceInfo(
                    info.Meta.Active,
                    info.Meta.Index,
                    info.Meta.Id,
                    NamespaceTypeExtension.GetRawValue(info.Namespace.Type),
                    info.Namespace.Depth,
                    ExtractLevels(info.Namespace.Level0, info.Namespace.Level1, info.Namespace.Level2),
                    new NamespaceId(info.Namespace.ParentId.ToUInt64()),
                    PublicAccount.CreateFromPublicKey(info.Namespace.Owner, networkType.Wait()),
                    info.Namespace.StartHeight.ToUInt64(),
                    info.Namespace.EndHeight.ToUInt64(),
                    new Alias(AliasTypeExtension.GetRawValue(info.Namespace.Alias.Type),
                        info.Namespace.Alias.Address != null
                            ? Address.CreateFromRawAddress(info.Namespace.Alias.Address)
                            : null,
                        info.Namespace.Alias.MosaicId != null
                            ? new MosaicId(info.Namespace.Alias.MosaicId.ToUInt64())
                            : null
                    )
                )).ToList());
        }

        /// <summary>
        ///     Gets an array of namespaces for a given account address
        /// </summary>
        /// <param name="account">The public the account.</param>
        /// <param name="query">The query parameters</param>
        /// <returns></returns>
        public IObservable<List<NamespaceInfo>> GetNamespacesFromAccount(PublicAccount account, QueryParams query)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));

            var route = $"{BasePath}/account/{account.PublicKey}/namespaces";

            if (query != null)
            {
                if (query.PageSize > 0) route.SetQueryParam("pageSize", query.PageSize);

                if (!string.IsNullOrEmpty(query.Id)) route.SetQueryParam("id", query.Id);
            }

            var networkType = GetNetworkTypeObservable().Take(1);

            return Observable.FromAsync(async ar => await route.GetJsonAsync<List<NamespaceInfoDTO>>())
                .Select(i => i.Select(info => new NamespaceInfo(
                    info.Meta.Active,
                    info.Meta.Index,
                    info.Meta.Id,
                    NamespaceTypeExtension.GetRawValue(info.Namespace.Type),
                    info.Namespace.Depth,
                    ExtractLevels(info.Namespace.Level0, info.Namespace.Level1, info.Namespace.Level2),
                    new NamespaceId(info.Namespace.ParentId.ToUInt64()),
                    PublicAccount.CreateFromPublicKey(info.Namespace.Owner, networkType.Wait()),
                    info.Namespace.StartHeight.ToUInt64(),
                    info.Namespace.EndHeight.ToUInt64(),
                    new Alias(AliasTypeExtension.GetRawValue(info.Namespace.Alias.Type),
                        info.Namespace.Alias.Address != null
                            ? Address.CreateFromRawAddress(info.Namespace.Alias.Address)
                            : null,
                        info.Namespace.Alias.MosaicId != null
                            ? new MosaicId(info.Namespace.Alias.MosaicId.ToUInt64())
                            : null
                    )
                )).ToList());
        }

        /// <summary>
        ///     Get readable names for a set of namespaces
        /// </summary>
        /// <param name="namespaceIds">The list of namespaceIds</param>
        /// <returns>IObservable&lt;List&lt;NamespaceId&gt;&gt;</returns>
        public IObservable<List<NamespaceName>> GetNamespacesNames(List<NamespaceId> namespaceIds)
        {
            if (namespaceIds == null) throw new ArgumentNullException(nameof(namespaceIds));

            var route = $"{BasePath}/namespace/names";

            var namespaces = new NamespaceIds
            {
                _NamespaceIds = namespaceIds.Select(n => n.HexId).ToList()
            };

            return Observable.FromAsync(async ar =>
                    await route.PostJsonAsync(namespaces).ReceiveJson<List<NamespaceNameDTO>>())
                .Select(i => i.Select(n => new NamespaceName(
                    new NamespaceId(n.NamespaceId.ToUInt64()),
                    n.Name,
                    n.ParentId != null ? new NamespaceId(n.ParentId.ToUInt64()) : null
                )).ToList());
        }

        /// <summary>
        ///     ExtractLevels
        /// </summary>
        /// <param name="level0">The root namespace</param>
        /// <param name="level1">The sub namespace</param>
        /// <param name="level2">The sub namespace</param>
        /// <returns>List&lt;NamespaceId&gt;</returns>
        private static List<NamespaceId> ExtractLevels(UInt64DTO level0, UInt64DTO level1, UInt64DTO level2)
        {
            var list = new List<NamespaceId>();

            if (level0 != null)
                list.Add(new NamespaceId(level0.ToUInt64()));

            if (level1 != null)
                list.Add(new NamespaceId(level1.ToUInt64()));

            if (level2 != null)
                list.Add(new NamespaceId(level2.ToUInt64()));

            return list;
        }
    }
}