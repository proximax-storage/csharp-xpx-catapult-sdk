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

namespace ProximaX.Sirius.Chain.Sdk.Model.Blockchain
{
    /// <summary>
    ///     Class BlockchainStorageInfo
    /// </summary>
    public class BlockchainStorageInfo
    {
        public BlockchainStorageInfo(int numAccounts, int numBlocks, int numTransactions)
        {
            NumAccounts = numAccounts;
            NumBlocks = numBlocks;
            NumTransactions = numTransactions;
        }

        /// <summary>
        ///     Returns number of accounts published in the blockchain
        /// </summary>
        /// <value>The number accounts.</value>
        public int NumAccounts { get; }

        /// <summary>
        ///     Returns number of confirmed blocks.
        /// </summary>
        /// <value>The number blocks.</value>
        public int NumBlocks { get; }

        /// <summary>
        ///     Returns number of confirmed transactions.
        /// </summary>
        /// <value>The number transactions.</value>
        public int NumTransactions { get; }
    }
}