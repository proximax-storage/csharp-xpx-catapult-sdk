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

using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions
{
    /// <summary>
    ///     TransactionType
    /// </summary>
    public enum TransactionType
    {
        /// <summary>
        ///     The transfer type
        /// </summary>
        TRANSFER = 0x4154,

        /// <summary>
        ///     The namespace creation type
        /// </summary>
        REGISTER_NAMESPACE = 0x414E,

        /// <summary>
        ///     Address alias transaction type
        /// </summary>
        ADDRESS_ALIAS = 0x424E,

        /// <summary>
        ///     Mosaic alias transaction type
        /// </summary>
        MOSAIC_ALIAS = 0x434E,

        /// <summary>
        ///     The mosaic creation type
        /// </summary>
        MOSAIC_DEFINITION = 0x414D,

        /// <summary>
        ///     The mosaic supply change type
        /// </summary>
        MOSAIC_SUPPLY_CHANGE = 0x424D,

        /// <summary>
        ///     The multisig modification type
        /// </summary>
        MODIFY_MULTISIG_ACCOUNT = 0x4155,

        /// <summary>
        ///     The aggregate type
        /// </summary>
        AGGREGATE_COMPLETE = 0x4141,

        /// <summary>
        ///     The bonded aggregate type
        /// </summary>
        AGGREGATE_BONDED = 0x4241,

        /// <summary>
        ///     The lock type
        /// </summary>
        LOCK = 0x4148,

        /// <summary>
        ///     The secret lock type
        /// </summary>
        SECRET_LOCK = 0x4152,

        /// <summary>
        ///     The secret proof type
        /// </summary>
        SECRET_PROOF = 0x4252,

        /// <summary>
        ///     Account property address transaction type
        /// </summary>
        MODIFY_ACCOUNT_PROPERTY_ADDRESS = 0x4150,

        /// <summary>
        ///     Account property mosaic transaction type
        /// </summary>
        MODIFY_ACCOUNT_PROPERTY_MOSAIC = 0x4250,

        /// <summary>
        ///     Account property entity type transaction type
        /// </summary>
        MODIFY_ACCOUNT_PROPERTY_ENTITY_TYPE = 0x4350,

        /// <summary>
        ///     Link account transaction type
        /// </summary>
        LINK_ACCOUNT = 0x414C,

        /// <summary>
        ///     Modify account/address related metadata transaction type
        /// </summary>
        MODIFY_ADDRESS_METADATA = 0x413D,

        /// <summary>
        ///     Modify mosaic related metadata transaction type
        /// </summary>
        MODIFY_MOSAIC_METADATA = 0x423D,

        /// <summary>
        ///     Modify namespace related metadata transaction type
        /// </summary>
        MODIFY_NAMESPACE_METADATA = 0x433D
    }

    public static class TransactionTypeExtension
    {
        /// <summary>
        ///     Get raw value extension
        /// </summary>
        /// <param name="value">The transaction type</param>
        /// <returns>TransactionType</returns>
        public static TransactionType GetRawValue(int? value)
        {
            return value.HasValue
                ? EnumExtensions.GetEnumValue<TransactionType>(value.Value)
                : TransactionType.TRANSFER;
        }

        /// <summary>
        ///     Get value extension
        /// </summary>
        /// <param name="type">The transaction typ</param>
        /// <returns>int</returns>
        public static ushort GetValue(this TransactionType type)
        {
            return (ushort) type;
        }
    }
}