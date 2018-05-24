using System.Collections.Generic;
using System.Linq;

namespace io.nem2.sdk.Infrastructure.Buffers.SchemaHelpers
{
    internal class TableAttribute : SchemaAttribute
    {
        private readonly List<SchemaAttribute> _schema;

        internal TableAttribute(string name, List<SchemaAttribute> schema) : base(name)
        {
            _schema = schema;
        }

        internal override byte[] Serialize(byte[] buffer, int position, int innerObjectPosition)
        {
            var resultBytes = new byte[0];
            var tableStartPosition = FindObjectStartPosition(innerObjectPosition, position, buffer);
            return _schema
                    .Select((t, i) => _schema[i]
                    .Serialize(buffer, 4 + (i * 2), tableStartPosition))
                    .Aggregate(resultBytes, Schema.Concat);
        }
    }
}
