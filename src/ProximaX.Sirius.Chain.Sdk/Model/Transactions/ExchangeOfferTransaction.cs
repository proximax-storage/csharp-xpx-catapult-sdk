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
        public List<ExchangeOffer> Offers { get; private set; }

        public ExchangeOfferTransaction(NetworkType networkType, int version, Deadline deadline, ulong? maxFee, List<ExchangeOffer> offers, string signature = null, PublicAccount signer = null, TransactionInfo transactionInfo = null) :
            base(networkType, version, EntityType.EXCHANGE_OFFER, deadline, maxFee, signature, signer, transactionInfo)
        {

            Offers = offers ?? throw new ArgumentNullException(nameof(offers));
        }

        public static int CalculatePayloadSize(int offerCount)
        {
            // offer count + offer count * (id, amount, cost, type, owner)
            return 1 + offerCount * (8 + 8 + 8 + 1 + 32);
        }

        protected override int GetPayloadSerializedSize()
        {
            return CalculatePayloadSize(Offers.Count);
        }

        internal override byte[] GenerateBytes()
        {
            var builder = new FlatBufferBuilder(1);


            // create offers
            var offerOffsets = new Offset<ExchangeOfferBuffer>[Offers.Count];

            for (var i = 0; i < offerOffsets.Length; i++)
            {
                var offer = Offers[i];

                var mosaicIdVector = ExchangeOfferBuffer.CreateMosaicIdVector(builder, offer.MosaicId.Id.ToUInt8Array());
                var mosaicAmountVector = ExchangeOfferBuffer.CreateMosaicAmountVector(builder, offer.MosaicAmount.ToUInt8Array());
                var costVector = ExchangeOfferBuffer.CreateCostVector(builder, offer.Cost.ToUInt8Array());
                var ownerVector = ExchangeOfferBuffer.CreateOwnerVector(builder, offer.Owner.PublicKey.DecodeHexString());

                ExchangeOfferBuffer.StartExchangeOfferBuffer(builder);
                ExchangeOfferBuffer.AddMosaicId(builder, mosaicIdVector);
                ExchangeOfferBuffer.AddMosaicAmount(builder, mosaicAmountVector);
                ExchangeOfferBuffer.AddCost(builder, costVector);
                ExchangeOfferBuffer.AddType(builder, offer.Type.GetValueInByte());
                ExchangeOfferBuffer.AddOwner(builder, ownerVector);
                offerOffsets[i] = ExchangeOfferBuffer.EndExchangeOfferBuffer(builder);

            }

            // create vectors
            var signatureVector = ExchangeOfferTransactionBuffer.CreateSignatureVector(builder, new byte[64]);
            var signerVector = ExchangeOfferTransactionBuffer.CreateSignerVector(builder, GetSigner());
            var feeVector = ExchangeOfferTransactionBuffer.CreateMaxFeeVector(builder, MaxFee?.ToUInt8Array());
            var deadlineVector =
                ExchangeOfferTransactionBuffer.CreateDeadlineVector(builder, Deadline.Ticks.ToUInt8Array());
            var offersVector = ExchangeOfferTransactionBuffer.CreateOffersVector(builder, offerOffsets);


            // add size of the transaction
            int totalSize = GetSerializedSize();

            // create version
            var version = GetTxVersionSerialization();


            // ADD to buffer
            ExchangeOfferTransactionBuffer.StartExchangeOfferTransactionBuffer(builder);
            ExchangeOfferTransactionBuffer.AddSize(builder, (uint)totalSize);
            ExchangeOfferTransactionBuffer.AddSignature(builder, signatureVector);
            ExchangeOfferTransactionBuffer.AddSigner(builder, signerVector);
            ExchangeOfferTransactionBuffer.AddVersion(builder, (uint)version);
            ExchangeOfferTransactionBuffer.AddType(builder, TransactionType.GetValue());
            ExchangeOfferTransactionBuffer.AddMaxFee(builder, feeVector);
            ExchangeOfferTransactionBuffer.AddDeadline(builder, deadlineVector);
            ExchangeOfferTransactionBuffer.AddOffersCount(builder, (byte)offerOffsets.Length);
            ExchangeOfferTransactionBuffer.AddOffers(builder, offersVector);

            // Calculate size
            var codedTransaction = ExchangeOfferTransactionBuffer.EndExchangeOfferTransactionBuffer(builder);
            builder.Finish(codedTransaction.Value);

            var output = new ExchangeOfferAddTransactionSchema().Serialize(builder.SizedByteArray());

            if (output.Length != totalSize) throw new SerializationException("Serialized form has incorrect length");

            return output;
        }
    }
}
