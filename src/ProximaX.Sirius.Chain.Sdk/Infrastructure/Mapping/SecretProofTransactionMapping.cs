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
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Utils;
using System;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.Mapping
{
    /// <summary>
    ///     Class SecretProofTransactionMapping
    /// </summary>
    public class SecretProofTransactionMapping : TransactionMapping
    {
        /// <summary>
        ///     Apply SecretProofTransaction
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public new SecretProofTransaction Apply(JObject input)
        {
            return ToSecretProofTransaction(input, TransactionMappingHelper.CreateTransactionInfo(input));
        }

        /// <summary>
        ///     ToSecretProofTransaction
        /// </summary>
        /// <param name="tx"></param>
        /// <param name="txInfo"></param>
        /// <returns></returns>
        private static SecretProofTransaction ToSecretProofTransaction(JObject tx, TransactionInfo txInfo)
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
            var recipient = Recipient.From(Address.CreateFromHex(transaction["recipient"].ToObject<string>()));
            var hashType = HashTypeExtension.GetRawValue(transaction["hashAlgorithm"].ToObject<int>());
            var secret = transaction["secret"].ToObject<string>();
            var proof = transaction["proof"].ToObject<string>();

            return new SecretProofTransaction(
                network, txVersion, deadline, maxFee, hashType, recipient,secret,
                proof, signature, signer, txInfo);
        }
    }
}