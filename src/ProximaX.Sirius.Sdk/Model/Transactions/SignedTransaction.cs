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
using System.Text.RegularExpressions;
using ProximaX.Sirius.Sdk.Crypto.Core.Chaso.NaCl;
using ProximaX.Sirius.Sdk.Model.Blockchain;

namespace ProximaX.Sirius.Sdk.Model.Transactions
{
    /// <summary>
    ///     SignedTransaction
    /// </summary>
    public class SignedTransaction
    {
        /// <summary>
        ///     Initializes a new instance of the &lt;see cref="SignedTransaction"/&gt; class.
        /// </summary>
        /// <param name="payload">The transaction serialized data</param>
        /// <param name="hash">The transaction hash</param>
        /// <param name="signer">The transaction signer</param>
        /// <param name="transactionType">The transaction type</param>
        /// <param name="networkType">The network type</param>
        public SignedTransaction(string payload, string hash, string signer, TransactionType transactionType,
            NetworkType networkType)
        {
            if (hash.Length != 64 || !Regex.IsMatch(hash, @"\A\b[0-9a-fA-F]+\b\Z"))
                throw new ArgumentException("Invalid hash.");

            Payload = payload;
            Hash = hash;
            Signer = signer;
            TransactionType = transactionType;
            NetworkType = networkType;
        }

        /// <summary>
        ///     Transaction serialized data
        /// </summary>
        public string Payload { get; }

        /// <summary>
        ///     Transaction hash
        /// </summary>
        public string Hash { get; }

        /// <summary>
        ///     Transaction signer
        /// </summary>
        public string Signer { get; }

        /// <summary>
        ///     Transaction type
        /// </summary>
        public TransactionType TransactionType { get; }

        /// <summary>
        ///     Network type
        /// </summary>
        public NetworkType NetworkType { get; }

        /// <summary>
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="hash"></param>
        /// <param name="signer"></param>
        /// <param name="transactionType"></param>
        /// <param name="networkType"></param>
        /// <returns></returns>
        public static SignedTransaction Create(byte[] payload, byte[] hash, byte[] signer,
            TransactionType transactionType, NetworkType networkType)
        {
            if (payload == null) throw new ArgumentNullException(nameof(payload));
            if (hash == null) throw new ArgumentNullException(nameof(hash));
            if (hash.Length != 32) throw new ArgumentException("invalid hash length");
            if (signer == null) throw new ArgumentNullException(nameof(signer));
            if (signer.Length != 32) throw new ArgumentException("invalid signer length");

            return new SignedTransaction(payload.ToHexLower().ToUpper(), hash.ToHexLower().ToUpper(),
                signer.ToHexLower().ToUpper(), transactionType, networkType);
        }
    }
}