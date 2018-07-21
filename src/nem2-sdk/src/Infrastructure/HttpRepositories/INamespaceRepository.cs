// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="INamespaceRepository.cs" company="Nem.io">   
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
using io.nem2.sdk.Infrastructure.Buffers.Model;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Namespace;
using io.nem2.sdk.Model.Namespace.io.nem2.sdk.Model.Mosaics;

namespace io.nem2.sdk.Infrastructure.HttpRepositories
{
    /// <summary>
    /// Interface INamespaceRepository
    /// </summary>
    interface INamespaceRepository
    {
        /// <summary>
        /// Gets the namespace.
        /// </summary>
        /// <param name="namespaceId">The namespace identifier.</param>
        /// <returns>IObservable&lt;NamespaceInfoDTO&gt;.</returns>
        IObservable<NamespaceInfo> GetNamespace(NamespaceId namespaceId);

        /// <summary>
        /// Gets the namespaces from account.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns>IObservable&lt;List&lt;NamespaceInfoDTO&gt;&gt;.</returns>
        IObservable<List<NamespaceInfo>> GetNamespacesFromAccount(PublicAccount account);

        /// <summary>
        /// Gets the namespaces from account.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="query">The query.</param>
        /// <returns>IObservable&lt;List&lt;NamespaceInfoDTO&gt;&gt;.</returns>
        IObservable<List<NamespaceInfo>> GetNamespacesFromAccount(PublicAccount account, QueryParams query);

        /// <summary>
        /// Gets the namespaces from accounts.
        /// </summary>
        /// <param name="accounts">The accounts.</param>
        /// <returns>IObservable&lt;List&lt;NamespaceInfoDTO&gt;&gt;.</returns>
        IObservable<List<NamespaceInfo>> GetNamespacesFromAccounts(List<PublicAccount> accounts);

        /// <summary>
        /// Gets the namespaces from accounts.
        /// </summary>
        /// <param name="accounts">The accounts.</param>
        /// <param name="query">The query.</param>
        /// <returns>IObservable&lt;List&lt;NamespaceInfoDTO&gt;&gt;.</returns>
        IObservable<List<NamespaceInfo>> GetNamespacesFromAccounts(List<PublicAccount> accounts, QueryParams query);

        /// <summary>
        /// Gets the namespaces names.
        /// </summary>
        /// <param name="namespaceIds">The namespace ids.</param>
        /// <returns>IObservable&lt;List&lt;NamespaceNameDTO&gt;&gt;.</returns>
        IObservable<List<NamespaceId>> GetNamespacesNames(List<NamespaceId> namespaceIds);
    }
}
