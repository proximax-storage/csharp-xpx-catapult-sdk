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
    public class TransactionHttp : BaseHttp
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="TransactionHttp" /> class.
        /// </summary>
        /// <param name="host">The host</param>
        /// <param name="networkHttp">The network http</param>
        public TransactionHttp(string host, NetworkHttp networkHttp) : base(host, networkHttp)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="TransactionHttp" /> class.
        /// </summary>
        /// <param name="host">The host</param>
        public TransactionHttp(string host) : this(host, new NetworkHttp(host))
        {
        }

        /// <summary>
        ///     Gets a transaction for a transaction hash
        /// </summary>
        /// <param name="transactionHash">Transaction hash</param>
        /// <returns>IObservable&lt;Transaction&gt;</returns>
        public IObservable<Transaction> GetTransaction(string transactionHash)
        {
            if (string.IsNullOrEmpty(transactionHash))
                throw new ArgumentException("Value cannot be null or empty.", nameof(transactionHash));

            var route = $"{BasePath}/transaction/{transactionHash}";

            return Observable.FromAsync(async ar => await route.GetJsonAsync<JObject>())
                .Select(t => new TransactionMapping().Apply(t));
        }


        /// <summary>
        ///     Returns transactions information for a given array of transactionIds
        /// </summary>
        /// <param name="transactionHashes">Transaction hashes</param>
        /// <returns>IObservable&lt;List&lt;Transaction&gt;&gt;</returns>
        public IObservable<List<Transaction>> GetTransactions(List<string> transactionHashes)
        {
            if (transactionHashes.Count < 0) throw new ArgumentNullException(nameof(transactionHashes));

            var hashes = new TransactionIds
            {
                _TransactionIds = transactionHashes
            };

            var route = $"{BasePath}/transaction";

            return Observable.FromAsync(async ar => await route.PostJsonAsync(hashes).ReceiveJson<List<JObject>>())
                .Select(h => h.Select(t => new TransactionMapping().Apply(t)).ToList());
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

            var route = $"{BasePath}/transaction/{transactionHash}/status";

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

            var route = $"{BasePath}/transaction/statuses";

            return Observable.FromAsync(async ar =>
                    await route.PostJsonAsync(hashes).ReceiveJson<List<TransactionStatusDTO>>())
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

            var route = $"{BasePath}/transaction";

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

            var route = $"{BasePath}/transaction/partial";

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
        public IObservable<TransactionAnnounceResponse> AnnounceAggregateBondedCosignature(
            CosignatureSignedTransaction signedTransaction)
        {
            if (signedTransaction == null) throw new ArgumentException(nameof(signedTransaction));

            var route = $"{BasePath}/transaction/cosignature";

            var payload = new TransactionPayload
            {
                Payload = JsonConvert.SerializeObject(signedTransaction)
            };

            return Observable.FromAsync(async ar =>
                    await route.PutJsonAsync(payload).ReceiveJson<AnnounceTransactionInfoDTO>())
                .Select(m => new TransactionAnnounceResponse(m.Message));
        }
    }
}