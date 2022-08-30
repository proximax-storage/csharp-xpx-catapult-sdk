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

using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using System;
using System.Collections;
using System.Collections.Generic;

namespace ProximaX.Sirius.Chain.Sdk.Model.Mosaics
{
    /// <summary>
    ///     MosaicInfo
    /// </summary>
    public class MosaicRichList
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="address">The address</param>
        /// <param name="amount">The mosaic supply</param>

        public MosaicRichList(Address address, ulong amount)
                   // public MosaicRichList(Address address, string? publicKey)

        {
            Address = address;
            // PublicKey = publicKey;
            Amount = amount;
        }

        /// <summary>
        ///     The Address
        /// </summary>
        public Address Address { get; }

        /// <summary>
        ///     The PublicKey
        /// </summary>
        // public string PublicKey { get; }

        /// <summary>
        ///     The Amount
        /// </summary>
        public ulong Amount { get; }



       public override string ToString()
        {
            return $"{nameof(Address)}: {Address}, {nameof(Amount)}: {Amount}";

            //return $"{nameof(Address)}: {Address}, {nameof(PublicKey)}: {PublicKey}";

        }


    }
}