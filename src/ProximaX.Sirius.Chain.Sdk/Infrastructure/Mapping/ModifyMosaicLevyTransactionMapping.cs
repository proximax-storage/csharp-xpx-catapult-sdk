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
using Newtonsoft.Json.Linq;
using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.Mapping
{
    /// <summary>
    /// Class of ModifyMosaicLevyTransactionMapping
    /// </summary>
    public class ModifyMosaicLevyTransactionMapping : TransactionMapping
    {
        /// <summary>
        /// Applies the AccountLinkTransactionMapping
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public new ModifyMosaicLevyTransaction Apply(JObject input)
        {
            return ToMosaicModifyLevyTransaction(input, TransactionMappingHelper.CreateTransactionInfo(input));
        }

        /// <summary>
        /// Converts to AccountLinkTransactionMapping
        /// </summary>
        /// <param name="tx"></param>
        /// <param name="txInfo"></param>
        /// <returns></returns>
        private static ModifyMosaicLevyTransaction ToMosaicModifyLevyTransaction(JObject tx, TransactionInfo txInfo)
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
            var signature = transaction["signature"].ToObject<string>();
            var signer = new PublicAccount(transaction["signer"].ToObject<string>(), network);

            var mosaicId = new MosaicId(transaction["mosaicId"].ToObject<UInt64DTO>().ToUInt64());
            var levy = transaction["levy"].ToObject<JObject>();
            var fee = levy["fee"].ToObject<UInt64DTO>().ToUInt64();
            var type = MosaicLevyTypeExtension.GetRawValue(levy["type"].ToObject<int>());
            var levy_mosaicId = new MosaicId(levy["mosaicId"].ToObject<UInt64DTO>().ToUInt64());
            var recipient = levy["recipient"].ToObject<string>();

            var modifyMosaicLevyTransaction = new ModifyMosaicLevyTransaction(network, txVersion, deadline,
                 mosaicId, new MosaicLevy(type, Recipient.From(Address.CreateFromHex(recipient)), levy_mosaicId, fee), maxFee, signature, signer, txInfo);

            return modifyMosaicLevyTransaction;
        }
    }
}