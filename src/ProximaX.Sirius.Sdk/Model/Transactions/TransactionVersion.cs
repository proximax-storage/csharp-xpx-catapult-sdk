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

using ProximaX.Sirius.Sdk.Utils;

namespace ProximaX.Sirius.Sdk.Model.Transactions
{
    /// <summary>
    ///     TransactionVersion
    /// </summary>
    public enum TransactionVersion
    {
        /// <summary>
        ///     Transfer Transaction transaction version.
        /// </summary>
        TRANSFER = 3,

        /// <summary>
        ///     Register namespace transaction version.
        /// </summary>
        REGISTER_NAMESPACE = 2,

        /// <summary>
        ///     Mosaic definition transaction version.
        /// </summary>
        MOSAIC_DEFINITION = 3,

        /// <summary>
        ///     Mosaic supply change transaction.
        /// </summary>
        MOSAIC_SUPPLY_CHANGE = 2,

        /// <summary>
        ///     Modify multisig account transaction version.
        /// </summary>
        MODIFY_MULTISIG_ACCOUNT = 3,

        /// <summary>
        ///     Aggregate complete transaction version.
        /// </summary>
        AGGREGATE_COMPLETE = 2,

        /// <summary>
        ///     Aggregate bonded transaction version
        /// </summary>
        AGGREGATE_BONDED = 2,

        /// <summary>
        ///     Lock transaction version
        /// </summary>
        LOCK = 1,

        /// <summary>
        ///     Secret Lock transaction version
        /// </summary>
        SECRET_LOCK = 1,

        /// <summary>
        ///     Secret Proof transaction version
        /// </summary>
        SECRET_PROOF = 1,

        /// <summary>
        ///     Address Alias transaction version
        /// </summary>
        ADDRESS_ALIAS = 1,

        /// <summary>
        ///     Mosaic Alias transaction version
        /// </summary>
        MOSAIC_ALIAS = 1,

        /// <summary>
        ///     Account Property address transaction version
        /// </summary>
        MODIFY_ACCOUNT_PROPERTY_ADDRESS = 1,

        /// <summary>
        ///     Account Property mosaic transaction version
        /// </summary>
        MODIFY_ACCOUNT_PROPERTY_MOSAIC = 1,

        /// <summary>
        ///     Account Property entity type transaction version
        /// </summary>
        MODIFY_ACCOUNT_PROPERTY_ENTITY_TYPE = 1,

        /// <summary>
        ///     Link account transaction version
        /// </summary>
        LINK_ACCOUNT = 2,

        /// <summary>
        ///     Modify metadata transactions version
        /// </summary>
        MODIFY_METADATA = 1
    }

    public static class TransactionVersionExtension
    {
        /// <summary>
        ///     Get raw value extension
        /// </summary>
        /// <param name="value">The transaction version</param>
        /// <returns>TransactionVersion</returns>
        public static TransactionVersion GetRawValue(int? value)
        {
            return value.HasValue
                ? EnumExtensions.GetEnumValue<TransactionVersion>(value.Value)
                : TransactionVersion.TRANSFER;
        }

        /// <summary>
        ///     Get value extension
        /// </summary>
        /// <param name="type">The transaction version</param>
        /// <returns>int</returns>
        public static int GetValue(this TransactionVersion type)
        {
            return (int) type;
        }
    }
}