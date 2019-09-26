
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
