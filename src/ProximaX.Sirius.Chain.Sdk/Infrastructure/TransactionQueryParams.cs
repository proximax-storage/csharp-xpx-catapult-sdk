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

using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure
{
    /// <summary>
    ///     TransactionQueryParams
    /// </summary>
    public class TransactionQueryParams
    {
        public TransactionQueryParams(int pageNumber, EntityType type, bool embedded, TransactionSortingField sortField, int toHeight, int fromHeight, int height, string signerPublicKey, string recipientAddress, string address, int pageSize, string id = null, Order order = Order.DESC)
        {
            PageSize = pageSize;
            Id = id;
            Order = order;
            PageNumber = pageNumber;
            Type = type;
            Embedded = embedded;
            SortField = sortField;
            ToHeight = toHeight;
            FromHeight = fromHeight;
            Height = height;
            SignerPublicKey = signerPublicKey;
            RecipientAddress = recipientAddress;
            Address = address;
        }

        public int PageSize { get; }

        /// <summary>
        ///     Identifier
        /// </summary>
        public string Id { get; }

        /// <summary>
        ///     Sorted order
        /// </summary>
        public Order Order { get; }

        public int PageNumber { get; }

        /// <summary>
        ///     Type
        /// </summary>
        public EntityType Type { get; }

        /// <summary>
        ///     Embedded
        /// </summary>
        public bool Embedded { get; }

        /// <summary>
        ///     Sort Field
        /// </summary>
        public TransactionSortingField SortField { get; }

        /// <summary>
        ///     To Height
        /// </summary>
        public int ToHeight { get; }

        /// <summary>
        ///     From Height
        /// </summary>
        public int FromHeight { get; }

        /// <summary>
        ///    Height
        /// </summary>
        public int Height { get; }

        /// <summary>
        ///     Signer Public Key
        /// </summary>
        public string SignerPublicKey { get; }

        /// <summary>
        ///     Recipient Address
        /// </summary>
        public string RecipientAddress { get; }

        /// <summary>
        ///     Address
        /// </summary>
        public string Address { get; }
    }

    public enum TransactionSortingField
    {
        BLOCK
    }
}