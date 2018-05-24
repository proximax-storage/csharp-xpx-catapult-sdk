// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="MosaicHttp.cs" company="Nem.io">   
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
using System.Linq;
using System.Reactive.Linq;
using System.Text.RegularExpressions;
using io.nem2.sdk.Core.Crypto.Chaso.NaCl;
using io.nem2.sdk.Infrastructure.Buffers.Model;
using io.nem2.sdk.Infrastructure.Imported.Api;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Blockchain;
using io.nem2.sdk.Model.Mosaics;
using io.nem2.sdk.Model.Namespace;

namespace io.nem2.sdk.Infrastructure.HttpRepositories
{
    /// <summary>
    /// Mosaic Http Repository.
    /// </summary>
    /// <seealso cref="io.nem2.sdk.Infrastructure.HttpRepositories.HttpRouter" />
    /// <seealso cref="io.nem2.sdk.Infrastructure.HttpRepositories.IMosaicRepository" />
    /// <seealso cref="HttpRouter" />
    /// <seealso cref="IMosaicRepository" />
    public class MosaicHttp : HttpRouter, IMosaicRepository
    {
        /// <summary>
        /// Gets or sets the mosaic routes API.
        /// </summary>
        /// <value>The mosaic routes API.</value>
        private MosaicRoutesApi MosaicRoutesApi { get; }


        /// <summary>
        /// Initializes a new instance of the <see cref="MosaicHttp" /> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <exception cref="ArgumentException">Value cannot be null or empty. - host</exception>
        public MosaicHttp(string host) 
            : this(host, new NetworkHttp(host)) { }


        /// <summary>
        /// Initializes a new instance of the <see cref="MosaicHttp" /> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="networkHttp">The network HTTP.</param>
        /// <exception cref="ArgumentNullException">networkHttp</exception>
        /// <exception cref="ArgumentException">Value cannot be null or empty. - host</exception>
        public MosaicHttp(string host, NetworkHttp networkHttp) : base(host, networkHttp)
        {
            MosaicRoutesApi = new MosaicRoutesApi(host);
        }

        /// <summary>
        /// Gets the mosaic.
        /// </summary>
        /// <param name="mosaicId">The mosaic identifier.</param>
        /// <returns>IObservable&lt;MosaicInfoDTO&gt;.</returns>
        /// <exception cref="ArgumentNullException">mosaicId</exception>
        /// <exception cref="ArgumentException">invalid mosaic id</exception>
        public IObservable<MosaicInfo> GetMosaic(string mosaicId)
        {
            if (mosaicId == null) throw new ArgumentNullException(nameof(mosaicId));
            if(mosaicId.Length != 16 || !Regex.IsMatch(mosaicId, @"\A\b[0-9a-fA-F]+\b\Z")) throw new ArgumentException("invalid mosaic id");

            IObservable<NetworkType.Types> networkTypeResolve = GetNetworkTypeObservable().Take(1);

            return Observable.FromAsync(async ar => await MosaicRoutesApi.GetMosaicAsync(mosaicId))
                .Select(mosaic => new MosaicInfo(
                        mosaic.Meta.Active,
                        mosaic.Meta.Index,
                        mosaic.Meta.Id,
                        new NamespaceId( Convert.ToUInt64(mosaic.Mosaic.NamespaceId, 16)),
                        new MosaicId(Convert.ToUInt64(mosaic.Mosaic.MosaicId, 16)),
                        mosaic.Mosaic.Supply,
                        mosaic.Mosaic.Height,
                        new PublicAccount(mosaic.Mosaic.Owner, networkTypeResolve.Wait()),
                        ExtractMosaicProperties(mosaic.Mosaic.Properties)));

            
            
        }

        /// <summary>
        /// Extracts the mosaic properties.
        /// </summary>
        /// <param name="properties">The properties.</param>
        /// <returns>MosaicProperties.</returns>
        private MosaicProperties ExtractMosaicProperties(ulong[] properties)
        {
            var flags = "00" + Convert.ToString((long)properties[0], 2);
            var bitMapFlags = flags.Substring(flags.Length - 3, 3);

            return new MosaicProperties(bitMapFlags.ToCharArray()[2] == '1',
                bitMapFlags.ToCharArray()[1] == '1',
                bitMapFlags.ToCharArray()[0] == '1',
                (int)properties[1],
                properties.Count() == 3 ? properties[2] : 0);
        }

        /// <summary>
        /// Gets the mosaics.
        /// </summary>
        /// <param name="mosaicIds">The mosaic ids.</param>
        /// <returns>IObservable&lt;List&lt;MosaicInfoDTO&gt;&gt;.</returns>
        /// <exception cref="ArgumentNullException">mosaicIds</exception>
        /// <exception cref="ArgumentException">Value cannot be an empty collection. - mosaicIds
        /// or
        /// Collection contains invalid id.</exception>
        public IObservable<List<MosaicInfo>> GetMosaics(List<string> mosaicIds)
        {
            if (mosaicIds == null) throw new ArgumentNullException(nameof(mosaicIds));
            if (mosaicIds.Count == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(mosaicIds));
            if (mosaicIds.Any(e => e.Length != 16 || !Regex.IsMatch(e, @"\A\b[0-9a-fA-F]+\b\Z"))) throw new ArgumentException("Collection contains invalid id.");

            IObservable<NetworkType.Types> networkTypeResolve = GetNetworkTypeObservable().Take(1);

            return Observable.FromAsync(async ar => await MosaicRoutesApi.GetMosaicsAsync(new MosaicIds(){mosaicIds = mosaicIds}))
                .Select(e =>e. Select(mosaic => new MosaicInfo(
                    mosaic.Meta.Active,
                    mosaic.Meta.Index,
                    mosaic.Meta.Id,
                    new NamespaceId(BitConverter.ToUInt64(mosaic.Mosaic.NamespaceId.FromHex(), 0)),
                    new MosaicId(BitConverter.ToUInt64(mosaic.Mosaic.MosaicId.FromHex(), 0)),
                    mosaic.Mosaic.Supply,
                    mosaic.Mosaic.Height,
                    new PublicAccount(mosaic.Mosaic.Owner, networkTypeResolve.Wait()),
                    ExtractMosaicProperties(mosaic.Mosaic.Properties))).ToList());
        }

        /// <summary>
        /// Gets the mosaics from namespace.
        /// </summary>
        /// <param name="namespaceId">The namespace identifier.</param>
        /// <returns>IObservable&lt;List&lt;MosaicInfoDTO&gt;&gt;.</returns>
        /// <exception cref="ArgumentNullException">namespaceId</exception>
        /// <exception cref="ArgumentException">invalid namespace id</exception>
        public IObservable<List<MosaicInfo>> GetMosaicsFromNamespace(NamespaceId namespaceId)
        {
            if (namespaceId == null) throw new ArgumentNullException(nameof(namespaceId));
            if(namespaceId.HexId.Length != 16 || !Regex.IsMatch(namespaceId.HexId, @"\A\b[0-9a-fA-F]+\b\Z")) throw new ArgumentException("invalid namespace id");

            IObservable<NetworkType.Types> networkTypeResolve = GetNetworkTypeObservable().Take(1);

            return Observable.FromAsync(async ar => await MosaicRoutesApi.GetMosaicsFromNamespaceAsync(namespaceId.HexId))
                .Select(e => e.Select(mosaic => new MosaicInfo(
                    mosaic.Meta.Active,
                    mosaic.Meta.Index,
                    mosaic.Meta.Id,
                    new NamespaceId(BitConverter.ToUInt64(mosaic.Mosaic.NamespaceId.FromHex(), 0)),
                    new MosaicId(BitConverter.ToUInt64(mosaic.Mosaic.MosaicId.FromHex(), 0)),
                    mosaic.Mosaic.Supply,
                    mosaic.Mosaic.Height,
                    new PublicAccount(mosaic.Mosaic.Owner, networkTypeResolve.Wait()),
                    ExtractMosaicProperties(mosaic.Mosaic.Properties))).ToList());
        }

        /// <summary>
        /// Gets the name of the mosaics.
        /// </summary>
        /// <param name="mosaicIds">The mosaic ids.</param>
        /// <returns>IObservable&lt;MosaicNameDTO[]&gt;.</returns>
        /// <exception cref="ArgumentNullException">mosaicIds</exception>
        /// <exception cref="ArgumentException">Value cannot be an empty collection. - mosaicIds
        /// or
        /// Collection contains invalid id.</exception>
        public IObservable<List<MosaicName>> GetMosaicsName(List<string> mosaicIds)
        {
            if (mosaicIds == null) throw new ArgumentNullException(nameof(mosaicIds));
            if (mosaicIds.Count == 0) throw new ArgumentException("Value cannot be an empty collection.", nameof(mosaicIds));
            if (mosaicIds.Any(e => e.Length != 16 || !Regex.IsMatch(e, @"\A\b[0-9a-fA-F]+\b\Z"))) throw new ArgumentException("Collection contains invalid id.");

            return Observable.FromAsync(async ar => await MosaicRoutesApi.GetMosaicsNameAsync(new MosaicIds() { mosaicIds = mosaicIds }))
                .Select(e => e.Select(mosaic => new MosaicName(new MosaicId(mosaic.MosaicId), mosaic.Name, new NamespaceId(mosaic.ParentId))).ToList());
        }  
    } 
}
