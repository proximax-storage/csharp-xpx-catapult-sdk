using System;
using System.Collections.Generic;
using System.Text;

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
