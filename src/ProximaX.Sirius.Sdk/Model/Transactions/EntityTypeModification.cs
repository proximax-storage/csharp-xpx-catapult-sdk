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
using System.Collections.Generic;
using GuardNet;
using ProximaX.Sirius.Sdk.Model.Accounts;
using ProximaX.Sirius.Sdk.Model.Blockchain;

namespace ProximaX.Sirius.Sdk.Model.Transactions
{
    /// <summary>
    ///     Class EntityTypeModification
    /// </summary>
    public class EntityTypeModification : ModifyAccountPropertyTransaction<TransactionType>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="EntityTypeModification" /> class.
        /// </summary>
        /// <param name="networkType"></param>
        /// <param name="version"></param>
        /// <param name="deadline"></param>
        /// <param name="maxFee"></param>
        /// <param name="propertyType"></param>
        /// <param name="propertyModifications"></param>
        /// <param name="signature"></param>
        /// <param name="signer"></param>
        /// <param name="transactionInfo"></param>
        public EntityTypeModification(NetworkType networkType, int version, Deadline deadline,
            PropertyType propertyType,
            IList<AccountPropertyModification<TransactionType>> propertyModifications,
            ulong? maxFee,
            string signature = null, PublicAccount signer = null,
            TransactionInfo transactionInfo = null) : base(networkType, version,
            TransactionType.MODIFY_ACCOUNT_PROPERTY_ENTITY_TYPE, deadline, propertyType,
            propertyModifications, maxFee, signature, signer, transactionInfo)
        {
        }

        /// <summary>
        ///     GetValueBytesFromModification
        /// </summary>
        /// <param name="mod">The modification property</param>
        /// <returns>byte[]</returns>
        protected override byte[] GetValueBytesFromModification(AccountPropertyModification<TransactionType> mod)
        {
            const int valueByteLength = 2;

            var byteValues = BitConverter.GetBytes(mod.Value.GetValue());

            Guard.NotEqualTo(byteValues.Length, valueByteLength,
                new ArgumentException(
                    $"Entity Type should be serialized to {valueByteLength} bytes but was {byteValues.Length} from {mod.Value}"));

            return byteValues;
        }
    }
}