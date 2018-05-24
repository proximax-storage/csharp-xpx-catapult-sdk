// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="NetworkType.cs" company="Nem.io">   
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

namespace io.nem2.sdk.Model.Blockchain
{
    /// <summary>
    /// Static class containing network type constants enum.
    /// </summary>
    public static class NetworkType
    {
        /// <summary>
        /// Contains the valid network type.
        /// </summary>
        public enum Types
        {
            /// <summary>
            /// The main net
            /// </summary>
            MAIN_NET = 104,
            /// <summary>
            /// The test net
            /// </summary>
            TEST_NET = 152,
            /// <summary>
            /// The mijin
            /// </summary>
            MIJIN = 96,
            /// <summary>
            /// The mijin test
            /// </summary>
            MIJIN_TEST = 144
        }


        /// <summary>
        /// Gets the network identifier byte.
        /// </summary>
        /// <param name="type">The network byte.</param>
        /// <returns>System.Byte.</returns>
        public static byte GetNetworkByte(this Types type)
        {
            return (byte)type;
        }

        /// <summary>
        /// Gets the network identifier.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>Types.</returns>
        public static Types GetNetwork(string name)
        {
            switch (name)
            {
                case "mijinTest":
                    return Types.MIJIN_TEST;
                case "mijin":
                    return Types.MIJIN;
                case "testnet":
                    return Types.TEST_NET;
                case "mainnet":
                    return Types.MAIN_NET;
                default:
                    throw  new ArgumentException("invalid network name.");
            }
        }

        /// Gets the network identifier.
        /// <param name="value">Value</param>
        /// <returns>Types.</returns>
        public static Types GetRawValue(int value)
        {
            switch (value)
            {
                    
                case 144:
                    return Types.MIJIN_TEST;
                case 142:
                    return Types.MIJIN;
                case 96:
                    return Types.TEST_NET;
                case 104:
                    return Types.MAIN_NET;
                default:
                    throw new ArgumentException("invalid network name.");
            }
        }
    }  
}
