﻿// Copyright 2021 ProximaX
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
using System.Text;
using System.Threading.Tasks;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions
{
    public class Pagination
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="totalEntries"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <param name="totalPages"></param>

        public Pagination(int totalEntries, int pageNumber, int pageSize, int totalPages)
        {
            TotalEntries = totalEntries;
            PageNumber = pageNumber;
            PageSize = pageSize;
            TotalPages = totalPages;
        }

        /// <summary>
        ///     The total entries
        /// </summary>
        public int TotalEntries { get; }

        /// <summary>
        ///     The page number
        /// </summary>
        public int PageNumber { get; }

        /// <summary>
        ///     The page size
        /// </summary>
        public int PageSize { get; }

        /// <summary>
        ///     The total pages
        /// </summary>
        public int TotalPages { get; }
    }
}