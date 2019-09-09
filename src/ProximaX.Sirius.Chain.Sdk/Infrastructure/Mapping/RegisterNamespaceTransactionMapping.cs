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

using Newtonsoft.Json.Linq;
using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Namespaces;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Utils;
using System;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.Mapping
{
    public class RegisterNamespaceTransactionMapping : TransactionMapping
    {
        public new RegisterNamespaceTransaction Apply(JObject input)
        {
            return ToRegisterNamespaceTransaction(input, TransactionMappingHelper.CreateTransactionInfo(input));
        }

        private static RegisterNamespaceTransaction ToRegisterNamespaceTransaction(JObject tx, TransactionInfo txInfo)
        {
            var transaction = tx["transaction"].ToObject<JObject>();
            var version = transaction["version"];

            //Bug - It seems the dotnetcore does not 
            //understand the Integer.
            //The workaround it to double cast the version
            int versionValue;
            try
            {
                versionValue = (int)((uint)version);
            }
            catch (Exception)
            {
                versionValue = (int)version;
            }

            var network = TransactionMappingUtils.ExtractNetworkType(versionValue);
            var txVersion = TransactionMappingUtils.ExtractTransactionVersion(versionValue);
            var deadline = new Deadline(transaction["deadline"].ToObject<UInt64DTO>().ToUInt64());
            var maxFee = transaction["maxFee"]?.ToObject<UInt64DTO>().ToUInt64();
            var namespaceName = transaction["name"].ToObject<string>();
            var namespaceType = NamespaceTypeExtension.GetRawValue(transaction["namespaceType"].ToObject<int>());
            var namespaceId = new NamespaceId(transaction["namespaceId"].ToObject<UInt64DTO>().ToUInt64());
            var duration = namespaceType == NamespaceType.ROOT_NAMESPACE
                ? transaction["duration"].ToObject<UInt64DTO>().ToUInt64()
                : 0;
            var parentId = namespaceType == NamespaceType.SUB_NAMESPACE
                ? new NamespaceId(transaction["parentId"].ToObject<UInt64DTO>()
                    .ToUInt64())
                : null;
            var signature = transaction["signature"].ToObject<string>();
            var signer = new PublicAccount(transaction["signer"].ToObject<string>(), network);

            return new RegisterNamespaceTransaction(network, txVersion, deadline, maxFee,
                namespaceName, namespaceId, namespaceType, duration, parentId,
                signature, signer, txInfo);
        }
    }
}