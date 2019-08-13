using Flurl;
using Flurl.Http;
using Newtonsoft.Json.Linq;
using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Infrastructure.Mapping;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Receipts;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure
{
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

        public IObservable<BlockchainStorageInfo> GetBlockStorage()
        {
            var route = $"{BasePath}/diagnostic/storage";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<BlockchainStorageInfoDTO>())
                .Select(i => new BlockchainStorageInfo(i.NumAccounts.Value, i.NumBlocks.Value, i.NumTransactions.Value));
        }

        public IObservable<BlockInfo> GetBlockByHeight(ulong height)
        {
            var route = $"{BasePath}/block/{height}";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<BlockInfoDTO>())
                .Select(i =>  BlockInfo.FromDto(i));
        }

        public IObservable<List<BlockInfo>> GetBlockByHeighWithLimit(ulong height, BlocksLimit limit)
        {
            var route = $"{BasePath}/blocks/{height}/limit/{limit}";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<List<BlockInfoDTO>>())
                .Select(i => i.Select(b => BlockInfo.FromDto(b)).ToList());
        }

        public IObservable<Receipts> GetBlockReceipts(ulong height)
        {
            var route = $"{BasePath}/blocks/{height}/receipts";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<StatementsDTO>())
                .Select(i =>  Receipts.FromDto(i));
        }

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
