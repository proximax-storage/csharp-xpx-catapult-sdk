using System.Collections.Generic;
using System.Linq;

namespace io.nem2.sdk.Infrastructure.Buffers.SchemaHelpers
{
    internal class Schema
    {
        private readonly List<SchemaAttribute> _schemaDefinition;

        internal Schema(List<SchemaAttribute> schemaDefinition)
        {
            _schemaDefinition = schemaDefinition;
        }

        internal static byte[] Concat(byte[] first, byte[] second)
        {
            return first.Concat(second).ToArray();
        }

        public byte[] Serialize(byte[] bytes)
        {           
            var resultBytes = new byte[0];

            return _schemaDefinition
                .Select((t, i) => t.Serialize(bytes, 4 + (i * 2)))
                .Aggregate(resultBytes, Concat);
        }
    }
}
