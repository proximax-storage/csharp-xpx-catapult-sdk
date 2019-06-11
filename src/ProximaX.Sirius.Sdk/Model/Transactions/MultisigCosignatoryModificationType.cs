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

namespace ProximaX.Sirius.Sdk.Model.Transactions
{
    /// <summary>
    ///     MultisigCosignatoryModificationType
    /// </summary>
    public enum MultisigCosignatoryModificationType
    {
        /// <summary>
        ///     ADD
        /// </summary>
        ADD = 0,

        /// <summary>
        ///     REMOVE
        /// </summary>
        REMOVE = 1
    }

    /// <summary>
    ///     MultisigCosignatoryModificationTypeExtension
    /// </summary>
    public static class MultisigCosignatoryModificationTypeExtension
    {
        /// <summary>
        ///     Get raw value extension
        /// </summary>
        /// <param name="value">The MultisigCosignatoryModificationType</param>
        /// <returns>MultisigCosignatoryModificationType</returns>
        public static MultisigCosignatoryModificationType GetRawValue(int? value)
        {
            return value.HasValue
                ? EnumExtensions.GetEnumValue<MultisigCosignatoryModificationType>(value.Value)
                : throw new Exception("Unsupported MultisigCosignatoryModificationType");
        }

        /// <summary>
        ///     Get value extension
        /// </summary>
        /// <param name="type">The MultisigCosignatoryModificationType</param>
        /// <returns>int</returns>
        public static int GetValue(this MultisigCosignatoryModificationType type)
        {
            return (int) type;
        }

        /// <summary>
        ///     Get value extension
        /// </summary>
        /// <param name="type">The MultisigCosignatoryModificationType</param>
        /// <returns>byte</returns>
        public static byte GetValueInByte(this MultisigCosignatoryModificationType type)
        {
            return (byte) type;
        }
    }
}