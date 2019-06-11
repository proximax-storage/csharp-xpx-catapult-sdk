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

using GuardNet;

namespace ProximaX.Sirius.Sdk.Model.Metadata
{
    /// <summary>
    ///     Class Field
    /// </summary>
    public class Field
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public Field(string key, string value = null)
        {
            Guard.NotNullOrEmpty(key, nameof(key));

            Key = key;
            Value = value;
        }

        /// <summary>
        ///     The field key
        /// </summary>
        public string Key { get; }

        /// <summary>
        ///     The field value
        /// </summary>
        public string Value { get; }

        /// <summary>
        ///     Determines field has value
        /// </summary>
        public bool HasValue => Value != null;
    }
}