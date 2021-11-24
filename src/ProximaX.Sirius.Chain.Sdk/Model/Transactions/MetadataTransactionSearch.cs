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
using ProximaX.Sirius.Chain.Sdk.Model.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions
{
    public class MetadataTransactionSearch
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="entries"></param>
        /// <param name="paginations"></param>
        public MetadataTransactionSearch(List<MetadataEntry> entries, Pagination paginations)
        {
            Entries = entries;
            Paginations = paginations;
        }

        /// <summary>
        ///     The Entries
        /// </summary>
        public List<MetadataEntry> Entries { get; }

        /// <summary>
        ///     The Paginations
        /// </summary>
        public Pagination Paginations { get; }

        /// <summary>
        ///     ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return
                $"{nameof(Entries)}: {Entries}, {nameof(Paginations)}: {Paginations}";
        }
    }
}