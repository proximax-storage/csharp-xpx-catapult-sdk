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

namespace ProximaX.Sirius.Sdk.Model.Accounts
{
    /// <summary>
    ///     AccountMeta
    /// </summary>
    public class AccountMeta
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AccountMeta" /> class.
        /// </summary>
        /// <param name="type">The account meta type</param>
        /// <param name="value">The account meta value</param>
        public AccountMeta(string type, object value)
        {
            Type = type;
            Value = value;
        }

        /// <summary>
        ///     The account meta type
        /// </summary>
        public string Type { get; }

        /// <summary>
        ///     The account meta value
        /// </summary>
        public object Value { get; }

        /// <summary>
        ///     ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{nameof(Type)}: {Type}, {nameof(Value)}: {Value}";
        }
    }
}