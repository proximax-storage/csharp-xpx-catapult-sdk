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

using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.Mapping
{
    internal class ModifyMultisigAccountTransactionMapping : TransactionMapping
    {
        public new ModifyMultisigAccountTransaction Apply(JObject input)
        {
            return ToModifyMultisigAccountTransaction(input, TransactionMappingHelper.CreateTransactionInfo(input));
        }

        private static ModifyMultisigAccountTransaction ToModifyMultisigAccountTransaction(JObject tx,
            TransactionInfo txInfo)
        {
            var transaction = tx["transaction"].ToObject<JObject>();
            var version = transaction["version"].ToObject<int>();
            var network = version.ExtractNetworkType();
            var deadline = new Deadline(transaction["deadline"].ToObject<UInt64DTO>().ToUInt64());
            var maxFee = transaction["maxFee"]?.ToObject<UInt64DTO>().ToUInt64();
            var signature = transaction["signature"].ToObject<string>();
            var signer = new PublicAccount(transaction["signer"].ToObject<string>(), network);


            var minApproved = transaction["minApprovalDelta"].ToObject<int>();
            var minRemoved = transaction["minRemovalDelta"].ToObject<int>();

            var modifications = transaction["modifications"];
            var modificationList = modifications == null
                ? new List<MultisigCosignatoryModification>()
                : modifications.Select(e =>
                    new MultisigCosignatoryModification(
                        MultisigCosignatoryModificationTypeExtension.GetRawValue(e["type"].ToObject<int>()),
                        new PublicAccount(e["cosignatoryPublicKey"].ToObject<string>(), network))).ToList();

            return new ModifyMultisigAccountTransaction(
                network, version, deadline, maxFee, minApproved,
                minRemoved, modificationList,
                signature, signer, txInfo);
        }
    }
}