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
using ProximaX.Sirius.Sdk.Utils;

namespace ProximaX.Sirius.Sdk.Model.Mosaics
{
    /// <summary>
    ///     MosaicSupplyType
    /// </summary>
    public enum MosaicSupplyType
    {
        DECREASE = 0,
        INCREASE = 1
    }

    /// <summary>
    ///     MosaicSupplyTypeExtension
    /// </summary>
    public static class MosaicSupplyTypeExtension
    {
        /// <summary>
        ///     GetRawValue
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>MosaicSupplyType</returns>
        public static MosaicSupplyType GetRawValue(int? value)
        {
            return value.HasValue
                ? EnumExtensions.GetEnumValue<MosaicSupplyType>(value.Value)
                : throw new ArgumentOutOfRangeException("Unsupported MosaicSupplyType");
        }

        /// <summary>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static byte GetValueInByte(this MosaicSupplyType type)
        {
            return (byte) type;
        }
    }
}