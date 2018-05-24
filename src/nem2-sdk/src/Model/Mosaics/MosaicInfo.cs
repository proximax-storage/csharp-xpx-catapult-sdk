// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="MosaicInfo.cs" company="Nem.io">   
// Copyright 2018 NEM
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
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Namespace;

namespace io.nem2.sdk.Model.Mosaics
{
    /// <summary>
    /// Class MosaicInfo.
    /// </summary>
    public class MosaicInfo
    {
        /// <summary>
        /// The active
        /// </summary>
        /// <value><c>true</c> if this instance is active; otherwise, <c>false</c>.</value>
        public bool IsActive { get; }

        /// <summary>
        /// The index
        /// </summary>
        /// <value>The index.</value>
        public int Index { get; }

        /// <summary>
        /// The meta identifier
        /// </summary>
        /// <value>The meta identifier.</value>
        public string MetaId { get; }

        /// <summary>
        /// The namespace identifier
        /// </summary>
        /// <value>The namespace identifier.</value>
        public NamespaceId NamespaceId { get; }

        /// <summary>
        /// The mosaic identifier
        /// </summary>
        /// <value>The mosaic identifier.</value>
        public MosaicId MosaicId { get; }

        /// <summary>
        /// The supply
        /// </summary>
        /// <value>The supply.</value>
        public ulong Supply { get; }

        /// <summary>
        /// The height
        /// </summary>
        /// <value>The height.</value>
        public ulong Height { get; }

        /// <summary>
        /// The owner
        /// </summary>
        /// <value>The owner.</value>
        public PublicAccount Owner { get; }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>The properties.</value>
        public MosaicProperties Properties { get; }

        /// <summary>
        /// Gets a value indicating whether this instance is expired.
        /// </summary>
        /// <value><c>true</c> if this instance is expired; otherwise, <c>false</c>.</value>
        public bool IsExpired => !IsActive;

        /// <summary>
        /// Gets a value indicating whether this instance is supply mutable.
        /// </summary>
        /// <value><c>true</c> if this instance is supply mutable; otherwise, <c>false</c>.</value>
        public bool IsSupplyMutable => Properties.IsSupplyMutable;

        /// <summary>
        /// Gets a value indicating whether this instance is transferable.
        /// </summary>
        /// <value><c>true</c> if this instance is transferable; otherwise, <c>false</c>.</value>
        public bool IsTransferable => Properties.IsTransferable;

        /// <summary>
        /// Gets a value indicating whether this instance is levy mutable.
        /// </summary>
        /// <value><c>true</c> if this instance is levy mutable; otherwise, <c>false</c>.</value>
        public bool IsLevyMutable => Properties.IsLevyMutable;

        /// <summary>
        /// Gets the duration.
        /// </summary>
        /// <value>The duration.</value>
        public ulong Duration => Properties.Duration;

        /// <summary>
        /// Gets the divisibility.
        /// </summary>
        /// <value>The divisibility.</value>
        public int Divisibility => Properties.Divisibility;

        /// <summary>
        /// The mosaic info structure contains its properties, the owner and the namespace to which it belongs to.
        /// </summary>
        /// <param name="active">The mosaic activity.</param>
        /// <param name="index">The mosaic index.</param>
        /// <param name="metaId">The meta data id.</param>
        /// <param name="namespaceId">The namespace id.</param>
        /// <param name="mosaicId">The mosaic id.</param>
        /// <param name="supply">The mosaic supply.</param>
        /// <param name="height">The mosaic height.</param>
        /// <param name="owner">The mosaic owner.</param>
        /// <param name="properties">The properties.</param>
        /// <exception cref="ArgumentOutOfRangeException">index</exception>
        /// <exception cref="ArgumentNullException">
        /// owner
        /// or
        /// properties
        /// </exception>
        public MosaicInfo(bool active, int index, string metaId, NamespaceId namespaceId, MosaicId mosaicId, ulong supply, ulong height, PublicAccount owner, MosaicProperties properties)
        {
            Console.WriteLine(namespaceId.HexId);
            IsActive = active;
            Index = index;
            MetaId = metaId;
            NamespaceId = namespaceId;
            MosaicId = mosaicId;
            Supply = supply;
            Height = height;
            Owner = owner;
            Properties = properties;

        }
    }
}
