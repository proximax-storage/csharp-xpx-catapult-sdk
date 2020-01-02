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
    public class ExchangeOfferTransaction : Transaction
    {
        public List<AddExchangeOffer> Offers { get; private set; }

        public ExchangeOfferTransaction(NetworkType networkType, int version, Deadline deadline, ulong? maxFee, List<AddExchangeOffer> offers, string signature = null, PublicAccount signer = null, TransactionInfo transactionInfo = null) :
            base(networkType, version, EntityType.EXCHANGE_OFFER_ADD, deadline, maxFee, signature, signer, transactionInfo)
        {

            Offers = offers ?? throw new ArgumentNullException(nameof(offers));
        }

        public static int CalculatePayloadSize(int offerCount)
        {
            // offer count + offer count * (id, amount, cost, type, duration)
            return 1 + offerCount * (8 + 8 + 8 + 1 + 8);
        }

        protected override int GetPayloadSerializedSize()
        {
            return CalculatePayloadSize(Offers.Count);
        }

        internal override byte[] GenerateBytes()
        {
            var builder = new FlatBufferBuilder(1);


            // create offers
            var offerOffsets = new Offset<AddExchangeOfferBuffer>[Offers.Count];

            for (var i = 0; i < offerOffsets.Length; i++)
            {
                var offer = Offers[i];

                var mosaicIdVector = AddExchangeOfferBuffer.CreateMosaicIdVector(builder, offer.MosaicId.Id.ToUInt8Array());
                var mosaicAmountVector = AddExchangeOfferBuffer.CreateMosaicAmountVector(builder, offer.MosaicAmount.ToUInt8Array());
                var costVector = AddExchangeOfferBuffer.CreateCostVector(builder, offer.Cost.ToUInt8Array());
                var durationVector = AddExchangeOfferBuffer.CreateDurationVector(builder, offer.Duration.ToUInt8Array());

                AddExchangeOfferBuffer.StartAddExchangeOfferBuffer(builder);
                AddExchangeOfferBuffer.AddMosaicId(builder, mosaicIdVector);
                AddExchangeOfferBuffer.AddMosaicAmount(builder, mosaicAmountVector);
                AddExchangeOfferBuffer.AddCost(builder, costVector);
                AddExchangeOfferBuffer.AddType(builder, offer.Type.GetValueInByte());
                AddExchangeOfferBuffer.AddDuration(builder, durationVector);
                offerOffsets[i] = AddExchangeOfferBuffer.EndAddExchangeOfferBuffer(builder);

            }

            // create vectors
            var signatureVector = AddExchangeOfferTransactionBuffer.CreateSignatureVector(builder, new byte[64]);
            var signerVector = AddExchangeOfferTransactionBuffer.CreateSignerVector(builder, GetSigner());
            var feeVector = AddExchangeOfferTransactionBuffer.CreateMaxFeeVector(builder, MaxFee?.ToUInt8Array());
            var deadlineVector =
                AddExchangeOfferTransactionBuffer.CreateDeadlineVector(builder, Deadline.Ticks.ToUInt8Array());
            var offersVector = AddExchangeOfferTransactionBuffer.CreateOffersVector(builder, offerOffsets);


            // add size of the transaction
            int totalSize = GetSerializedSize();

            // create version
            var version = GetTxVersionSerialization();


            // ADD to buffer
            AddExchangeOfferTransactionBuffer.StartAddExchangeOfferTransactionBuffer(builder);
            AddExchangeOfferTransactionBuffer.AddSize(builder, (uint)totalSize);
            AddExchangeOfferTransactionBuffer.AddSignature(builder, signatureVector);
            AddExchangeOfferTransactionBuffer.AddSigner(builder, signerVector);
            AddExchangeOfferTransactionBuffer.AddVersion(builder, (uint)version);
            AddExchangeOfferTransactionBuffer.AddType(builder, TransactionType.GetValue());
            AddExchangeOfferTransactionBuffer.AddMaxFee(builder, feeVector);
            AddExchangeOfferTransactionBuffer.AddDeadline(builder, deadlineVector);
            AddExchangeOfferTransactionBuffer.AddOffersCount(builder, (byte)offerOffsets.Length);
            AddExchangeOfferTransactionBuffer.AddOffers(builder, offersVector);

            // Calculate size
            var codedTransaction = AddExchangeOfferTransactionBuffer.EndAddExchangeOfferTransactionBuffer(builder);
            builder.Finish(codedTransaction.Value);

            var output = new ExchangeOfferAddTransactionSchema().Serialize(builder.SizedByteArray());

            if (output.Length != totalSize) throw new SerializationException("Serialized form has incorrect length");

            return output;
        }
    }
}
