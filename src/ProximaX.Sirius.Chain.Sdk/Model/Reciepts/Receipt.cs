using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProximaX.Sirius.Chain.Sdk.Model.Reciepts
{
    public class Receipt
    {
        public Receipt(ReceiptVersion receiptVersion, ReceiptType receiptType, int? size)
        {
            ReceiptVersion = receiptVersion;
            ReceiptType = receiptType;
            Size = size;
        }

        public ReceiptVersion ReceiptVersion { get; }
        public ReceiptType ReceiptType { get; }
        public int? Size { get; }

        public static Receipt FromDto(JObject dto)
        {
            var type = ReceiptTypeExtension.GetRawValue(dto["type"].ToObject<int>());

            switch(type)
            {
                case ReceiptType.Harvest_Fee:
                case ReceiptType.LockHash_Created:
                case ReceiptType.LockHash_Completed:
                case ReceiptType.LockHash_Expired:
                case ReceiptType.LockSecret_Created:
                case ReceiptType.LockSecret_Completed:
                case ReceiptType.LockSecret_Expired:
                    return new Receipt(ReceiptVersion.BALANCE_CHANGE, type, null);
                case ReceiptType.Mosaic_Levy:
                case ReceiptType.Mosaic_Rental_Fee:
                case ReceiptType.Namespace_Rental_Fee:
                    return new Receipt(ReceiptVersion.BALANCE_TRANSFER, type, null);
                case ReceiptType.Mosaic_Expired:
                case ReceiptType.Namespace_Expired:
                    return new Receipt(ReceiptVersion.ARTIFACT_EXPIRY, type, null);
                case ReceiptType.Inflation:
                    return new Receipt(ReceiptVersion.INFLATION_RECEIPT, type, null);
            }

            throw new Exception("Unsupport Receipt");
        }
    }
}
