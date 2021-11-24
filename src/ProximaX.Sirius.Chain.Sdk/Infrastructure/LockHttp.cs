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
using GuardNet;
using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Lock;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure
{
    /// <summary>
    /// LockHttp class
    /// </summary>
    public class LockHttp : BaseHttp
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="LockHttp" /> class.
        /// </summary>
        /// <param name="host">The host</param>
        public LockHttp(string host) : this(host, new NetworkHttp(host))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="LockHttp" /> class.
        /// </summary>
        /// <param name="host">The host</param>
        /// <param name="networkHttp">The network http</param>
        public LockHttp(string host, NetworkHttp networkHttp) : base(host, networkHttp)
        {
        }

        /// <summary>
        /// Get lock hash by address
        /// </summary>
        /// <param name="address">The height of the block.</param>
        /// <returns>IObservable&lt;List&lt;HashLockWithMetaInfo&gt;&gt;</returns>
        public IObservable<List<HashLockWithMetaInfo>> GetAccountLockHash(Address address)
        {
            Guard.NotNull(address, nameof(address), "Address should not be null");

            var route = $"{BasePath}/account/{address.Plain}/lock/hash";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<List<HashLockWithMeta>>())
                .Select(i => i.Select(info => new HashLockWithMetaInfo(
                   new MetaLock(info.Meta.id), new HashLockInfo(info.Lock.Account, info.Lock.AccountAddress, info.Lock.MosaicId.ToUInt64(), info.Lock.Amount.ToUInt64(), info.Lock.Height.ToUInt64(), info.Lock.Status, info.Lock.Hash)
                )).ToList());
        }

        /// <summary>
        /// Get lock hash by public account
        /// </summary>
        /// <param name="account">The public account.</param>
        /// <returns>IObservable&lt;List&lt;HashLockWithMetaInfo&gt;&gt;</returns>
        public IObservable<List<HashLockWithMetaInfo>> GetAccountLockHash(PublicAccount account)
        {
            Guard.NotNull(account, nameof(account), "Public account should not be null");

            var route = $"{BasePath}/account/{account.PublicKey}/lock/hash";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<List<HashLockWithMeta>>())
                .Select(i => i.Select(info => new HashLockWithMetaInfo(
                   new MetaLock(info.Meta.id), new HashLockInfo(info.Lock.Account, info.Lock.AccountAddress, info.Lock.MosaicId.ToUInt64(), info.Lock.Amount.ToUInt64(), info.Lock.Height.ToUInt64(), info.Lock.Status, info.Lock.Hash)
                )).ToList());
        }

        /// <summary>
        /// Get lock hash
        /// </summary>
        /// <param name="hash">The hash.</param>
        /// <returns>IObservable&lt;HashLockWithMetaInfo&gt;</returns>
        public IObservable<HashLockWithMetaInfo> GetLockHash(string hash)
        {
            Guard.NotNull(hash, nameof(hash), "Hash should not be null");
            var route = $"{BasePath}/lock/hash/{hash}";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<HashLockWithMeta>()).Select(info => new HashLockWithMetaInfo(
                   new MetaLock(info.Meta.id), new HashLockInfo(info.Lock.Account, info.Lock.AccountAddress, info.Lock.MosaicId.ToUInt64(), info.Lock.Amount.ToUInt64(), info.Lock.Height.ToUInt64(), info.Lock.Status, info.Lock.Hash)
                ));
        }

        /// <summary>
        /// Get secret hash
        /// </summary>
        /// <param name="hash">The hash.</param>
        /// <returns>IObservable&lt;List&lt;SecretLockWithMetaInfo&gt;&gt;</returns>
        public IObservable<List<SecretLockWithMetaInfo>> GetSecretHash(string hash)
        {
            Guard.NotNull(hash, nameof(hash), "Hash should not be null");

            var route = $"{BasePath}/lock/secret/{hash}";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<List<SecretLockWithMeta>>())
               .Select(i => i.Select(info => new SecretLockWithMetaInfo(
                  new MetaLock(info.Meta.id), new SecretLockInfo(info.Lock.Account, info.Lock.AccountAddress, info.Lock.MosaicId.ToUInt64(), info.Lock.Amount.ToUInt64(), info.Lock.Height.ToUInt64(), info.Lock.Status, HashTypeExtension.GetRawValue((int)info.Lock.HashAlgorithm), info.Lock.Secret, info.Lock.Recipient, info.Lock.CompositeHash)
               )).ToList());
        }

        /// <summary>
        /// Get account secret hash
        /// </summary>
        /// <param name="account">The public account.</param>
        /// <returns>IObservable&lt;List&lt;SecretLockWithMetaInfo&gt;&gt;</returns>
        public IObservable<List<SecretLockWithMetaInfo>> GetAccountSecretLockHash(PublicAccount account)
        {
            Guard.NotNull(account, nameof(account), "Public account should not be null");
            var route = $"{BasePath}/account/{account.PublicKey}/lock/secret";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<List<SecretLockWithMeta>>())
               .Select(i => i.Select(info => new SecretLockWithMetaInfo(
                  new MetaLock(info.Meta.id), new SecretLockInfo(info.Lock.Account, info.Lock.AccountAddress, info.Lock.MosaicId.ToUInt64(), info.Lock.Amount.ToUInt64(), info.Lock.Height.ToUInt64(), info.Lock.Status, HashTypeExtension.GetRawValue((int)info.Lock.HashAlgorithm), info.Lock.Secret, info.Lock.Recipient, info.Lock.CompositeHash)
               )).ToList());
        }

        /// <summary>
        /// Get account secret hash
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>IObservable&lt;List&lt;SecretLockWithMetaInfo&gt;&gt;</returns>
        public IObservable<List<SecretLockWithMetaInfo>> GetAccountSecretLockHash(Address address)
        {
            HashTypeExtension.GetRawValue(1);

            var route = $"{BasePath}/account/{address.Plain}/lock/secret";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<List<SecretLockWithMeta>>())
                .Select(i => i.Select(info => new SecretLockWithMetaInfo(
                   new MetaLock(info.Meta.id), new SecretLockInfo(info.Lock.Account, info.Lock.AccountAddress, info.Lock.MosaicId.ToUInt64(), info.Lock.Amount.ToUInt64(), info.Lock.Height.ToUInt64(), info.Lock.Status, HashTypeExtension.GetRawValue((int)info.Lock.HashAlgorithm), info.Lock.Secret, info.Lock.Recipient, info.Lock.CompositeHash)
                )).ToList());
        }

        /// <summary>
        /// Get composite hash
        /// </summary>
        /// <param name="hash">The composite hash.</param>
        /// <returns>IObservable&lt;List&lt;SecretLockWithMetaInfo&gt;&gt;</returns>
        public IObservable<List<SecretLockWithMetaInfo>> GetCompositeHash(string hash)
        {
            HashTypeExtension.GetRawValue(1);

            var route = $"{BasePath}/lock/compositeHash/{hash}";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<List<SecretLockWithMeta>>())
                .Select(i => i.Select(info => new SecretLockWithMetaInfo(
                   new MetaLock(info.Meta.id), new SecretLockInfo(info.Lock.Account, info.Lock.AccountAddress, info.Lock.MosaicId.ToUInt64(), info.Lock.Amount.ToUInt64(), info.Lock.Height.ToUInt64(), info.Lock.Status, HashTypeExtension.GetRawValue((int)info.Lock.HashAlgorithm), info.Lock.Secret, info.Lock.Recipient, info.Lock.CompositeHash)
                )).ToList());
        }
    }
}