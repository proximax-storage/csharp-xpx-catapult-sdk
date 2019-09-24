
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
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Metadata;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Namespaces;
using System.Collections.Generic;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions.Builders
{
    public class ModifyMetadataTransactionBuilder : TransactionBuilder<ModifyMetadataTransactionBuilder, ModifyMetadataTransaction>
    {
        public MetadataType MetadataType { get; private set; }
        public IUInt64Id MetadataId;
        public Address Address;
        public IList<MetadataModification> Modifications;

        public ModifyMetadataTransactionBuilder(EntityType entityType) : base(entityType, EntityVersion.MODIFY_METADATA.GetValue())
        {
            Modifications = new List<MetadataModification>();
        }

        public override ModifyMetadataTransaction Build()
        {
            var maxFee = MaxFee ?? GetMaxFeeCalculation(ModifyMetadataTransaction.CalculatePayloadSize(Address!=null, Modifications));

            return new ModifyMetadataTransaction(NetworkType, Version, EntityType, Deadline, maxFee, MetadataType, MetadataId.Id, Address, Modifications);
        }

        protected override ModifyMetadataTransactionBuilder Self()
        {
            return this;
        }

        public ModifyMetadataTransactionBuilder SetMetadataId(IUInt64Id metadataId)
        {
            MetadataId = metadataId;
            return Self();
        }

        public ModifyMetadataTransactionBuilder SetMetadataId(Address address)
        {
            Address = address;
            return Self();
        }

        public ModifyMetadataTransactionBuilder SetMetadataType(MetadataType type)
        {
            MetadataType = type;
            return Self();
        }

        public ModifyMetadataTransactionBuilder SetModifications(List<MetadataModification> modifications)
        {
            Modifications = modifications;
            return Self();
        }

        public ModifyMetadataTransactionBuilder ForAddress(Address address)
        {
            return SetEntityType(EntityType.MODIFY_ADDRESS_METADATA).SetMetadataType(MetadataType.ADDRESS).SetMetadataId(address);
        }

        public ModifyMetadataTransactionBuilder ForMosaic(MosaicId mosaicId)
        {
            return SetEntityType(EntityType.MODIFY_MOSAIC_METADATA).SetMetadataType(MetadataType.MOSAIC).SetMetadataId(mosaicId);
        }

        public ModifyMetadataTransactionBuilder ForNamespace(NamespaceId namespaceId)
        {
            return SetEntityType(EntityType.MODIFY_NAMESPACE_METADATA).SetMetadataType(MetadataType.NAMESPACE)
                  .SetMetadataId(namespaceId);
        }
    }
}
