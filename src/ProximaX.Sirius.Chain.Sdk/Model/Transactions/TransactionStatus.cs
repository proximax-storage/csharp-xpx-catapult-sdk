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

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions
{
    /// <summary>
    ///     TransactionStatus
    /// </summary>
    public class TransactionStatus
    {
        /// <summary>
        ///     Create transaction status object
        /// </summary>
        /// <param name="group">The transaction status group</param>
        /// <param name="status">The transaction status</param>
        /// <param name="hash">The transaction hash</param>
        /// <param name="deadline">The transaction deadline</param>
        /// <param name="height">The height of the block</param>
        public TransactionStatus(string group, string status, string hash, ulong deadline, ulong? height)
        {
            Group = group;
            Status = status;
            Hash = hash;
            Deadline = deadline;
            Height = height;
        }

        /// <summary>
        ///     The transaction status group "failed", "unconfirmed", "confirmed", etc...
        /// </summary>
        public string Group { get; }

        /// <summary>
        ///     The transaction status being the error name in case of failure and success otherwise.
        /// </summary>
        public string Status { get; }

        /// <summary>
        ///     The transaction hash.
        /// </summary>
        public string Hash { get; }

        /// <summary>
        ///     The transaction deadline.
        /// </summary>
        public ulong Deadline { get; }

        /// <summary>
        ///     The height of the block at which it was confirmed or rejected.
        /// </summary>
        public ulong? Height { get; }
    }
}