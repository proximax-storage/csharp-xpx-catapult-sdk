// Copyright 2019 ProximaX
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Collections.Generic;
using System.Linq;

namespace ProximaX.Sirius.Sdk.Buffers.SchemaHelpers
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
                        t.Serialize(buffer, 4 + j * 2, startArrayPosition))
                    .Aggregate(resultBytes, Schema.Concat);
            }

            return resultBytes;
        }
    }
}