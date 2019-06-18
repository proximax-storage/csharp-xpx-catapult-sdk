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
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions
{
    public class AccountLinkTransaction : Transaction
    {
        public AccountLinkTransaction(NetworkType networkType, int version, TransactionType transactionType,
            Deadline deadline, ulong? maxFee, string signature = null, PublicAccount signer = null,
            TransactionInfo transactionInfo = null) : base(networkType, version, transactionType, deadline, maxFee,
            signature, signer, transactionInfo)
        {
        }

        internal override byte[] GenerateBytes()
        {
            throw new NotImplementedException();
        }
    }
}