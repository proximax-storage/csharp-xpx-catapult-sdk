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
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;

namespace ProximaX.Sirius.Chain.Sdk.Model.Namespaces
{
    /// <summary>
    ///     Alias
    /// </summary>
    public class Alias
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Alias" /> class.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="address"></param>
        /// <param name="mosaicId"></param>
        public Alias(AliasType type, Address address, MosaicId mosaicId)
        {
            Type = type;
            Address = address;
            MosaicId = mosaicId;
        }

        /// <summary>
        ///     The Alias type
        ///     0: No alias
        ///     1: Mosaic ID alias
        ///     2: Address alias
        /// </summary>
        public AliasType Type { get; }

        /// <summary>
        ///     The address alias
        /// </summary>
        public Address Address { get; }

        /// <summary>
        ///     The mosaicId alias
        /// </summary>
        public MosaicId MosaicId { get; }

        public override string ToString()
        {
            return $"{nameof(Type)}: {Type}, {nameof(Address)}: {Address}, {nameof(MosaicId)}: {MosaicId}";
        }
    }
}