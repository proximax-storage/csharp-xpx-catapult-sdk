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

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure
{
    /// <summary>
    ///     Class QueryParams
    /// </summary>
    public class QueryParams
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="QueryParams" /> class.
        /// </summary>
        /// <param name="page"></param>
        /// <param name="pageSize"></param>
        /// <param name="id"></param>
        /// <param name="order"></param>
        public QueryParams(int pageSize, string id = null,  Order order = Order.DESC, int page = 1)
        {
            PageSize = pageSize;
            Id = id;
            Order = order;
            Page = page;
        }

        /// <summary>
        ///     Page size
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        ///     Page size
        /// </summary>
        public int Page { get; }

        /// <summary>
        ///     Identifier
        /// </summary>
        public string Id { get; }

        /// <summary>
        ///     Sorted order
        /// </summary>
        public Order Order { get; }
    }

    /// <summary>
    ///     Order enums
    /// </summary>
    public enum Order
    {
        DESC,
        ASC
    }

    public enum MetadataSortingField
    {
        VALUE,
        VALUE_SIZE
    }
}