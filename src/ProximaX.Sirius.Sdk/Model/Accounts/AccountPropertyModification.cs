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
    ///     Class AccountPropertyModification<T />
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AccountPropertyModification<T>
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AccountPropertyModification&lt;T&gt;" /> class.
        /// </summary>
        /// <param name="type">The property modification type</param>
        /// <param name="value"></param>
        public AccountPropertyModification(PropertyModificationType type, T value)
        {
            Type = type;
            Value = value;
        }

        /// <summary>
        ///     PropertyModificationType
        /// </summary>
        public PropertyModificationType Type { get; }

        /// <summary>
        ///     Value
        /// </summary>
        public T Value { get; }

        /// <summary>
        ///     ADD value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static AccountPropertyModification<T> Add(T value)
        {
            return new AccountPropertyModification<T>(PropertyModificationType.ADD, value);
        }

        /// <summary>
        ///     REMOVE value
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static AccountPropertyModification<T> Remove(T value)
        {
            return new AccountPropertyModification<T>(PropertyModificationType.REMOVE, value);
        }

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