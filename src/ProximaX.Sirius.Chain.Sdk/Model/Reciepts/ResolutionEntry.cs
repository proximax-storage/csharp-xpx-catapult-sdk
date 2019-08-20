using Newtonsoft.Json.Linq;
using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Namespaces;
using ProximaX.Sirius.Chain.Sdk.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProximaX.Sirius.Chain.Sdk.Model.Reciepts
{
   public class ResolutionEntry
    {
        public ResolutionEntry(Alias resolved, ReceiptSource source)
        {
            Resolved = resolved;
            Source = source;
        }

        public Alias Resolved { get; }
        public ReceiptSource Source { get; }

        public static ResolutionEntry FromDto(ResolutionEntryDTO dto, bool isMosaic)
        {
            if(isMosaic)
            {
                var alias = new Alias(AliasType.MOSAIC_ID, null, new MosaicId(dto.Resolved.ToUInt64()));
                return new ResolutionEntry(alias, ReceiptSource.FromDto(dto.Source));
               
            } else
            {
                var alias = new Alias(AliasType.ADDRESS, Address.CreateFromHex(dto.Resolved.ToUInt64().ToHex()), null);
                return new ResolutionEntry(alias, ReceiptSource.FromDto(dto.Source));
            }
        }
    }
}
