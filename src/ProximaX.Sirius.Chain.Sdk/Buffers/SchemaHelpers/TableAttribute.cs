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

namespace ProximaX.Sirius.Chain.Sdk.Buffers.SchemaHelpers
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
                    .Serialize(buffer, 4 + i * 2, tableStartPosition))
                .Aggregate(resultBytes, Schema.Concat);
        }
    }
}