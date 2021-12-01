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
using Flurl;
using Flurl.Http;
using Newtonsoft.Json.Linq;
using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Infrastructure.Mapping;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Receipts;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure
{
    /// <summary>
    /// BlockHttp class
    /// </summary>
    public class BlockHttp : BaseHttp
    {
        #region Constructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="BlockHttp" /> class.
        /// </summary>
        /// <param name="host">The host</param>
        public BlockHttp(string host) : this(host, new NetworkHttp(host))
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="BlockHttp" /> class.
        /// </summary>
        /// <param name="host">The host</param>
        /// <param name="networkHttp">The network http</param>
        public BlockHttp(string host, NetworkHttp networkHttp) : base(host, networkHttp)
        {
        }

        #endregion Constructors

        /// <summary>
        /// Gets a block from the chain that has the given height
        /// </summary>
        /// <param name="height">The height of the block.</param>
        /// <returns>IObservable&lt;BlockInfo&gt;</returns>
        public IObservable<BlockInfo> GetBlockByHeight(ulong height)
        {
            var route = $"{BasePath}/block/{height}";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<BlockInfoDTO>())
                .Select(i => BlockInfo.FromDto(i));
        }

        /// <summary>
        /// Returns the network nemesis block generation hash
        /// </summary>
        /// <returns>IObservable&lt;string&gt;</returns>
        public IObservable<string> GetGenerationHash()
        {
            var route = $"{BasePath}/block/1";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<BlockInfoDTO>())
                .Select(i => BlockInfo.FromDto(i).GenerationHash);
        }

        /// <summary>
        /// Gets up to limit number of blocks after given block height.
        /// </summary>
        /// <param name="height">The height of the block. If height -1 is not a multiple of the limit provided, the inferior closest multiple + 1 is used instead.</param>
        /// <param name="limit">he number of blocks to be returned. Available values : 25, 50, 75, 100</param>
        /// <returns>IObservable&lt;List&lt;BlockInfo&gt;&gt;</returns>
        public IObservable<List<BlockInfo>> GetBlockByHeightWithLimit(ulong height, BlocksLimit limit)
        {
            var route = $"{BasePath}/blocks/{height}/limit/{limit}";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<List<BlockInfoDTO>>())
                .Select(i => i.Select(b => BlockInfo.FromDto(b)).ToList());
        }

        /// <summary>
        /// Gets blocks recipts after given block height.
        /// </summary>
        /// <param name="height">The height of the block. If height -1 is not a multiple of the limit provided, the inferior closest multiple + 1 is used instead.</param>
        /// <returns>IObservable&lt;Receipts&gt;</returns>
        public IObservable<Receipts> GetBlockReceipts(ulong height)
        {
            var route = $"{BasePath}/block/{height}/receipts";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<StatementsDTO>())
                .Select(i => Receipts.FromDto(i));
        }

        /// <summary>
        /// Gets receipt merkle path by given block height and recipentHash.
        /// </summary>
        /// <param name="height"></param>
        /// <param name="receiptHash"></param>
        /// <returns>IObservable&lt;MerklePath&gt;</returns>
        public IObservable<MerklePath> GetReceiptMerklePath(ulong height, string receiptHash)
        {
            var route = $"{BasePath}/block/{height}/receipt/{receiptHash}/merkle";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<MerkleProofInfoDTO>())
                .Select(i => MerklePath.FromDto(i));
        }

        /// <summary>
        /// Get Transaction Merkle Path by height and transactionHash
        /// </summary>
        /// <param name="height"></param>
        /// <param name="transactionHash"></param>
        /// <returns>IObservable&lt;MerklePath&gt;</returns>
        public IObservable<MerklePath> GetTransactionMerklePath(ulong height, string transactionHash)
        {
            var route = $"{BasePath}/block/{height}/transaction/{transactionHash}/merkle";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<MerkleProofInfoDTO>())
                .Select(i => MerklePath.FromDto(i));
        }

        /// <summary>
        ///      Gets config of network at height.
        /// </summary>
        /// <param name="height"></param>
        /// <returns>IObservable&lt;BlockchainUpgrade&gt;</returns>
        public IObservable<BlockchainUpgrade> GetBlockUpgrade(ulong height)
        {
            var route = $"{BasePath}/upgrade/{height}";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<BlockchainUpgradeDTO>())
                .Select(i => BlockchainUpgrade.FromDto(i));
        }

        /// <summary>
        ///  Get transactions from a block
        /// </summary>
        /// <param name="height"></param>
        /// <param name="query"></param>
        /// <returns>IObservable&lt;TransactionSearch&gt;</returns>
        public IObservable<TransactionSearch> GetBlockTransactions(ulong height, TransactionQueryParams query = null)
        {
            var route = $"{BasePath}/transactions/confirmed";
            if (height > 0)
            {
                route = route.SetQueryParam("height", height);
                route = route.SetQueryParam("embedded", true);
            }
            if (query != null)
            {
                if (query.PageSize > 0)
                {
                    if (query.PageSize < 10)
                    {
                        route = route.RemoveQueryParam("pageSize");
                    }
                    else if (query.PageSize > 100)
                    {
                        route = route.SetQueryParam("pageSize", 100);
                    }
                }
                if (query.Type != 0)
                {
                    route = route.SetQueryParam("type", query.Type);
                }
                if (query.Embedded != false)
                {
                    route = route.SetQueryParam("embedded", query.Embedded);
                }
                if (query.PageNumber <= 0)
                {
                    route = route.SetQueryParam("pageNumber", 1);
                }

                if (query.Height > 0)
                {
                    if (query.ToHeight > 0)
                    {
                        route = route.RemoveQueryParam("toheight");
                    }
                    if (query.FromHeight > 0)
                    {
                        route = route.RemoveQueryParam("fromHeight");
                    }
                }
                if (query.Address != null)
                {
                    if (query.RecipientAddress != null)
                    {
                        route = route.RemoveQueryParam("recipientAddress");
                    }
                    if (query.SignerPublicKey != null)
                    {
                        route = route.RemoveQueryParam("signerPublicKey");
                    }
                }

                switch (query.Order)
                {
                    case Order.ASC:
                        route = route.SetQueryParam("ordering", "-id");
                        route = route.SetQueryParam("block", "meta.height");

                        break;

                    case Order.DESC:
                        route = route.SetQueryParam("ordering", "-id");
                        route = route.SetQueryParam("block", "meta.height");
                        break;

                    default:
                        route = route.SetQueryParam("ordering", "-id");
                        route = route.SetQueryParam("block", "meta.height");
                        break;
                }
            }
            return Observable.FromAsync(async ar => await route.GetJsonAsync<JObject>()).Select(t => TransactionSearchMapping.Apply(t));
        }
    }
}