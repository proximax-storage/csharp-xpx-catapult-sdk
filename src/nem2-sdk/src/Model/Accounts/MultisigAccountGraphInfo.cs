// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="MultisigAccountGraphInfo.cs" company="Nem.io">   
// Copyright 2018 NEM
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
// </copyright>
// <summary></summary>
// ***********************************************************************

using System.Collections.Generic;

namespace io.nem2.sdk.Model.Accounts
{
    /// <summary>
    /// Class MultisigAccountGraphInfo.
    /// </summary>
    public class MultisigAccountGraphInfo
    {
        /// <summary>
        /// Returns multisig accounts.
        /// </summary>
        /// <value>The multisig accounts.</value>
        public Dictionary<int, List<MultisigAccountInfo>> MultisigAccounts { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MultisigAccountGraphInfo"/> class.
        /// </summary>
        /// <param name="multisigAccounts">The multisig accounts.</param>
        public MultisigAccountGraphInfo(Dictionary<int, List<MultisigAccountInfo>> multisigAccounts)
        {
            MultisigAccounts = multisigAccounts;
        }

        /// <summary>
        /// Returns multisig accounts levels number.
        /// </summary>
        /// <returns>Dictionary`2.KeyCollection.</returns>
        public Dictionary<int, List<MultisigAccountInfo>>.KeyCollection GetLevelsNumber()
        {
            return MultisigAccounts.Keys;
        }
    }
}
