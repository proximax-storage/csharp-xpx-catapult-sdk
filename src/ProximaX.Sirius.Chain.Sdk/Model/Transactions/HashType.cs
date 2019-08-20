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

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions
{
    public enum HashType
    {
        SHA3_256 = 0,
        KECCAK_256 = 1,
        HASH_160 = 2,
        HASH_256 = 3
    }

    public static class HashTypeExtension
    {
        public static bool Validate(this HashType type, string input)
        {
            if (input.IsHexString())
                switch (type)
                {
                    case HashType.SHA3_256:
                    case HashType.HASH_256:
                    case HashType.KECCAK_256:
                        return input.Length == 64;
                    case HashType.HASH_160:
                        return input.Length == 40;
                    default:
                        return false;
                }

            return false;
        }

        public static HashType GetRawValue(int? value)
        {
            return value.HasValue
                ? EnumExtensions.GetEnumValue<HashType>(value.Value)
                : throw new Exception("Unsupported Hash Type");
        }

        /// <summary>
        ///     Get value extension
        /// </summary>
        /// <param name="type">The hash typ</param>
        /// <returns>int</returns>
        public static int GetValue(this HashType type)
        {
            return (int) type;
        }

        /// <summary>
        ///     Get value extension
        /// </summary>
        /// <param name="type">The hash type</param>
        /// <returns>byte</returns>
        public static byte GetValueInByte(this HashType type)
        {
            return (byte) type;
        }
    }
}