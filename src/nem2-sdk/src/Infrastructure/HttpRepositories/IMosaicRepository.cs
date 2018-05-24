// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="IMosaicRepository.cs" company="Nem.io">   
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
using System.Collections.Generic;
using io.nem2.sdk.Model.Mosaics;
using io.nem2.sdk.Model.Namespace;

namespace io.nem2.sdk.Infrastructure.HttpRepositories
{
    /// <summary>
    /// Interface IMosaicRepository
    /// </summary>
    interface IMosaicRepository
    {

        /// <summary>
        /// Gets the mosaic.
        /// </summary>
        /// <param name="mosaicId">The mosaic identifier.</param>
        /// <returns>IObservable&lt;MosaicInfoDTO&gt;.</returns>
        IObservable<MosaicInfo> GetMosaic(string mosaicId);


        /// <summary>
        /// Gets the mosaics.
        /// </summary>
        /// <param name="mosaicIds">The mosaic ids.</param>
        /// <returns>IObservable&lt;List&lt;MosaicInfoDTO&gt;&gt;.</returns>
        IObservable<List<MosaicInfo>> GetMosaics(List<string> mosaicIds);


        /// <summary>
        /// Gets the mosaics from namespace.
        /// </summary>
        /// <param name="namespaceId">The namespace identifier.</param>
        /// <returns>IObservable&lt;List&lt;MosaicInfoDTO&gt;&gt;.</returns>
        IObservable<List<MosaicInfo>> GetMosaicsFromNamespace(NamespaceId namespaceId);


        /// <summary>
        /// Gets the name of the mosaics.
        /// </summary>
        /// <param name="mosaicIds">The mosaic ids.</param>
        /// <returns>IObservable&lt;MosaicNameDTO[]&gt;.</returns>
        IObservable<List<MosaicName>> GetMosaicsName(List<string> mosaicIds);
    }
}
