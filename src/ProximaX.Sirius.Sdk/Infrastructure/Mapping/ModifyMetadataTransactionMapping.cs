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
using ProximaX.Sirius.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Sdk.Model.Accounts;
using ProximaX.Sirius.Sdk.Model.Blockchain;
using ProximaX.Sirius.Sdk.Model.Metadata;
using ProximaX.Sirius.Sdk.Model.Transactions;
using ProximaX.Sirius.Sdk.Utils;

namespace ProximaX.Sirius.Sdk.Infrastructure.Mapping
{
    public class ModifyMetadataTransactionMapping : TransactionMapping
    {
        public new ModifyMetadataTransaction Apply(JObject input)
        {
            return ToModifyMetadataTransaction(input, TransactionMappingHelper.CreateTransactionInfo(input));
        }

        private static ModifyMetadataTransaction ToModifyMetadataTransaction(JObject tx, TransactionInfo txInfo)
        {
            var transaction = tx["transaction"].ToObject<JObject>();
            var version = transaction["version"].ToObject<int>();
            var network = version.ExtractNetworkType();
            var deadline = new Deadline(transaction["deadline"].ToObject<UInt64DTO>().ToUInt64());
            var maxFee = transaction["maxFee"]?.ToObject<UInt64DTO>().ToUInt64();
            var signature = transaction["signature"].ToObject<string>();
            var signer = new PublicAccount(transaction["signer"].ToObject<string>(), network);
            var type = TransactionTypeExtension.GetRawValue(transaction["type"].ToObject<int>());
            var metaType = MetadataTypeExtension.GetRawValue(transaction["metadataType"].ToObject<int>());


            var modifications = transaction["modifications"];
            var modificationList = modifications == null
                ? new List<MetadataModification>()
                : modifications.Select(e =>
                {
                    var modificationType =
                        MetadataModificationTypeExtension.GetRawValue(e["modificationType"].ToObject<int>());
                    var key = e["key"].ToObject<string>();
                    var value = e["value"]?.ToObject<string>();
                    MetadataModification metadataModification = null;
                    switch (modificationType)
                    {
                        case MetadataModificationType.ADD:
                            metadataModification = MetadataModification.Add(key, value);
                            break;
                        case MetadataModificationType.REMOVE:
                            metadataModification = MetadataModification.Remove(key);
                            break;
                    }

                    return metadataModification;
                }).ToList();

            ModifyMetadataTransaction modifyMetadataTransaction = null;

            switch (type)
            {
                case TransactionType.MODIFY_ADDRESS_METADATA:
                    var address = Address.CreateFromHex(transaction["metadataId"].ToObject<string>());
                    modifyMetadataTransaction = new ModifyMetadataTransaction(
                        network, version, type, deadline, maxFee, metaType, null, address,
                        modificationList, signature, signer, txInfo);
                    break;

                case TransactionType.MODIFY_MOSAIC_METADATA:
                    var mosaicId = transaction["metadataId"].ToObject<UInt64DTO>().ToUInt64();
                    modifyMetadataTransaction = new ModifyMetadataTransaction(
                        network, version, type, deadline, maxFee, metaType, mosaicId, null,
                        modificationList, signature, signer, txInfo);
                    break;
                case TransactionType.MODIFY_NAMESPACE_METADATA:
                    var namespaceId = transaction["metadataId"].ToObject<UInt64DTO>().ToUInt64();
                    modifyMetadataTransaction = new ModifyMetadataTransaction(
                        network, version, type, deadline, maxFee, metaType, namespaceId, null,
                        modificationList, signature, signer, txInfo);
                    break;
            }

            return modifyMetadataTransaction;
        }
    }
}