// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="IAccountRepository.cs" company="Nem.io">   
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
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Transactions;

namespace io.nem2.sdk.Infrastructure.HttpRepositories
{
    /// <summary>
    /// Interface IAccountRepository
    /// </summary>
    interface IAccountRepository
    {
        /// <summary>
        /// Gets the account information.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns>IObservable&lt;AccountInfoDTO&gt;.</returns>
        IObservable<AccountInfo> GetAccountInfo(PublicAccount account);

        /// <summary>
        /// Get account information.
        /// </summary>
        /// <param name="accountIds">The account ids for which account information should be returned.</param>
        /// <returns>An IObservable of a List of AccountInfoDTO</returns>
        IObservable<List<AccountInfo>> GetAccountsInfo(PublicKeysDTO accountIds);

        /// <summary>
        /// Get account information.
        /// </summary>
        /// <param name="accountIds">The account ids for which account information should be returned.</param>
        /// <returns>An IObservable of a List of AccountInfoDTO</returns>
        IObservable<List<AccountInfo>> GetAccountsInfo(AddressesDTO accountIds);

        /// <summary>
        /// Get multisig account information.
        /// </summary>
        /// <param name="account">The account for which multisig info should be returned.</param>
        /// <returns>An IObservable of type MultisigEntryDTO.</returns>
        IObservable<MultisigAccountInfo> GetMultisigAccountInfo(PublicAccount account);

        /// <summary>
        /// Get multisig graph information
        /// </summary>
        /// <param name="account">The account for which multisig graph information should be returned.</param>
        /// <returns>An IObservable list of MultisigAccountGraphInfoDTO</returns>
        IObservable<MultisigAccountGraphInfo> GetMultisigAccountGraphInfo(PublicAccount account);

        /// <summary>
        /// Get incoming transactions.
        /// </summary>
        /// <param name="account">The account for which transactions should be returned.</param>
        /// <returns>IObservable list of type Transaction.</returns>
        IObservable<List<Transaction>> IncomingTransactions(PublicAccount account);

        /// <summary>
        /// Get incoming transactions.
        /// </summary>
        /// <param name="account">The account for which transactions should be returned.</param>
        /// <param name="query">The query parameters.</param>
        /// <returns>IObservable list of type Transaction.</returns>
        IObservable<List<Transaction>> IncomingTransactions(PublicAccount account, QueryParams query);

        /// <summary>
        /// Get incoming transactions.
        /// </summary>
        /// <param name="account">The account for which transactions should be returned.</param>
        /// <returns>IObservable list of type Transaction.</returns>
        IObservable<List<Transaction>> OutgoingTransactions(PublicAccount account);

        /// <summary>
        /// Get incoming transactions.
        /// </summary>
        /// <param name="account">The account for which transactions should be returned.</param>
        /// <param name="query">The query parameters.</param>
        /// <returns>IObservable list of type Transaction.</returns>
        IObservable<List<Transaction>> OutgoingTransactions(PublicAccount account, QueryParams query);

        /// <summary>
        /// Get unconfirmed transactions.
        /// </summary>
        /// <param name="account">The account for which transactions should be returned.</param>
        /// <returns>IObservable list of type Transaction.</returns>
        IObservable<List<Transaction>> UnconfirmedTransactions(PublicAccount account);

        /// <summary>
        /// Get unconfirmed transactions.
        /// </summary>
        /// <param name="account">The account for which transactions should be returned.</param>
        /// <param name="query">The query.</param>
        /// <returns>IObservable list of type Transaction.</returns>
        IObservable<List<Transaction>> UnconfirmedTransactions(PublicAccount account, QueryParams query);

        /// <summary>
        /// Get partial transactions.
        /// </summary>
        /// <param name="account">The account for which transactions should be returned.</param>
        /// <returns>IObservable list of type Transaction.</returns>
        IObservable<List<AggregateTransaction>> AggregateBondedTransactions(PublicAccount account);

        /// <summary>
        /// Get partial transactions.
        /// </summary>
        /// <param name="account">The account for which transactions should be returned.</param>
        /// <param name="query">The query parameters.</param>
        /// <returns>IObservable list of type Transaction.</returns>
        IObservable<List<AggregateTransaction>> AggregateBondedTransactions(PublicAccount account, QueryParams query);

        /// <summary>
        /// Get all transactions.
        /// </summary>
        /// <param name="account">The account for which transactions should be returned.</param>
        /// <returns>IObservable list of type Transaction.</returns>
        IObservable<List<Transaction>> Transactions(PublicAccount account);

        /// <summary>
        /// Get all transactions.
        /// </summary>
        /// <param name="account">The account for which transactions should be returned.</param>
        /// <param name="query">The query parameters.</param>
        /// <returns>IObservable list of type Transaction.</returns>
        IObservable<List<Transaction>> Transactions(PublicAccount account, QueryParams query);
    }
}
