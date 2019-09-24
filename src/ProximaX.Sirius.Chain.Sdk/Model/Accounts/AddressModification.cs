﻿// Copyright 2019 ProximaX
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
using System.Collections.Generic;
using GuardNet;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Model.Accounts
{
    /// <summary>
    ///     Class AddressModification
    /// </summary>
    public class AddressModification : ModifyAccountPropertyTransaction<Address>
    {
        public static readonly int VALUE_BYTES_LENGTH = 25;

        /// <summary>
        ///     Initializes a new instance of the <see cref="AddressModification" /> class.
        /// </summary>
        /// <param name="networkType"></param>
        /// <param name="version"></param>
        /// <param name="deadline"></param>
        /// <param name="propertyType"></param>
        /// <param name="propertyModifications"></param>
        /// <param name="maxFee"></param>
        /// <param name="signature"></param>
        /// <param name="signer"></param>
        /// <param name="transactionInfo"></param>
        public AddressModification(NetworkType networkType, int version,
            Deadline deadline, PropertyType propertyType,
            IList<AccountPropertyModification<Address>> propertyModifications, ulong? maxFee, string signature = null,
            PublicAccount signer = null, TransactionInfo transactionInfo = null) : base(networkType, version,
            EntityType.MODIFY_ACCOUNT_PROPERTY_ADDRESS, deadline, propertyType, propertyModifications, maxFee,
            signature, signer, transactionInfo)
        {
        }

        public static int CalculatePayloadSize(int modCount)
        {
            // property type, mod count, mods
            return 1 + 1 + (1 + VALUE_BYTES_LENGTH) * modCount;
        }

        protected override int GetPayloadSerializedSize()
        {
            return CalculatePayloadSize(Modifications.Count);
        }


        /// <summary>
        ///     GetValueBytesFromModification
        /// </summary>
        /// <param name="mod">The modification property</param>
        /// <returns>byte[]</returns>
        protected override byte[] GetValueBytesFromModification(AccountPropertyModification<Address> mod)
        {
            

            var byteValues = mod.Value.Plain.FromBase32String();

            Guard.NotEqualTo(byteValues.Length, VALUE_BYTES_LENGTH,
                new ArgumentOutOfRangeException(
                    $"Address should be serialized to {VALUE_BYTES_LENGTH} bytes but was {byteValues.Length} from {mod.Value}"));

            return byteValues;
        }
    }
}