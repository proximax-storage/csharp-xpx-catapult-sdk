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
using System.Reactive.Linq;
using Flurl.Http;
using Flurl.Http.Configuration;
using GuardNet;
using Newtonsoft.Json;
using ProximaX.Sirius.Sdk.Model.Blockchain;

namespace ProximaX.Sirius.Sdk.Infrastructure
{
    /// <summary>
    ///     Class BaseHttp
    /// </summary>
    public class BaseHttp
    {
        /// <summary>
        ///     The base path
        /// </summary>
        protected readonly string BasePath;

        /// <summary>
        ///     Initializes a new instance of the <see cref="BaseHttp" /> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="networkHttp">The network HTTP.</param>
        protected BaseHttp(string host, NetworkHttp networkHttp)
        {
            Guard.NotNullOrEmpty(host, nameof(host), "Host could not be null or empty");

            NetworkHttp = networkHttp ?? throw new ArgumentNullException(nameof(networkHttp));

            BasePath = host;
           
            FlurlHttp.Configure(settings =>
            {
                var jsonSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ObjectCreationHandling = ObjectCreationHandling.Replace
                };
                settings.JsonSerializer = new NewtonsoftJsonSerializer(jsonSettings);
            });
            
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BaseHttp" /> class.
        /// </summary>
        /// <param name="host">The host</param>
        protected BaseHttp(string host)
        {
            Guard.NotNullOrEmpty(host, nameof(host), "Host could not be null or empty");

            BasePath = host;

            FlurlHttp.Configure(settings =>
            {
                var jsonSettings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore,
                    ObjectCreationHandling = ObjectCreationHandling.Replace
                };
                settings.JsonSerializer = new NewtonsoftJsonSerializer(jsonSettings);
            });
        }

        /// <summary>
        ///     The NetworkHttp
        /// </summary>
        protected NetworkHttp NetworkHttp { get; }

        /// <summary>
        ///     The Network Type
        /// </summary>
        public NetworkType NetworkType { get; set; }

        /// <summary>
        ///     Get the NetworkType observable
        /// </summary>
        /// <returns>IObservable&lt;NetworkType&gt;</returns>
        protected IObservable<NetworkType> GetNetworkTypeObservable()
        {
            if (NetworkType != NetworkType.NOT_SUPPORT) return Observable.Return(NetworkType);

            var typeObservable = Observable.FromAsync(async ar => await NetworkHttp.GetNetworkType());

            typeObservable.Subscribe(ar => { NetworkType = ar; });

            return typeObservable;
        }
    }
}