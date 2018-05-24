// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="MultisigModification.cs" company="Nem.io">
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
using System.Text.RegularExpressions;
using io.nem2.sdk.Model.Accounts;

namespace io.nem2.sdk.Model.Transactions
{
    /// <summary>
    /// Class MultisigCosignatoryModification.
    /// </summary>
    public class MultisigCosignatoryModification
    {
        /// <summary>
        /// Gets or sets the type of modification.
        /// </summary>
        /// <value>The type.</value>
        internal MultisigCosignatoryModificationType.Type Type { get; }

        /// <summary>
        /// Gets or sets the public key.
        /// </summary>
        /// <value>The public key.</value>
        public PublicAccount PublicAccount { get; }

        /// <summary>
        /// Gets a value indicating whether the modification is an addition.
        /// </summary>
        /// <value><c>true</c> if this modification is an addition; otherwise, <c>false</c>.</value>
        public bool IsAddition => Type == MultisigCosignatoryModificationType.Type.Add;

        /// <summary>
        /// Gets a value indicating whether the modification is a removal.
        /// </summary>
        /// <value><c>true</c> if the modification is a removal; otherwise, <c>false</c>.</value>
        public bool IsRemoval => Type == MultisigCosignatoryModificationType.Type.Remove;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultisigCosignatoryModification"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="publicAccount">The public key.</param>
        public MultisigCosignatoryModification(MultisigCosignatoryModificationType.Type type, PublicAccount publicAccount)
        {
            if (type != MultisigCosignatoryModificationType.Type.Add && type != MultisigCosignatoryModificationType.Type.Remove) throw new ArgumentOutOfRangeException(nameof(type));
            if (publicAccount == null) throw new ArgumentNullException(nameof(publicAccount));
            if (!Regex.IsMatch(publicAccount.PublicKey, @"\A\b[0-9a-fA-F]+\b\Z")) throw new ArgumentException("invalid public key length");
            if (publicAccount.PublicKey.Length != 64) throw new ArgumentException("invalid public key not hex");           

            Type = type;
            PublicAccount = publicAccount;
        }

        /// <summary>
        /// Creates a new instance of <see cref="MultisigCosignatoryModification"/>.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="publicKey">The public key.</param>
        /// <returns>MultisigCosignatoryModification.</returns>
        public static MultisigCosignatoryModification Create(MultisigCosignatoryModificationType.Type type, PublicAccount publicKey)
        {
            return new MultisigCosignatoryModification(type, publicKey);
        }
    }
}
