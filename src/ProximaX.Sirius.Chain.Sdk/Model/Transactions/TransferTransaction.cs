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
using System.Runtime.Serialization;
using FlatBuffers;
using GuardNet;
using ProximaX.Sirius.Chain.Sdk.Buffers;
using ProximaX.Sirius.Chain.Sdk.Buffers.Schema;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Namespaces;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions.Messages;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions
{
    public class TransferTransaction : Transaction
    {
        /*
     public TransferTransaction(NetworkType networkType, int version, Deadline deadline, ulong? maxFee,
         Address recipient, List<Mosaic> mosaics, IMessage message, string signature = null,
         PublicAccount signer = null, TransactionInfo transactionInfo = null)
         : base(networkType, version, TransactionType.TRANSFER, deadline, maxFee, signature, signer, transactionInfo)
     {
         Address = recipient ?? throw new ArgumentNullException(nameof(recipient));
         Message = message ?? EmptyMessage.Create();
         Mosaics = mosaics;
     }


     public TransferTransaction(NetworkType networkType, int version, Deadline deadline, ulong? maxFee,
         NamespaceId recipient, List<Mosaic> mosaics, IMessage message, string signature = null,
         PublicAccount signer = null, TransactionInfo transactionInfo = null)
         : base(networkType, version, TransactionType.TRANSFER, deadline, maxFee, signature, signer, transactionInfo)
     {
         NamespaceId = recipient ?? throw new ArgumentNullException(nameof(recipient));
         Message = message ?? EmptyMessage.Create();
         Mosaics = mosaics;
     }
     */
        public TransferTransaction(NetworkType networkType, int version, Deadline deadline, ulong? maxFee,
          Recipient recipient, IList<Mosaic> mosaics, IMessage message, string signature = null,
          PublicAccount signer = null, TransactionInfo transactionInfo = null)
          : base(networkType, version, EntityType.TRANSFER, deadline, maxFee, signature, signer, transactionInfo)
        {
            Guard.NotNull(recipient, nameof(recipient), "Recipient must not be null");
            Guard.NotNull(recipient, nameof(message), "Message must not be null");
            Guard.NotNull(recipient, nameof(mosaics), "Mosaics must not be null");
            Recipient = recipient;
            Message = message;
            Mosaics = mosaics;
        }

        public Recipient Recipient { get; }

        /// <summary>
        ///     Gets the address.
        /// </summary>
        /// <value>The address.</value>
        //public Address Address { get; }

        /// <summary>
        ///  Get the namespace id
        /// </summary>
        //public NamespaceId NamespaceId { get; }

        /// <summary>
        ///     Gets or sets the message.
        /// </summary>
        /// <value>The message.</value>
        public IMessage Message { get; private set; }

        /// <summary>
        ///     Gets the mosaics.
        /// </summary>
        /// <value>The mosaics.</value>
        public IList<Mosaic> Mosaics { get; }


        public static TransferTransaction Create(Deadline deadline, Address recipient, List<Mosaic> mosaics,
            IMessage message, NetworkType networkType, ulong? maxFee = 0)
        {
            return new TransferTransaction(networkType, EntityVersion.TRANSFER.GetValue(), deadline, maxFee,
                Recipient.From(recipient), mosaics, message);
        }

        public static TransferTransaction Create(Deadline deadline, NamespaceId recipient, List<Mosaic> mosaics,
            IMessage message, NetworkType networkType, ulong? maxFee = 0)
        {
            return new TransferTransaction(networkType, EntityVersion.TRANSFER.GetValue(), deadline, maxFee,
                Recipient.From(recipient), mosaics, message);
        }

        public static TransferTransaction Create(Deadline deadline, Recipient recipient, List<Mosaic> mosaics,
       IMessage message, NetworkType networkType, ulong? maxFee = 0)
        {
            return new TransferTransaction(networkType, EntityVersion.TRANSFER.GetValue(), deadline, maxFee,
                recipient, mosaics, message);
        }

        protected override int GetPayloadSerializedSize()
        {
            return CalculatePayloadSize(Message, Mosaics.Count);
        }

        public static int CalculatePayloadSize(IMessage message, int mosaicCount)
        {
            return
            // recipient is always 25 bytes
            25 +
                  // message size is short
                  2 +
                  // message type byte
                  1 +
                  // number of mosaics
                  1 +
                  // each mosaic has id and amount, both 8byte uint64
                  ((8 + 8) * mosaicCount) +
                  // number of message bytes
                  message.GetPayload().Length;
        }

        internal override byte[] GenerateBytes()
        {
            var builder = new FlatBufferBuilder(1);

            // create message
            var bytePayload = Message.GetPayload();
            var payload = MessageBuffer.CreatePayloadVector(builder, bytePayload);
            MessageBuffer.StartMessageBuffer(builder);
            MessageBuffer.AddType(builder, Message.GetMessageType());
            MessageBuffer.AddPayload(builder, payload);
            var messageVector = MessageBuffer.EndMessageBuffer(builder);

            // create mosaics
            var mosaicBuffers = new Offset<MosaicBuffer>[Mosaics.Count];
            for (var index = 0; index < Mosaics.Count; ++index)
            {
                var mosaic = Mosaics[index];
                var id = MosaicBuffer.CreateIdVector(builder, mosaic.Id.Id.ToUInt8Array());
                var amount = MosaicBuffer.CreateAmountVector(builder, mosaic.Amount.ToUInt8Array());
                MosaicBuffer.StartMosaicBuffer(builder);
                MosaicBuffer.AddId(builder, id);
                MosaicBuffer.AddAmount(builder, amount);
                mosaicBuffers[index] = MosaicBuffer.EndMosaicBuffer(builder);
            }

            // create recipient
            byte[] recipientBytes = Recipient.GetBytes();

            // create vectors
            var signatureVector = TransferTransactionBuffer.CreateSignatureVector(builder, new byte[64]);
            var signerVector = TransferTransactionBuffer.CreateSignerVector(builder, GetSigner());
            var deadlineVector = TransferTransactionBuffer.CreateDeadlineVector(builder, Deadline.Ticks.ToUInt8Array());
            var feeVector = TransferTransactionBuffer.CreateMaxFeeVector(builder, MaxFee?.ToUInt8Array());
            var recipientVector =
               TransferTransactionBuffer.CreateRecipientVector(builder, recipientBytes);
            var mosaicsVector = TransferTransactionBuffer.CreateMosaicsVector(builder, mosaicBuffers);

            // total size of transaction
            /*var totalSize = HEADER_SIZE
                + 25 // recipient
                + 2 // message size is short
                + 1 // message type byte
                + 1 // no of mosaics
                + ((8 + 8) * Mosaics.Count) //each mosaic has id(8bytes) and amount(8bytes)
                + bytePayload.Length; // number of message bytes*/

            var totalSize = GetSerializedSize();

            // create version
            var version = GetTxVersionSerialization();


            // add vectors
            TransferTransactionBuffer.StartTransferTransactionBuffer(builder);
            TransferTransactionBuffer.AddSize(builder, (uint)totalSize);
            TransferTransactionBuffer.AddSignature(builder, signatureVector);
            TransferTransactionBuffer.AddSigner(builder, signerVector);
            TransferTransactionBuffer.AddVersion(builder, (uint)version);
            TransferTransactionBuffer.AddType(builder, EntityType.TRANSFER.GetValue());
            TransferTransactionBuffer.AddMaxFee(builder, feeVector);
            TransferTransactionBuffer.AddDeadline(builder, deadlineVector);

            TransferTransactionBuffer.AddRecipient(builder, recipientVector);
            TransferTransactionBuffer.AddNumMosaics(builder, (byte)Mosaics.Count);
            TransferTransactionBuffer.AddMessageSize(builder, (ushort)(bytePayload.Length + 1));
            TransferTransactionBuffer.AddMessage(builder, messageVector);
            TransferTransactionBuffer.AddMosaics(builder, mosaicsVector);

            // end build
            var codedTransfer = TransferTransactionBuffer.EndTransferTransactionBuffer(builder);
            builder.Finish(codedTransfer.Value);

            // validate size
            var output = new TransferTransactionSchema().Serialize(builder.SizedByteArray());

            if (output.Length != totalSize) throw new SerializationException("Serialized form has incorrect length");

            return output;
        }
    }
}