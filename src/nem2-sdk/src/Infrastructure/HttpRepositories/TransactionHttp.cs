// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 02-01-2018
// ***********************************************************************
// <copyright file="TransactionHttp.cs" company="Nem.io">
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
using io.nem2.sdk.Infrastructure.Buffers.Model;
using io.nem2.sdk.Infrastructure.Imported.Api;
using io.nem2.sdk.Model.Transactions;
using Newtonsoft.Json.Linq;

namespace io.nem2.sdk.Infrastructure.HttpRepositories
{
    /// <summary>
    /// Class TransactionHttp.
    /// </summary>
    /// <seealso cref="io.nem2.sdk.Infrastructure.HttpRepositories.HttpRouter" />
    /// <seealso cref="io.nem2.sdk.Infrastructure.HttpRepositories.ITransactionRepository" />
    /// <inheritdoc />
    /// <seealso cref="T:io.nem2.sdk.Infrastructure.HttpRepositories.HttpRouter" />
    /// <seealso cref="T:io.nem2.sdk.Infrastructure.HttpRepositories.ITransactionRepository" />
    public class TransactionHttp : HttpRouter, ITransactionRepository
    {
        /// <summary>
        /// Gets the transaction routes API.
        /// </summary>
        /// <value>The transaction routes API.</value>
        private TransactionRoutesApi TransactionRoutesApi { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionHttp" /> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <exception cref="ArgumentException">Value cannot be null or empty. - host</exception>
        public TransactionHttp(string host) 
            : this(host, new NetworkHttp(host)) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="TransactionHttp" /> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="networkHttp">The network HTTP.</param>
        /// <exception cref="ArgumentNullException">networkHttp</exception>
        /// <exception cref="ArgumentException">Value cannot be null or empty. - host</exception>
        public TransactionHttp(string host, NetworkHttp networkHttp) : base(host, networkHttp)
        {
            TransactionRoutesApi = new TransactionRoutesApi(host);
        }

        /// <summary>
        /// Announces the transaction.
        /// </summary>
        /// <param name="signedTransaction">The signed transaction.</param>
        /// <returns>IObservable&lt;TransactionAnnounceResponse&gt;.</returns>
        /// <exception cref="ArgumentNullException">signedTransaction</exception>
        public IObservable<TransactionAnnounceResponse> Announce(SignedTransaction signedTransaction)
        {
            if (signedTransaction == null) throw new ArgumentNullException(nameof(signedTransaction));

            return Observable.FromAsync(async ar => await TransactionRoutesApi.AnnounceTransactionAsync(signedTransaction));
        }

        /// <summary>
        /// Announces the partial transaction.
        /// </summary>
        /// <param name="signedTransaction">The signed transaction.</param>
        /// <returns>IObservable&lt;TransactionAnnounceResponse&gt;.</returns>
        /// <exception cref="ArgumentNullException">signedTransaction</exception>
        public IObservable<TransactionAnnounceResponse> AnnounceAggregateBonded(SignedTransaction signedTransaction)
        {
            if (signedTransaction == null) throw new ArgumentNullException(nameof(signedTransaction));

            return Observable.FromAsync(async ar => await TransactionRoutesApi.AnnouncePartialTransactionAsync(signedTransaction));
        }

        /// <summary>
        /// Announces the cosignature transaction.
        /// </summary>
        /// <param name="signedTransaction">The signed transaction.</param>
        /// <returns>IObservable&lt;TransactionAnnounceResponse&gt;.</returns>
        /// <exception cref="ArgumentNullException">signedTransaction</exception>
        public IObservable<TransactionAnnounceResponse> AnnounceAggregateBondedCosignature(CosignatureSignedTransaction signedTransaction)
        {
            if (signedTransaction == null) throw new ArgumentNullException(nameof(signedTransaction));

            return Observable.FromAsync(async ar => await TransactionRoutesApi.AnnounceCosignatureTransactionAsync(signedTransaction));
        }

        /// <summary>
        /// Gets the transaction.
        /// </summary>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <returns>IObservable&lt;Transaction&gt;.</returns>
        /// <exception cref="ArgumentNullException">transactionId</exception>
        /// <exception cref="ArgumentException">Value cannot be null or empty. - transactionId</exception>
        public IObservable<Transaction> GetTransaction(string transactionId)
        {
            if (transactionId == null) throw new ArgumentNullException(nameof(transactionId));
            if (string.IsNullOrEmpty(transactionId)) throw new ArgumentException("Value cannot be null or empty.", nameof(transactionId));

            return Observable.FromAsync(async ar => await TransactionRoutesApi.GetTransactionAsync(transactionId));
        }

        /// <summary>
        /// Gets the transactions.
        /// </summary>
        /// <param name="transactionIds">The transaction identifier.</param>
        /// <returns>IObservable&lt;List&lt;Transaction&gt;&gt;.</returns>
        /// <exception cref="ArgumentNullException">transactionId</exception>
        /// <exception cref="ArgumentException">Collection contains one or more invalid ids.</exception>
        public IObservable<List<Transaction>> GetTransactions(List<string> transactionIds)
        {
            if (transactionIds == null) throw new ArgumentNullException(nameof(transactionIds));
            if (transactionIds.Any(hash => hash.Length != 24 || !Regex.IsMatch(hash, @"\A\b[0-9a-fA-F]+\b\Z"))) throw new ArgumentException("Collection contains one or more invalid ids.");

            return Observable.FromAsync(async ar => await TransactionRoutesApi.GetTransactionsAsync(JObject.FromObject(new
            {
                transactionIds = transactionIds.Select(i => i)
            })));
        }

        /// <summary>
        /// Gets the transaction status.
        /// </summary>
        /// <param name="hash">The hash.</param>
        /// <returns>IObservable&lt;TransactionStatusDTO&gt;.</returns>
        /// <exception cref="ArgumentException">Value cannot be null or empty. - hash
        /// or
        /// Invalid hash.</exception>
        public IObservable<TransactionStatus> GetTransactionStatus(string hash)
        {
            if (string.IsNullOrEmpty(hash)) throw new ArgumentException("Value cannot be null or empty.", nameof(hash));
            if (hash.Length != 64 || !Regex.IsMatch(hash, @"\A\b[0-9a-fA-F]+\b\Z")) throw new ArgumentException("Invalid hash.");        

            return Observable.FromAsync(async ar => await TransactionRoutesApi.GetTransactionStatusAsync(hash));
        }

        /// <summary>
        /// Gets the transaction statuses.
        /// </summary>
        /// <param name="hashes">The hashes.</param>
        /// <returns>IObservable&lt;List&lt;TransactionStatusDTO&gt;&gt;.</returns>
        /// <exception cref="ArgumentNullException">hashes</exception>
        /// <exception cref="ArgumentException">Collection contains one or more invalid hashes.</exception>
        public IObservable<List<TransactionStatus>> GetTransactionStatuses(List<string> hashes)
        {
            if (hashes == null) throw new ArgumentNullException(nameof(hashes));
            if (hashes.Any(hash => hash.Length != 64 || !Regex.IsMatch(hash, @"\A\b[0-9a-fA-F]+\b\Z"))) throw new ArgumentException("Collection contains one or more invalid hashes.");

            return Observable.FromAsync(async ar => await TransactionRoutesApi.GetTransactionsStatusesAsync(JObject.FromObject(new
            {
                hashes = hashes.Select(i => i)
            })));
        }
    }
}
