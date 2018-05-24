// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="ITransactionRepository.cs" company="Nem.io">   
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
using io.nem2.sdk.Infrastructure.Buffers.Model;
using io.nem2.sdk.Model.Transactions;

namespace io.nem2.sdk.Infrastructure.HttpRepositories
{
    /// <summary>
    /// Interface ITransactionRepository
    /// </summary>
    interface ITransactionRepository
    {
        /// <summary>
        /// Gets the transaction.
        /// </summary>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <returns>IObservable&lt;TransactionInfoDTO&gt;.</returns>
        IObservable<Transaction> GetTransaction(string transactionId);


        /// <summary>
        /// Gets the transactions.
        /// </summary>
        /// <param name="transactionId">The transaction identifier.</param>
        /// <returns>IObservable&lt;List&lt;TransactionInfoDTO&gt;&gt;.</returns>
        IObservable<List<Transaction>> GetTransactions(TransactionIds transactionId);


        /// <summary>
        /// Gets the transaction status.
        /// </summary>
        /// <param name="hash">The hash.</param>
        /// <returns>IObservable&lt;TransactionStatusDTO&gt;.</returns>
        IObservable<TransactionStatusDTO> GetTransactionStatus(string hash);


        /// <summary>
        /// Gets the transaction statuses.
        /// </summary>
        /// <param name="hashes">The hashes.</param>
        /// <returns>IObservable&lt;List&lt;TransactionStatusDTO&gt;&gt;.</returns>
        IObservable<List<TransactionStatusDTO>> GetTransactionStatuses(TransactionHashes hashes);


        /// <summary>
        /// Announces the transaction.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <returns>IObservable&lt;TransactionAnnounceResponse&gt;.</returns>
        IObservable<TransactionAnnounceResponse> Announce(SignedTransaction payload);


        /// <summary>
        /// Announces the partial transaction.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <returns>IObservable&lt;TransactionAnnounceResponse&gt;.</returns>
        IObservable<TransactionAnnounceResponse> AnnounceAggregateBonded(SignedTransaction payload);


        /// <summary>
        /// Announces the cosignature transaction.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <returns>IObservable&lt;TransactionAnnounceResponse&gt;.</returns>
        IObservable<TransactionAnnounceResponse> AnnounceAggregateBondedCosignature(CosignatureSignedTransactionDTO payload);
    }
}
