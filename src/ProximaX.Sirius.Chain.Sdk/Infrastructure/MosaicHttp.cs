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
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
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
                    ExtractMosaicProperties(info.Mosaic.Properties),
                    null)
                );
        }

        /// <summary>
        ///     Get Mosaic Rich List
        /// </summary>
        /// <param name="mosaicId">The mosaicId</param>
        /// <param name="query">The query parameters</param>
        /// <returns>IObservable&lt;List&lt;MosaicRichList&gt;&gt;</returns>
        public IObservable<List<MosaicRichList>> GetMosaicRichlist(MosaicId mosaicId, QueryParams query = null)
        {
            if (mosaicId == null) throw new ArgumentNullException(nameof(mosaicId.HexId));
            var route = $"{BasePath}/mosaic/{mosaicId.HexId}/richlist";

            if (query != null)
            {
                if (query.Page <= 0)
                {
                    route = route.SetQueryParam("page", 1);
                }

                if (query.PageSize > 0) route = route.SetQueryParam("pageSize", query.PageSize);
            }
            return Observable.FromAsync(async ar => await route.GetJsonAsync<List<MosaicRichListDTO>>())
             .Select(i => i.Select(info => new MosaicRichList(Address.CreateFromHex(info.Address), info.PublicKey, info.Amount.ToUInt64())).ToList());
        }

        /// <summary>
        ///     Gets the mosaic list.
        /// </summary>
        /// <param name="mosaicIDs">The mosaicId</param>
        /// <returns>IObservable&lt;List&lt;MosaicInfo&gt;&gt;</returns>
        /// <exception cref="ArgumentNullException">mosaicId</exception>
        public IObservable<List<MosaicInfo>> GetMosaicListAsync(List<string> mosaicIDs)
        {
            if (mosaicIDs.Count < 0) throw new ArgumentNullException(nameof(mosaicIDs));
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
                    ExtractMosaicProperties(info.Mosaic.Properties),
                    null
                )).ToList());
        }

        /// <summary>
        ///     Gets the mosaic names.
        /// </summary>
        /// <param name="mosaicIDs">The mosaicId</param>
        /// <returns>IObservable&lt;List&lt;MosaicNames&gt;&gt;</returns>
        /// <exception cref="ArgumentNullException">mosaicId</exception>
        public IObservable<List<MosaicNames>> GetMosaicNames(List<string> mosaicIDs)
        {
            if (mosaicIDs.Count < 0) throw new ArgumentNullException(nameof(mosaicIDs));
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
        ///     Gets the mosaic levy.
        /// </summary>
        /// <param name="mosaicId">The mosaicId</param>
        /// <returns>IObservable&lt;MosaicLevy&gt;</returns>
        /// <exception cref="ArgumentNullException">mosaicId</exception>
        public IObservable<MosaicLevy> GetMosaicLevyInfo(MosaicId mosaicId)
        {
            if (mosaicId == null) throw new ArgumentNullException(nameof(mosaicId.HexId));

            var route = $"{BasePath}/mosaic/{mosaicId.HexId}/levy";

            var networkType = GetNetworkTypeObservable().Take(1);

            return Observable.FromAsync(async ar => await route.GetJsonAsync<MosaicLevyInfoDTO>())
                .Select(info => new MosaicLevy(
                    MosaicLevyTypeExtension.GetRawValue((int)info.Type),
                    new Recipient(new Address(info.Recipient, networkType.Wait())),
                    new MosaicId(info.mosaic.FromUInt8Array()),
                    info.Fee.ToUInt64()
                    ));
        }

        private static MosaicProperties ExtractMosaicProperties(List<MosaicPropertyDTO> properties)
        {
            var flags = "00" + Convert.ToString((long)properties[0].Value.FromUInt8Array(), 2);
            var bitMapFlags = flags.Substring(flags.Length - 3, 3);

            return MosaicProperties.Create(bitMapFlags.ToCharArray()[2] == '1',
                bitMapFlags.ToCharArray()[1] == '1',
                bitMapFlags.ToCharArray()[0] == '1',
                (int)properties[1].Value.FromUInt8Array(),
                properties.Count == 3 ? properties[2].Value.FromUInt8Array() : 0);
        }
    }
}