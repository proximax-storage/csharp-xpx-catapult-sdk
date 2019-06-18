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

using ProximaX.Sirius.Chain.Sdk.Model.Accounts;

namespace ProximaX.Sirius.Chain.Sdk.Model.Blockchain
{
    /// <summary>
    ///     Class BlockInfo
    /// </summary>
    public class BlockInfo
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="BlockInfo" /> class.
        /// </summary>
        /// <param name="hash"></param>
        /// <param name="generationHash"></param>
        /// <param name="totalFee"></param>
        /// <param name="numTransactions"></param>
        /// <param name="signature"></param>
        /// <param name="signer"></param>
        /// <param name="networkType"></param>
        /// <param name="version"></param>
        /// <param name="type"></param>
        /// <param name="height"></param>
        /// <param name="timestamp"></param>
        /// <param name="difficulty"></param>
        /// <param name="previousBlockHash"></param>
        /// <param name="blockTransactionsHash"></param>
        public BlockInfo(string hash, string generationHash, ulong? totalFee,
            int? numTransactions, string signature, PublicAccount signer, NetworkType networkType,
            int version, int type, ulong height, ulong timestamp, ulong difficulty,
            string previousBlockHash, string blockTransactionsHash)
        {
            Hash = hash;
            GenerationHash = generationHash;
            TotalFee = totalFee;
            NumTransactions = numTransactions;
            Signature = signature;
            Signer = signer;
            NetworkType = networkType;
            Version = version;
            Type = type;
            Height = height;
            Timestamp = timestamp;
            Difficulty = difficulty;
            PreviousBlockHash = previousBlockHash;
            BlockTransactionsHash = blockTransactionsHash;
        }

        /// <summary>
        ///     The block hash
        /// </summary>
        public string Hash { get; }

        /// <summary>
        ///     The block generation hash
        /// </summary>
        public string GenerationHash { get; }

        /// <summary>
        ///     The block total fee
        /// </summary>
        public ulong? TotalFee { get; }

        /// <summary>
        ///     The number of transactions
        /// </summary>
        public int? NumTransactions { get; }

        /// <summary>
        ///     The signature
        /// </summary>
        public string Signature { get; }

        /// <summary>
        ///     The signer
        /// </summary>
        public PublicAccount Signer { get; }

        /// <summary>
        ///     The network type
        /// </summary>
        public NetworkType NetworkType { get; }

        /// <summary>
        ///     The version
        /// </summary>
        public int Version { get; }

        /// <summary>
        ///     The block type
        /// </summary>
        public int Type { get; }

        /// <summary>
        ///     The block height
        /// </summary>
        public ulong Height { get; }

        /// <summary>
        ///     The timestamp
        /// </summary>
        public ulong Timestamp { get; }

        /// <summary>
        ///     The difficulty
        /// </summary>
        public ulong Difficulty { get; }

        /// <summary>
        ///     The previous block hash
        /// </summary>
        public string PreviousBlockHash { get; }

        /// <summary>
        ///     The block transaction hash
        /// </summary>
        public string BlockTransactionsHash { get; }

        public override string ToString()
        {
            return "BlockInfo{" +
                   "hash='" + Hash + '\'' +
                   ", generationHash='" + GenerationHash + '\'' +
                   ", totalFee=" + TotalFee +
                   ", numTransactions=" + NumTransactions +
                   ", signature='" + Signature + '\'' +
                   ", signer=" + Signer +
                   ", networkType=" + NetworkType +
                   ", version=" + Version +
                   ", type=" + Type +
                   ", height=" + Height +
                   ", timestamp=" + Timestamp +
                   ", difficulty=" + Difficulty +
                   ", previousBlockHash='" + PreviousBlockHash + '\'' +
                   ", blockTransactionsHash='" + BlockTransactionsHash + '\'' +
                   '}';
        }
    }
}