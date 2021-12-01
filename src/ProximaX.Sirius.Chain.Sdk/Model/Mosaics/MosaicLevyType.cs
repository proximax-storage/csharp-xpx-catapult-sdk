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

namespace ProximaX.Sirius.Chain.Sdk.Model.Mosaics
{
    /// <summary>
    ///     MosaicSupplyType
    /// </summary>
    public enum MosaicLevyType
    {
        LevyNone = 0x0,
        LevyAbsoluteFee = 0x1,
        LevyPercentileFee = 0x2
    }

    /// <summary>
    ///     MosaicLevyTypeExtension
    /// </summary>
    public static class MosaicLevyTypeExtension
    {
        /// <summary>
        ///     GetRawValue
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>MosaicLevyTypeExtension</returns>
        public static MosaicLevyType GetRawValue(int? value)
        {
            return value.HasValue
                ? EnumExtensions.GetEnumValue<MosaicLevyType>(value.Value)
                : throw new ArgumentOutOfRangeException("Unsupported MosaicLevyType");
        }

        /// <summary>
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static byte GetValueInByte(this MosaicLevyType type)
        {
            return (byte)type;
        }
    }
}