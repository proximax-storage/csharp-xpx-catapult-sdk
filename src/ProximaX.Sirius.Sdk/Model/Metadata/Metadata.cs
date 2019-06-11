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

namespace ProximaX.Sirius.Sdk.Model.Metadata
{
    /// <summary>
    ///     Class Metadata
    /// </summary>
    public class Metadata
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="fields"></param>
        public Metadata(MetadataType type, IList<Field> fields)
        {
            Type = type;
            Fields = fields;
        }

        /// <summary>
        ///     The metadata type
        /// </summary>
        public MetadataType Type { get; }

        /// <summary>
        ///     The fields list
        /// </summary>
        public IList<Field> Fields { get; }
    }
}