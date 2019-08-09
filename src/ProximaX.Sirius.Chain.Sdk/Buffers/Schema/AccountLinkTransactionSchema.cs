using ProximaX.Sirius.Chain.Sdk.Buffers.SchemaHelpers;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProximaX.Sirius.Chain.Sdk.Buffers.Schema
{
   
    internal class AccountLinkTransactionSchema : SchemaHelpers.Schema
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AggregateTransactionSchema" /> class.
        /// </summary>
        internal AccountLinkTransactionSchema() : base(
            new List<SchemaAttribute>
            {
                new ScalarAttribute("size", Constants.Value.SIZEOF_INT),
                new ArrayAttribute("signature", Constants.Value.SIZEOF_BYTE),
                new ArrayAttribute("signer", Constants.Value.SIZEOF_BYTE),
                new ScalarAttribute("version", Constants.Value.SIZEOF_SHORT),
                new ScalarAttribute("type", Constants.Value.SIZEOF_SHORT),
                new ArrayAttribute("fee", Constants.Value.SIZEOF_INT),
                new ArrayAttribute("deadline", Constants.Value.SIZEOF_INT),
                new ArrayAttribute("remoteAccountKey", Constants.Value.SIZEOF_BYTE),
                new ScalarAttribute("linkAction", Constants.Value.SIZEOF_BYTE)
            })
        {
        }
    }
}
