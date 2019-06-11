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


namespace ProximaX.Sirius.Sdk.Model.Transactions
{
    /// <summary>
    ///     Class CosignatureSignedTransaction.
    /// </summary>
    public class CosignatureSignedTransaction
    {
        /// <summary>
        /// </summary>
        /// <param name="parentHash"></param>
        /// <param name="signature"></param>
        /// <param name="signer"></param>
        public CosignatureSignedTransaction(string parentHash, string signature, string signer)
        {
            ParentHash = parentHash;
            Signature = signature;
            Signer = signer;
        }

        /// <summary>
        ///     Gets or sets the parent hash.
        /// </summary>
        /// <value>The parent hash.</value>
        public string ParentHash { get; set; }

        /// <summary>
        ///     Gets or sets the signature.
        /// </summary>
        /// <value>The signature.</value>
        public string Signature { get; set; }

        /// <summary>
        ///     Gets or sets the signer.
        /// </summary>
        /// <value>The signer.</value>
        public string Signer { get; set; }
    }
}