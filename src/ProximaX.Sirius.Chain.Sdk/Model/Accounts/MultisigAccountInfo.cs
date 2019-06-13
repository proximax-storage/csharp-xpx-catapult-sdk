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
    ///     The multisig account graph info structure describes the information of all the mutlisig levels an account is
    ///     involved in
    /// </summary>
    public class MultisigAccountInfo
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MultisigAccountInfo" /> class.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="minApproval">The minimum approval.</param>
        /// <param name="minRemoval">The minimum removal.</param>
        /// <param name="cosignatories">The cosignatories.</param>
        /// <param name="multisigAccounts">The multisig accounts.</param>
        public MultisigAccountInfo(PublicAccount account, int minApproval, int minRemoval,
            List<PublicAccount> cosignatories, List<PublicAccount> multisigAccounts)
        {
            Account = account;
            MinApproval = minApproval;
            MinRemoval = minRemoval;
            Cosignatories = cosignatories;
            MultisigAccounts = multisigAccounts;
        }

        /// <summary>
        ///     Returns account multisig public account.
        /// </summary>
        /// <value>The account.</value>
        public PublicAccount Account { get; }

        /// <summary>
        ///     Returns number of signatures needed to approve a transaction.
        /// </summary>
        /// <value>The minimum approval.</value>
        public int MinApproval { get; }

        /// <summary>
        ///     Returns number of signatures needed to remove a cosignatory.
        /// </summary>
        /// <value>The minimum removal.</value>
        public int MinRemoval { get; }

        /// <summary>
        ///     Returns multisig account cosignatories.
        /// </summary>
        /// <value>The cosignatories.</value>
        public List<PublicAccount> Cosignatories { get; }

        /// <summary>
        ///     Returns multisig accounts this account is cosigner of.
        /// </summary>
        /// <value>The multisig accounts.</value>
        public List<PublicAccount> MultisigAccounts { get; }

        /// <summary>
        ///     Checks if the account is a multisig account.
        /// </summary>
        /// <value><c>true</c> if this instance is multisig; otherwise, <c>false</c>.</value>
        public bool IsMultisig => MinApproval != 0 && MinRemoval != 0;

        /// <summary>
        ///     Checks if an account is cosignatory of the multisig account.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns><c>true</c> if the specified account has cosigners; otherwise, <c>false</c>.</returns>
        public bool HasCosigners(PublicAccount account)
        {
            return Cosignatories.Contains(account);
        }

        /// <summary>
        ///     Checks if the multisig account is cosignatory of an account.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <returns><c>true</c> if [is cosigner of multisig account] [the specified account]; otherwise, <c>false</c>.</returns>
        public bool IsCosignerOfMultisigAccount(PublicAccount account)
        {
            return MultisigAccounts.Contains(account);
        }

        public override string ToString()
        {
            return
                $"{nameof(Account)}: {Account}, {nameof(MinApproval)}: {MinApproval}, {nameof(MinRemoval)}: {MinRemoval}, {nameof(Cosignatories)}: {Cosignatories}, {nameof(MultisigAccounts)}: {MultisigAccounts}, {nameof(IsMultisig)}: {IsMultisig}";
        }
    }
}