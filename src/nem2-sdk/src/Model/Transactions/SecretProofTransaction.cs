// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="TransactionTypes.cs" company="Nem.io">
// Copyright 2018 NEM
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
// </copyright>
// <summary></summary>
// ***********************************************************************

using io.nem2.sdk.Core.Crypto.Chaso.NaCl;
using io.nem2.sdk.Core.Utils;
using io.nem2.sdk.Infrastructure.Buffers;
using io.nem2.sdk.Infrastructure.Buffers.Schema;
using io.nem2.sdk.Infrastructure.Imported.FlatBuffers;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Blockchain;

namespace io.nem2.sdk.Model.Transactions
{
    /// <inheritdoc />
    /// <summary>
    /// Class SecretProofTransaction.
    /// </summary>
    /// <seealso cref="T:io.nem2.sdk.Model.Transactions.Transaction" />
    public class SecretProofTransaction : Transaction
    {
        /// <summary>
        /// Gets the proof.
        /// </summary>
        /// <value>The proof.</value>
        internal byte[] Proof { get; }

        public string ProofString() => Proof.ToHexUpper();
        /// <summary>
        /// Gets the secret.
        /// </summary>
        /// <value>The secret.</value>
        internal byte[] Secret { get; }

        /// <summary>
        /// Secrets the string.
        /// </summary>
        /// <returns>System.String.</returns>
        public string SecretString() => Secret.ToHexUpper();

        /// <summary>
        /// Gets the hash algo.
        /// </summary>
        /// <value>The hash algo.</value>
        public HashType.Types HashAlgo { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecretProofTransaction"/> class.
        /// </summary>
        /// <param name="version">The transaction version</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="hashAlgo">The hash algo.</param>
        /// <param name="secret">The secret.</param>
        /// <param name="proof">The proof.</param>
        /// <param name="networkType">Type of the network.</param>
        public SecretProofTransaction(NetworkType.Types networkType, int version, Deadline deadline, ulong fee, HashType.Types hashAlgo, string secret, string proof)
        : this(networkType, 3,deadline, fee, hashAlgo, secret, proof, null, null, null) {}

        /// <summary>
        /// Initializes a new instance of the <see cref="SecretProofTransaction"/> class.
        /// </summary>
        /// <param name="deadline">The deadline.</param>
        /// <param name="hashAlgo">The hash algo.</param>
        /// <param name="secret">The secret.</param>
        /// <param name="proof">The proof.</param>
        /// <param name="networkType">Type of the network.</param>
        public SecretProofTransaction(NetworkType.Types networkType, int version, Deadline deadline, ulong fee, HashType.Types hashAlgo, string secret, string proof, string signature, PublicAccount signer, TransactionInfo transactionInfo)
        {
            Deadline = deadline;
            Version = version;
            Fee = fee;
            NetworkType = networkType;
            HashAlgo = hashAlgo;
            Secret = secret.FromHex();
            Proof = proof.FromHex();
            TransactionType = TransactionTypes.Types.SecretProof;
            Signer = signer;
            Signature = signature;
            TransactionInfo = transactionInfo;
        }

        /// <summary>
        /// Creates the specified <see cref="SecretProofTransaction"/>.
        /// </summary>
        /// <param name="deadline">The deadline.</param>
        /// <param name="hashAlgo">The hash algo.</param>
        /// <param name="secret">The secret.</param>
        /// <param name="proof">The proof.</param>
        /// <param name="netowrkType">Type of the netowrk.</param>
        /// <returns><see cref="SecretProofTransaction"/></returns>
        public static SecretProofTransaction Create(NetworkType.Types netowrkType, Deadline deadline, ulong fee, HashType.Types hashAlgo, string secret, string proof)
        {
            return new SecretProofTransaction(netowrkType, 3, deadline, fee, hashAlgo, secret, proof);
        }

        /// <summary>
        /// Creates the specified <see cref="SecretProofTransaction"/>.
        /// </summary>
        /// <param name="deadline">The deadline.</param>
        /// <param name="hashAlgo">The hash algo.</param>
        /// <param name="secret">The secret.</param>
        /// <param name="proof">The proof.</param>
        /// <param name="netowrkType">Type of the netowrk.</param>
        /// <returns><see cref="SecretProofTransaction"/></returns>
        public static SecretProofTransaction Create(NetworkType.Types netowrkType, Deadline deadline, HashType.Types hashAlgo, string secret, string proof)
        {
            return new SecretProofTransaction(netowrkType, 3, deadline, 0, hashAlgo, secret, proof);
        }

        internal override byte[] GenerateBytes()
        {
            var builder = new FlatBufferBuilder(1);

            // create vectors
            var signatureVector = SecretProofTransactionBuffer.CreateSignatureVector(builder, new byte[64]);
            var signerVector = SecretProofTransactionBuffer.CreateSignerVector(builder, GetSigner());
            var feeVector = SecretProofTransactionBuffer.CreateFeeVector(builder, Fee.ToUInt8Array());
            var deadlineVector = SecretProofTransactionBuffer.CreateDeadlineVector(builder, Deadline.Ticks.ToUInt8Array());
            var secretVector = SecretProofTransactionBuffer.CreateSecretVector(builder, Secret);
            var proofVector = SecretProofTransactionBuffer.CreateProofVector(builder, Proof);

            ushort version = ushort.Parse(NetworkType.GetNetworkByte().ToString("X") + "0" + Version.ToString("X"), System.Globalization.NumberStyles.HexNumber);

            // add vectors
            SecretProofTransactionBuffer.StartSecretProofTransactionBuffer(builder);
            SecretProofTransactionBuffer.AddSize(builder, (uint)(187 + Proof.Length));
            SecretProofTransactionBuffer.AddSignature(builder, signatureVector);
            SecretProofTransactionBuffer.AddSigner(builder, signerVector);
            SecretProofTransactionBuffer.AddVersion(builder, version);
            SecretProofTransactionBuffer.AddType(builder, TransactionTypes.Types.SecretProof.GetValue());
            SecretProofTransactionBuffer.AddFee(builder, feeVector);
            SecretProofTransactionBuffer.AddDeadline(builder, deadlineVector);
            SecretProofTransactionBuffer.AddHashAlgorithm(builder, HashAlgo.GetHashTypeValue());
            SecretProofTransactionBuffer.AddSecret(builder, secretVector);
            SecretProofTransactionBuffer.AddProofSize(builder, (ushort)Proof.Length);
            SecretProofTransactionBuffer.AddProof(builder, proofVector);

            // end build
            var codedTransfer = SecretProofTransactionBuffer.EndSecretProofTransactionBuffer(builder);
            builder.Finish(codedTransfer.Value);

            return new SecretProofTransactionSchema().Serialize(builder.SizedByteArray());
        }
    }
}
