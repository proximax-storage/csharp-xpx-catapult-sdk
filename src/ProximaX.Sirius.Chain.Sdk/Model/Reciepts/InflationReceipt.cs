
using Newtonsoft.Json.Linq;
using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Model.Reciepts
{
    public class InflationReceipt : Receipt
    {
        public InflationReceipt(MosaicId mosaicId, ulong amount, ReceiptVersion receiptVersion, ReceiptType receiptType, int? size) : base(receiptVersion, receiptType, size)
        {
            MosaicId = mosaicId;
            Amount = amount;
        }

        public MosaicId MosaicId { get; }
        public ulong Amount { get; }

        public static InflationReceipt FromDto(JObject dto)
        {
            var type = ReceiptTypeExtension.GetRawValue(dto["type"].ToObject<int>());
            
            var mosaicId = dto["mosaicId"].ToObject<UInt64DTO>().ToUInt64();
            var amount = dto["amount"].ToObject<UInt64DTO>().ToUInt64();
           
            return new InflationReceipt(new MosaicId(mosaicId), amount, ReceiptVersion.INFLATION_RECEIPT, type,null);
        }
    }
}
