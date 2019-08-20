using ProximaX.Sirius.Chain.Sdk.Model.Reciepts;
using System.Collections.Generic;

namespace ProximaX.Sirius.Chain.Sdk.Model.Receipts
{
    public class TransactionStatement
    {
        public TransactionStatement(ulong height, ReceiptSource source, List<Receipt> receipts)
        {
            Height = height;

            Source = source;

            Receipts = receipts;
        }

        public ulong Height { get; }
        public ReceiptSource Source { get; }
        public List<Receipt> Receipts { get; }
    }
}