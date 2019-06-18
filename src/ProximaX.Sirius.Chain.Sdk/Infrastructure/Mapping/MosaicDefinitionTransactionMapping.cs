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
using System.Linq;
using Newtonsoft.Json.Linq;
using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.Mapping
{
    public class MosaicDefinitionTransactionMapping : TransactionMapping
    {
        public new MosaicDefinitionTransaction Apply(JObject input)
        {
            return ToMosaicDefinitionTransaction(input, TransactionMappingHelper.CreateTransactionInfo(input));
        }

        private static MosaicDefinitionTransaction ToMosaicDefinitionTransaction(JObject tx, TransactionInfo txInfo)
        {
            var transaction = tx["transaction"].ToObject<JObject>();

            var mosaicProperties = transaction["properties"];
            var flags = "00" + Convert.ToString((int) mosaicProperties[0].ExtractBigInteger("value"), 2);
            var bitMapFlags = flags.Substring(flags.Length - 3, 3);

            var properties = new MosaicProperties(
                bitMapFlags.ToCharArray()[2] == '1',
                bitMapFlags.ToCharArray()[1] == '1',
                bitMapFlags.ToCharArray()[0] == '1',
                (int) mosaicProperties[1].ExtractBigInteger("value"),
                mosaicProperties.ToList().Count == 3 ? mosaicProperties[2].ExtractBigInteger("value") : 0);

            var version = transaction["version"].ToObject<int>();
            var network = version.ExtractNetworkType();
            var deadline = new Deadline(transaction["deadline"].ToObject<UInt64DTO>().ToUInt64());
            var maxFee = transaction["maxFee"]?.ToObject<UInt64DTO>().ToUInt64() ?? 0;
            var mosaicNonce = TransactionMappingHelper.ExtractMosaicNonce(tx);
            var mosaicId = new MosaicId(transaction["mosaicId"].ToObject<UInt64DTO>().ToUInt64());
            var signature = transaction["signature"].ToObject<string>();
            var signer = new PublicAccount(transaction["signer"].ToObject<string>(), network);

            return new MosaicDefinitionTransaction(
                network, version, deadline, maxFee, mosaicNonce, mosaicId, properties, signature, signer, txInfo);
        }
    }
}