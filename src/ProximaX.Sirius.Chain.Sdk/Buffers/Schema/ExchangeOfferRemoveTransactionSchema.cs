using ProximaX.Sirius.Chain.Sdk.Buffers.SchemaHelpers;
using System.Collections.Generic;

namespace ProximaX.Sirius.Chain.Sdk.Buffers.Schema
{
    internal class ExchangeOfferRemoveTransactionSchema : SchemaHelpers.Schema
    {
        internal ExchangeOfferRemoveTransactionSchema() : base(
       new List<SchemaAttribute>
       {
                new ScalarAttribute("size", Constants.Value.SIZEOF_INT),
                new ArrayAttribute("signature", Constants.Value.SIZEOF_BYTE),
                new ArrayAttribute("signer", Constants.Value.SIZEOF_BYTE),
                new ScalarAttribute("version", Constants.Value.SIZEOF_INT),
                new ScalarAttribute("type", Constants.Value.SIZEOF_SHORT),
                new ArrayAttribute("maxFee", Constants.Value.SIZEOF_INT),
                new ArrayAttribute("deadline", Constants.Value.SIZEOF_INT),
                new ScalarAttribute("offersCount", Constants.Value.SIZEOF_BYTE),
                new TableArrayAttribute("offers", new List<SchemaAttribute>
                {
                    new ArrayAttribute("mosaicId", Constants.Value.SIZEOF_INT),
                    new ScalarAttribute("type", Constants.Value.SIZEOF_BYTE),
                })
       })
        {
        }
    }
}
