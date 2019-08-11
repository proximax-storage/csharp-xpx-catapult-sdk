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
using FlatBuffers;
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

        public AccountLinkTransaction(NetworkType networkType, int version, Deadline deadline,
            ulong? maxFee, TransactionType transactionType,
             PublicAccount remoteAccount, AccountLinkAction action,
            string signature = null, PublicAccount signer = null,
            TransactionInfo transactionInfo = null) : base(networkType, version, transactionType, deadline, maxFee,
            signature, signer, transactionInfo)
        {
            RemoteAccount = remoteAccount;
            Action = action;
        }

        public static AccountLinkTransaction Create(PublicAccount remoteAccount, AccountLinkAction action, Deadline deadline, ulong? maxFee, NetworkType networkType)
        {
            return new AccountLinkTransaction(networkType, TransactionVersion.LINK_ACCOUNT.GetValue(), deadline,
               maxFee, TransactionType.LINK_ACCOUNT, remoteAccount, action);
        }

        internal override byte[] GenerateBytes()
        {
            var builder = new FlatBufferBuilder(1);


            // create vectors
            var signatureVector = AccountLinkTransactionBuffer.CreateSignatureVector(builder, new byte[64]);
            var signerVector = AccountLinkTransactionBuffer.CreateSignerVector(builder, GetSigner());
            var feeVector = AccountLinkTransactionBuffer.CreateMaxFeeVector(builder, MaxFee?.ToUInt8Array());
            var deadlineVector = AccountLinkTransactionBuffer.CreateDeadlineVector(builder, Deadline.Ticks.ToUInt8Array());
            var remoteAccountVector = AccountLinkTransactionBuffer.CreateRemoteAccountKeyVector(builder, RemoteAccount.PublicKey.DecodeHexString());

            var fixedSize =
            // header
            120 +
            // remote account public key
            32 +
            // link action
            1;

            var version = ushort.Parse(NetworkType.GetValueInByte().ToString("X") + "0" + Version.ToString("X"),
                NumberStyles.HexNumber);


            AccountLinkTransactionBuffer.StartAccountLinkTransactionBuffer(builder);
            AccountLinkTransactionBuffer.AddSize(builder, (uint)fixedSize);
            AccountLinkTransactionBuffer.AddSignature(builder, signatureVector);
            AccountLinkTransactionBuffer.AddSigner(builder, signerVector);
            AccountLinkTransactionBuffer.AddVersion(builder, version);
            AccountLinkTransactionBuffer.AddType(builder, TransactionType.GetValue());
            AccountLinkTransactionBuffer.AddMaxFee(builder, feeVector);
            AccountLinkTransactionBuffer.AddDeadline(builder, deadlineVector);
            AccountLinkTransactionBuffer.AddRemoteAccountKey(builder, remoteAccountVector);
            AccountLinkTransactionBuffer.AddLinkAction(builder, Action.GetValueInByte());

            // end build
            var codedTransaction = AccountLinkTransactionBuffer.EndAccountLinkTransactionBuffer(builder).Value;
            builder.Finish(codedTransaction);

            return new AccountLinkTransactionSchema().Serialize(builder.SizedByteArray());
        }
    }
}