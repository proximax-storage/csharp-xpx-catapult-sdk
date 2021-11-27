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
    public class AccountMetadataTransactionBuilder : TransactionBuilder<AccountMetadataTransactionBuilder, AccountMetadataTransaction>
    {
        public ulong ScopedKey { get; private set; }
        public PublicAccount TargetPublicKey { get; private set; }
        public ulong? TargetId { get; private set; }
        public string Value { get; private set; }
        public short ValueSizeDelta { get; private set; }
        public ushort ValueSize { get; private set; }

        //public IUInt64Id MetadataId;
        //public Address Address;
        //public IList<MetadataModification> Modifications;

        public AccountMetadataTransactionBuilder() : base(EntityType.ACCOUNT_METADATA_V2, EntityVersion.METADATA_ACCOUNT.GetValue())
        {
        }

        public override AccountMetadataTransaction Build()
        {
            var maxFee = MaxFee ?? GetMaxFeeCalculation(AccountMetadataTransaction.CalculatePayloadSize(ValueSize));

            return new AccountMetadataTransaction(NetworkType, Version, EntityType, Deadline, maxFee, ScopedKey, TargetPublicKey, Value, ValueSizeDelta, ValueSize);
        }

        protected override AccountMetadataTransactionBuilder Self()
        {
            return this;
        }

        public AccountMetadataTransactionBuilder SetScopedKey(ulong scopedKey)
        {
            ScopedKey = scopedKey;
            return Self();
        }

        public AccountMetadataTransactionBuilder SetTargetPublicKey(PublicAccount targetPublicKey)
        {
            TargetPublicKey = targetPublicKey;
            return Self();
        }

        public AccountMetadataTransactionBuilder SetValue(string value)
        {
            Value = value;
            return Self();
        }

        public AccountMetadataTransactionBuilder SetValueSizeDelta(short valueSizeDelta)
        {
            ValueSizeDelta = valueSizeDelta;
            return Self();
        }

        public AccountMetadataTransactionBuilder SetValueSize(ushort valueSize)
        {
            ValueSize = valueSize;
            return Self();
        }
    }
}