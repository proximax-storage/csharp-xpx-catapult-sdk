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

using ProximaX.Sirius.Chain.Sdk.Model.Namespaces;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Model.Mosaics
{
    /// <summary>
    ///     The mosaic name info structure describes basic information of a mosaic and name.
    /// </summary>
    public class Mosaic
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Mosaic" /> class.
        /// </summary>
        /// <param name="id">The mosaic identifier</param>
        /// <param name="amount">The mosaic account</param>
        public Mosaic(ulong id, ulong amount)
        {
            Id = id;
            Amount = amount;
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Mosaic" /> class.
        /// </summary>
        /// <param name="id">The mosaic identifier</param>
        /// <param name="amount">The mosaic account</param>
        public Mosaic(MosaicId id, ulong amount)
        {
            Id = id.Id;
            Amount = amount;
        }


        /// <summary>
        ///     Initializes a new instance of the <see cref="Mosaic" /> class.
        /// </summary>
        /// <param name="id">The namespace linked to mosaic identifier</param>
        /// <param name="amount">The mosaic account</param>
        public Mosaic(NamespaceId id, ulong amount)
        {
            Id = id.Id;
            Amount = amount;
        }

        /// <summary>
        ///     The mosaic id.
        /// </summary>
        public ulong Id { get; }

        /// <summary>
        ///     The mosaic hex Id
        /// </summary>
        public string HexId => Id.ToHex();

        /// <summary>
        ///     The mosaic amount
        /// </summary>
        public ulong Amount { get; }

        public override string ToString()
        {
            return $"{nameof(Id)}: {Id}, {nameof(HexId)}: {HexId}, {nameof(Amount)}: {Amount}";
        }
    }
}