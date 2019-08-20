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
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Model.Namespaces
{
    /// <summary>
    ///     AliasActionType
    /// </summary>
    public enum AliasActionType
    {
        /// <summary>
        ///     Link alias action
        /// </summary>
        LINK = 0,

        /// <summary>
        ///     Unlink alias action
        /// </summary>
        UNLINK = 1
    }


    public static class AliasActionTypeExtension
    {
        public static AliasActionType GetRawValue(int? value)
        {
            return value.HasValue
                ? EnumExtensions.GetEnumValue<AliasActionType>(value.Value)
                : throw new Exception("Unsupported AliasActionType");
        }

        /// <summary>
        ///     Get value extension
        /// </summary>
        /// <param name="type">The alias action type</param>
        /// <returns>AliasActionType</returns>
        public static byte GetValueInByte(this AliasActionType type)
        {
            return (byte) type;
        }
    }
}