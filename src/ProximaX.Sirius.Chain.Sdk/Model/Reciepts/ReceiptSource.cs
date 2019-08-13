

using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;

namespace ProximaX.Sirius.Chain.Sdk.Model.Reciepts
{
   public class ReceiptSource
    {
        public ReceiptSource(int primaryId, int secondaryId)
        {
            PrimaryId = primaryId;
            SecondaryId = secondaryId;
        }

        public int PrimaryId { get; }
        public int SecondaryId { get; }

        public static ReceiptSource FromDto(SourceDTO receiptSourceDto)
        {
            return new ReceiptSource(receiptSourceDto.PrimaryId.Value, receiptSourceDto.SecondaryId.Value);
        }
    }
}
