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
using System.ComponentModel;
using ProximaX.Sirius.Chain.Sdk.Model.Exceptions;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Model.Blockchain
{
    /// <summary>
    ///     Enum NetworkType
    /// </summary>
    public enum NetworkType
    {
        /// <summary>
        ///     The main net
        /// </summary>
        [Description("Public")] MAIN_NET = 184,

        /// <summary>
        ///     The test net
        /// </summary>
        [Description("PublicTest")] TEST_NET = 168,

        /// <summary>
        ///     The mijin
        /// </summary>
        [Description("Mijin")] MIJIN = 96,

        /// <summary>
        ///     The mijin test
        /// </summary>
        [Description("MijinTest")] MIJIN_TEST = 144,

        /// <summary>
        ///     The private net
        /// </summary>
        [Description("Private")] PRIVATE = 200,

        /// <summary>
        ///     The private test net
        /// </summary>
        [Description("PrivateTest")] PRIVATE_TEST = 176,

        /// <summary>
        ///     The NotSupport
        /// </summary>
        [Description("NotSupport")] NOT_SUPPORT = 0
    }

    /// <summary>
    ///     Class NetworkTypeExtension
    /// </summary>
    public static class NetworkTypeExtension
    {
        /// <summary>
        ///     GetValue
        /// </summary>
        /// <param name="type">The network type</param>
        /// <returns>int</returns>
        public static int GetValue(this NetworkType type)
        {
            return (int) type;
        }

        /// <summary>
        ///     GetValueInByte
        /// </summary>
        /// <param name="type">The network type</param>
        /// <returns>byte</returns>
        public static byte GetValueInByte(this NetworkType type)
        {
            return (byte) type;
        }

        /// <summary>
        ///     ExtractNetworkType
        /// </summary>
        /// <param name="version">The version of the network type</param>
        /// <returns>NetworkType</returns>
        public static NetworkType ExtractNetworkType(this int version)
        {
            var networkType = (int) Convert.ToInt64(version.ToString("X").MySubstring(0, 2), 16);

            return GetRawValue(networkType);
        }

        /// <summary>
        ///     GetRawValue
        /// </summary>
        /// <param name="value">The value of the network type</param>
        /// <returns>NetworkType</returns>
        public static NetworkType GetRawValue(int value)
        {
            return EnumExtensions.GetEnumValue<NetworkType>(value);
        }

        /// <summary>
        ///     GetRawValue
        /// </summary>
        /// <param name="value">The value of the network type</param>
        /// <returns>NetworkType</returns>
        public static NetworkType GetRawValue(string value)
        {
            if (string.IsNullOrEmpty(value))
                return NetworkType.NOT_SUPPORT;

            switch (value.ToUpper())
            {
                case "PUBLIC":
                    return NetworkType.MAIN_NET;
                case "PUBLICTEST":
                
                    return NetworkType.TEST_NET;
                case "PRIVATE":
                    return NetworkType.PRIVATE;
                case "PRIVATETEST":
                    return NetworkType.PRIVATE_TEST;
                case "MIJINTEST":
                    return NetworkType.MIJIN_TEST;
                case "MIJIN":
                    return NetworkType.MIJIN;
                default:
                    throw new TypeNotSupportException(nameof(value));
            }
        }

    }
}