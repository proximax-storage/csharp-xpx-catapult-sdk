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
using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure
{
    /// <summary>
    ///     MosaicHttp
    /// </summary>
    public class MosaicHttp : BaseHttp
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MosaicHttp" /> class.
        /// </summary>
        /// <param name="host">The host</param>
        public MosaicHttp(string host) : this(host, new NetworkHttp(host))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="MosaicHttp" /> class.
        /// </summary>
        /// <param name="host">The host</param>
        /// <param name="networkHttp">The network http</param>
        public MosaicHttp(string host, NetworkHttp networkHttp) : base(host, networkHttp)
        {
        }

        /// <summary>
        ///     Gets the mosaic.
        /// </summary>
        /// <param name="mosaicId">The mosaic identifier</param>
        /// <returns>IObservable&lt;MosaicInfo&gt;</returns>
        /// <exception cref="ArgumentNullException">mosaicId</exception>
        public IObservable<MosaicInfo> GetMosaic(MosaicId mosaicId)
        {
            Guard.NotNull(mosaicId, nameof(mosaicId), "MosaicId should not be null");

            var route = $"{BasePath}/mosaic/{mosaicId.HexId}";

            var networkType = GetNetworkTypeObservable().Take(1);

            return Observable.FromAsync(async ar => await route.GetJsonAsync<MosaicInfoDTO>())
                .Select(info => new MosaicInfo(
                    info.Meta.Id,
                    new MosaicId(info.Mosaic.MosaicId.ToUInt64()),
                    info.Mosaic.Supply.ToUInt64(),
                    info.Mosaic.Height.ToUInt64(),
                    PublicAccount.CreateFromPublicKey(info.Mosaic.Owner, networkType.Wait()),
                    info.Mosaic.Revision,
                    ExtractMosaicProperties(info.Mosaic.Properties.ToUInt64Array()),
                    info.Mosaic.Levy)
                );
        }

        /// <summary>
        ///     Gets the mosaic list.
        /// </summary>
        /// <returns>IObservable&lt;MosaicInfo&gt;</returns>
        /// <exception cref="ArgumentNullException">mosaicId</exception>
        public IObservable<List<MosaicInfo>> GetMosaicListAsync(List<string> mosaicIDs)
        {
            var route = $"{BasePath}/mosaic";

            var mosaicList = new MosaicIds
            {
                _MosaicIds = mosaicIDs
            };

            var networkType = GetNetworkTypeObservable().Take(1);

            return Observable.FromAsync(async ar =>
                    await route.PostJsonAsync(mosaicList).ReceiveJson<List<MosaicInfoDTO>>())
                .Select(l => l.Select(info => new MosaicInfo(
                    info.Meta.Id,
                    new MosaicId(info.Mosaic.MosaicId.ToUInt64()),
                    info.Mosaic.Supply.ToUInt64(),
                    info.Mosaic.Height.ToUInt64(),
                    PublicAccount.CreateFromPublicKey(info.Mosaic.Owner, networkType.Wait()),
                    info.Mosaic.Revision,
                    ExtractMosaicProperties(info.Mosaic.Properties.ToUInt64Array()),
                    info.Mosaic.Levy
                )).ToList());
        }

        /// <summary>
        ///     Gets the mosaic names.
        /// </summary>
        /// <returns>IObservable&lt;List&lt;MosaicNames&gt;&gt;</returns>
        /// <exception cref="ArgumentNullException">mosaicId</exception>
        public IObservable<List<MosaicNames>> GetMosaicNames(List<string> mosaicIDs)
        {
            var route = $"{BasePath}/mosaic/names";

            var mosaicList = new MosaicIds
            {
                _MosaicIds = mosaicIDs
            };

            var networkType = GetNetworkTypeObservable().Take(1);

            return Observable.FromAsync(async ar =>
                    await route.PostJsonAsync(mosaicList).ReceiveJson<List<MosaicNamesDTO>>())
                .Select(l => l.Select(m => new MosaicNames(new MosaicId(m.MosaicId.FromUInt8Array()), m.Names)).ToList());
        }

        /// <summary>
        ///     Extracts the mosaic properties.
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <returns>MosaicProperties.</returns>
        private static MosaicProperties ExtractMosaicProperties(IReadOnlyList<ulong> properties)
        {
            var flags = "00" + Convert.ToString((long)properties[0], 2);
            var bitMapFlags = flags.Substring(flags.Length - 3, 3);

            return MosaicProperties.Create(bitMapFlags.ToCharArray()[2] == '1',
                bitMapFlags.ToCharArray()[1] == '1',
                bitMapFlags.ToCharArray()[0] == '1',
                (int)properties[1],
                properties.Count == 3 ? properties[2] : 0);
        }
    }
}