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

using System.Collections.Generic;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;

namespace ProximaX.Sirius.Chain.Sdk.Model.Metadata
{
    /// <summary>
    ///     Class MosaicMetadata
    /// </summary>
    public class MosaicMetadata : Metadata
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="fields"></param>
        /// <param name="id"></param>
        public MosaicMetadata(IList<Field> fields, MosaicId id) : base(MetadataType.MOSAIC, fields)
        {
            Id = id;
        }

        /// <summary>
        ///     The mosaic id metadata
        /// </summary>
        public MosaicId Id { get; set; }
    }
}