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
    public static class TransactionTypes
    {
        /// <summary>
        /// Enum Types
        /// </summary>
        public enum Types
        {
            /// <summary>
            /// The transfer type
            /// </summary>
            Transfer = 0x4154,

            /// <summary>
            /// The namespace creation type
            /// </summary>
            RegisterNamespace = 0x414e,

            /// <summary>
            /// The mosaic creation type
            /// </summary>
            MosaicDefinition = 0x414d,

            /// <summary>
            /// The mosaic supply change type
            /// </summary>
            MosaicSupplyChange = 0x424d,

            /// <summary>
            /// The mosaic levy change type
            /// </summary>
            MosaicLevyChange = 0x434d,

            /// <summary>
            /// The multisig modification type
            /// </summary>
            ModifyMultisigAccount = 0x4155,

            /// <summary>
            /// The aggregate type
            /// </summary>
            AggregateComplete = 0x4141,

            /// <summary>
            /// The bonded aggregate type
            /// </summary>
            AggregateBonded = 0x4241,

            /// <summary>
            /// The hashlock type
            /// </summary>
            LockFunds = 0x414C,

            /// <summary>
            /// The secret lock type
            /// </summary>
            SecretLock = 0x424C,

            /// <summary>
            /// The secret proof type
            /// </summary>
            SecretProof = 0x434C
        }

        /// <summary>
        /// Gets the value of the type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The int16 value of the type.</returns>
        /// <exception cref="InvalidEnumArgumentException">type</exception>
        public static ushort GetValue(this Types type)
        {
            if (!Enum.IsDefined(typeof(Types), type))
                throw new InvalidEnumArgumentException(nameof(type), (int) type, typeof(Types));

            return (ushort) type;
        }

        /// <summary>
        /// Gets the type for the given value.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The Type associated with the given int16 value.</returns>
        /// <exception cref="InvalidEnumArgumentException">type</exception>
        public static Types GetRawValue(this ushort type)
        {
            switch (type)
            {
                case 0x4154:
                    return Types.Transfer;
                case 0x414e:
                    return Types.RegisterNamespace;
                case 0x414d:
                    return Types.MosaicDefinition;
                case 0x424d:
                    return Types.MosaicSupplyChange;
                case 0x434d:
                    return Types.MosaicLevyChange;
                case 0x4155:
                    return Types.ModifyMultisigAccount;
                case 0x4141:
                    return Types.AggregateComplete;
                case 0x4241:
                    return Types.AggregateBonded;
                case 0x414C:
                    return Types.LockFunds;
                case 0x424C:
                    return Types.SecretLock;
                case 0x434C:
                    return Types.SecretProof;
                default:
                    throw new ArgumentException("invalid transaction type.");
            }
        }
    }
}
