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
    ///     AccountHttp
    /// </summary>
    public class AccountHttp : BaseHttp
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="AccountHttp" /> class.
        /// </summary>
        /// <param name="host">The host</param>
        public AccountHttp(string host) : this(host, new NetworkHttp(host))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="AccountHttp" /> class.
        /// </summary>
        /// <param name="host">The host</param>
        /// <param name="networkHttp">The network http</param>
        public AccountHttp(string host, NetworkHttp networkHttp) : base(host, networkHttp)
        {
        }

        #endregion

        #region Account Info

        /// <summary>
        ///     Get account information
        /// </summary>
        /// <param name="address">The address</param>
        /// <returns>IObservable&lt;AccountInfo&gt;</returns>
        public IObservable<AccountInfo> GetAccountInfo(Address address)
        {
            Guard.NotNull(address, nameof(address), "Address should not be null");

            var route = $"{BasePath}/account/{address.Plain}";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<AccountInfoDTO>())
                .Select(info => new AccountInfo(
                    Address.CreateFromHex(info.Account.Address.ToString()),
                    info.Account.AddressHeight.ToUInt64(),
                    info.Account.PublicKey,
                    info.Account.PublicKeyHeight.ToUInt64(),
                    info.Account.Mosaics.Select(mosaic =>
                        new Mosaic(new MosaicId(mosaic.Id.ToUInt64()), mosaic.Amount.ToUInt64())).ToList(),
                    info.Account.LinkedAccountKey,
                    info.Meta
                ));
        }

        /// <summary>
        ///     Get account information
        /// </summary>
        /// <param name="addresses">The addresses</param>
        /// <returns>IObservable&lt;AccountInfo&gt;</returns>
        public IObservable<List<AccountInfo>> GetAccountsInfo(List<Address> addresses)
        {
            var route = $"{BasePath}/account";

            if (addresses.Count < 0) throw new ArgumentNullException(nameof(addresses));

            var addressList = new Addresses
            {
                _Addresses = addresses.Select(a => a.Plain).ToList()
            };

            return Observable.FromAsync(async ar =>
                    await route.PostJsonAsync(addressList).ReceiveJson<List<AccountInfoDTO>>())
                .Select(a => a
                    .Select(info => new AccountInfo(
                        Address.CreateFromHex(info.Account.Address.ToString()),
                        info.Account.AddressHeight.ToUInt64(),
                        info.Account.PublicKey,
                        info.Account.PublicKeyHeight.ToUInt64(),
                        info.Account.Mosaics.Select(mosaic =>
                            new Mosaic(new MosaicId(mosaic.Id.ToUInt64()), mosaic.Amount.ToUInt64())).ToList(),
                         info.Account.LinkedAccountKey,
                        info.Meta
                    )).ToList());
        }

        #endregion

        #region Account Properties

        /// <summary>
        ///     Gets account property by public account
        /// </summary>
        /// <param name="account">The public account</param>
        /// <returns>IObservable&lt;AccountPropertiesInfo&gt;</returns>
        public IObservable<AccountPropertiesInfo> GetAccountProperty(PublicAccount account)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));

            var route = $"{BasePath}/account/{account.PublicKey}/properties";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<AccountPropertiesInfoDTO>())
                .Select(info => new AccountPropertiesInfo(null,
                    new AccountProperties(
                        Address.CreateFromHex(info.AccountProperties.Address),
                        info.AccountProperties.Properties
                            .Select(ap =>
                                new AccountProperty(PropertyTypeExtension.GetRawValue((int)ap.PropertyType), ap.Values))
                            .OrderBy(pt => pt.PropertyType).ToList()
                    )
                ));
        }

        /// <summary>
        ///     Gets account property by address
        /// </summary>
        /// <param name="address">The address</param>
        /// <returns>IObservable&lt;AccountPropertiesInfo&gt;</returns>
        public IObservable<AccountPropertiesInfo> GetAccountProperty(Address address)
        {
            if (address == null) throw new ArgumentNullException(nameof(address));

            var route = $"{BasePath}/account/{address.Plain}/properties";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<AccountPropertiesInfoDTO>())
                .Select(info => new AccountPropertiesInfo(null,
                    new AccountProperties(
                        Address.CreateFromHex(info.AccountProperties.Address),
                        info.AccountProperties.Properties
                            .Select(ap =>
                                new AccountProperty(PropertyTypeExtension.GetRawValue((int)ap.PropertyType), ap.Values))
                            .OrderBy(pt => pt.PropertyType).ToList()
                    )
                ));
        }

        /// <summary>
        ///     Get the account properties by list of addresses
        /// </summary>
        /// <param name="addresses">The list of addresses</param>
        /// <returns></returns>
        public IObservable<List<AccountPropertiesInfo>> GetAccountProperties(List<Address> addresses)
        {
            if (addresses.Count < 0) throw new ArgumentNullException(nameof(addresses));

            var route = $"{BasePath}/account/properties";

            var addressList = new Addresses
            {
                _Addresses = addresses.Select(a => a.Plain).ToList()
            };

            return Observable.FromAsync(async ar =>
                    await route.PostJsonAsync(addressList).ReceiveJson<List<AccountPropertiesInfoDTO>>())
                .Select(apl => apl.Select(info => new AccountPropertiesInfo(null,
                    new AccountProperties(
                        Address.CreateFromHex(info.AccountProperties.Address),
                        info.AccountProperties.Properties
                            .Select(ap =>
                                new AccountProperty(PropertyTypeExtension.GetRawValue((int)ap.PropertyType), ap.Values))
                            .OrderBy(pt => pt.PropertyType).ToList()
                    )
                )).OrderBy(l => l.AccountProperties.Address.Plain).ToList());

            /*
            return Observable.FromAsync(async ar =>
                    await route.PostJsonAsync(addressList).ReceiveJson<List<AccountPropertiesInfoDTO>>())
                .Select(apl => apl.Select(info => new AccountPropertiesInfo(info.Meta == null ? "" : info.Meta.Id,
                    new AccountProperties(
                        Address.CreateFromHex(info.AccountProperties.Address),
                        info.AccountProperties.Properties
                            .Select(ap =>
                                new AccountProperty(PropertyTypeExtension.GetRawValue(ap.PropertyType), ap.Values))
                            .OrderBy(pt => pt.PropertyType).ToList()
                    )
                )).OrderBy(l => l.AccountProperties.Address.Plain).ToList());*/
        }

        #endregion

        #region Account Transactions

        /// <summary>
        ///     Get transactions for which an account is the sender or receiver.
        /// </summary>
        /// <param name="account">The public account</param>
        /// <param name="query">The query parameters</param>
        /// <returns>IObservable&lt;List&lt;Transaction&gt;&gt;</returns>
        public IObservable<List<Transaction>> Transactions(PublicAccount account, QueryParams query = null)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));

            var route = $"{BasePath}/account/{account.PublicKey}/transactions";

            if (query != null)
            {
                if (query.PageSize > 0) route.SetQueryParam("pageSize", query.PageSize);

                if (!string.IsNullOrEmpty(query.Id)) route.SetQueryParam("id", query.Id);

                switch (query.Order)
                {
                    case Order.ASC:
                        route.SetQueryParam("ordering", "id");
                        break;
                    case Order.DESC:
                        route.SetQueryParam("ordering", "-id");
                        break;
                    default:
                        route.SetQueryParam("ordering", "-id");
                        break;
                }
            }

            return Observable.FromAsync(async ar => await route.GetJsonAsync<List<JObject>>())
                .Select(h => h.Select(t => new TransactionMapping().Apply(t)).ToList());
        }

        /// <summary>
        ///     Get incoming transactions for which an account is the sender or receiver.
        /// </summary>
        /// <param name="account">The public account</param>
        /// <param name="query">The query parameters</param>
        /// <returns>IObservable&lt;List&lt;Transaction&gt;&gt;</returns>
        public IObservable<List<Transaction>> IncomingTransactions(PublicAccount account, QueryParams query = null)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));

            var route = $"{BasePath}/account/{account.PublicKey}/transactions/incoming";

            if (query != null)
            {
                if (query.PageSize > 0) route.SetQueryParam("pageSize", query.PageSize);

                if (!string.IsNullOrEmpty(query.Id)) route.SetQueryParam("id", query.Id);

                switch (query.Order)
                {
                    case Order.ASC:
                        route.SetQueryParam("ordering", "id");
                        break;
                    case Order.DESC:
                        route.SetQueryParam("ordering", "-id");
                        break;
                    default:
                        route.SetQueryParam("ordering", "-id");
                        break;
                }
            }

            return Observable.FromAsync(async ar => await route.GetJsonAsync<List<JObject>>())
                .Select(h => h.Select(t => new TransactionMapping().Apply(t)).ToList());
        }

        /// <summary>
        ///     Get outgoing transactions for which an account is the sender or receiver.
        /// </summary>
        /// <param name="account">The public account</param>
        /// <param name="query">The query parameters</param>
        /// <returns>IObservable&lt;List&lt;Transaction&gt;&gt;</returns>
        public IObservable<List<Transaction>> OutgoingTransactions(PublicAccount account, QueryParams query = null)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));

            var route = $"{BasePath}/account/{account.PublicKey}/transactions/outgoing";

            if (query != null)
            {
                if (query.PageSize > 0) route.SetQueryParam("pageSize", query.PageSize);

                if (!string.IsNullOrEmpty(query.Id)) route.SetQueryParam("id", query.Id);

                switch (query.Order)
                {
                    case Order.ASC:
                        route.SetQueryParam("ordering", "id");
                        break;
                    case Order.DESC:
                        route.SetQueryParam("ordering", "-id");
                        break;
                    default:
                        route.SetQueryParam("ordering", "-id");
                        break;
                }
            }

            return Observable.FromAsync(async ar => await route.GetJsonAsync<List<JObject>>())
                .Select(h => h.Select(t => new TransactionMapping().Apply(t)).ToList());
        }

        public IObservable<List<Transaction>> AggregateBondedTransactions(PublicAccount account, QueryParams query = null)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));

            var route = $"{BasePath}/account/{account.PublicKey}/transactions/partial";

            if (query != null)
            {
                if (query.PageSize > 0) route.SetQueryParam("pageSize", query.PageSize);

                if (!string.IsNullOrEmpty(query.Id)) route.SetQueryParam("id", query.Id);

                switch (query.Order)
                {
                    case Order.ASC:
                        route.SetQueryParam("ordering", "id");
                        break;
                    case Order.DESC:
                        route.SetQueryParam("ordering", "-id");
                        break;
                    default:
                        route.SetQueryParam("ordering", "-id");
                        break;
                }
            }

            return Observable.FromAsync(async ar => await route.GetJsonAsync<List<JObject>>())
                .Select(h => h.Select(t => new TransactionMapping().Apply(t)).ToList());
        }


        /// <summary>
        ///     Get unconfirmed transactions for which an account is the sender or receiver.
        /// </summary>
        /// <param name="account">The public account</param>
        /// <param name="query">The query parameters</param>
        /// <returns>IObservable&lt;List&lt;Transaction&gt;&gt;</returns>
        public IObservable<List<Transaction>> UnconfirmedTransactions(PublicAccount account, QueryParams query = null)
        {
            if (account == null) throw new ArgumentNullException(nameof(account));

            var route = $"{BasePath}/account/{account.PublicKey}/transactions/unconfirmed";

            if (query != null)
            {
                if (query.PageSize > 0) route.SetQueryParam("pageSize", query.PageSize);

                if (!string.IsNullOrEmpty(query.Id)) route.SetQueryParam("id", query.Id);

                switch (query.Order)
                {
                    case Order.ASC:
                        route.SetQueryParam("ordering", "id");
                        break;
                    case Order.DESC:
                        route.SetQueryParam("ordering", "-id");
                        break;
                    default:
                        route.SetQueryParam("ordering", "-id");
                        break;
                }
            }

            return Observable.FromAsync(async ar => await route.GetJsonAsync<List<JObject>>())
                .Select(h => h.Select(t => new TransactionMapping().Apply(t)).ToList());
        }

        #endregion

        #region MultiSig Account

        /// <summary>
        ///     Gets a MultisigAccountInfo for an account.
        /// </summary>
        /// <param name="address">The address</param>
        /// <returns>IObservable&lt;MultisigAccountInfo&gt;</returns>
        public IObservable<MultisigAccountInfo> GetMultisigAccountInfo(Address address)
        {
            if (address == null) throw new ArgumentNullException(nameof(address));

            var route = $"{BasePath}/account/{address.Plain}/multisig";

            var networkType = GetNetworkTypeObservable().Take(1);

            return Observable.FromAsync(async ar => await route.GetJsonAsync<MultisigAccountInfoDTO>())
                .Select(info => new MultisigAccountInfo(
                    new PublicAccount(info.Multisig.Account, networkType.Wait()),
                    info.Multisig.MinApproval.Value,
                    info.Multisig.MinRemoval.Value,
                    info.Multisig.Cosignatories.Select(cos => new PublicAccount(
                        cos, networkType.Wait())).ToList(),
                    info.Multisig.MultisigAccounts.Select(mul => new PublicAccount(
                        mul, networkType.Wait())).ToList()
                ));
        }

        /// <summary>
        ///     Gets a MultisigAccountGraphInfo for an account.
        /// </summary>
        /// <param name="address">The add</param>
        /// <returns>IObservable&lt;List&lt;MultisigAccountGraphInfo&gt;&gt;</returns>
        public IObservable<MultisigAccountGraphInfo> GetMultisigAccountGraphInfo(Address address)
        {
            if (address == null) throw new ArgumentNullException(nameof(address));

            var route = $"{BasePath}/account/{address.Plain}/multisig/graph";

            var networkType = GetNetworkTypeObservable().Take(1);

            return Observable.FromAsync(async ar => await route.GetJsonAsync<List<MultisigAccountGraphInfoDTO>>())
                .Select(entry =>
                    {
                        var graphInfoMap = new Dictionary<int, List<MultisigAccountInfo>>();
                        entry.ForEach(item =>
                            graphInfoMap.Add(
                                item.Level.Value,
                                item.MultisigEntries.Select(info =>
                                    new MultisigAccountInfo(
                                        new PublicAccount(info.Multisig.Account, networkType.Wait()),
                                        info.Multisig.MinApproval.Value,
                                        info.Multisig.MinRemoval.Value,
                                        info.Multisig.Cosignatories.Select(cos =>
                                            new PublicAccount(
                                                cos, networkType.Wait())).ToList(),
                                        info.Multisig.MultisigAccounts.Select(mul =>
                                            new PublicAccount(
                                                mul, networkType.Wait())).ToList())).ToList()));

                        return new MultisigAccountGraphInfo(graphInfoMap);
                    }
                );
        }

        #endregion
    }
}