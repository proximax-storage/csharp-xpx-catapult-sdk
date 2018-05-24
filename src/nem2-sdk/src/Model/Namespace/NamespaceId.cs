// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 02-01-2018
//
// Last Modified By : kailin
// Last Modified On : 02-01-2018
// ***********************************************************************
// <copyright file="NamespaceId.cs" company="Nem.io">
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
using System.Linq;
using io.nem2.sdk.Core.Crypto.Chaso.NaCl;
using io.nem2.sdk.Core.Utils;

namespace io.nem2.sdk.Model.Namespace
{
    /// <summary>
    /// Class NamespaceId.
    /// </summary>
    public class NamespaceId
    {
        /// <summary>
        /// Gets the identifier.
        /// </summary>
        /// <value>The identifier.</value>
        public ulong Id { get; }

        /// <summary>
        /// Gets the name of the namespace.
        /// </summary>
        /// <value>The name of the namespace.</value>
        public string Name { get; }

        /// <summary>
        /// Gets the hexadecimal identifier.
        /// </summary>
        /// <value>The hexadecimal identifier.</value>
        public string HexId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="NamespaceId"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <exception cref="ArgumentException">id</exception>
        /// <exception cref="ArgumentNullException">id</exception>
        public NamespaceId(string id)
        {     
            if (id == null) throw new ArgumentNullException(nameof(id) + " cannot be null");

            Id = IdGenerator.GenerateId(0, id);
            Name = id;
            HexId = BitConverter.GetBytes(Id).Reverse().ToArray().ToHexUpper();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NamespaceId"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        public NamespaceId(ulong id)
        {
            Id = id;
            HexId = BitConverter.GetBytes(id).Reverse().ToArray().ToHexUpper();
        }


        /// <summary>
        /// Creates a NamespaceId instance from a given Id.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>NamespaceId.</returns>
        public static NamespaceId Create(string id)
        {
            return new NamespaceId(id);
        }
    }
}
