// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="AccountInfo.cs" company="Nem.io">   
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

using System.Collections.Generic;
using io.nem2.sdk.Model.Mosaics;

namespace io.nem2.sdk.Model.Accounts
{
    /// <summary>
    /// The account info structure describes basic information for an account.
    /// </summary>
    public class AccountInfo
    {

        /// <summary>
        /// Gets or sets the address.
        /// </summary>
        /// <value>The address.</value>
        public Address Address { get; }


        /// <summary>
        /// Gets or sets the height of the address.
        /// </summary>
        /// <value>The height of the address.</value>
        public ulong AddressHeight { get; }


        /// <summary>
        /// Gets or sets the public key.
        /// </summary>
        /// <value>The public key.</value>
        public string PublicKey { get; }


        /// <summary>
        /// Gets or sets the height of the public key.
        /// </summary>
        /// <value>The height of the public key.</value>
        public ulong PublicKeyHeight { get; }


        /// <summary>
        /// Gets or sets the importance.
        /// </summary>
        /// <value>The importance.</value>
        public ulong Importance { get; }


        /// <summary>
        /// Gets or sets the height of the importance.
        /// </summary>
        /// <value>The height of the importance.</value>
        public ulong ImportanceHeight { get; }


        /// <summary>
        /// Gets or sets the mosaics.
        /// </summary>
        /// <value>The mosaics.</value>
        public List<Mosaic> Mosaics { get; }

        /// <summary>
        /// Gets the public account.
        /// </summary>
        /// <value>The public account.</value>
        public PublicAccount PublicAccount => new PublicAccount(
            PublicKey,
            Address.NetworkByte
        );

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountInfo"/> class.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="addressHeight">Height of the address.</param>
        /// <param name="publicKey">The public key.</param>
        /// <param name="publicKeyHeight">Height of the public key.</param>
        /// <param name="importance">The importance.</param>
        /// <param name="importanceHeight">Height of the importance.</param>
        /// <param name="mosaics">The mosaics.</param>
        public AccountInfo(Address address, ulong addressHeight,  string publicKey, ulong publicKeyHeight, ulong importance, ulong importanceHeight, List<Mosaic> mosaics)
        {
            Address = address;
            AddressHeight = addressHeight;
            PublicKey = publicKey;
            PublicKeyHeight = publicKeyHeight;
            Importance = importance;
            ImportanceHeight = importanceHeight;
            Mosaics = mosaics;           
        }
    }
}
