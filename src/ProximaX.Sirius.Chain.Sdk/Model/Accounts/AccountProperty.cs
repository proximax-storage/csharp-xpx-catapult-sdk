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

namespace ProximaX.Sirius.Chain.Sdk.Model.Accounts
{
    /// <summary>
    ///     Account property structure describes property information.
    /// </summary>
    public class AccountProperty
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AccountProperty" /> class.
        /// </summary>
        /// <param name="propertyType"></param>
        /// <param name="values"></param>
        public AccountProperty(PropertyType propertyType, List<object> values)
        {
            PropertyType = propertyType;
            Values = values;
        }

        /// <summary>
        ///     Account property type
        /// </summary>
        public PropertyType PropertyType { get; }

        /// <summary>
        ///     Property values.
        /// </summary>
        public List<object> Values { get; }

        /// <summary>
        ///     ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{nameof(PropertyType)}: {PropertyType}, {nameof(Values)}: {Values}";
        }
    }
}