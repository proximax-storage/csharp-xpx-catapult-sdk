// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="MosainName.cs" company="Nem.io">   
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

using io.nem2.sdk.Model.Namespace;

namespace io.nem2.sdk.Model.Mosaics
{
    /// <summary>
    /// The mosaic name info structure describes basic information of a mosaic and name.
    /// </summary>
    public class MosaicName
    {
        /// <summary>
        /// The mosaic id.
        /// </summary>
        /// <value>The mosaic identifier.</value>
        public MosaicId MosaicId { get; }

        /// <summary>
        /// The mosaic name.
        /// </summary>
        /// <value>The name.</value>
        public string Name { get; }

        /// <summary>
        /// The parent id.
        /// </summary>
        /// <value>The parent identifier.</value>
        public NamespaceId ParentId { get; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mosaicId">The mosaic id.</param>
        /// <param name="name">The mosaic name.</param>
        /// <param name="parentId">The parent id.</param>
        public MosaicName(MosaicId mosaicId, string name, NamespaceId parentId)
        {
            MosaicId = mosaicId;
            Name = name;
            ParentId = parentId;
        }
    }
}
