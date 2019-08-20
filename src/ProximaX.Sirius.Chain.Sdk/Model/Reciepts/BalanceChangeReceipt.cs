using Newtonsoft.Json.Linq;
using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Model.Reciepts
{
    public class BalanceChangeReceipt : Receipt
    {
        public BalanceChangeReceipt(PublicAccount account, MosaicId mosaicId, ulong amount, ReceiptVersion receiptVersion, ReceiptType receiptType, int? size) : base(receiptVersion, receiptType, size)
        {
            Account = account;
            MosaicId = mosaicId;
            Amount = amount;
        }

        public PublicAccount Account { get; }
        public MosaicId MosaicId { get; }
        public ulong Amount { get; }

        public static BalanceChangeReceipt FromDto(JObject dto, NetworkType networkType)
        {
            var type = ReceiptTypeExtension.GetRawValue(dto["type"].ToObject<int>());
            var account = PublicAccount.CreateFromPublicKey(dto["account"].ToObject<string>(), networkType);
            var mosaicId = dto["mosaicId"].ToObject<UInt64DTO>().ToUInt64();
            var amount = dto["amount"].ToObject<UInt64DTO>().ToUInt64();

            return new BalanceChangeReceipt(account, new MosaicId(mosaicId), amount, ReceiptVersion.BALANCE_CHANGE, type, null);
        }
    }
}
