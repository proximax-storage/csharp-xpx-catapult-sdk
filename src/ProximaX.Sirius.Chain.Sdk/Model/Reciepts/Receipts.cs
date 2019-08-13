using Newtonsoft.Json.Linq;
using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;

using System;
using System.Collections.Generic;


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
            


            return new Receipts(tStatements, aStatements, mStatements);
        }
    }
}
