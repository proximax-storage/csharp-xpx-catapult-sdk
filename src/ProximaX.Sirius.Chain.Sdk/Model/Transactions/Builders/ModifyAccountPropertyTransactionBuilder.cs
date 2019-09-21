using ProximaX.Sirius.Chain.Sdk.Model.Accounts;

using System.Collections.Generic;


namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions.Builders
{
    public abstract class ModifyAccountPropertyTransactionBuilder<T> : TransactionBuilder<ModifyAccountPropertyTransactionBuilder<T>, ModifyAccountPropertyTransaction<T>>
    {
        public PropertyType PropertyType { get; private set; }

        public IList<AccountPropertyModification<T>> Modifications;

        public ModifyAccountPropertyTransactionBuilder(EntityType entityType, int version) : base(entityType, version)
        {
        }

    
        protected override ModifyAccountPropertyTransactionBuilder<T> Self()
        {
            return this;
        }

        public ModifyAccountPropertyTransactionBuilder<T> propertyType(PropertyType propertyType)
        {
            PropertyType = propertyType;
            return Self();
        }

        public ModifyAccountPropertyTransactionBuilder<T> SetModifications(List<AccountPropertyModification<T>> modifications)
        {
            Modifications = modifications;
            return Self();
        }

    }
}
