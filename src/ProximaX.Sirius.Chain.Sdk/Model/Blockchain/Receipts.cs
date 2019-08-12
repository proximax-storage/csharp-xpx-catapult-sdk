using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProximaX.Sirius.Chain.Sdk.Model.Blockchain
{
    public class Receipts
    {
        public Receipts(JArray transactionStatements, JArray addressResolutionStatements,
            JArray mosaicResolutionStatements)
        {
            TransactionStatements = transactionStatements;
            AddressResolutionStatements = addressResolutionStatements;
            MosaicResolutionStatements = mosaicResolutionStatements;
        }

        public JArray TransactionStatements { get; }
        public JArray AddressResolutionStatements { get; }
        public JArray MosaicResolutionStatements { get; }

        public static Receipts FromJson(JObject json)
        {
            JArray tStatements = json["transactionStatements"].ToObject<JArray>();
            JArray aStatements = json["addressResolutionStatements"].ToObject<JArray>();
            JArray mStatements = json["mosaicResolutionStatements"].ToObject<JArray>();
            return new Receipts(tStatements, aStatements, mStatements);
        }
    }
}
