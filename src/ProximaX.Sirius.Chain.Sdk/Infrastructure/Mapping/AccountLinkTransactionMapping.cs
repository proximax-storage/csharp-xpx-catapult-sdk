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
using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.Mapping
{
    /// <summary>
    /// AccountLinkTransactionMapping Class
    /// </summary>
    public class AccountLinkTransactionMapping : TransactionMapping
    {
        /// <summary>
        /// Applies the AccountLinkTransactionMapping
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public new AccountLinkTransaction Apply(JObject input)
        {
            return ToAccountLinkTransaction(input, TransactionMappingHelper.CreateTransactionInfo(input));
        }

        /// <summary>
        /// Converts to AccountLinkTransactionMapping
        /// </summary>
        /// <param name="tx"></param>
        /// <param name="txInfo"></param>
        /// <returns></returns>
        private static AccountLinkTransaction ToAccountLinkTransaction(JObject tx, TransactionInfo txInfo)
        {
            var transaction = tx["transaction"].ToObject<JObject>();
            var version = transaction["version"].ToObject<int>();
            var network = version.ExtractNetworkType();
            var deadline = new Deadline(transaction["deadline"].ToObject<UInt64DTO>().ToUInt64());
            var maxFee = transaction["maxFee"]?.ToObject<UInt64DTO>().ToUInt64();
            var signature = transaction["signature"].ToObject<string>();
            var signer = new PublicAccount(transaction["signer"].ToObject<string>(), network);
            var remoteAccount = new PublicAccount(transaction["remoteAccountKey"].ToObject<string>(), network);
            var linkAccount = AccountLinkActionExtension.GetRawValue(transaction["linkAction"].ToObject<int>());
            return new AccountLinkTransaction(network, version, deadline, maxFee, TransactionType.LINK_ACCOUNT,
              remoteAccount, linkAccount,signature,signer);
        }
    }
}