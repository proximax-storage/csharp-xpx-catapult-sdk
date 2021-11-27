﻿// Copyright 2019 ProximaX
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

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions
{
    /// <summary>
    ///    Class of TransactionInfo
    /// </summary>
    public class TransactionInfo
    {
        /// <summary>
        ///     Create transaction info object for transfer transaction
        /// </summary>
        /// <param name="height">Block height in which the transaction was included.</param>
        /// <param name="index">The transaction index.</param>
        /// <param name="id">The transaction id.</param>
        /// <param name="hash">The transaction hash</param>
        /// <param name="merkleComponentHash">The merkle component hash </param>
        /// <param name="aggregateHash">The aggregate hash</param>
        /// <param name="aggregateId">The aggregate id</param>
        public TransactionInfo(ulong height, int? index, string id, string hash, string merkleComponentHash, string aggregateHash, string aggregateId, string uniqueAggregateHash)
        {
            Height = height;
            Index = index;
            Id = id;
            Hash = hash;
            MerkleComponentHash = merkleComponentHash;
            AggregateHash = aggregateHash;
            AggregateId = aggregateId;
            UniqueAggregateHash = uniqueAggregateHash;
        }

        /// <summary>
        ///     Returns block height in which the transaction was included.
        /// </summary>
        /// <value>The height.</value>
        public ulong Height { get; }

        /// <summary>
        ///     Returns index representing either transaction index/position within block or within an aggregate transaction.
        /// </summary>
        /// <value>The index.</value>
        public int? Index { get; }

        /// <summary>
        ///     Returns transaction id.
        /// </summary>
        /// <value>The identifier.</value>
        public string Id { get; }

        /// <summary>
        ///     Returns transaction hash.
        /// </summary>
        /// <value>The hash.</value>
        public string Hash { get; }

        /// <summary>
        ///     Returns transaction merkle component hash.
        /// </summary>
        /// <value>The merkle component hash.</value>
        public string MerkleComponentHash { get; }

        /// <summary>
        ///     Returns hash of the aggregate transaction.
        /// </summary>
        /// <value>The aggregate hash.</value>
        public string AggregateHash { get; }

        /// <summary>
        ///     Returns id of the aggregate transaction.
        /// </summary>
        /// <value>The aggregate identifier.</value>
        public string AggregateId { get; }

        /// <summary>
        ///     Returns UniqueAggregateHash of the aggregate transaction.
        /// </summary>
        /// <value>The Unique Aggregate Hash.</value>
        public string UniqueAggregateHash { get; }

        /// <summary>
        ///     Create transaction info object for aggregate transaction inner transaction.
        /// </summary>
        /// <param name="height">Block height in which the transaction was included.</param>
        /// <param name="index">The transaction index.</param>
        /// <param name="id">The transaction id.</param>
        /// <param name="aggregateHash">The hash of the aggregate transaction.</param>
        /// <param name="aggregateId">The id of the aggregate transaction.</param>
        /// <returns>TransactionInfo.</returns>
        public static TransactionInfo CreateAggregate(ulong height, int? index, string id, string aggregateHash, string aggregateId, string uniqueAggregateHash)
        {
            return new TransactionInfo(height, index, id, null, null, aggregateHash, aggregateId, uniqueAggregateHash);
        }

        /// <summary>
        ///     Create transaction info object for a transaction.
        /// </summary>
        /// <param name="height">Block height in which the transaction was included.</param>
        /// <param name="index">The transaction index.</param>
        /// <param name="id">The transaction id.</param>
        /// <param name="hash">The transaction hash.</param>
        /// <param name="merkleComponentHash">The transaction merkle component hash.</param>
        /// <returns>TransactionInfo.</returns>
        public static TransactionInfo Create(ulong height, int? index, string id, string hash,
            string merkleComponentHash)
        {
            return new TransactionInfo(height, index, id, hash, merkleComponentHash, null, null, null);
        }

        /// <summary>
        ///     Create transaction info retrieved by listener.
        /// </summary>
        /// <param name="height">Block height in which the transaction was included.</param>
        /// <param name="hash">The transaction merkle component hash.</param>
        /// <param name="merkleComponentHash">The merkle component hash.</param>
        /// <returns>TransactionInfo.</returns>
        public static TransactionInfo Create(ulong height, string hash, string merkleComponentHash)
        {
            return new TransactionInfo(height, null, null, hash, merkleComponentHash, null, null, null);
        }
    }
}