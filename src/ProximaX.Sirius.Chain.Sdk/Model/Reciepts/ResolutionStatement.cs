using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Reciepts;
using System.Collections.Generic;

namespace ProximaX.Sirius.Chain.Sdk.Model.Receipts
{
    public class ResolutionStatement
    {
        public ResolutionStatement(ulong height, Address address,List<ResolutionEntry> resolutionEntries )
        {
            Height = height;
            Address = address;
            ResolutionEntries = resolutionEntries;
        }

        public ResolutionStatement(ulong height, MosaicId mosaicId, List<ResolutionEntry> resolutionEntries)
        {
            Height = height;
            MosaicId = mosaicId;
            ResolutionEntries = resolutionEntries;
        }

        public ulong Height { get; }
        public MosaicId MosaicId { get; }
        public Address Address { get; }
        public List<ResolutionEntry> ResolutionEntries { get; }
    }
}