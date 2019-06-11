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

using ProximaX.Sirius.Sdk.Model.Accounts;

namespace ProximaX.Sirius.Sdk.Model.Mosaics
{
    /// <summary>
    ///     MosaicInfo
    /// </summary>
    public class MosaicInfo
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="metaId">The meta id</param>
        /// <param name="mosaicId">The mosaic identifier</param>
        /// <param name="supply">The mosaic supply</param>
        /// <param name="height">The mosaic height</param>
        /// <param name="owner">The owner public account</param>
        /// <param name="revision">The revision</param>
        /// <param name="levy">The network levy</param>
        /// <param name="mosaicProperties"></param>
        public MosaicInfo(string metaId, MosaicId mosaicId, ulong supply, ulong height, PublicAccount owner,
            int? revision, MosaicProperties mosaicProperties, object levy = null)
        {
            MetaId = metaId;
            MosaicId = mosaicId;
            Supply = supply;
            Height = height;
            Owner = owner;
            Revision = revision;
            Levy = levy;
            MosaicProperties = mosaicProperties;
        }

        /// <summary>
        ///     The mosaic meta id
        /// </summary>
        public string MetaId { get; }

        /// <summary>
        ///     The mosaic id
        /// </summary>
        public MosaicId MosaicId { get; }

        /// <summary>
        ///     The mosaic supply
        /// </summary>
        public ulong Supply { get; }

        /// <summary>
        ///     The mosaic height
        /// </summary>
        public ulong Height { get; }

        /// <summary>
        ///     The owner account
        /// </summary>
        public PublicAccount Owner { get; }

        /// <summary>
        ///     The revision
        /// </summary>
        public int? Revision { get; }

        /// <summary>
        ///     The mosaic level
        /// </summary>
        public object Levy { get; }

        /// <summary>
        ///     The mosaic properties
        /// </summary>
        public MosaicProperties MosaicProperties { get; }

        /// <summary>
        ///     IsSupplyMutable
        /// </summary>
        public bool IsSupplyMutable => MosaicProperties.IsSupplyMutable;

        /// <summary>
        ///     IsTransferable
        /// </summary>
        public bool IsTransferable => MosaicProperties.IsTransferable;

        /// <summary>
        ///     Divisibility
        /// </summary>
        public int Divisibility => MosaicProperties.Divisibility;

        /// <summary>
        ///     Duration
        /// </summary>
        public ulong Duration => MosaicProperties.Duration;

        /// <summary>
        ///     IsLevyMutable
        /// </summary>
        public bool IsLevyMutable => MosaicProperties.IsLevyMutable;

        public override string ToString()
        {
            return
                $"{nameof(MetaId)}: {MetaId}, {nameof(MosaicId)}: {MosaicId}, {nameof(Supply)}: {Supply}, {nameof(Height)}: {Height}, {nameof(Owner)}: {Owner}, {nameof(Revision)}: {Revision}, {nameof(Levy)}: {Levy}, {nameof(MosaicProperties)}: {MosaicProperties}, {nameof(IsSupplyMutable)}: {IsSupplyMutable}, {nameof(IsTransferable)}: {IsTransferable}, {nameof(Divisibility)}: {Divisibility}, {nameof(Duration)}: {Duration}, {nameof(IsLevyMutable)}: {IsLevyMutable}";
        }
    }
}