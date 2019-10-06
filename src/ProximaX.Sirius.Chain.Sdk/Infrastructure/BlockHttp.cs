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

        #endregion

        /// <summary>
        /// Get the block storage
        /// </summary>
        /// <returns></returns>
        public IObservable<BlockchainStorageInfo> GetBlockStorage()
        {
            var route = $"{BasePath}/diagnostic/storage";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<BlockchainStorageInfoDTO>())
                .Select(i => new BlockchainStorageInfo(i.NumAccounts.Value, i.NumBlocks.Value, i.NumTransactions.Value));
        }

        /// <summary>
        /// Gets a block from the chain that has the given height
        /// </summary>
        /// <param name="height">The height of the block.</param>
        /// <returns>IObservable&lt;BlockInfo&gt;</returns>
        public IObservable<BlockInfo> GetBlockByHeight(ulong height)
        {
            var route = $"{BasePath}/block/{height}";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<BlockInfoDTO>())
                .Select(i =>  BlockInfo.FromDto(i));
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
        public IObservable<List<BlockInfo>> GetBlockByHeighWithLimit(ulong height, BlocksLimit limit)
        {
            var route = $"{BasePath}/blocks/{height}/limit/{limit}";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<List<BlockInfoDTO>>())
                .Select(i => i.Select(b => BlockInfo.FromDto(b)).ToList());
        }

        /// <summary>
        /// GetBlockReceipts
        /// </summary>
        /// <param name="height">The height of the block. If height -1 is not a multiple of the limit provided, the inferior closest multiple + 1 is used instead.</param>
        /// <returns></returns>
        public IObservable<Receipts> GetBlockReceipts(ulong height)
        {
            var route = $"{BasePath}/blocks/{height}/receipts";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<StatementsDTO>())
                .Select(i =>  Receipts.FromDto(i));
        }

        /// <summary>
        /// GetReceiptMerklePath
        /// </summary>
        /// <param name="height"></param>
        /// <param name="receiptHash"></param>
        /// <returns></returns>
        public IObservable<MerklePath> GetReceiptMerklePath(ulong height, string receiptHash)
        {
            var route = $"{BasePath}/block/{height}/receipt/{receiptHash}/merkle";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<MerkleProofInfoDTO>())
                .Select(i => MerklePath.FromDto(i));

        }

        /// <summary>
        /// GetTransactionMerklePath
        /// </summary>
        /// <param name="height"></param>
        /// <param name="transactionHash"></param>
        /// <returns></returns>
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
        /// <returns></returns>
        public IObservable<BlockchainUpgrade> GetBlockUpgrade(ulong height)
        {
            var route = $"{BasePath}/upgrade/{height}";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<BlockchainUpgradeDTO>())
                .Select(i => BlockchainUpgrade.FromDto(i));

        }

        /// <summary>
        /// GetBlockTransactions
        /// </summary>
        /// <param name="height"></param>
        /// <param name="query"></param>
        /// <returns></returns>
        public IObservable<List<Transaction>> GetBlockTransactions(ulong height, QueryParams query = null)
        {
            var route = $"{BasePath}/block/{height}/transactions";

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

    }
}
