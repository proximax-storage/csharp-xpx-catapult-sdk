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

using System.Text;
using GuardNet;

namespace ProximaX.Sirius.Sdk.Model.Metadata
{
    /// <summary>
    ///     Class MetadataModification
    /// </summary>
    public class MetadataModification
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="type"></param>
        /// <param name="field"></param>
        public MetadataModification(MetadataModificationType type, Field field)
        {
            Guard.NotNull(field, nameof(field), "field could not be null");
            Type = type;
            Field = field;
        }

        /// <summary>
        ///     The metadata modification type
        /// </summary>
        public MetadataModificationType Type { get; }

        /// <summary>
        ///     The metadata field
        /// </summary>
        public Field Field { get; }

        /// <summary>
        ///     ADD metadata field
        /// </summary>
        /// <param name="key">The field key</param>
        /// <param name="value">The field value</param>
        /// <returns></returns>
        public static MetadataModification Add(string key, string value)
        {
            Guard.NotNullOrEmpty(key, nameof(key), "Key could not be null");
            Guard.NotNullOrEmpty(value, nameof(value), "Value could not be null");
            Guard.NotGreaterThan(Encoding.UTF8.GetBytes(key).Length, 128, nameof(key),
                "The key could not excess 128 bytes");
            Guard.NotGreaterThan(Encoding.UTF8.GetBytes(value).Length, 1024, nameof(value),
                "The value could not excess 1024 bytes");
            return new MetadataModification(MetadataModificationType.ADD, new Field(key, value));
        }

        /// <summary>
        ///     REMOVE metadata field
        /// </summary>
        /// <param name="key">The field key</param>
        /// <returns></returns>
        public static MetadataModification Remove(string key)
        {
            Guard.NotNullOrEmpty(key, nameof(key), "Key could not be null");

            return new MetadataModification(MetadataModificationType.REMOVE, new Field(key));
        }
    }
}