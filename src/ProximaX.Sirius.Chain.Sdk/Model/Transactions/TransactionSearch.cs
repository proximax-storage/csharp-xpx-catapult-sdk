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
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.Serialization;
using FlatBuffers;
using GuardNet;
using ProximaX.Sirius.Chain.Sdk.Buffers;
using ProximaX.Sirius.Chain.Sdk.Buffers.Schema;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions
{
    /// <summary>
    ///    Class of TransactionSearch
    /// </summary>
    public class TransactionSearch
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="transactions"></param>
        /// <param name="paginations"></param>
        public TransactionSearch(List<Transaction> transactions, Pagination paginations)
        {
            Transactions = transactions;
            Paginations = paginations;
        }

        /// <summary>
        ///     The Transactions
        /// </summary>
        public List<Transaction> Transactions { get; }

        /// <summary>
        ///     The Paginations
        /// </summary>
        public Pagination Paginations { get; }
    }
}