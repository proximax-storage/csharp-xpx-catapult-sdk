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
using ProximaX.Sirius.Chain.Sdk.Model.Exceptions;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Model.Accounts
{
    /// <summary>
    ///     Enum PropertyModificationType
    /// </summary>
    public enum PropertyModificationType
    {
        /// <summary>
        ///     ADD property value
        /// </summary>
        ADD = 0x00,

        /// <summary>
        ///     REMOVE property value
        /// </summary>
        REMOVE = 0x01
    }

    /// <summary>
    ///     Class PropertyModificationTypeExtension
    /// </summary>
    public static class PropertyModificationTypeExtension
    {
        /// <summary>
        ///     GetRawValue
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static PropertyModificationType GetRawValue(int? value)
        {
            return value.HasValue
                ? EnumExtensions.GetEnumValue<PropertyModificationType>(value.Value)
                : throw new TypeNotSupportException(nameof(value));
        }

        /// <summary>
        ///     Get value in byte
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static byte GetValueInByte(this PropertyModificationType type)
        {
            return (byte) type;
        }
    }
}