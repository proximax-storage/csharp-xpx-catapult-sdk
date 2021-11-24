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

using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure
{
    /// <summary>
    /// MetadataQueryParams class
    /// </summary>
    public class MetadataQueryParams
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="pageSize"></param>
        /// <param name="order"></param>
        /// <param name="pageNumber"></param>
        /// <param name="sourceAddress"></param>
        /// <param name="targetId"></param>
        /// <param name="targetKey"></param>
        /// <param name="scopedMetadataKey"></param>
        /// <param name="metadataType"></param>

        public MetadataQueryParams(int pageSize, Order order, int pageNumber, Address sourceAddress = null, string targetId = null, string targetKey = null, string scopedMetadataKey = null, MetadataType metadataType = MetadataType.NONE)
        {
            PageSize = pageSize;
            Order = order;
            PageNumber = pageNumber;
            SourceAddress = sourceAddress;
            TargetId = targetId;
            TargetKey = targetKey;
            ScopedMetadataKey = scopedMetadataKey;
            Type = metadataType;
        }

        /// <summary>
        ///     Page size
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        ///     Order
        /// </summary>
        public Order Order { get; }

        /// <summary>
        ///     Page Number
        /// </summary>
        public int PageNumber { get; }

        /// <summary>
        ///     Source Address
        /// </summary>
        public Address SourceAddress { get; }

        /// <summary>
        ///     Target Id
        /// </summary>
        public string TargetId { get; }

        /// <summary>
        ///     Target Key
        /// </summary>
        public string TargetKey { get; }

        /// <summary>
        ///    Scoped Metadata Key
        /// </summary>
        public string ScopedMetadataKey { get; }

        /// <summary>
        ///    Scoped Metadata Key
        /// </summary>
        public MetadataType Type { get; }
    }
}