
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

using ProximaX.Sirius.Chain.Sdk.Model.Accounts;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions.Builders
{
    public class AccountLinkTransactionBuilder : TransactionBuilder<AccountLinkTransactionBuilder, AccountLinkTransaction>
    {
        public PublicAccount RemoteAccount { get; private set; }
        public AccountLinkAction Action { get; private set; }

        public AccountLinkTransactionBuilder(): base(EntityType.LINK_ACCOUNT, EntityVersion.LINK_ACCOUNT.GetValue())
        {
           
        }

        public AccountLinkTransactionBuilder(EntityType entityType, int version) : base(entityType, version)
        {
        }

        public AccountLinkTransactionBuilder SetRemoteAccount(PublicAccount remoteAccount)
        {
            RemoteAccount = remoteAccount;
            return Self();
        }

        public AccountLinkTransactionBuilder SetAction(AccountLinkAction action)
        {
            Action = action;
            return Self();
        }

        public override AccountLinkTransaction Build()
        {
            var maxFee = MaxFee ?? GetMaxFeeCalculation(AccountLinkTransaction.CalculatePayloadSize());

            return new AccountLinkTransaction(NetworkType, Version, Deadline, maxFee, RemoteAccount, Action);
        }

        protected override AccountLinkTransactionBuilder Self()
        {
            return this;
        }

        public AccountLinkTransactionBuilder Link(PublicAccount remoteAccount)
        {
            return SetAction(AccountLinkAction.LINK).SetRemoteAccount(remoteAccount);
        }

        public AccountLinkTransactionBuilder UnLink(PublicAccount remoteAccount)
        {
            return SetAction(AccountLinkAction.UNLINK).SetRemoteAccount(remoteAccount);
        }
    }
}
