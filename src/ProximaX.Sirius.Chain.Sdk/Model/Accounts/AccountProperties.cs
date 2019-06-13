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
    ///     Account properties structure describes property information for an account.
    /// </summary>
    public class AccountProperties
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AccountProperties" /> class.
        /// </summary>
        /// <param name="address">The account address</param>
        /// <param name="properties">The account properties</param>
        public AccountProperties(Address address, List<AccountProperty> properties)
        {
            Address = address;
            Properties = properties;
        }

        /// <summary>
        ///     Account Address
        /// </summary>
        public Address Address { get; }

        /// <summary>
        ///     Account Properties
        /// </summary>
        public List<AccountProperty> Properties { get; }

        /// <summary>
        ///     ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{nameof(Address)}: {Address}, {nameof(Properties)}: {Properties}";
        }
    }
}