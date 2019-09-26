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

using ProximaX.Sirius.Chain.Sdk.Model.Namespaces;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions.Builders
{
    public class RegisterNamespaceTransactionBuilder : TransactionBuilder<RegisterNamespaceTransactionBuilder, RegisterNamespaceTransaction>
    {

        public string NamespaceName { get; private set; }
        public NamespaceId NamespaceId { get; private set; }

        public NamespaceId ParentId { get; private set; }
        public NamespaceType NamespaceType { get; private set; }


        public RegisterNamespaceTransactionBuilder() : base(EntityType.REGISTER_NAMESPACE, EntityVersion.REGISTER_NAMESPACE.GetValue())
        {
        }

        public override RegisterNamespaceTransaction Build()
        {
            var maxFee = MaxFee ?? GetMaxFeeCalculation(RegisterNamespaceTransaction.CalculatePayloadSize(NamespaceName.Length));

            return new RegisterNamespaceTransaction(NetworkType, Version, Deadline, maxFee, NamespaceName, NamespaceId, NamespaceType, Duration);
        }

        protected override RegisterNamespaceTransactionBuilder Self()
        {
            return this;
        }

        public RegisterNamespaceTransactionBuilder SetNamespaceName(string namespaceName)
        {
            NamespaceName = namespaceName;
            return Self();
        }

        public RegisterNamespaceTransactionBuilder SetNamespaceId(NamespaceId namespaceId)
        {
            NamespaceId = namespaceId;
            return Self();
        }
        public RegisterNamespaceTransactionBuilder SetParentId(NamespaceId parentId)
        {
            ParentId = parentId;
            return Self();
        }

        public RegisterNamespaceTransactionBuilder SetNamespaceType(NamespaceType namespaceType)
        {
            NamespaceType = namespaceType;
            return Self();
        }

        public RegisterNamespaceTransactionBuilder SetRootNamespace(string name)
        {
            NamespaceType = NamespaceType.ROOT_NAMESPACE;
            SetNamespaceName(name);
            SetNamespaceId(new NamespaceId(name));
           
            return Self();
        }

        public RegisterNamespaceTransactionBuilder SetSubNamespace(NamespaceId parentId, string name)
        {
            NamespaceType = NamespaceType.SUB_NAMESPACE;
            SetNamespaceName(name);
            SetParentId(parentId);
            var subNamespace = IdGenerator.GenerateSubNamespaceIdFromParentId(parentId.Id, name);
            SetNamespaceId(new NamespaceId(subNamespace));
           
            return Self();
        }
    }
}
