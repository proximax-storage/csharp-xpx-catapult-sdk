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


    }
}
