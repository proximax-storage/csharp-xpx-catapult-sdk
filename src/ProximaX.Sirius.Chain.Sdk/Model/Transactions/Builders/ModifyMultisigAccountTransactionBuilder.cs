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

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions.Builders
{
    public class ModifyMultisigAccountTransactionBuilder : TransactionBuilder<ModifyMultisigAccountTransactionBuilder, ModifyMultisigAccountTransaction>
    {
        public int MinApprovalDelta { get; private set; }
        public int MinRemovalDelta { get; private set; }
        public IList<MultisigCosignatoryModification> Modifications { get; private set; }

        public ModifyMultisigAccountTransactionBuilder() : base(EntityType.MODIFY_MULTISIG_ACCOUNT, EntityVersion.MODIFY_MULTISIG_ACCOUNT.GetValue())
        {
            MinApprovalDelta = 0;
            MinRemovalDelta = 0;
            Modifications = new List<MultisigCosignatoryModification>();
        }

        public override ModifyMultisigAccountTransaction Build()
        {
            var maxFee = MaxFee ?? GetMaxFeeCalculation(ModifyMultisigAccountTransaction.CalculatePayloadSize(Modifications.Count));

            return new ModifyMultisigAccountTransaction(NetworkType, Version, Deadline, maxFee, MinApprovalDelta, MinRemovalDelta, Modifications);
        }

        protected override ModifyMultisigAccountTransactionBuilder Self()
        {
            return this;
        }

        public ModifyMultisigAccountTransactionBuilder SetMinApprovalDelta(int minApprovalDelta)
        {
            MinApprovalDelta = minApprovalDelta;
            return Self();
        }

        public ModifyMultisigAccountTransactionBuilder SetMinRemovalDelta(int minRemovalDelta)
        {
            MinRemovalDelta = minRemovalDelta;
            return Self();
        }

        public ModifyMultisigAccountTransactionBuilder SetModifications(IList<MultisigCosignatoryModification> modifications)
        {
            Modifications = modifications;
            return Self();
        }
    }
}
