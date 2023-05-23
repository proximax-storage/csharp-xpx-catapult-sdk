// Copyright 2021 ProximaX
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
    public enum EntityType
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
        HASH_LOCK = 0x4148,

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
        ///     Address related metadata transaction type
        /// </summary>
        MODIFY_ADDRESS_METADATA = 0x413D,

        /// <summary>
        ///     Mosaic related metadata transaction type
        /// </summary>
        MODIFY_MOSAIC_METADATA = 0x423D,

        /// <summary>
        ///     Namespace related metadata transaction type
        /// </summary>
        MODIFY_NAMESPACE_METADATA = 0x433D,

        /// <summary>
        ///    Blockchain version update transaction
        /// </summary>
        BLOCKCHAIN_UPGRADE = 0x4158,

        /// <summary>
        ///    Blockchain configuration change transaction
        /// </summary>
        BLOCKCHAIN_CONFIG = 0x4159,

        /// <summary>
        ///     Add exchange offer
        /// </summary>
        EXCHANGE_OFFER_ADD = 0x415D,

        /// <summary>
        ///     Exchange offer
        /// </summary>
        EXCHANGE_OFFER = 0x425D,

        /// <summary>
        ///     Remove exchange offer
        /// </summary>
        EXCHANGE_OFFER_REMOVE = 0x435D,

        /// <summary>
        ///     Adress related metadata transaction type
        /// </summary>
        ACCOUNT_METADATA_V2 = 0x413F,

        /// <summary>
        ///     Mosaic related metadata transaction type
        /// </summary>
        MOSAIC_METADATA_V2 = 0x423F,

        /// <summary>
        ///    Namespace related metadata transaction type
        /// </summary>
        NAMESPACE_METADATA_V2 = 0x433F,

        /// <summary>
        ///    Modify Mosaic Levy
        /// </summary>
        MODIFY_MOSAIC_LEVY = 0x434D,

        /// <summary>
        ///    Remove Mosaic Levy
        /// </summary>
        REMOVE_MOSAIC_LEVY = 0x444D,
    }

    public static class EntityTypeExtension
    {
        /// <summary>
        ///     Get raw value extension
        /// </summary>
        /// <param name="value">The transaction type</param>
        /// <returns>TransactionType</returns>
        public static EntityType GetRawValue(int? value)
        {
            return value.HasValue
                ? EnumExtensions.GetEnumValue<EntityType>(value.Value)
                : EntityType.TRANSFER;
        }

        /// <summary>
        ///     Get value extension
        /// </summary>
        /// <param name="type">The transaction typ</param>
        /// <returns>int</returns>
        public static ushort GetValue(this EntityType type)
        {
            return (ushort)type;
        }
    }
}