// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="BlockchainHttp.cs" company="Nem.io">   
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
using io.nem2.sdk.Core.Utils;
using io.nem2.sdk.Infrastructure.Buffers.Model;
using io.nem2.sdk.Infrastructure.Imported.Api;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Blockchain;
using io.nem2.sdk.Model.Transactions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleJson;

namespace io.nem2.sdk.Infrastructure.HttpRepositories
{
    /// <summary>
    /// Blockchain Http Repository.
    /// </summary>
    /// <seealso cref="io.nem2.sdk.Infrastructure.HttpRepositories.HttpRouter" />
    /// <seealso cref="io.nem2.sdk.Infrastructure.HttpRepositories.IBlockchainRepository" />
    /// <seealso cref="HttpRouter" />
    /// <seealso cref="IBlockchainRepository" />
    public class BlockchainHttp : HttpRouter, IBlockchainRepository
    {
        /// <summary>
        /// Gets the blockchain routes API.
        /// </summary>
        /// <value>The blockchain routes API.</value>
        private BlockchainRoutesApi BlockchainRoutesApi { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockchainHttp" /> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <exception cref="ArgumentException">Value cannot be null or whitespace. - host</exception>
        public BlockchainHttp(string host) 
            : this(host, new NetworkHttp(host)) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="BlockchainHttp" /> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="networkHttp">The network HTTP.</param>
        /// <exception cref="ArgumentNullException">networkHttp</exception>
        /// <exception cref="ArgumentException">Value cannot be null or empty. - host</exception>
        public BlockchainHttp(string host, NetworkHttp networkHttp) : base(host, networkHttp)
        {
            BlockchainRoutesApi = new BlockchainRoutesApi(Url);
        }

       

        /// <summary>
        /// Gets the blockchain score.
        /// </summary>
        /// <returns>IObservable&lt;BlockchainScore&gt;.</returns>
        public IObservable<ulong> GetBlockchainScore()
        {
            return Observable.FromAsync(async ar => await BlockchainRoutesApi.GetBlockchainScoreAsync())
                .Select(i => new[]
                {
                    BitConverter.ToInt32(BitConverter.GetBytes(i.ExtractBigInteger("scoreLow")), 0),
                    BitConverter.ToInt32(BitConverter.GetBytes(i.ExtractBigInteger("scoreHigh")), 0)
                }.FromInt8Array());
        }

        /// <summary>
        /// Gets the chain height.
        /// </summary>
        /// <returns>An IObservable of ChainHeightDTO</returns>
        public IObservable<ulong> GetBlockchainHeight()
        {
            return Observable.FromAsync(async ar => await BlockchainRoutesApi.GetBlockchainHeightAsync()).Select(i => i.ExtractBigInteger("height"));
        }

        /// <summary>
        /// Gets a BlockInfo for a given block height.
        /// </summary>
        /// <param name="height">The height.</param>
        /// <returns>An IObservable of BlockInfoDTO</returns>
        public IObservable<BlockInfo> GetBlockByHeight(ulong height)
        {
            return Observable.FromAsync(async ar => await BlockchainRoutesApi.GetBlockByHeightAsync(height))
                .Select(i => new BlockInfo(
                    i["meta"]["hash"].ToString(),
                    i["meta"]["generationHash"].ToString(),
                    i["meta"].ExtractBigInteger("totalFee"),
                    int.Parse(i["meta"]["numTransactions"].ToString()),
                    i["block"]["signature"].ToString(),
                    new PublicAccount(i["block"]["signer"].ToString(), int.Parse(i["block"]["version"].ToString()).ExtractNetworkType()),
                    int.Parse(i["block"]["version"].ToString()).ExtractNetworkType(),
                    int.Parse(i["block"]["version"].ToString()).ExtractVersion(),
                    int.Parse(i["block"]["type"].ToString()),
                    i["block"].ExtractBigInteger( "height"),
                    i["block"].ExtractBigInteger("timestamp"),
                    i["block"].ExtractBigInteger("difficulty"),
                    i["block"]["previousBlockHash"].ToString(),
                    i["block"]["blockTransactionsHash"].ToString()));
        }

        /// <summary>
        /// Gets the block by height with limit.
        /// </summary>
        /// <param name="height">The height.</param>
        /// <param name="limit">The limit.</param>
        /// <returns>IObservable&lt;List&lt;BlockInfoDTO&gt;&gt;.</returns>
        public IObservable<List<BlockInfo>> GetBlockByHeightWithLimit(ulong height, int limit = 100)
        {
            return Observable.FromAsync(async ar =>
                await BlockchainRoutesApi.GetBlocksByHeightWithLimitAsync(height, limit)).Select(e => 
                e.Select(i => new BlockInfo(
                    i["meta"]["hash"].ToString(),
                    i["meta"]["generationHash"].ToString(),
                    i["meta"].ExtractBigInteger("totalFee"),
                    int.Parse(i["meta"]["numTransactions"].ToString()),
                    i["block"]["signature"].ToString(),
                    new PublicAccount(i["block"]["signer"].ToString(), int.Parse(i["block"]["version"].ToString()).ExtractNetworkType()),
                    int.Parse(i["block"]["version"].ToString()).ExtractNetworkType(),
                    int.Parse(i["block"]["version"].ToString()).ExtractVersion(),
                    int.Parse(i["block"]["type"].ToString()),
                    i["block"].ExtractBigInteger("height"),
                    i["block"].ExtractBigInteger("timestamp"),
                    i["block"].ExtractBigInteger("difficulty"),
                    i["block"]["previousBlockHash"].ToString(),
                    i["block"]["blockTransactionsHash"].ToString())).ToList()); 

        }

        /// <summary>
        /// Gets the block transactions.
        /// </summary>
        /// <param name="height">The height.</param>
        /// <returns>IObservable&lt;List&lt;TransactionInfoDTO&gt;&gt;.</returns>
        public IObservable<List<Transaction>> GetBlockTransactions(ulong height)
        {
            return GetBlockTransactions(height, new QueryParams(10));
        }

        /// <summary>
        /// Gets the block transactions.
        /// </summary>
        /// <param name="height">The height.</param>
        /// <param name="query">The query.</param>
        /// <returns>IObservable&lt;List&lt;TransactionInfoDTO&gt;&gt;.</returns>
        /// <exception cref="ArgumentNullException">query</exception>
        public IObservable<List<Transaction>> GetBlockTransactions(ulong height, QueryParams query)
        {
            if (query == null) throw new ArgumentNullException(nameof(query));

            return Observable.FromAsync(async ar => await BlockchainRoutesApi.GetBlockTransactionsAsync(height, query.GetPageSize(), query.GetId()));
        }

        /// <summary>
        /// Gets the blockchain chain storage information for the chain.
        /// </summary>
        /// <returns>An IObservable of BlockchainStorageInfo</returns>
        public IObservable<BlockchainStorageInfo> GetBlockchainDiagnosticStorage()
        {
            return Observable.FromAsync(async ar => await BlockchainRoutesApi.GetDiagnosticStorageAsync()).Select(i =>
                    new BlockchainStorageInfo(int.Parse(i["numAccounts"].ToString()), int.Parse(i["numBlocks"].ToString()), int.Parse(i["numTransactions"].ToString())
            ));
        }

        /// <summary>
        /// Gets the blockchain diagnostic blocks with limit.
        /// </summary>
        /// <param name="height">The height.</param>
        /// <param name="limit">The limit.</param>
        /// <returns>IObservable&lt;List&lt;BlockInfoDTO&gt;&gt;.</returns>
        public IObservable<List<BlockInfo>> GetBlockchainDiagnosticBlocksWithLimit(ulong height, int limit = 100)
        {
            return Observable.FromAsync(async ar => await BlockchainRoutesApi.GetDiagnosticBlocksWithLimitAsync(height, limit)).Select(e =>
                e.Select(i => new BlockInfo(
                    i["meta"]["hash"].ToString(),
                    i["meta"]["generationHash"].ToString(),
                    i["meta"].ExtractBigInteger("totalFee"),
                    int.Parse(i["meta"]["numTransactions"].ToString()),
                    i["block"]["signature"].ToString(),
                    new PublicAccount(i["block"]["signer"].ToString(), int.Parse(i["block"]["version"].ToString()).ExtractNetworkType()),
                    int.Parse(i["block"]["version"].ToString()).ExtractNetworkType(),
                    int.Parse(i["block"]["version"].ToString()).ExtractVersion(),
                    int.Parse(i["block"]["type"].ToString()),
                    i["block"].ExtractBigInteger("height"),
                    i["block"].ExtractBigInteger("timestamp"),
                    i["block"].ExtractBigInteger("difficulty"),
                    i["block"]["previousBlockHash"].ToString(),
                    i["block"]["blockTransactionsHash"].ToString())).ToList());
        }
    }
}
