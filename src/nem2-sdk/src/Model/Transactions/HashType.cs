// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="TransactionTypes.cs" company="Nem.io">
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
    /// Class TransactionTypes.
    /// </summary>
    public static class HashType
    {
        /// <summary>
        /// Enum Types
        /// </summary>
        public enum Types
        {
            /// <summary>
            /// The transfer type
            /// </summary>
            SHA3_512 = 0x00,

        }

        /// <summary>
        /// Gets the value of the type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The int16 value of the type.</returns>
        /// <exception cref="InvalidEnumArgumentException">type</exception>
        public static byte GetHashTypeValue(this Types type)
        {
            if (!Enum.IsDefined(typeof(Types), type))
                throw new InvalidEnumArgumentException(nameof(type), (int)type, typeof(Types));

            return (byte)type;
        }

        /// <summary>
        /// Gets the type for the given value.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The Type associated with the given int16 value.</returns>
        /// <exception cref="InvalidEnumArgumentException">type</exception>
        public static Types GetRawValue(byte type)
        {
            switch (type)
            {
                case 0x00:
                    return Types.SHA3_512;                   
                default:
                    throw new ArgumentException("invalid transaction type.");
            }
        }
    }
}


