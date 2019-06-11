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

using ProximaX.Sirius.Sdk.Model.Accounts;
using ProximaX.Sirius.Sdk.Model.Blockchain;
using ProximaX.Sirius.Sdk.Model.Mosaics;

namespace ProximaX.Sirius.Sdk.Model.Transactions
{
    /// <summary>
    ///     HashLockTransaction
    /// </summary>
    public class HashLockTransaction : LockFundsTransaction
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
        public HashLockTransaction(NetworkType networkType, int version, Deadline deadline, ulong maxFee, Mosaic mosaic,
            ulong duration, SignedTransaction transaction, string signature = null, PublicAccount signer = null,
            TransactionInfo transactionInfo = null)
            : base(networkType, version, deadline, maxFee, mosaic, duration, transaction, signature, signer,
                transactionInfo)
        {
        }
    }
}