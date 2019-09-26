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

        public TransferTransactionBuilder() : base(EntityType.TRANSFER, EntityVersion.TRANSFER.GetValue())
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
