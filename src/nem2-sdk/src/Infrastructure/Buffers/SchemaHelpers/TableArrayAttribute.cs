using System.Collections.Generic;
using System.Linq;

namespace io.nem2.sdk.Infrastructure.Buffers.SchemaHelpers
{
    internal class TableArrayAttribute : SchemaAttribute
    {
        private readonly List<SchemaAttribute> _schema;

        internal TableArrayAttribute(string name, List<SchemaAttribute> schema) : base(name)
        {
            _schema = schema;
        }

        internal override byte[] Serialize(byte[] buffer, int position, int innerObjectPosition)
        {
            var resultBytes = new byte[0];

            var arrayLength = FindArrayLength(innerObjectPosition, position, buffer);

            for (var i = 0; i < arrayLength; i++)
            {
                var startArrayPosition = FindObjectArrayElementStartPosition(innerObjectPosition, position, buffer, i);

                resultBytes = _schema.Select((t, j) => 
                    t.Serialize(buffer, 4 + (j * 2), startArrayPosition))
                    .Aggregate(resultBytes, Schema.Concat);
            }

            return resultBytes;
        }
    }
}

