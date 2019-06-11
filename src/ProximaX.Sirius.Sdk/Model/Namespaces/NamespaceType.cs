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

namespace ProximaX.Sirius.Sdk.Model.Namespaces
{
    /// <summary>
    ///     NamespaceType
    /// </summary>
    public enum NamespaceType
    {
        /// <summary>
        ///     The root namespace
        /// </summary>
        ROOT_NAMESPACE = 0,

        /// <summary>
        ///     The sub namespace type
        /// </summary>
        SUB_NAMESPACE = 1
    }

    /// <summary>
    ///     Namespace extension
    /// </summary>
    public static class NamespaceTypeExtension
    {
        /// <summary>
        ///     Get value extension
        /// </summary>
        /// <param name="type">The namespace type</param>
        /// <returns>NamespaceType</returns>
        public static int GetValue(this NamespaceType type)
        {
            return (int) type;
        }

        /// <summary>
        ///     Get value extension
        /// </summary>
        /// <param name="type">The namespace type</param>
        /// <returns>NamespaceType</returns>
        public static byte GetValueInByte(this NamespaceType type)
        {
            return (byte) type;
        }


        /// <summary>
        ///     Get raw value extension
        /// </summary>
        /// <param name="value">The namespace type</param>
        /// <returns>NamespaceType</returns>
        public static NamespaceType GetRawValue(int? value)
        {
            return value.HasValue
                ? EnumExtensions.GetEnumValue<NamespaceType>(value.Value)
                : throw new ArgumentOutOfRangeException("Unsupported namespace type");
        }
    }
}