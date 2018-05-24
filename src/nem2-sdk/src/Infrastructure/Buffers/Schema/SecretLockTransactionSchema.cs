using System.Collections.Generic;
using io.nem2.sdk.Infrastructure.Buffers.SchemaHelpers;

namespace io.nem2.sdk.Infrastructure.Buffers.Schema
{


internal class SecretLockTransactionSchema : SchemaHelpers.Schema
{
    internal SecretLockTransactionSchema() : base(
        new List<SchemaAttribute> {
            new ScalarAttribute("size", Constants.Value.SIZEOF_INT),
            new ArrayAttribute("signature", Constants.Value.SIZEOF_BYTE),
            new ArrayAttribute("signer", Constants.Value.SIZEOF_BYTE),
            new ScalarAttribute("version", Constants.Value.SIZEOF_SHORT),
            new ScalarAttribute("type", Constants.Value.SIZEOF_SHORT),
            new ArrayAttribute("fee", Constants.Value.SIZEOF_INT),
            new ArrayAttribute("deadline", Constants.Value.SIZEOF_INT),
            new ArrayAttribute("id", Constants.Value.SIZEOF_INT),
            new ArrayAttribute("amount", Constants.Value.SIZEOF_INT),
            new ArrayAttribute("duration", Constants.Value.SIZEOF_INT),
            new ScalarAttribute("hashAlgorithm", Constants.Value.SIZEOF_BYTE),
            new ArrayAttribute("secret", Constants.Value.SIZEOF_BYTE),
            new ArrayAttribute("recipient", Constants.Value.SIZEOF_BYTE),
        })
    { }
}
}
