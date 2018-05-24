// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="Mosaic.cs" company="Nem.io">   
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

namespace io.nem2.sdk.Model.Mosaics
{
    /// <summary>
    /// A mosaic describes an instance of a mosaic definition.
    /// Mosaics can be transferred by means of a transfer transaction.
    /// </summary>
    public class Mosaic
    {
        /// <summary>
        /// Gets the mosaic identifier.
        /// </summary>
        /// <value>The mosaic identifier.</value>
        public MosaicId MosaicId { get; }
        /// <summary>
        /// Gets the amount.
        /// </summary>
        /// <value>The amount.</value>
        public ulong Amount { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Mosaic"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="amount">The amount.</param>
        public Mosaic(MosaicId id, ulong amount)
        {
            MosaicId = id ?? throw new ArgumentNullException(nameof(id));
            Amount = amount;
        }

        /// <summary>
        /// Create Mosaic from identifier and amount.
        /// </summary>
        /// <param name="identifier">The mosaic identifier. ex: nem:xem or test.namespace:token</param>
        /// <param name="amount">The mosaic amount.</param>
        /// <returns>A Mosaic instance</returns>
        /// <exception cref="System.ArgumentException">
        /// </exception>
        public static Mosaic CreateFromIdentifier(string identifier, ulong amount)
        {
            if (string.IsNullOrEmpty(identifier)) throw new ArgumentException(identifier + " is not valid");
            if (!identifier.Contains(":")) throw new ArgumentException(identifier + " is not valid");
            var parts = identifier.Split(':');
            if (parts.Length != 2) throw new ArgumentException(identifier + " is not valid");
            if (parts[0] == "") throw new ArgumentException(identifier + " is not valid");
            if (parts[1] == "") throw new ArgumentException(identifier + " is not valid");
            
            return new Mosaic(new MosaicId(identifier), amount);
        }
    }
}
