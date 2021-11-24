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

using Flurl.Http;
using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Diagnostic;
using ProximaX.Sirius.Chain.Sdk.Utils;
using System;
using System.Reactive.Linq;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure
{
    public class DiagnosticHttp : BaseHttp
    {
        // <summary>
        ///     Initializes a new instance of the <see cref="DiagnosticHttp" /> class.
        /// </summary>
        /// <param name="host">The host</param>
        public DiagnosticHttp(string host) : this(host, new NetworkHttp(host))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ChainHttp" /> class.
        /// </summary>
        /// <param name="host">The host</param>
        /// <param name="networkHttp">The network http</param>
        public DiagnosticHttp(string host, NetworkHttp networkHttp) : base(host, networkHttp)
        {
        }

        /* /// <summary>
         /// Get the block storage
         /// </summary>
         /// <returns></returns>
         public IObservable<ServerInfo> GetBlockStorage()
         {
             var route = $"{BasePath}/diagnostic/storage";

             return Observable.FromAsync(async ar => await route.GetJsonAsync<ServerDTO>())
                 .Select(i => new ServerInfo(i.ServerInfo.RestVersion,i.ServerInfo.SdkVersion));
         }*/

        /// <summary>
        /// Get block storage info
        /// </summary>
        /// <returns>IObservable&lt;BlockchainStorageInfo&gt;</returns>
        public IObservable<BlockchainStorageInfo> GetBlockStorage()
        {
            var route = $"{BasePath}/diagnostic/storage";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<BlockchainStorageInfoDTO>())
                .Select(i => new BlockchainStorageInfo(i.NumAccounts.Value, i.NumBlocks.Value, i.NumTransactions.Value));
        }
    }
}