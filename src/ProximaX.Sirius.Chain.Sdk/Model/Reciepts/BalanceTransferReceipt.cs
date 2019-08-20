using Newtonsoft.Json.Linq;
using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Namespaces;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Model.Reciepts
{
    public class BalanceTransferReceipt : Receipt
    {
        public BalanceTransferReceipt(PublicAccount sender, Address address, MosaicId mosaicId,ulong amount, ReceiptVersion receiptVersion, ReceiptType receiptType, int? size) : base(receiptVersion, receiptType, size)
        {
            Sender = sender;
            Address = address;
            MosaicId = mosaicId;
        }

        public BalanceTransferReceipt(PublicAccount sender, NamespaceId namespaceId, MosaicId mosaicId, ulong amount, ReceiptVersion receiptVersion, ReceiptType receiptType, int? size) : base(receiptVersion, receiptType, size)
        {
            Sender = sender;
            NamespaceId = namespaceId;
            MosaicId = mosaicId;
        }

        public PublicAccount Sender { get; }
        public Address Address { get; }

        public NamespaceId NamespaceId { get; }

        public MosaicId MosaicId { get; }

       public static BalanceTransferReceipt FromDto(JObject dto, NetworkType networkType)
        {
            var type = ReceiptTypeExtension.GetRawValue(dto["type"].ToObject<int>());
            var account = PublicAccount.CreateFromPublicKey(dto["account"].ToObject<string>(), networkType);
            var mosaicId = dto["mosaicId"].ToObject<UInt64DTO>().ToUInt64();
            var amount = dto["amount"].ToObject<UInt64DTO>().ToUInt64();
            var address = Address.CreateFromHex("");

            return new BalanceTransferReceipt(account, address,new MosaicId(mosaicId), amount, ReceiptVersion.BALANCE_TRANSFER, type,null);
            
        }
    }
}
