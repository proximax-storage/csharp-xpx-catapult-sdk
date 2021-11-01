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
    public class NamespaceMetadataTransactionBuilder : TransactionBuilder<NamespaceMetadataTransactionBuilder, NamespaceMetadataTransaction>
    {
        public ulong ScopedKey { get; private set; }
        public PublicAccount TargetPublicKey { get; private set; }
        public NamespaceId TargetId { get; private set; }
        public string Value { get; private set; }
        public short ValueSizeDelta { get; private set; }
        public ushort ValueSize { get; private set; }

        public NamespaceMetadataTransactionBuilder() : base(EntityType.NAMESPACE_METADATA_V2, EntityVersion.METADATA_NAMESPACE.GetValue())
        {
        }

        public override NamespaceMetadataTransaction Build()
        {
            var maxFee = MaxFee ?? GetMaxFeeCalculation(MosaicMetadataTransaction.CalculatePayloadSize(ValueSize));

            return new NamespaceMetadataTransaction(NetworkType, Version, EntityType, Deadline, maxFee, ScopedKey, TargetPublicKey, TargetId, Value, ValueSizeDelta, ValueSize);
        }

        protected override NamespaceMetadataTransactionBuilder Self()
        {
            return this;
        }

        public NamespaceMetadataTransactionBuilder SetScopedKey(ulong scopedKey)
        {
            ScopedKey = scopedKey;
            return Self();
        }

        public NamespaceMetadataTransactionBuilder SetTargetPublicKey(PublicAccount targetPublicKey)
        {
            TargetPublicKey = targetPublicKey;
            return Self();
        }

        public NamespaceMetadataTransactionBuilder SetTargetId(NamespaceId targetId)
        {
            TargetId = targetId;
            return Self();
        }

        public NamespaceMetadataTransactionBuilder SetValue(string value)
        {
            Value = value;
            return Self();
        }

        public NamespaceMetadataTransactionBuilder SetValueSizeDelta(short valueSizeDelta)
        {
            ValueSizeDelta = valueSizeDelta;
            return Self();
        }

        public NamespaceMetadataTransactionBuilder SetValueSize(ushort valueSize)
        {
            ValueSize = valueSize;
            return Self();
        }
    }
}