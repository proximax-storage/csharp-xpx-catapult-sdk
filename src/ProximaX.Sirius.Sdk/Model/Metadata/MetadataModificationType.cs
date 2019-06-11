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

using System;
using ProximaX.Sirius.Sdk.Model.Exceptions;
using ProximaX.Sirius.Sdk.Utils;

namespace ProximaX.Sirius.Sdk.Model.Metadata
{
    /// <summary>
    ///     MetadataModificationType
    /// </summary>
    public enum MetadataModificationType
    {
        /// <summary>
        ///     ADD metadata
        /// </summary>
        ADD = 0,

        /// <summary>
        ///     REMOVE metadata
        /// </summary>
        REMOVE = 1
    }

    /// <summary>
    ///     Class MetadataModificationTypeExtension
    /// </summary>
    public static class MetadataModificationTypeExtension
    {
        /// <summary>
        ///     GetRawValue
        /// </summary>
        /// <param name="value">The value of metadata modification type</param>
        /// <returns>MetadataModificationType</returns>
        public static MetadataModificationType GetRawValue(int? value)
        {
            return value.HasValue
                ? EnumExtensions.GetEnumValue<MetadataModificationType>(value.Value)
                : throw new TypeNotSupportException(nameof(value));
        }

        /// <summary>
        ///     GetValueInByte
        /// </summary>
        /// <param name="type">The metadata modification type</param>
        /// <returns>byte</returns>
        public static byte GetValueInByte(this MetadataModificationType type)
        {
            return (byte) type;
        }

        /// <summary>
        ///     GetValue
        /// </summary>
        /// <param name="type">The metadata modification type</param>
        /// <returns>byte</returns>
        public static int GetValue(this MetadataModificationType type)
        {
            return (int) type;
        }
    }
}