// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="MosaicId.cs" company="Nem.io">   
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
using System.Collections.Generic;
using io.nem2.sdk.Core.Utils;

namespace io.nem2.sdk.Model.Mosaics
{
    /// <summary>
    /// A mosaic describes an instance of a mosaic definition.
    /// Mosaics can be transferred by means of a transfer transaction.
    /// </summary>
    public class MosaicId
    {
        /// <summary>
        /// The mosaic id.
        /// </summary>
        /// <value>The identifier.</value>
        public ulong Id { get; }

        /// <summary>
        /// The mosaic name.
        /// </summary>
        /// <value>The name of the mosaic.</value>
        public string MosaicName { get; }

        /// <summary>
        /// Gets or sets the full name.
        /// </summary>
        /// <value>The full name.</value>
        public string FullName { get; }

        /// <summary>
        /// Gets or sets the hexadecimal identifier.
        /// </summary>
        /// <value>The hexadecimal identifier.</value>
        public string HexId { get; }

        /// <summary>
        /// Names the present.
        /// </summary>
        /// <returns><c>true</c> if MosaicName is present, <c>false</c> otherwise.</returns>
        public bool IsNamePresent => MosaicName != null;

        /// <summary>
        /// Fulls the name present.
        /// </summary>
        /// <returns><c>true</c> if FullName is present, <c>false</c> otherwise.</returns>
        public bool IsFullNamePresent => FullName != null;

        /// <summary>
        /// Constructor.
        /// </summary>
        /// <param name="id">The mosaic id in uint64 format.</param>
        public MosaicId(ulong id)
        {
            Id = id;
            HexId = Id.ToString("X");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MosaicId"/> class.
        /// </summary>
        /// <param name="identifier">The identifier.</param>
        /// <exception cref="System.ArgumentException">
        /// </exception>
        public MosaicId(string identifier)
        {
            if (string.IsNullOrEmpty(identifier)) throw new ArgumentException(identifier + " is not valid");
            if (!identifier.Contains(":")) throw new ArgumentException(identifier + " is not valid");
            var parts = identifier.Split(':');
            if (parts.Length != 2) throw new ArgumentException(identifier + " is not valid");
            if (parts[0] == "") throw new ArgumentException(identifier + " is not valid");
            if (parts[1] == "") throw new ArgumentException(identifier + " is not valid");
            var namespaceName = parts[0];
            MosaicName = parts[1];
            FullName = identifier;
            Id = IdGenerator.GenerateId(IdGenerator.GenerateId(0, namespaceName), MosaicName);     
            HexId = Id.ToString("X");
        }

        /// <summary>
        /// Create Mosaic from identifier and amount.
        /// </summary>
        /// <param name="identifier">The mosaic identifier. ex: nem:xem or test.namespace:token</param>
        /// <returns>A Mosaic instance</returns>
        /// <exception cref="System.ArgumentException">
        /// </exception>
        public static MosaicId CreateFromMosaicIdentifier(string identifier)
        {
            return new MosaicId(identifier);
        } 

        /// <summary>
        /// Determines whether the specified <see cref="System.Object" /> is equal to this instance.
        /// </summary>
        /// <param name="obj">The object to compare with the current object.</param>
        /// <returns><c>true</c> if the specified <see cref="System.Object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return Id == ((MosaicId) obj)?.Id;
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
        public override int GetHashCode()
        {
            var hashCode = 1792400168;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(MosaicName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(FullName);
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(HexId);
            return hashCode;
        }
    }
}
