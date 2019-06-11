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
                .Select((t, i) => t.Serialize(bytes, 4 + i * 2))
                .Aggregate(resultBytes, Concat);
        }
    }
}