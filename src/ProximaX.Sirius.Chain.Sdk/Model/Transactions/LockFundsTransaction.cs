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

using System;
using System.Globalization;
using System.Runtime.Serialization;
using FlatBuffers;
using ProximaX.Sirius.Chain.Sdk.Buffers;
using ProximaX.Sirius.Chain.Sdk.Buffers.Schema;
using ProximaX.Sirius.Chain.Sdk.Crypto.Core.Chaso.NaCl;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions
{
    public class LockFundsTransaction : Transaction
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="networkType"></param>
        /// <param name="version"></param>
        /// <param name="deadline"></param>
        /// <param name="maxFee"></param>
        /// <param name="mosaic"></param>
        /// <param name="duration"></param>
        /// <param name="transaction"></param>
        /// <param name="signature"></param>
        /// <param name="signer"></param>
        /// <param name="transactionInfo"></param>
        public LockFundsTransaction(NetworkType networkType, int version, Deadline deadline, ulong? maxFee,
            Mosaic mosaic, ulong duration, SignedTransaction transaction, string signature = null,
            PublicAccount signer = null, TransactionInfo transactionInfo = null)
            : base(networkType, version, EntityType.HASHLOCK, deadline, maxFee, signature, signer, transactionInfo)
        {
            Mosaic = mosaic;
            Duration = duration;
            Transaction = transaction;
        }

        /// <summary>
        ///     Gets or sets the mosaic.
        /// </summary>
        /// <value>The mosaic.</value>
        public Mosaic Mosaic { get; }

        /// <summary>
        ///     Gets or sets the duration.
        /// </summary>
        /// <value>The duration.</value>
        public ulong Duration { get; }

        /// <summary>
        ///     Gets or sets the transaction.
        /// </summary>
        /// <value>The transactions.</value>
        public SignedTransaction Transaction { get; }

        /// <summary>
        ///     Creates LockFundsTransaction
        /// </summary>
        /// <param name="deadline"></param>
        /// <param name="mosaic"></param>
        /// <param name="duration"></param>
        /// <param name="signedTransaction"></param>
        /// <param name="networkType"></param>
        /// <returns></returns>
        public static LockFundsTransaction Create(Deadline deadline, Mosaic mosaic, ulong duration,
            SignedTransaction signedTransaction, NetworkType networkType)
        {
            return new LockFundsTransaction(networkType, EntityVersion.HASHLOCK.GetValue(), deadline, 0, mosaic,
                duration, signedTransaction);
        }

        public static int CalculatePayloadSize()
        {
            // mosaic id, amount, duration, hash
            return 8 + 8 + 8 + 32;
        }

        protected override int GetPayloadSerializedSize()
        {
            return CalculatePayloadSize();
        }

        internal override byte[] GenerateBytes()
        {
            var builder = new FlatBufferBuilder(1);


            // create vectors
            var signatureVector = LockFundsTransactionBuffer.CreateSignatureVector(builder, new byte[64]);
            var signerVector = LockFundsTransactionBuffer.CreateSignerVector(builder, GetSigner());
            var feeVector = LockFundsTransactionBuffer.CreateMaxFeeVector(builder, MaxFee?.ToUInt8Array());
            var deadlineVector =
                LockFundsTransactionBuffer.CreateDeadlineVector(builder, Deadline.Ticks.ToUInt8Array());
            var mosaicIdVector = LockFundsTransactionBuffer.CreateMosaicIdVector(builder, Mosaic.Id.Id.ToUInt8Array());
            var mosaicAmountVector =
                LockFundsTransactionBuffer.CreateMosaicAmountVector(builder, Mosaic.Amount.ToUInt8Array());
            var durationVector = LockFundsTransactionBuffer.CreateDurationVector(builder, Duration.ToUInt8Array());
            var hashVector = LockFundsTransactionBuffer.CreateHashVector(builder, Transaction.Hash.FromHex());
            
            // create version
            var version = GetTxVersionSerialization();

            int totalSize = GetSerializedSize();
                

            LockFundsTransactionBuffer.StartLockFundsTransactionBuffer(builder);
            LockFundsTransactionBuffer.AddSize(builder, (uint)totalSize);
            LockFundsTransactionBuffer.AddSignature(builder, signatureVector);
            LockFundsTransactionBuffer.AddSigner(builder, signerVector);
            LockFundsTransactionBuffer.AddVersion(builder, (uint)version);
            LockFundsTransactionBuffer.AddType(builder, TransactionType.GetValue());
            LockFundsTransactionBuffer.AddMaxFee(builder, feeVector);
            LockFundsTransactionBuffer.AddDeadline(builder, deadlineVector);
            LockFundsTransactionBuffer.AddMosaicId(builder, mosaicIdVector);
            LockFundsTransactionBuffer.AddMosaicAmount(builder, mosaicAmountVector);
            LockFundsTransactionBuffer.AddDuration(builder, durationVector);
            LockFundsTransactionBuffer.AddHash(builder, hashVector);

            // end build
            var codedTransfer = LockFundsTransactionBuffer.EndLockFundsTransactionBuffer(builder);
            builder.Finish(codedTransfer.Value);

            var output = new LockFundsTransactionSchema().Serialize(builder.SizedByteArray());

            if (output.Length != totalSize) throw new SerializationException("Serialized form has incorrect length");

            return output;

            
        }
    }
}