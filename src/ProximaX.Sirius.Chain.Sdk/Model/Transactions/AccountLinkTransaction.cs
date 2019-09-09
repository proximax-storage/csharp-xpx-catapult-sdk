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
using GuardNet;
using ProximaX.Sirius.Chain.Sdk.Buffers;
using ProximaX.Sirius.Chain.Sdk.Buffers.Schema;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions
{
    public class AccountLinkTransaction : Transaction
    {
        public PublicAccount RemoteAccount { get; set; }
        public AccountLinkAction Action { get; set; }

        /// <summary>
        /// Create new instance of account link transaction
        /// </summary>
        /// <param name="NetworkType">The network type</param>
        /// <param name="version">The transaction version</param>
        /// <param name="deadline">The transaction deadline</param>
        /// <param name="maxFee">The max fee</param>
        /// <param name="transactionType">The transaction type</param>
        /// <param name="remoteAccount">The remote account</param>
        /// <param name="action">The action</param>
        /// <param name="signature">The signature</param>
        /// <param name="signer">The signer</param>
        /// <param name="transactionInfo">The transaction info</param>
        public AccountLinkTransaction(NetworkType NetworkType, int version, Deadline deadline,
            ulong? maxFee, EntityType transactionType,
             PublicAccount remoteAccount, AccountLinkAction action,
            string signature = null, PublicAccount signer = null,
            TransactionInfo transactionInfo = null) : base(NetworkType, version, transactionType, deadline, maxFee,
            signature, signer, transactionInfo)
        {
            Guard.NotNull(remoteAccount, nameof(remoteAccount), "remoteAccount has to be specified");

            RemoteAccount = remoteAccount;
            Action = action;
        }

        /// <summary>
        /// Create new instance of account link transaction
        /// </summary>
        /// <param name="remoteAccount">The remote account</param>
        /// <param name="action">The action</param>
        /// <param name="deadline">The transaction deadline</param>
        /// <param name="maxFee">The max fee</param>
        /// <param name="NetworkType">The network type</param>
        /// <returns>AccountLinkTransaction</returns>
        public static AccountLinkTransaction Create(PublicAccount remoteAccount, AccountLinkAction action, Deadline deadline, ulong? maxFee, NetworkType NetworkType)
        {
            return new AccountLinkTransaction(NetworkType, EntityVersion.LINK_ACCOUNT.GetValue(), deadline,
               maxFee, EntityType.LINK_ACCOUNT, remoteAccount, action);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        internal override byte[] GenerateBytes()
        {
            var builder = new FlatBufferBuilder(1);


            // create vectors
            var signatureVector = AccountLinkTransactionBuffer.CreateSignatureVector(builder, new byte[64]);
            var signerVector = AccountLinkTransactionBuffer.CreateSignerVector(builder, GetSigner());
            var feeVector = AccountLinkTransactionBuffer.CreateMaxFeeVector(builder, MaxFee?.ToUInt8Array());
            var deadlineVector = AccountLinkTransactionBuffer.CreateDeadlineVector(builder, Deadline.Ticks.ToUInt8Array());
            var remoteAccountVector = AccountLinkTransactionBuffer.CreateRemoteAccountKeyVector(builder, RemoteAccount.PublicKey.DecodeHexString());

            var totalSize =
            // header
            HEADER_SIZE +
            // remote account public key
            32 +
            // link action
            1;

            // create version
            var version = GetTxVersionSerialization();


            AccountLinkTransactionBuffer.StartAccountLinkTransactionBuffer(builder);
            AccountLinkTransactionBuffer.AddSize(builder, (uint)totalSize);
            AccountLinkTransactionBuffer.AddSignature(builder, signatureVector);
            AccountLinkTransactionBuffer.AddSigner(builder, signerVector);
            AccountLinkTransactionBuffer.AddVersion(builder, (uint)version);
            AccountLinkTransactionBuffer.AddType(builder, TransactionType.GetValue());
            AccountLinkTransactionBuffer.AddMaxFee(builder, feeVector);
            AccountLinkTransactionBuffer.AddDeadline(builder, deadlineVector);
            AccountLinkTransactionBuffer.AddRemoteAccountKey(builder, remoteAccountVector);
            AccountLinkTransactionBuffer.AddLinkAction(builder, Action.GetValueInByte());

            // end build
            var codedTransaction = AccountLinkTransactionBuffer.EndAccountLinkTransactionBuffer(builder).Value;
            builder.Finish(codedTransaction);

            var output = new AccountLinkTransactionSchema().Serialize(builder.SizedByteArray());

            if (output.Length != totalSize) throw new SerializationException("Serialized form has incorrect length");

            return output;

        }
    }
}