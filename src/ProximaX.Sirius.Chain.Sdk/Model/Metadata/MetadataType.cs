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

using ProximaX.Sirius.Chain.Sdk.Model.Exceptions;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Model.Metadata
{
    /// <summary>
    ///     Enum MetadataType
    /// </summary>
    public enum MetadataType
    {
        /// <summary>
        ///     NONE
        /// </summary>
        NONE = 0,

        /// <summary>
        ///     Address metadata type
        /// </summary>
        ADDRESS = 1,

        /// <summary>
        ///     Mosaic metadata type
        /// </summary>
        MOSAIC = 2,

        /// <summary>
        ///     Namespace metadata type
        /// </summary>
        NAMESPACE = 3
    }

    public static class MetadataTypeExtension
    {
        /// <summary>
        ///     GetRawValue
        /// </summary>
        /// <param name="value">The metadata type value</param>
        /// <returns>MetadataType</returns>
        public static MetadataType GetRawValue(int? value)
        {
            return value.HasValue
                ? EnumExtensions.GetEnumValue<MetadataType>(value.Value)
                : throw new TypeNotSupportException(nameof(value));
        }

        /// <summary>
        ///     GetValueInByte
        /// </summary>
        /// <param name="type">The metadata type value</param>
        /// <returns>byte</returns>
        public static byte GetValueInByte(this MetadataType type)
        {
            return (byte) type;
        }
    }
}