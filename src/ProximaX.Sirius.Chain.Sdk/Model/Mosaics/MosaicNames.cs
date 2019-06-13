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

namespace ProximaX.Sirius.Chain.Sdk.Model.Mosaics
{
    /// <summary>
    ///     MosaicNames
    /// </summary>
    public class MosaicNames
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="mosaicId">The mosaic id</param>
        /// <param name="names">The mosaic names</param>
        public MosaicNames(MosaicId mosaicId, List<string> names)
        {
            MosaicId = mosaicId;
            Names = names;
        }

        /// <summary>
        ///     The mosaic id
        /// </summary>
        public MosaicId MosaicId { get; }

        /// <summary>
        ///     The mosaic names
        /// </summary>
        public List<string> Names { get; }

        public override string ToString()
        {
            return $"{nameof(MosaicId)}: {MosaicId}, {nameof(Names)}: {Names}";
        }
    }
}