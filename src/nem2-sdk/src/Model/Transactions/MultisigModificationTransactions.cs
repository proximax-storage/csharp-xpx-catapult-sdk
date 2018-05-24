// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kaili
// Created          : 01-15-2018
//
// Last Modified By : kaili
// Last Modified On : 02-01-2018
// ***********************************************************************
// <copyright file="MultisigModificationTransactions.cs" company="Nem.io">
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

using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Class ModifyMultisigAccountTransaction.
    /// </summary>
    /// <seealso cref="T:io.nem2.sdk.Model.Transactions.Transaction" />
    public class ModifyMultisigAccountTransaction : Transaction
    {
        /// <summary>
        /// Minimum cosignatory removal delta.
        /// </summary>
        /// <value>The minimum removal delta.</value>
        public int MinRemovalDelta { get; }

        /// <summary>
        /// Minimum cosignatory approval delta.
        /// </summary>
        /// <value>The minimum approval delta.</value>
        public int MinApprovalDelta { get; }

        /// <summary>
        /// Modifcations to be made.
        /// </summary>
        /// <value>The modifications.</value>
        public MultisigCosignatoryModification[] Modifications { get; }


        /// <summary>
        /// Initializes a new instance of the <see cref="ModifyMultisigAccountTransaction"/> class.
        /// </summary>
        /// <param name="networkType">Type of the network.</param>
        /// <param name="version">The version.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="fee">The fee.</param>
        /// <param name="minApprovalDelta">The minimum approval delta.</param>
        /// <param name="minRemovalDelta">The minimum removal delta.</param>
        /// <param name="modifications">The modifications.</param>
        /// <exception cref="ArgumentNullException">modifications</exception>
        /// <exception cref="InvalidEnumArgumentException">networkType</exception>
        public ModifyMultisigAccountTransaction(NetworkType.Types networkType, int version, Deadline deadline, ulong fee, int minApprovalDelta, int minRemovalDelta, List<MultisigCosignatoryModification> modifications)
            : this (networkType, version, deadline, fee, minApprovalDelta, minRemovalDelta, modifications, null, null, null){}

        /// <summary>
        /// Initializes a new instance of the <see cref="ModifyMultisigAccountTransaction"/> class.
        /// </summary>
        /// <param name="networkType">Type of the network.</param>
        /// <param name="version">The transaction version.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="fee">The fee.</param>
        /// <param name="minApprovalDelta">The minimum approval delta.</param>
        /// <param name="minRemovalDelta">The minimum removal delta.</param>
        /// <param name="modifications">The modifications.</param>
        /// <param name="signature">The signature.</param>
        /// <param name="signer">The signer.</param>
        /// <param name="transactionInfo">The transaction information.</param>
        /// <exception cref="ArgumentNullException">modifications</exception>
        /// <exception cref="InvalidEnumArgumentException">networkType</exception>
        public ModifyMultisigAccountTransaction(NetworkType.Types networkType, int version, Deadline deadline, ulong fee, int minApprovalDelta, int minRemovalDelta, List<MultisigCosignatoryModification> modifications, string signature, PublicAccount signer, TransactionInfo transactionInfo)
        {
            if (modifications == null) throw new ArgumentNullException(nameof(modifications));
            if (!Enum.IsDefined(typeof(NetworkType.Types), networkType))
                throw new InvalidEnumArgumentException(nameof(networkType), (int)networkType,
                    typeof(NetworkType.Types));
            
            Deadline = deadline;
            NetworkType = networkType;
            Version = version;
            Fee = fee;
            MinRemovalDelta = minRemovalDelta;
            MinApprovalDelta = minApprovalDelta;
            Modifications = modifications.ToArray();
            TransactionType = TransactionTypes.Types.ModifyMultisigAccount;
            Signer = signer;
            Signature = signature;
            TransactionInfo = transactionInfo;
        }

        /// <summary>
        /// Static create an instance of <see cref="ModifyMultisigAccountTransaction" />
        /// </summary>
        /// <param name="deadline">The deadline.</param>
        /// <param name="minApprovalDelta">The minimum approval delta.</param>
        /// <param name="minRemovalDelta">The minimum removal delta.</param>
        /// <param name="modifications">The modifications to make.</param>
        /// <param name="networkType">The network type.</param>
        /// <returns><see cref="ModifyMultisigAccountTransaction" />.</returns>
        public static ModifyMultisigAccountTransaction Create(NetworkType.Types networkType, Deadline deadline, int minApprovalDelta, int minRemovalDelta, List<MultisigCosignatoryModification> modifications)
        {
            return new ModifyMultisigAccountTransaction(networkType, 3, deadline, 0, minApprovalDelta, minRemovalDelta, modifications);
        }


        /// <summary>
        /// Generates the bytes.
        /// </summary>
        /// <returns>System.Byte[].</returns>
        internal override byte[] GenerateBytes()
        {
            var builder = new FlatBufferBuilder(1);

            // Create modifications vector
            var modificationsArray = new Offset<CosignatoryModificationBuffer>[Modifications.Length];

            for (var index = 0; index < Modifications.Length; index++)
            {              
                var modification = Modifications[index];

                var cosignatoryPublicKeyVector = CosignatoryModificationBuffer.CreateCosignatoryPublicKeyVector(builder, modification.PublicAccount.PublicKey.FromHex());
                CosignatoryModificationBuffer.StartCosignatoryModificationBuffer(builder);
                CosignatoryModificationBuffer.AddType(builder, modification.Type.GetValue());
                CosignatoryModificationBuffer.AddCosignatoryPublicKey(builder, cosignatoryPublicKeyVector);
                modificationsArray[index] = CosignatoryModificationBuffer.EndCosignatoryModificationBuffer(builder);
            }

            // create vectors
            var signatureVector = MosaicCreationTransactionBuffer.CreateSignatureVector(builder, new byte[64]);
            var signerVector = MosaicCreationTransactionBuffer.CreateSignerVector(builder, GetSigner());
            var feeVector = MosaicCreationTransactionBuffer.CreateFeeVector(builder, Fee.ToUInt8Array());
            var deadlineVector = MosaicCreationTransactionBuffer.CreateDeadlineVector(builder, Deadline.Ticks.ToUInt8Array());         
            var modificationVector = MultisigModificationTransactionBuffer.CreateModificationsVector(builder, modificationsArray);

            ushort version = ushort.Parse(NetworkType.GetNetworkByte().ToString("X") + "0" + Version.ToString("X"), System.Globalization.NumberStyles.HexNumber);

            MultisigModificationTransactionBuffer.StartMultisigModificationTransactionBuffer(builder);
            MultisigModificationTransactionBuffer.AddSize(builder, (uint)(123 + 33 * Modifications.Length));
            MultisigModificationTransactionBuffer.AddSignature(builder, signatureVector);
            MultisigModificationTransactionBuffer.AddSigner(builder, signerVector);
            MultisigModificationTransactionBuffer.AddVersion(builder, version);
            MultisigModificationTransactionBuffer.AddType(builder, TransactionTypes.Types.ModifyMultisigAccount.GetValue());
            MultisigModificationTransactionBuffer.AddFee(builder, feeVector);
            MultisigModificationTransactionBuffer.AddDeadline(builder, deadlineVector);
            MultisigModificationTransactionBuffer.AddMinRemovalDelta(builder, (byte)MinRemovalDelta);
            MultisigModificationTransactionBuffer.AddMinApprovalDelta(builder, (byte)MinApprovalDelta);
            MultisigModificationTransactionBuffer.AddNumModifications(builder,  (byte)Modifications.Length);
            MultisigModificationTransactionBuffer.AddModifications(builder, modificationVector);

            // end build
            var codedTransfer = TransferTransactionBuffer.EndTransferTransactionBuffer(builder);
            builder.Finish(codedTransfer.Value);

            return new MultisigModificationTransactionSchema().Serialize(builder.SizedByteArray());
        }
    }
}
