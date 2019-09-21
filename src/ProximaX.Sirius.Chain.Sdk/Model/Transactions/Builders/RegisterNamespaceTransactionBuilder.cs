using ProximaX.Sirius.Chain.Sdk.Model.Namespaces;
using ProximaX.Sirius.Chain.Sdk.Utils;
using System;
using System.Collections.Generic;
using System.Text;

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
