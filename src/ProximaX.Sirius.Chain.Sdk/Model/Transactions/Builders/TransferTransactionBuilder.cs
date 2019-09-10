using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Namespaces;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions.Messages;
using System.Collections.Generic;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions.Builders
{
    public class TransferTransactionBuilder : TransactionBuilder<TransferTransactionBuilder, TransferTransaction>
    {
        public Recipient Recipient { get; private set; }
        public List<Mosaic> Mosaics { get; private set; }
        public IMessage Message { get; private set; }

        public TransferTransactionBuilder(EntityType entityType, int version) : base(entityType, version)
        {
            Message = EmptyMessage.Create();
            Mosaics = new List<Mosaic>();
        }

        public override TransferTransaction Build()
        {
            var maxFee = MaxFee ?? GetMaxFeeCalculation(TransferTransaction.CalculatePayloadSize(Message, Mosaics.Count));

            return new TransferTransaction(NetworkType, Version, Deadline, maxFee, Recipient, Mosaics, Message);
        }

        protected override TransferTransactionBuilder Self()
        {
            return this;
        }

        public TransferTransactionBuilder SetRecipient(Recipient recipient)
        {
            Recipient = recipient;
            return this;
        }

        public TransferTransactionBuilder SetMosaics(List<Mosaic> mosaics)
        {
            Mosaics = mosaics;
            return this;
        }

        public TransferTransactionBuilder SetMessage(IMessage message)
        {
            Message = message;
            return this;
        }

        public TransferTransactionBuilder To(Recipient recipient)
        {
            return SetRecipient(recipient);
        }

        public TransferTransactionBuilder To(Address address)
        {
            return SetRecipient(Recipient.From(address));
        }

        public TransferTransactionBuilder To(NamespaceId namespaceId)
        {
            return SetRecipient(Recipient.From(namespaceId));
        }

       
    }

}
