using Newtonsoft.Json.Linq;
using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Reciepts;
using ProximaX.Sirius.Chain.Sdk.Utils;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ProximaX.Sirius.Chain.Sdk.Model.Receipts
{
    public class Receipts
    {
        public Receipts(List<TransactionStatement> transactionStatements, List<ResolutionStatement> addressResolutionStatements,
             List<ResolutionStatement> mosaicResolutionStatements)
        {
            TransactionStatement = transactionStatements;
            AddressResolutionStatements = addressResolutionStatements;
            MosaicResolutionStatements = mosaicResolutionStatements;
        }

        public List<TransactionStatement> TransactionStatement { get; }
        public List<ResolutionStatement> AddressResolutionStatements { get; }
        public List<ResolutionStatement> MosaicResolutionStatements { get; }

        public static Receipts FromDto(StatementsDTO statementsDTO)
        {
            List<TransactionStatement> tStatements = new List<TransactionStatement>();
            List<ResolutionStatement> aStatements = new List<ResolutionStatement>();
            List<ResolutionStatement> mStatements = new List<ResolutionStatement>();
           
          
            foreach(var tsDto in statementsDTO.TransactionStatements)
            {
                List<Receipt> receipts = (from r in tsDto.Receipts
                                          select Receipt.FromDto(r as JObject)).ToList();

                var ts = new TransactionStatement(tsDto.Height.ToUInt64(), ReceiptSource.FromDto(tsDto.Source), receipts);
                tStatements.Add(ts);
            }

            foreach (var mrsDto in statementsDTO.AddressResolutionStatements)
            {
                List<ResolutionEntry> resolutionEntries = (from ResolutionEntryDTO r in mrsDto.ResolutionEntries
                                                           let re = ResolutionEntry.FromDto(r, false)
                                                           select re).ToList();

                var address = Address.CreateFromHex(mrsDto.Unresolved.ToUInt64().ToHex());

                var rs = new ResolutionStatement(mrsDto.Height.ToUInt64(), address, resolutionEntries);

                mStatements.Add(rs);
            }

            foreach (var mrsDto in statementsDTO.MosaicResolutionStatements)
            {
                List<ResolutionEntry> resolutionEntries = (from ResolutionEntryDTO r in mrsDto.ResolutionEntries
                                                           let re = ResolutionEntry.FromDto(r, true)
                                                           select re).ToList();

                var mosaicId = new MosaicId(mrsDto.Unresolved.ToUInt64());
                
                var rs = new ResolutionStatement(mrsDto.Height.ToUInt64(), mosaicId, resolutionEntries);

                mStatements.Add(rs);
            }

            return new Receipts(tStatements, aStatements, mStatements);
        }
    }
}
