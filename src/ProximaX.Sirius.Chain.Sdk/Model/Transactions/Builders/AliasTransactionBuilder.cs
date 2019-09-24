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
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Namespaces;
using System;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions.Builders
{
    public class AliasTransactionBuilder : TransactionBuilder<AliasTransactionBuilder, AliasTransaction>
    {

        public MosaicId MosaicId { get; private set; }
        public Address Address { get; private set; }
        public NamespaceId NamespaceId { get; private set; }
        public AliasActionType AliasActionType { get; private set; }

        private AliasTransactionBuilder(EntityType entityType, int version) : base(entityType, version)
        {
        }

        public static AliasTransactionBuilder CreateForAddress()
        {
            return new AliasTransactionBuilder(EntityType.ADDRESS_ALIAS, EntityVersion.ADDRESS_ALIAS.GetValue());

        }

        public static AliasTransactionBuilder CreateForMosaic()
        {
            return new AliasTransactionBuilder(EntityType.MOSAIC_ALIAS, EntityVersion.MOSAIC_ALIAS.GetValue());

        }
        public AliasTransactionBuilder SetMosaicId(MosaicId mosaicId)
        {
            if (EntityType != EntityType.MOSAIC_ALIAS)
            {
                throw new Exception("Mosaic ID alias can be created only by mosaic alias builder");
            }

            MosaicId = mosaicId;
            return Self();
        }

        public AliasTransactionBuilder SetAddress(Address address)
        {
            if (EntityType != EntityType.ADDRESS_ALIAS)
            {
                throw new Exception("Address  alias can be created only by address alias builder");
            }

            Address = address;
            return Self();
        }

        public AliasTransactionBuilder SetNamespaceId(NamespaceId namespaceId)
        {

            NamespaceId = namespaceId;
            return Self();
        }

        public AliasTransactionBuilder SetAliasAction(AliasActionType aliasActionType)
        {
            AliasActionType = aliasActionType;
            return Self();
        }

        public override AliasTransaction Build()
        {
            var maxFee = MaxFee ?? GetMaxFeeCalculation(AliasTransaction.CalculatePayloadSize(Address!=null));

            return new AliasTransaction(NetworkType, Version, Deadline, maxFee, EntityType, NamespaceId, AliasActionType, MosaicId
                , Address);
        }

        protected override AliasTransactionBuilder Self()
        {
            return this;
        }

        public AliasTransactionBuilder Link(MosaicId mosaicId)
        {
            return SetAliasAction(AliasActionType.LINK).SetMosaicId(mosaicId);
        }

        public AliasTransactionBuilder Link(Address address)
        {
            return SetAliasAction(AliasActionType.LINK).SetAddress(address);
        }

        public AliasTransactionBuilder UnLink(MosaicId mosaicId)
        {
            return SetAliasAction(AliasActionType.UNLINK).SetMosaicId(mosaicId);
        }

        public AliasTransactionBuilder UnLink(Address address)
        {
            return SetAliasAction(AliasActionType.UNLINK).SetAddress(address);
        }
    }
}
