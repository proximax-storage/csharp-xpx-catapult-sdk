// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-26-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="MultisigModificationType.cs" company="Nem.io">
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
using System.ComponentModel;

namespace io.nem2.sdk.Model.Transactions
{
    /// <summary>
    /// Class MultisigModificationType.
    /// </summary>
    public static class MultisigCosignatoryModificationType
    {
        /// <summary>
        /// Enum Type
        /// </summary>
        public enum Type
        {
            /// <summary>
            /// Type pf Addition.
            /// </summary>
            Add = 0x00,

            /// <summary>
            /// Type of removal.
            /// </summary>
            Remove = 0x01
        }

        /// <summary>
        /// Gets the byte value of the modification type.
        /// </summary>
        /// <param name="value">The byte value.</param>
        /// <returns>System.Byte.</returns>
        /// <exception cref="InvalidEnumArgumentException">value</exception>
        public static byte GetValue(this Type value)
        {
            if (!Enum.IsDefined(typeof(Type), value))
                throw new InvalidEnumArgumentException(nameof(value), (int) value, typeof(Type));

            return (byte) value;
        }

        /// <summary>
        /// Gets the enum value.
        /// </summary>
        /// <param name="value">The enum value from a modification type byte.</param>
        /// <returns>Type.</returns>
        /// <exception cref="ArgumentOutOfRangeException">value</exception>
        public static Type GetRawValue(byte value)
        {
            if (value != Type.Add.GetValue() && value != Type.Add.GetValue()) throw new ArgumentOutOfRangeException(nameof(value));

            return value == Type.Add.GetValue() ? Type.Add : Type.Remove;
        }
    }
}
