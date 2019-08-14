using Newtonsoft.Json.Linq;
using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Namespaces;
using ProximaX.Sirius.Chain.Sdk.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProximaX.Sirius.Chain.Sdk.Model.Reciepts
{
    public class ArtifactExpiryReceipt : Receipt
    {
        public ArtifactExpiryReceipt(MosaicId mosaicId , ReceiptVersion receiptVersion, ReceiptType receiptType, int? size) : base(receiptVersion, receiptType, size)
        {
            MosaicId = mosaicId;
        }

        public ArtifactExpiryReceipt(NamespaceId namespaceId, ReceiptVersion receiptVersion, ReceiptType receiptType, int? size) : base(receiptVersion, receiptType, size)
        {
            NamespaceId = namespaceId;
        }

        public MosaicId MosaicId { get; }

        public NamespaceId NamespaceId { get; }

        public static ArtifactExpiryReceipt FromDto(JObject dto)
        {
            var type = ReceiptTypeExtension.GetRawValue(dto["type"].ToObject<int>());
      
            switch(type)
            {
                case ReceiptType.Mosaic_Expired: //Mosaic_Expired
                    var mosaicId = dto["mosaicId"].ToObject<UInt64DTO>().ToUInt64();
                    return new ArtifactExpiryReceipt(new MosaicId(mosaicId), ReceiptVersion.ARTIFACT_EXPIRY, type, null);
                    
                case ReceiptType.Namespace_Expired: //Namespace_Expired
                    var namespaceId = dto["namespaceId"].ToObject<UInt64DTO>().ToUInt64();
                    return new ArtifactExpiryReceipt(new NamespaceId(namespaceId), ReceiptVersion.ARTIFACT_EXPIRY, type, null);
                default:
                    throw new Exception("Reciept Type is not support");
            }
        }
    }
}
