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
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions
{
    /// <summary>
    ///     Class AggregateTransactionCosignature.
    /// </summary>
    public class AggregateTransactionCosignature
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AggregateTransactionCosignature" /> class.
        /// </summary>
        /// <param name="signature">The signature.</param>
        /// <param name="signer">The signer.</param>
        /// <exception cref="System.ArgumentNullException">
        ///     signature
        ///     or
        ///     signer
        /// </exception>
        public AggregateTransactionCosignature(string signature, PublicAccount signer)
        {
            Signature = signature ?? throw new ArgumentNullException(nameof(signature));
            Signer = signer ?? throw new ArgumentNullException(nameof(signer));
        }

        /// <summary>
        ///     The signature
        /// </summary>
        public string Signature { get; }

        /// <summary>
        ///     The signer
        /// </summary>
        public PublicAccount Signer { get; }
    }
}