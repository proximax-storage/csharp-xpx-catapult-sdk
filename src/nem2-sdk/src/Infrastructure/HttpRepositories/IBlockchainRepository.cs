// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-31-2018
// ***********************************************************************
// <copyright file="IBlockchainRepository.cs" company="Nem.io">
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
using io.nem2.sdk.Model.Blockchain;
using io.nem2.sdk.Model.Transactions;

namespace io.nem2.sdk.Infrastructure.HttpRepositories
{
    /// <summary>
    /// Interface IBlockchainRepository
    /// </summary>
    interface IBlockchainRepository
    {

        /// <summary>
        /// Gets the blockchain score.
        /// </summary>
        /// <returns>IObservable&lt;BlockchainScore&gt;.</returns>
        IObservable<ulong> GetBlockchainScore();

        /// <summary>
        /// Gets the chain height.
        /// </summary>
        /// <returns>An IObservable of ChainHeightDTO</returns>
        IObservable<ulong> GetBlockchainHeight();

        /// <summary>
        /// Gets a BlockInfo for a given block height.
        /// </summary>
        /// <param name="height">The height.</param>
        /// <returns>An IObservable of BlockInfoDTO</returns>
        IObservable<BlockInfo> GetBlockByHeight(ulong height);

        /// <summary>
        /// Gets the block by height with limit.
        /// </summary>
        /// <param name="height">The height.</param>
        /// <param name="limit">The limit.</param>
        /// <returns>IObservable&lt;List&lt;BlockInfoDTO&gt;&gt;.</returns>
        IObservable<List<BlockInfo>> GetBlockByHeightWithLimit(ulong height, int limit = 100);

        /// <summary>
        /// Gets the block transactions.
        /// </summary>
        /// <param name="height">The height.</param>
        /// <returns>IObservable&lt;List&lt;TransactionInfoDTO&gt;&gt;.</returns>
        IObservable<List<Transaction>> GetBlockTransactions(ulong height);

        /// <summary>
        /// Gets the block transactions.
        /// </summary>
        /// <param name="height">The height.</param>
        /// <param name="query">The query.</param>
        /// <returns>IObservable&lt;List&lt;TransactionInfoDTO&gt;&gt;.</returns>
        IObservable<List<Transaction>> GetBlockTransactions(ulong height, QueryParams query);

        /// <summary>
        /// Gets the blockchain chain storage information for the chain.
        /// </summary>
        /// <returns>An IObservable of BlockchainStorageInfo</returns>
        IObservable<BlockchainStorageInfo> GetBlockchainDiagnosticStorage();

        /// <summary>
        /// Gets the blockchain diagnostic blocks with limit.
        /// </summary>
        /// <param name="height">The height.</param>
        /// <param name="limit">The limit.</param>
        /// <returns>IObservable&lt;List&lt;BlockInfoDTO&gt;&gt;.</returns>
        IObservable<List<BlockInfo>> GetBlockchainDiagnosticBlocksWithLimit(ulong height, int limit = 100);
    }
}
