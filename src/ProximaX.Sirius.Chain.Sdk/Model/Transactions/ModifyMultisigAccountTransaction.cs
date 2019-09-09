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
using System.Collections.Generic;
using System.Globalization;
using FlatBuffers;
using ProximaX.Sirius.Chain.Sdk.Buffers;
using ProximaX.Sirius.Chain.Sdk.Buffers.Schema;
using ProximaX.Sirius.Chain.Sdk.Crypto.Core.Chaso.NaCl;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions
{
    public class ModifyMultisigAccountTransaction : Transaction
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="networkType"></param>
        /// <param name="version"></param>
        /// <param name="deadline"></param>
        /// <param name="maxFee"></param>
        /// <param name="minApprovalDelta"></param>
        /// <param name="minRemovalDelta"></param>
        /// <param name="modifications"></param>
        /// <param name="signature"></param>
        /// <param name="signer"></param>
        /// <param name="transactionInfo"></param>
        public ModifyMultisigAccountTransaction(NetworkType networkType, int version, Deadline deadline, ulong? maxFee,
            int minApprovalDelta, int minRemovalDelta, List<MultisigCosignatoryModification> modifications,
            string signature = null, PublicAccount signer = null, TransactionInfo transactionInfo = null)
            : base(networkType, version, TransactionType.MODIFY_MULTISIG_ACCOUNT, deadline, maxFee, signature, signer,
                transactionInfo)
        {
            MinApprovalDelta = minApprovalDelta;
            MinRemovalDelta = minRemovalDelta;
            Modifications = modifications;
        }

        /// <summary>
        ///     Minimum cosignatory removal delta.
        /// </summary>
        /// <value>The minimum removal delta.</value>
        public int MinRemovalDelta { get; }

        /// <summary>
        ///     Minimum cosignatory approval delta.
        /// </summary>
        /// <value>The minimum approval delta.</value>
        public int MinApprovalDelta { get; }

        /// <summary>
        ///     Modifications to be made.
        /// </summary>
        /// <value>The modifications.</value>
        public List<MultisigCosignatoryModification> Modifications { get; }

        /// <summary>
        ///     Creates ModifyMultisigAccountTransaction
        /// </summary>
        /// <param name="deadline"></param>
        /// <param name="minApprovalDelta"></param>
        /// <param name="minRemovalDelta"></param>
        /// <param name="modifications"></param>
        /// <param name="networkType"></param>
        /// <returns></returns>
        public static ModifyMultisigAccountTransaction Create(Deadline deadline, int minApprovalDelta,
            int minRemovalDelta, List<MultisigCosignatoryModification> modifications, NetworkType networkType)
        {
            return new ModifyMultisigAccountTransaction(networkType,
                TransactionVersion.MODIFY_MULTISIG_ACCOUNT.GetValue(),
                deadline, 0L, minApprovalDelta, minRemovalDelta, modifications);
        }

        internal override byte[] GenerateBytes()
        {
            var builder = new FlatBufferBuilder(1);

            // Create modifications vector
            var modificationsArray = new Offset<CosignatoryModificationBuffer>[Modifications.Count];

            for (var index = 0; index < Modifications.Count; ++index)
            {
                var modification = Modifications[index];

                var cosignatoryPublicKeyVector =
                    CosignatoryModificationBuffer.CreateCosignatoryPublicKeyVector(builder,
                        modification.CosignatoryPublicAccount.PublicKey.FromHex());
                CosignatoryModificationBuffer.StartCosignatoryModificationBuffer(builder);
                CosignatoryModificationBuffer.AddType(builder, modification.Type.GetValueInByte());
                CosignatoryModificationBuffer.AddCosignatoryPublicKey(builder, cosignatoryPublicKeyVector);
                modificationsArray[index] = CosignatoryModificationBuffer.EndCosignatoryModificationBuffer(builder);
            }

            // create vectors
            var signatureVector = ModifyMultisigAccountTransactionBuffer.CreateSignatureVector(builder, new byte[64]);
            var signerVector = ModifyMultisigAccountTransactionBuffer.CreateSignerVector(builder, GetSigner());
            var feeVector = ModifyMultisigAccountTransactionBuffer.CreateMaxFeeVector(builder, MaxFee?.ToUInt8Array());
            var deadlineVector =
                ModifyMultisigAccountTransactionBuffer.CreateDeadlineVector(builder, Deadline.Ticks.ToUInt8Array());
            var modificationVector =
                ModifyMultisigAccountTransactionBuffer.CreateModificationsVector(builder, modificationsArray);

            // create version
            var version = GetTxVersionSerialization();

            var fixedSize = HEADER_SIZE +
                +1 // min approval
                + 1 //  min removal
                + 1 // mod count
                + (1 + 32) * Modifications.Count; // (type, pub key) * count

            ModifyMultisigAccountTransactionBuffer.StartModifyMultisigAccountTransactionBuffer(builder);
            ModifyMultisigAccountTransactionBuffer.AddSize(builder, (uint)fixedSize);
            ModifyMultisigAccountTransactionBuffer.AddSignature(builder, signatureVector);
            ModifyMultisigAccountTransactionBuffer.AddSigner(builder, signerVector);
            ModifyMultisigAccountTransactionBuffer.AddVersion(builder,(uint) version);
            ModifyMultisigAccountTransactionBuffer.AddType(builder, TransactionType.GetValue());
            ModifyMultisigAccountTransactionBuffer.AddMaxFee(builder, feeVector);
            ModifyMultisigAccountTransactionBuffer.AddDeadline(builder, deadlineVector);
            ModifyMultisigAccountTransactionBuffer.AddMinRemovalDelta(builder, Convert.ToSByte(MinRemovalDelta));
            ModifyMultisigAccountTransactionBuffer.AddMinApprovalDelta(builder, Convert.ToSByte(MinApprovalDelta));
            ModifyMultisigAccountTransactionBuffer.AddNumModifications(builder, Convert.ToByte(Modifications.Count));
            ModifyMultisigAccountTransactionBuffer.AddModifications(builder, modificationVector);

            // end build
            var codedTransfer =
                ModifyMultisigAccountTransactionBuffer.EndModifyMultisigAccountTransactionBuffer(builder);
            builder.Finish(codedTransfer.Value);

            return new ModifyMultisigAccountTransactionSchema().Serialize(builder.SizedByteArray());
        }
    }
}