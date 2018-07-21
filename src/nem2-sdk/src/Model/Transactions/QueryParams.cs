// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="QueryParams.cs" company="Nem.io">   
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

namespace io.nem2.sdk.Infrastructure.Buffers.Model
{
    /// <summary>
    /// Class QueryParams.
    /// </summary>
    public class QueryParams
    {
        /// <summary>
        /// The page size
        /// </summary>
        private readonly int _pageSize;
        /// <summary>
        /// The identifier
        /// </summary>
        private readonly string _id;

        /// <summary>
        /// Initializes a new instance of the <see cref="QueryParams"/> class.
        /// </summary>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="id">The identifier.</param>
        public QueryParams(int pageSize, string id = null)
        {
            _pageSize = pageSize >= 10 && pageSize <= 100 ? pageSize : 10;
            _id = id;
        }

        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <returns>String.</returns>
        public string GetId() { return _id; }

        /// <summary>
        /// Gets the size of the page.
        /// </summary>
        /// <returns>System.Int32.</returns>
        public int GetPageSize() { return _pageSize; }

        /// <summary>
        /// To the URL.
        /// </summary>
        /// <returns>String.</returns>
        public string ToUrl()
        {
            return "?pageSize=" + _pageSize + (_id != null  ? "&id=" + _id : "");
        }
    }
}
