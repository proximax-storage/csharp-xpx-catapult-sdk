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

using System;
using Newtonsoft.Json.Linq;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.Mapping
{
    public class AccountLinkTransactionMapping : TransactionMapping
    {
        public new AccountLinkTransaction Apply(JObject input)
        {
            return ToAccountLinkTransaction(input, TransactionMappingHelper.CreateTransactionInfo(input));
        }

        private static AccountLinkTransaction ToAccountLinkTransaction(JObject tx, TransactionInfo txInfo)
        {
            throw new NotImplementedException();
        }
    }
}