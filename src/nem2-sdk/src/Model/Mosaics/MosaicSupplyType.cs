// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-25-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="MosaicSupplyType.cs" company="Nem.io">
// Copyright 2018 NEM
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
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace io.nem2.sdk.Model.Mosaics
{
    /// <summary>
    /// Contains mosaic supply alteration type constant values.
    /// </summary>
    public static class MosaicSupplyType
    {
        /// <summary>
        /// The Types of mosaic supply alteration Types.
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// The decrease
            /// </summary>
            DECREASE = 0,

            /// <summary>
            /// The increase
            /// </summary>
            INCREASE = 1
        }

        /// <summary>
        /// Get the absolute value of the supply alteration type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The type absolute value.</returns>
        public static byte GetValue(this Type type)
        {
            return (byte) type;
        }

        /// <summary>
        /// Get the raw value of the type enum.
        /// </summary>
        /// <param name="value">The absolute value of the type.</param>
        /// <returns>The supply alteration type raw value.</returns>
        /// <exception cref="System.ArgumentException"></exception>
        public static Type GetRawValue(byte value)
        {
            switch (value)
            {
                case 0:
                    return Type.DECREASE;
                case 1:
                    return Type.INCREASE;
                default:
                    throw new ArgumentException(value + " is not a valid value");
            }
        }
    }
}


