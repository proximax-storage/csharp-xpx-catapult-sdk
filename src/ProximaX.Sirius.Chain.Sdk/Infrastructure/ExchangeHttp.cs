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
using ProximaX.Sirius.Chain.Sdk.Model.Exchange;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure
{
    /// <summary>
    ///     ExchangeHttp
    /// </summary>
    public class ExchangeHttp : BaseHttp
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ExchangeHttp" /> class.
        /// </summary>
        /// <param name="host">The host</param>
        public ExchangeHttp(string host) : this(host, new NetworkHttp(host))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="ExchangeHttp" /> class.
        /// </summary>
        /// <param name="host">The host</param>
        /// <param name="networkHttp">The network http</param>
        public ExchangeHttp(string host, NetworkHttp networkHttp) : base(host, networkHttp)
        {
        }

        /// <summary>
        /// Get exchange offers by public account
        /// </summary>
        /// <param name="publicAccount">The public account.</param>
        /// <returns>IObservable&lt;AccountExchange&gt;</returns>
        public IObservable<AccountExchange> getAccountExchanges(PublicAccount publicAccount)
        {
            Guard.NotNull(publicAccount.PublicKey, nameof(publicAccount.PublicKey), "Account ID should not be null");

            var route = $"{BasePath}/account/{publicAccount.PublicKey}/exchange";

            var networkType = GetNetworkTypeObservable().Take(1);
            return Observable.FromAsync(async ar => await route.GetAsync().ReceiveJson<JObject>()).Select(exchange =>  AccountExchange.FromDTO(exchange));
        }

        /// <summary>
        /// Get exchange offers by address
        /// </summary>
        /// <param name="address">The public account.</param>
        /// <returns>IObservable&lt;AccountExchange&gt;</returns>
        public IObservable<AccountExchange> getAccountExchanges(Address address)
        {
            Guard.NotNull(address, nameof(address), "Account ID should not be null");

            var route = $"{BasePath}/account/{address.Plain}/exchange";

            var networkType = GetNetworkTypeObservable().Take(1);

            return Observable.FromAsync(async ar => await route.GetAsync().ReceiveJson<JObject>()).Select(exchange => AccountExchange.FromDTO(exchange));
        }

        /// <summary>
        /// Get exchange offer list
        /// </summary>
        /// <returns>IObservable&lt;List&lt;ExchangeMosaicID&gt;&gt;</returns>
        public IObservable<List<ExchangeMosaicID>> GetExchangeMosaic()
        {
            var route = $"{BasePath}/exchange/mosaics";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<List<ExchangeMosaicDTO>>()).Select(i => i.Select(id => new ExchangeMosaicID(id.mosaicId)).ToList());
        }

        /// <summary>
        /// Get exchange offer by type and mosaic id
        /// </summary>
        /// <param name="offerType">The offer type.</param>
        /// <param name="mosaicsid">The mosaics id.</param>

        /// <returns>IObservable&lt;List&lt;ExchangeOfferInfo&gt;&gt;</returns>
        public IObservable<List<ExchangeOfferInfo>> getExchangeOffers(string offerType, string mosaicsid)
        {
            var route = $"{BasePath}/exchange/{offerType}/{mosaicsid}";
            Guard.NotNull(offerType, nameof(offerType), "Offer type should not be null");
            Guard.NotNull(mosaicsid, nameof(mosaicsid), "Mosaic Id should not be null");

            return Observable.FromAsync(async ar => await route.GetJsonAsync<List<ExchangesDTO>>()).Select(i => i.Select(info => new ExchangeOfferInfo(info.MosaicId.ToUInt64(), info.Amount.ToUInt64(), info.InitialAmount.ToUInt64(), info.InitialCost.ToUInt64(), info.Deadline.ToUInt64(), info.Price, info.Owner, info.Type)).ToList());
        }
    }
}