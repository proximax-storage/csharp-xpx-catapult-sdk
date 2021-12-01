﻿// Copyright 2021 ProximaX
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

namespace ProximaX.Sirius.Chain.Sdk.Model.Accounts
{
    /// <summary>
    ///     Class of AccountPropertiesInfo
    /// </summary>
    public class AccountPropertiesInfo
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="AccountPropertiesInfo" /> class.
        /// </summary>
        /// <param name="accountProperties">The account properties</param>
        public AccountPropertiesInfo(AccountProperties accountProperties)
        {
            AccountProperties = accountProperties;
        }

        /// <summary>
        /// The account properties
        /// </summary>
        public AccountProperties AccountProperties { get; }

        /// <summary>
        ///     ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{nameof(AccountProperties)}: {AccountProperties}";
        }
    }
}