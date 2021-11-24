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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Infrastructure.Mapping;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure
{
    /// <summary>
    ///     Transaction http
    /// </summary>
    public class _transactionHttp : BaseHttp
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="_transactionHttp" /> class.
        /// </summary>
        /// <param name="host">The host</param>
        /// <param name="networkHttp">The network http</param>
        public _transactionHttp(string host, NetworkHttp networkHttp) : base(host, networkHttp)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="_transactionHttp" /> class.
        /// </summary>
        /// <param name="host">The host</param>
        public _transactionHttp(string host) : this(host, new NetworkHttp(host))
        {
        }

        /// <summary>
        ///     Gets a transaction for a transaction hash
        /// </summary>
        /// <param name="transactionid">Transaction hash</param>
        /// <returns>IObservable&lt;Transaction&gt;</returns>
        public IObservable<Transaction> GetTransaction(string transactionid)
        {
            if (string.IsNullOrEmpty(transactionid))
                throw new ArgumentException("Value cannot be null or empty.", nameof(transactionid));

            var route = $"{BasePath}/transactions/confirmed/{transactionid}";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<JObject>())
                .Select(t => new TransactionMapping().Apply(t));
        }

        /// <summary>
        ///     Returns transactions information for a given array of transactionIds
        /// </summary>
        /// <param name="transactionHashes">Transaction hashes</param>
        /// <returns>IObservable&lt;List&lt;Transaction&gt;&gt;</returns>
        public IObservable<List<Transaction>> GetTransactions(List<string> transactionHashes, TransactionGroupType group)
        {
            if (transactionHashes.Count < 0) throw new ArgumentNullException(nameof(transactionHashes));

            var hashes = new TransactionIds
            {
                _TransactionIds = transactionHashes
            };

            var route = $"{BasePath}/transactions/{group}";

            return Observable.FromAsync(async ar => await route.PostJsonAsync(hashes).ReceiveJson<List<JObject>>())
                .Select(h => h.Select(t => new TransactionMapping().Apply(t)).ToList());
        }

        /// <summary>
        ///    Get transactionTypes count
        /// </summary>
        /// <param name="transactionHashes">Transaction hashes</param>
        /// <returns>IObservable&lt;List&lt;TransactionCount&gt;&gt;</returns>
        public IObservable<List<TransactionCount>> GetTransactionsCount(List<EntityType> transactionTypes)
        {
            if (transactionTypes.Count == 0) throw new ArgumentNullException(nameof(transactionTypes));

            var transaction_Types = new TransactionEntityTypes
            {
                _EntityType = transactionTypes
            };

            var route = $"{BasePath}/transactions/count";

            return Observable.FromAsync(async ar => await route.PostJsonAsync(transaction_Types).ReceiveJson<List<TransactionCountDTO>>()).Select(h => h.Select(t => new TransactionCount(t.Count, t.Type)).ToList());
        }

        /// <summary>
        ///     Gets a transaction status for a transaction hash
        /// </summary>
        /// <param name="transactionHash">Transaction hash</param>
        /// <returns>IObservable&lt;TransactionStatus&gt;</returns>
        public IObservable<TransactionStatus> GetTransactionStatus(string transactionHash)
        {
            if (string.IsNullOrEmpty(transactionHash))
                throw new ArgumentException("Value cannot be null or empty.", nameof(transactionHash));

            var route = $"{BasePath}/transactionStatus/{transactionHash}";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<TransactionStatusDTO>())
                .Select(t => new TransactionStatus(t.Group, t.Status, t.Hash,
                    t.Deadline.ToUInt64(), t.Height.ToUInt64()));
        }

        /// <summary>
        ///     Get a list of transaction statuses for a given array of transaction hashes
        /// </summary>
        /// <param name="transactionHashes">Transaction hashes</param>
        /// <returns>List&lt;IObservable&lt;TransactionStatus&gt;&gt;</returns>
        public IObservable<List<TransactionStatus>> GetTransactionStatuses(List<string> transactionHashes)
        {
            if (transactionHashes.Count < 0) throw new ArgumentNullException(nameof(transactionHashes));

            var hashes = new TransactionHashes
            {
                Hashes = transactionHashes
            };

            var route = $"{BasePath}/transactionStatus/{transactionHashes}";

            return Observable.FromAsync(async ar =>
                    await route.PostJsonAsync(transactionHashes).ReceiveJson<List<TransactionStatusDTO>>())
                .Select(h => h.Select(hash => new TransactionStatus(hash.Group, hash.Status, hash.Hash,
                    hash.Deadline.ToUInt64(), hash.Height.ToUInt64())).ToList());
        }

        /// <summary>
        ///     Announce transaction
        /// </summary>
        /// <param name="signedTransaction">The signed transaction</param>
        /// <returns>IObservable&lt;TransactionAnnounceResponse&gt;</returns>
        public IObservable<TransactionAnnounceResponse> Announce(SignedTransaction signedTransaction)
        {
            if (signedTransaction == null) throw new ArgumentException(nameof(signedTransaction));

            var route = $"{BasePath}/transactions";

            var payload = new TransactionPayload
            {
                Payload = signedTransaction.Payload
            };

            return Observable.FromAsync(async ar =>
                    await route.PutJsonAsync(payload).ReceiveJson<AnnounceTransactionInfoDTO>())
                .Select(m => new TransactionAnnounceResponse(m.Message));
        }

        /// <summary>
        ///     Announce aggregate bonded
        /// </summary>
        /// <param name="signedTransaction">The signedTransaction</param>
        /// <returns>IObservable&lt;TransactionAnnounceResponse&gt;</returns>
        public IObservable<TransactionAnnounceResponse> AnnounceAggregateBonded(SignedTransaction signedTransaction)
        {
            if (signedTransaction == null) throw new ArgumentException(nameof(signedTransaction));

            var route = $"{BasePath}/transactions/partial";

            var payload = new TransactionPayload
            {
                Payload = signedTransaction.Payload
            };

            return Observable.FromAsync(async ar =>
                    await route.PutJsonAsync(payload).ReceiveJson<AnnounceTransactionInfoDTO>())
                .Select(m => new TransactionAnnounceResponse(m.Message));
        }

        /// <summary>
        ///     Announce aggregate bonded cosignature
        /// </summary>
        /// <param name="signedTransaction">The signed transaction</param>
        /// <returns>IObservable&lt;TransactionAnnounceResponse&gt;</returns>
        public IObservable<TransactionAnnounceResponse> AnnounceAggregateBondedCosignatureAsync(
            CosignatureSignedTransaction cosignatureSignedTransaction)
        {
            if (cosignatureSignedTransaction == null) throw new ArgumentException(nameof(cosignatureSignedTransaction));

            var route = $"{BasePath}/transactions/cosignature";

            var payload = new
            {
                parentHash = cosignatureSignedTransaction.ParentHash,
                signature = cosignatureSignedTransaction.Signature,
                signer = cosignatureSignedTransaction.Signer
            };
            //var rb = JsonConvert.SerializeObject(payload);

            return Observable.FromAsync(async ar =>
                         await route.PutJsonAsync(payload).ReceiveJson<AnnounceTransactionInfoDTO>())
                     .Select(m => new TransactionAnnounceResponse(m.Message));
        }

        /// <summary>
        ///     Get transactions information
        /// </summary>
        /// <param name="transactionType">The transaction type</param>
        /// <param name="transactionHash">The transaction hash</param>
        /// <returns>IObservable&lt;Transaction&gt;</returns>
        public IObservable<Transaction> SearchTransactions(TransactionGroupType transactionType, string transactionHash)
        {
            //if (transactionType == null) throw new ArgumentException(nameof(transactionType));
            if (transactionHash == null) throw new ArgumentException(nameof(transactionHash));

            var route = $"{BasePath}/transactions/{transactionType}/{transactionHash}";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<JObject>())
              .Select(t => new TransactionMapping().Apply(t));
        }

        /// <summary>
        ///     Get transactions information
        /// </summary>
        /// <param name="transactionType">The transaction group type</param>
        /// <param name="query">The query params</param>
        /// <returns>IObservable&lt;TransactionSearch&gt;</returns>
        public IObservable<TransactionSearch> SearchTransactions(TransactionGroupType transactionType, TransactionQueryParams query = null)
        {
            var route = $"{BasePath}/transactions/{transactionType}";
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