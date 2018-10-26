// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="NamespaceHttp.cs" company="Nem.io">   
// Copyright 2018 NEM
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
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using io.nem2.sdk.Core.Utils;
using io.nem2.sdk.Infrastructure.Buffers.Model;
using io.nem2.sdk.Infrastructure.Imported.Api;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Blockchain;
using io.nem2.sdk.Model.Namespace;
using io.nem2.sdk.Model.Namespace.io.nem2.sdk.Model.Mosaics;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace io.nem2.sdk.Infrastructure.HttpRepositories
{

    /// <summary>
    /// Class NamespaceHttp.
    /// </summary>
    /// <seealso cref="io.nem2.sdk.Infrastructure.HttpRepositories.HttpRouter" />
    /// <seealso cref="io.nem2.sdk.Infrastructure.HttpRepositories.INamespaceRepository" />
    /// <seealso cref="HttpRouter" />
    /// <seealso cref="INamespaceRepository" />
    public class NamespaceHttp : HttpRouter, INamespaceRepository
    {
        /// <summary>
        /// Gets or sets the namespace routes API.
        /// </summary>
        /// <value>The namespace routes API.</value>
        private NamespaceRoutesApi NamespaceRoutesApi { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NamespaceHttp" /> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <exception cref="ArgumentException">Value cannot be null or empty. - host</exception>
        public NamespaceHttp(string host) 
            : this(host, new NetworkHttp(host)) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="NamespaceHttp" /> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="networkHttp">The network HTTP.</param>
        /// <exception cref="ArgumentNullException">networkHttp</exception>
        /// <exception cref="ArgumentException">Value cannot be null or empty. - host</exception>
        public NamespaceHttp(string host, NetworkHttp networkHttp) : base(host, networkHttp)
        {
            NamespaceRoutesApi = new NamespaceRoutesApi(host);
        }

        /// <summary>
        /// Gets the namespace.
        /// </summary>
        /// <param name="namespaceId">The namespace identifier.</param>
        /// <returns>IObservable&lt;NamespaceInfoDTO&gt;.</returns>
        /// <exception cref="ArgumentException">Value cannot be null or empty. - namespaceId
        /// or
        /// Invalid namespace</exception>
        public IObservable<NamespaceInfo> GetNamespace(NamespaceId namespaceId)
        {
            if (string.IsNullOrEmpty(namespaceId.Name)) throw new ArgumentException("Value cannot be null or empty.", nameof(namespaceId));
            if(namespaceId.HexId.Length != 16 || !Regex.IsMatch(namespaceId.HexId, @"\A\b[0-9a-fA-F]+\b\Z")) throw new ArgumentException("Invalid namespace");

            IObservable<NetworkType.Types> networkTypeResolve = GetNetworkTypeObservable().Take(1);

            return Observable.FromAsync(async ar => await NamespaceRoutesApi.GetNamespaceAsync(namespaceId.HexId))
                .Select(e =>
                {
                    Console.WriteLine(e);
                    return new NamespaceInfo(
                        bool.Parse(e["meta"]["active"].ToString()),
                        int.Parse(e["meta"]["index"].ToString()),
                        e["meta"]["id"].ToString(),
                        NamespaceTypes.GetRawValue(byte.Parse(e["namespace"]["type"].ToString())),
                        int.Parse(e["namespace"]["depth"].ToString()),
                        ExtractLevels(e["namespace"]),
                        new NamespaceId(e["namespace"].ExtractBigInteger("parentId")),
                        e["namespace"].ExtractBigInteger( "startHeight"),
                        e["namespace"].ExtractBigInteger("endHeight"),
                        new PublicAccount(e["namespace"]["owner"].ToString(), networkTypeResolve.Wait())
                    );
                });
        }

        /// <summary>
        /// Gets the namespaces from account.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns>IObservable&lt;List&lt;NamespaceInfoDTO&gt;&gt;.</returns>
        /// <exception cref="ArgumentNullException">account</exception>
        public IObservable<List<NamespaceInfo>> GetNamespacesFromAccount(PublicAccount account)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));

            return GetNamespacesFromAccount(account, new QueryParams(10));
        }

        /// <summary>
        /// Gets the namespaces from account.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns>IObservable&lt;List&lt;NamespaceInfoDTO&gt;&gt;.</returns>
        /// <exception cref="ArgumentNullException">account</exception>
        public IObservable<List<NamespaceInfo>> GetNamespacesFromAccount(Address account)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));

            return GetNamespacesFromAccount(account, new QueryParams(10));
        }

        /// <summary>
        /// Gets the namespaces from account.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="query">The query.</param>
        /// <returns>IObservable&lt;List&lt;NamespaceInfoDTO&gt;&gt;.</returns>
        /// <exception cref="ArgumentNullException">account
        /// or
        /// query</exception>
        public IObservable<List<NamespaceInfo>> GetNamespacesFromAccount(Address account, QueryParams query)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));
            if (query == null) throw new ArgumentNullException(nameof(query));

            IObservable<NetworkType.Types> networkTypeResolve = GetNetworkTypeObservable().Take(1);

            return Observable.FromAsync(async ar => await NamespaceRoutesApi.GetNamespacesFromAccountAsync(account.Plain, query.GetPageSize(), query.GetId()))
                .Select(i => i.Select(e =>
                    new NamespaceInfo(
                        bool.Parse(e["meta"]["active"].ToString()),
                        int.Parse(e["meta"]["index"].ToString()),
                        e["meta"]["id"].ToString(),
                        NamespaceTypes.GetRawValue(byte.Parse(e["namespace"]["type"].ToString())),
                        int.Parse(e["namespace"]["depth"].ToString()),
                        ExtractLevels(e["namespace"]),
                        new NamespaceId(e["namespace"].ExtractBigInteger("parentId")),
                        e["namespace"].ExtractBigInteger("startHeight"),
                        e["namespace"].ExtractBigInteger("endHeight"),
                        new PublicAccount(e["namespace"]["owner"].ToString(), networkTypeResolve.Wait())
                    )).ToList());
        }

        /// <summary>
        /// Gets the namespaces from account.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="query">The query.</param>
        /// <returns>IObservable&lt;List&lt;NamespaceInfoDTO&gt;&gt;.</returns>
        /// <exception cref="ArgumentNullException">account
        /// or
        /// query</exception>
        public IObservable<List<NamespaceInfo>> GetNamespacesFromAccount(PublicAccount account, QueryParams query)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));
            if (query == null) throw new ArgumentNullException(nameof(query));

            IObservable<NetworkType.Types> networkTypeResolve = GetNetworkTypeObservable().Take(1);

            return Observable.FromAsync(async ar => await NamespaceRoutesApi.GetNamespacesFromAccountAsync(account.PublicKey, query.GetPageSize(), query.GetId()))
                .Select(i => i.Select(e =>
                    new NamespaceInfo(
                        bool.Parse(e["meta"]["active"].ToString()),
                        int.Parse(e["meta"]["index"].ToString()),
                        e["meta"]["id"].ToString(),
                        NamespaceTypes.GetRawValue(byte.Parse(e["namespace"]["type"].ToString())),
                        int.Parse(e["namespace"]["depth"].ToString()),
                        ExtractLevels(e["namespace"]),
                        new NamespaceId(e["namespace"].ExtractBigInteger("parentId")),
                        e["namespace"].ExtractBigInteger( "startHeight"),
                        e["namespace"].ExtractBigInteger( "endHeight"),
                        new PublicAccount(e["namespace"]["owner"].ToString(), networkTypeResolve.Wait())
                    )).ToList());
        }

        /// <summary>
        /// Gets the namespaces from accounts.
        /// </summary>
        /// <param name="accounts">The accounts.</param>
        /// <returns>IObservable&lt;List&lt;NamespaceInfoDTO&gt;&gt;.</returns>
        /// <exception cref="ArgumentNullException">accounts</exception>
        /// <exception cref="ArgumentException">Value cannot be an empty collection. - accounts</exception>
        public IObservable<List<NamespaceInfo>> GetNamespacesFromAccounts(List<PublicAccount> accounts)
        {
            if (accounts == null) throw new ArgumentNullException(nameof(accounts));
            if (accounts.Count == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(accounts));

            return GetNamespacesFromAccounts(accounts, new QueryParams(10));
        }

        /// <summary>
        /// Gets the namespaces from accounts.
        /// </summary>
        /// <param name="accounts">The accounts.</param>
        /// <returns>IObservable&lt;List&lt;NamespaceInfoDTO&gt;&gt;.</returns>
        /// <exception cref="ArgumentNullException">accounts</exception>
        /// <exception cref="ArgumentException">Value cannot be an empty collection. - accounts</exception>
        public IObservable<List<NamespaceInfo>> GetNamespacesFromAccounts(List<Address> accounts)
        {
            if (accounts == null) throw new ArgumentNullException(nameof(accounts));
            if (accounts.Count == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(accounts));

            return GetNamespacesFromAccounts(accounts, new QueryParams(10));
        }

        /// <summary>
        /// Gets the namespaces from accounts.
        /// </summary>
        /// <param name="publicKeys">The accounts.</param>
        /// <param name="query">The query.</param>
        /// <returns>IObservable&lt;List&lt;NamespaceInfoDTO&gt;&gt;.</returns>
        /// <exception cref="ArgumentNullException">accounts
        /// or
        /// query</exception>
        /// <exception cref="ArgumentException">Value cannot be an empty collection. - accounts</exception>
        public IObservable<List<NamespaceInfo>> GetNamespacesFromAccounts(List<PublicAccount> publicKeys, QueryParams query)
        {
            if (publicKeys == null) throw new ArgumentNullException(nameof(publicKeys));
            if (query == null) throw new ArgumentNullException(nameof(query));
            if (publicKeys.Count == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(publicKeys));

            IObservable<NetworkType.Types> networkTypeResolve = GetNetworkTypeObservable().Take(1);

            return Observable.FromAsync(async ar => await NamespaceRoutesApi.GetNamespacesFromAccountsAsync(
                    JObject.FromObject(new
                    {
                        publicKeys = publicKeys.Select(i => i.PublicKey)
                    }), query.GetPageSize(), query.GetId()))
                .Select(i => i.Select(e =>
                new NamespaceInfo(
                    bool.Parse(e["meta"]["active"].ToString()),
                    int.Parse(e["meta"]["index"].ToString()),
                    e["meta"]["id"].ToString(),
                    NamespaceTypes.GetRawValue(byte.Parse(e["namespace"]["type"].ToString())),
                    int.Parse(e["namespace"]["depth"].ToString()),
                    ExtractLevels(e["namespace"]),
                    new NamespaceId(e["namespace"].ExtractBigInteger( "parentId")),
                    e["namespace"].ExtractBigInteger( "startHeight"),
                    e["namespace"].ExtractBigInteger( "endHeight"),
                    new PublicAccount(e["namespace"]["owner"].ToString(), networkTypeResolve.Wait())
                )).ToList());
        }

        /// <summary>
        /// Gets the namespaces from accounts.
        /// </summary>
        /// <param name="addresses">The addresses</param>
        /// <param name="query">The query.</param>
        /// <returns>IObservable&lt;List&lt;NamespaceInfoDTO&gt;&gt;.</returns>
        /// <exception cref="ArgumentNullException">accounts
        /// or
        /// query</exception>
        /// <exception cref="ArgumentException">Value cannot be an empty collection. - accounts</exception>
        public IObservable<List<NamespaceInfo>> GetNamespacesFromAccounts(List<Address> addresses, QueryParams query)
        {
            if (addresses == null) throw new ArgumentNullException(nameof(addresses));
            if (query == null) throw new ArgumentNullException(nameof(query));
            if (addresses.Count == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(addresses));

            IObservable<NetworkType.Types> networkTypeResolve = GetNetworkTypeObservable().Take(1);

            return Observable.FromAsync(async ar => await NamespaceRoutesApi.GetNamespacesFromAccountsAsync(
                    JObject.FromObject(new
                    {
                        addresses = addresses.Select(i => i.Plain)
                    }), query.GetPageSize(), query.GetId()))
                .Select(i => i.Select(e =>
                new NamespaceInfo(
                    bool.Parse(e["meta"]["active"].ToString()),
                    int.Parse(e["meta"]["index"].ToString()),
                    e["meta"]["id"].ToString(),
                    NamespaceTypes.GetRawValue(byte.Parse(e["namespace"]["type"].ToString())),
                    int.Parse(e["namespace"]["depth"].ToString()),
                    ExtractLevels(e["namespace"]),
                    new NamespaceId(e["namespace"].ExtractBigInteger( "parentId")),
                    e["namespace"].ExtractBigInteger( "startHeight"),
                    e["namespace"].ExtractBigInteger( "endHeight"),
                    new PublicAccount(e["namespace"]["owner"].ToString(), networkTypeResolve.Wait())
                )).ToList());
        }

        /// <summary>
        /// Gets the namespaces names.
        /// </summary>
        /// <param name="namespaceIds">The namespace ids.</param>
        /// <returns>IObservable&lt;List&lt;NamespaceNameDTO&gt;&gt;.</returns>
        /// <exception cref="ArgumentNullException">namespaceIds</exception>
        /// <exception cref="ArgumentException">Value cannot be an empty collection. - namespaceIds
        /// or
        /// Collection contains invalid id.</exception>
        public IObservable<List<NamespaceId>> GetNamespacesNames(List<NamespaceId> namespaceIds)
        {

            if (namespaceIds == null) throw new ArgumentNullException(nameof(namespaceIds));
            if (namespaceIds.Count == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(namespaceIds));
            if(namespaceIds.Any(e => e.HexId.Length != 16 || !Regex.IsMatch(e.HexId, @"\A\b[0-9a-fA-F]+\b\Z"))) throw new ArgumentException("Collection contains invalid id.");

            return Observable.FromAsync(async ar => await NamespaceRoutesApi.GetNamespacesNamesAsync(JObject.FromObject(new
                {
                    namespaceIds = namespaceIds.Select(i => i.HexId)
                }))).Select(i => i.Select(e => new NamespaceId(e["name"].ToString())).ToList());
        }

        /// <summary>
        /// Extracts the levels.
        /// </summary>
        /// <param name="namespaceDTO">The namespace dto.</param>
        /// <returns>List&lt;NamespaceId&gt;.</returns>
        private List<NamespaceId> ExtractLevels(JToken namespaceDTO)
        {
            List<NamespaceId> levels = new List<NamespaceId>();
            if (namespaceDTO.Contains("level0"))
            {
                levels.Add(new NamespaceId(namespaceDTO.ExtractBigInteger( "level0")));
            }

            if (namespaceDTO.Contains("level1"))
            {
                levels.Add(new NamespaceId(namespaceDTO.ExtractBigInteger( "level1")));
            }

            if (namespaceDTO.Contains("level2"))
            {
                levels.Add(new NamespaceId(namespaceDTO["level2"].ToString()));
            }

            return levels;
        }
    }
}
