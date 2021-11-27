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

using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using System.Collections.Generic;

namespace ProximaX.Sirius.Chain.Sdk.Model.Metadata
{
    public class MetadataEntry
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="version"></param>
        /// <param name="compositeHash"></param>
        /// <param name="sourceAddress"></param>
        /// <param name="targetKey"></param>
        /// <param name="scopedMetadataKey"></param>
        /// <param name="targetId"></param>
        /// <param name="metadataType"></param>
        /// <param name="valueSize"></param>
        /// <param name="value"></param>
        /// <param name="id"></param>
        public MetadataEntry(int version, string compositeHash, Address sourceAddress, string targetKey, ulong scopedMetadataKey, ulong targetId, int metadataType, int valueSize, string value, string id)
        {
            Version = version;
            CompositeHash = compositeHash;
            SourceAddress = sourceAddress;
            TargetKey = targetKey;
            ScopedMetadataKey = scopedMetadataKey;
            TargetId = targetId;
            MetadataType = metadataType;
            ValueSize = valueSize;
            Value = value;
            Id = id;
        }

        /// <summary>
        ///     The version
        /// </summary>
        public int Version { get; }

        /// <summary>
        ///     The Composite Hash
        /// </summary>
        public string CompositeHash { get; }

        /// <summary>
        ///     The Source Address
        /// </summary>
        public Address SourceAddress { get; }

        /// <summary>
        ///     The Target Key
        /// </summary>
        public string TargetKey { get; }

        /// <summary>
        ///     The Scoped Metadata Key
        /// </summary>
        public ulong ScopedMetadataKey { get; }

        /// <summary>
        ///     The Target ID
        /// </summary>
        public ulong TargetId { get; }

        /// <summary>
        ///     The Metadata Type
        /// </summary>
        public int MetadataType { get; }

        /// <summary>
        ///     The Value Size
        /// </summary>
        public int ValueSize { get; }

        /// <summary>
        ///     The Value
        /// </summary>
        public string Value { get; }

        /// <summary>
        ///     The Id
        /// </summary>
        public string Id { get; }

        /// <summary>
        ///     ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return
                $"{nameof(Version)}: {Version}, {nameof(CompositeHash)}: {CompositeHash}, {nameof(SourceAddress)}: {SourceAddress}, {nameof(TargetKey)}: {TargetKey}, {nameof(ScopedMetadataKey)}: {ScopedMetadataKey}, {nameof(TargetId)}: {TargetId}, {nameof(MetadataType)}: {MetadataType},{nameof(ValueSize)}: {ValueSize},{nameof(Value)}: {Value},{nameof(Id)}: {Id}";
        }
    }
}