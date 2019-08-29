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
using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure
{
    /// <summary>
    ///     Class NetworkHttp
    /// </summary>
    public class NetworkHttp : BaseHttp
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="NetworkHttp" /> class.
        /// </summary>
        /// <param name="host">The host</param>
        public NetworkHttp(string host) : base(host)
        {
        }


        /// <summary>
        ///     GetNetworkType
        /// </summary>
        /// <returns>IObservable&lt;NetworkType&gt;</returns>
        public IObservable<NetworkType> GetNetworkType()
        {
            var route = $"{BasePath}/network";

            return Observable.FromAsync(async res => await route.GetJsonAsync<NetworkTypeDTO>())
                .Select(r => NetworkTypeExtension.GetRawValue(r.Name));
        }


        /// <summary>
        ///      Gets config of network at height.
        /// </summary>
        /// <param name="height"></param>
        /// <returns></returns>
        public IObservable<BlockchainConfig> GetBlockConfiguration(ulong height)
        {
            var route = $"{BasePath}/config/{height}";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<CatapultConfigDTO>())
                .Select(i => BlockchainConfig.FromDto(i.CatapultConfig));

        }

    }
}