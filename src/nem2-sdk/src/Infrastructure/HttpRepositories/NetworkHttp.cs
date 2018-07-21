// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 02-01-2018
// ***********************************************************************
// <copyright file="NetworkHttp.cs" company="Nem.io">
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
using System.Reactive.Linq;
using io.nem2.sdk.Infrastructure.Imported.Api;
using io.nem2.sdk.Model.Blockchain;

namespace io.nem2.sdk.Infrastructure.HttpRepositories
{
    /// <summary>
    /// Class NetworkHttp.
    /// </summary>
    /// <seealso cref="io.nem2.sdk.Infrastructure.HttpRepositories.HttpRouter" />
    /// <seealso cref="io.nem2.sdk.Infrastructure.HttpRepositories.INetworkRepository" />
    /// <seealso cref="HttpRouter" />
    /// <seealso cref="INetworkRepository" />
    public class NetworkHttp : HttpRouter, INetworkRepository
    {
        /// <summary>
        /// Gets or sets the network routes API.
        /// </summary>
        /// <value>The network routes API.</value>
        internal NetworkRoutesApi NetworkRoutesApi { get;  }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkHttp" /> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <exception cref="ArgumentException">Value cannot be null or empty. - host</exception>
        public NetworkHttp(string host) : base(host)
        {
            NetworkRoutesApi = new NetworkRoutesApi(host);
        }

        /// <summary>
        /// Get current network type.
        /// </summary>
        /// <returns>an IObservable of NetworkTypeDTO</returns>
        public IObservable<NetworkType.Types> GetNetworkType()
        {
            return Observable.FromAsync(async ar => await NetworkRoutesApi.GetNetworkTypeAsync()).Select(e => NetworkType.GetNetwork(e["name"].ToString()));
        }
    }
}
