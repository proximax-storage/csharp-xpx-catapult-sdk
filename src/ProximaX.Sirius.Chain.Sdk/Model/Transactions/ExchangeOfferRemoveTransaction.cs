using FlatBuffers;
using ProximaX.Sirius.Chain.Sdk.Buffers;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Exchange;
using System;
using System.Collections.Generic;
using System.Text;
using ProximaX.Sirius.Chain.Sdk.Utils;
using ProximaX.Sirius.Chain.Sdk.Buffers.Schema;
using System.Runtime.Serialization;
using System.Linq;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions
{
    public class ExchangeOfferRemoveTransaction : Transaction
    {
        public List<RemoveExchangeOffer> Offers { get; private set; }

        public ExchangeOfferRemoveTransaction(NetworkType networkType, int version, Deadline deadline, ulong? maxFee, List<RemoveExchangeOffer> offers, string signature = null, PublicAccount signer = null, TransactionInfo transactionInfo = null) :
            base(networkType, version, EntityType.EXCHANGE_OFFER_REMOVE, deadline, maxFee, signature, signer, transactionInfo)
        {

            Offers = offers ?? throw new ArgumentNullException(nameof(offers));
        }

        public static int CalculatePayloadSize(int offerCount)
        {
            // offer count + offer count * (id, type)
            return 1 + offerCount * (8 + 1);
        }

        protected override int GetPayloadSerializedSize()
        {
            return CalculatePayloadSize(Offers.Count);
        }

        internal override byte[] GenerateBytes()
        {
            var builder = new FlatBufferBuilder(1);


            // create offers
            var offerOffsets = new Offset<RemoveExchangeOfferBuffer>[Offers.Count];

            for (var i = 0; i < offerOffsets.Length; i++)
            {
                var offer = Offers[i];

                var mosaicIdVector = RemoveExchangeOfferBuffer.CreateMosaicIdVector(builder, offer.MosaicId.Id.ToUInt8Array());


                RemoveExchangeOfferBuffer.StartRemoveExchangeOfferBuffer(builder);
                RemoveExchangeOfferBuffer.AddMosaicId(builder, mosaicIdVector);
                RemoveExchangeOfferBuffer.AddType(builder, offer.Type.GetValueInByte());
             
                offerOffsets[i] = RemoveExchangeOfferBuffer.EndRemoveExchangeOfferBuffer(builder);

            }

            // create vectors
            var signatureVector = RemoveExchangeOfferTransactionBuffer.CreateSignatureVector(builder, new byte[64]);
            var signerVector = RemoveExchangeOfferTransactionBuffer.CreateSignerVector(builder, GetSigner());
            var feeVector = RemoveExchangeOfferTransactionBuffer.CreateMaxFeeVector(builder, MaxFee?.ToUInt8Array());
            var deadlineVector =
                RemoveExchangeOfferTransactionBuffer.CreateDeadlineVector(builder, Deadline.Ticks.ToUInt8Array());
            var offersVector = RemoveExchangeOfferTransactionBuffer.CreateOffersVector(builder, offerOffsets);


            // add size of the transaction
            int totalSize = GetSerializedSize();

            // create version
            var version = GetTxVersionSerialization();


            // ADD to buffer
            RemoveExchangeOfferTransactionBuffer.StartRemoveExchangeOfferTransactionBuffer(builder);
            RemoveExchangeOfferTransactionBuffer.AddSize(builder, (uint)totalSize);
            RemoveExchangeOfferTransactionBuffer.AddSignature(builder, signatureVector);
            RemoveExchangeOfferTransactionBuffer.AddSigner(builder, signerVector);
            RemoveExchangeOfferTransactionBuffer.AddVersion(builder, (uint)version);
            RemoveExchangeOfferTransactionBuffer.AddType(builder, TransactionType.GetValue());
            RemoveExchangeOfferTransactionBuffer.AddMaxFee(builder, feeVector);
            RemoveExchangeOfferTransactionBuffer.AddDeadline(builder, deadlineVector);
            RemoveExchangeOfferTransactionBuffer.AddOffersCount(builder, (byte)offerOffsets.Length);
            RemoveExchangeOfferTransactionBuffer.AddOffers(builder, offersVector);

            // Calculate size
            var codedTransaction = RemoveExchangeOfferTransactionBuffer.EndRemoveExchangeOfferTransactionBuffer(builder);
            builder.Finish(codedTransaction.Value);

            var output = new ExchangeOfferRemoveTransactionSchema().Serialize(builder.SizedByteArray());

            if (output.Length != totalSize) throw new SerializationException("Serialized form has incorrect length");

            return output;
        }
    }
}
