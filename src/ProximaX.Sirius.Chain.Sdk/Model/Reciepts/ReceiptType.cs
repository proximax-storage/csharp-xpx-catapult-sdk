using ProximaX.Sirius.Chain.Sdk.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProximaX.Sirius.Chain.Sdk.Model.Reciepts
{
    public enum ReceiptType
    {
        /**
         * The recipient, account and amount of fees received for harvesting a block. It is recorded when a block is harvested.
         */
        Harvest_Fee = 0x2143,
        /**
         * The unresolved and resolved alias. It is recorded when a transaction indicates a valid address alias instead of an address.
         */
        Address_Alias_Resolution = 0xF143,
        /**
         * The unresolved and resolved alias. It is recorded when a transaction indicates a valid mosaic alias instead of a mosaicId.
         */
        Mosaic_Alias_Resolution = 0xF243,
        /**
         * A collection of state changes for a given source. It is recorded when a state change receipt is issued.
         */
        Transaction_Group = 0xE143,
        /**
         * The mosaicId expiring in this block. It is recorded when a mosaic expires.
         */
        Mosaic_Expired = 0x414D,
        /**
         * The sender and recipient of the levied mosaic, the mosaicId and amount. It is recorded when a transaction has a levied mosaic.
         */
        Mosaic_Levy = 0x124D,
        /**
         * The sender and recipient of the mosaicId and amount representing the cost of registering the mosaic.
         * It is recorded when a mosaic is registered.
         */
        Mosaic_Rental_Fee = 0x134D,
        /**
         * The namespaceId expiring in this block. It is recorded when a namespace expires.
         */
        Namespace_Expired = 0x414E,
        /**
         * The sender and recipient of the mosaicId and amount representing the cost of extending the namespace.
         * It is recorded when a namespace is registered or its duration is extended.
         */
        Namespace_Rental_Fee = 0x124E,
        /**
         * The lockhash sender, mosaicId and amount locked. It is recorded when a valid HashLockTransaction is announced.
         */
        LockHash_Created = 0x3148,
        /**
         * The haslock sender, mosaicId and amount locked that is returned.
         * It is recorded when an aggregate bonded transaction linked to the hash completes.
         */
        LockHash_Completed = 0x2248,
        /**
         * The account receiving the locked mosaic, the mosaicId and the amount. It is recorded when a lock hash expires.
         */
        LockHash_Expired = 0x2348,
        /**
         * The secretlock sender, mosaicId and amount locked. It is recorded when a valid SecretLockTransaction is announced.
         */
        LockSecret_Created = 0x3152,
        /**
         * The secretlock sender, mosaicId and amount locked. It is recorded when a secretlock is proved.
         */
        LockSecret_Completed = 0x2252,
        /**
         * The account receiving the locked mosaic, the mosaicId and the amount. It is recorded when a secretlock expires
         */
        LockSecret_Expired = 0x2352,

        /**
         * The amount of native currency mosaics created. The receipt is recorded when the network has inflation configured,
         * and a new block triggers the creation of currency mosaics.
         */
        Inflation = 0x5143,
    }

    public static class ReceiptTypeExtension
    {
        /// <summary>
        ///     Get raw value extension
        /// </summary>
        /// <param name="value">The transaction type</param>
        /// <returns>TransactionType</returns>
        public static ReceiptType GetRawValue(int? value)
        {
            return EnumExtensions.GetEnumValue<ReceiptType>(value.Value);
        }

        /// <summary>
        ///     Get value extension
        /// </summary>
        /// <param name="type">The transaction typ</param>
        /// <returns>int</returns>
        public static byte GetValue(this ReceiptType type)
        {
            return (byte)type;
        }

    }
}
