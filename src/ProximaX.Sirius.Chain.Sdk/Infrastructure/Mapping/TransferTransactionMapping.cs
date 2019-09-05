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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json.Linq;
using ProximaX.Sirius.Chain.Sdk.Crypto.Core.Chaso.NaCl;
using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions.Messages;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.Mapping
{
    public class TransferTransactionMapping : TransactionMapping
    {
        public new TransferTransaction Apply(JObject input)
        {
            return ToTransferTransaction(input, TransactionMappingHelper.CreateTransactionInfo(input));
        }


        private static TransferTransaction ToTransferTransaction(JObject tx, TransactionInfo txInfo)
        {
            var transaction = tx["transaction"].ToObject<JObject>();
            //Bug - It seems the dotnetcore does not 
            //understand the Integer.
            //The workaround it to double cast the version
            int versionValue = -1879048189;
            var version = transaction["version"];

            try
            {
                versionValue = (int)((uint)version);
            } catch (Exception)
            {
                versionValue = (int)version;
            }

            var network = TransactionMappingUtils.ExtractNetworkType(versionValue);
            var deadline = transaction["deadline"].ToObject<UInt64DTO>().ToUInt64();
            var maxFee = transaction["maxFee"]?.ToObject<UInt64DTO>().ToUInt64();
            var recipient = transaction["recipient"]?.ToObject<string>();
            var mosaics = transaction["mosaics"].ToObject<List<MosaicDTO>>();
            var message = transaction["message"].ToObject<JObject>();
            var signature = transaction["signature"].ToObject<string>();
            var signer = transaction["signer"].ToObject<string>();
            return new TransferTransaction(network,
                TransactionMappingUtils.ExtractTransactionVersion(versionValue),
                new Deadline(deadline),
                maxFee,
                Recipient.From(Address.CreateFromHex(recipient)),
                mosaics.Select(m => new Mosaic(new MosaicId(m.Id.ToUInt64()), m.Amount.ToUInt64())).ToList(),
                GetMessage(message),
                signature,
                new PublicAccount(signer, network),
                txInfo
            );
        }

        private static IMessage GetMessage(JObject msg)
        {
            var msgType = msg["type"].ToObject<int>();
            var payload = msg["payload"].ToObject<string>().FromHex();

            switch (MessageTypeExtension.GetRawValue(msgType))
            {
                case MessageType.PLAIN_MESSAGE:
                    return PlainMessage.Create(Encoding.UTF8.GetString(payload));
                case MessageType.SECURED_MESSAGE:
                    return SecureMessage.CreateFromEncodedPayload(payload);
                default:
                    return EmptyMessage.Create();
            }
        }
    }
}