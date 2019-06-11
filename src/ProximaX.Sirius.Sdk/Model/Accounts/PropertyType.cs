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

using ProximaX.Sirius.Sdk.Model.Exceptions;
using ProximaX.Sirius.Sdk.Utils;

namespace ProximaX.Sirius.Sdk.Model.Accounts
{
    /// <summary>
    ///     PropertyType enum
    /// </summary>
    public enum PropertyType : byte
    {
        /// <summary>
        ///     The property type is an address
        /// </summary>
        ALLOW_ADDRESS = 0x01,

        /// <summary>
        ///     The property type is mosaic id.
        /// </summary>
        ALLOW_MOSAIC = 0x02,

        /// <summary>
        ///     The property type is a transaction type.
        /// </summary>
        ALLOW_TRANSACTION = 0x04,

        /// <summary>
        ///     Property type sentinel.
        /// </summary>
        SENTINEL = 0x05,

        /// <summary>
        ///     The property type is an address interpreted as a blocking operation
        /// </summary>
        BLOCK_ADDRESS = 0x80 + 0x01,

        /// <summary>
        ///     The property type is mosaic id interpreted as a blocking operation
        /// </summary>
        BLOCK_MOSAIC = 0x80 + 0x02,

        /// <summary>
        ///     The property type is transaction type interpreted as a blocking operation
        /// </summary>
        BLOCK_TRANSACTION = 0x80 + 0x04
    }

    /// <summary>
    /// Class PropertyTypeExtension
    /// </summary>
    public static class PropertyTypeExtension
    {
        /// <summary>
        ///     Get value extension
        /// </summary>
        /// <param name="type">The property type</param>
        /// <returns>PropertyType</returns>
        public static int? GetValue(this PropertyType type)
        {
            return (int) type;
        }

        /// <summary>
        ///     Get value in byte
        /// </summary>
        /// <param name="type">The property type</param>
        /// <returns>byte</returns>
        public static byte GetValueInByte(this PropertyType type)
        {
            return (byte) type;
        }

        /// <summary>
        ///     Get raw value extension
        /// </summary>
        /// <param name="value">The property type</param>
        /// <returns>PropertyType</returns>
        public static PropertyType GetRawValue(int? value)
        {
            return value.HasValue
                ? EnumExtensions.GetEnumValue<PropertyType>(value.Value)
                : throw new TypeNotSupportException(nameof(value));
        }

        /// <summary>
        ///     Get raw value extension
        /// </summary>
        /// <param name="value">The property type</param>
        /// <returns>PropertyType</returns>
        public static PropertyType GetRawValue(byte? value)
        {
            return value.HasValue
                ? EnumExtensions.GetEnumValue<PropertyType>(value.Value)
                : throw new TypeNotSupportException(nameof(value));
        }
    }
}