// ***********************************************************************
// Assembly         : nis2
// Author           : kaili
// Created          : 05-24-2018
//
// Last Modified By : kaili
// Last Modified On : 05-24-2018
// ***********************************************************************
// <copyright file="NamespaceInfo.cs" company="Nem.io">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;
using io.nem2.sdk.Model.Accounts;

namespace io.nem2.sdk.Model.Namespace
{
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
    namespace io.nem2.sdk.Model.Mosaics
    {
        /// <summary>
        /// Class MosaicInfo.
        /// </summary>
        public class NamespaceInfo
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
            public NamespaceId ParentId { get; }

            /// <summary>
            /// Gets the type of the namespace.
            /// </summary>
            /// <value>The type of the namespace.</value>
            public NamespaceTypes.Types NamespaceType { get; }

            /// <summary>
            /// The supply
            /// </summary>
            /// <value>The supply.</value>
            public int Depth { get; }

            /// <summary>
            /// Gets the levels.
            /// </summary>
            /// <value>The levels.</value>
            public List<NamespaceId> Levels { get; }
            /// <summary>
            /// The height at which the namespace was registered
            /// </summary>
            /// <value>The height.</value>
            public ulong StartHeight { get; }

            /// <summary>
            /// The height at which the namespace expires
            /// </summary>
            /// <value>The height.</value>
            public ulong EndHeight { get; }
            /// <summary>
            /// The owner
            /// </summary>
            /// <value>The owner.</value>
            public PublicAccount Owner { get; }


            /// <summary>
            /// Gets a value indicating whether this instance is expired.
            /// </summary>
            /// <value><c>true</c> if this instance is expired; otherwise, <c>false</c>.</value>
            public bool IsExpired => !IsActive;

            /// <summary>
            /// Initializes a new instance of the <see cref="NamespaceInfo" /> class.
            /// </summary>
            /// <param name="active">if set to <c>true</c> [active].</param>
            /// <param name="index">The index.</param>
            /// <param name="metaId">The meta identifier.</param>
            /// <param name="namespaceId">The namespace identifier.</param>
            /// <param name="depth">The depth.</param>
            /// <param name="levels">The levels.</param>
            /// <param name="parentId">The parent identifier.</param>
            /// <param name="startHeight">The start height.</param>
            /// <param name="endHeight">The end height.</param>
            /// <param name="owner">The owner.</param>
            public NamespaceInfo(bool active, int index, string metaId, NamespaceTypes.Types namespaceId, int depth, List<NamespaceId> levels, NamespaceId parentId, ulong startHeight, ulong endHeight, PublicAccount owner)
            {
                IsActive = active;
                Owner = owner;
                Index = index;
                MetaId = metaId;
                NamespaceType = namespaceId;
                ParentId = parentId;
                StartHeight = startHeight;
                EndHeight = endHeight;
                Depth = depth;
                Levels = levels;
            }
        }
    }

}
