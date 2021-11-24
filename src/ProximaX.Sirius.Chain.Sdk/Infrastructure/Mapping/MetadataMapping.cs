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
using Newtonsoft.Json.Linq;
using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Metadata;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Namespaces;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.Mapping
{
    /// <summary>
    ///     Class MetadataMapping
    /// </summary>
    public class MetadataMapping
    {
        public Metadata Apply(JObject input)
        {
            var metadata = input["metadata"].ToObject<JObject>();
            var metadataType = MetadataTypeExtension.GetRawValue(metadata["metadataType"].ToObject<int>());
            var fields = metadata["fields"].ToObject<List<FieldDTO>>();
            var fieldList = fields.Select(f => new Field(f.Key, f.Value)).ToList();
            Metadata metadataInfo = null;

            switch (metadataType)
            {
                case MetadataType.ACCOUNT:
                    var address = Address.CreateFromHex(metadata["metadataId"].ToObject<string>());
                    metadataInfo = new AddressMetadata(fieldList, address);
                    break;

                case MetadataType.NAMESPACE:
                    var namespaceId = new NamespaceId(metadata["metadataId"].ToObject<UInt64DTO>().ToUInt64());
                    metadataInfo = new NamespaceMetadata(fieldList, namespaceId);
                    break;

                case MetadataType.MOSAIC:
                    var mosaicId = new MosaicId(metadata["metadataId"].ToObject<UInt64DTO>().ToUInt64());
                    metadataInfo = new MosaicMetadata(fieldList, mosaicId);
                    break;

                case MetadataType.NONE:
                    break;

                default:
                    throw new ArgumentOutOfRangeException();
            }

            return metadataInfo;
        }
    }
}