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

using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Digests;
using ProximaX.Sirius.Chain.Sdk.Crypto.Core.Chaso.NaCl;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;

namespace ProximaX.Sirius.Chain.Sdk.Model.Accounts
{
    /// <summary>
    ///     An account is a key pair (private and public key) associated with a mutable state stored in the blockchain
    /// </summary>
    public class Account
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="Account" /> class.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <param name="keyPair">The key pair.</param>
        public Account(Address address, KeyPair keyPair)
        {
            Address = address;
            KeyPair = keyPair;
            PublicAccount = new PublicAccount(keyPair.PublicKeyString, address.NetworkType);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Account" /> class.
        /// </summary>
        /// <param name="keyPair">The address.</param>
        /// <param name="networkType">The key pair.</param>
        public Account(KeyPair keyPair, NetworkType networkType)
        {
            KeyPair = keyPair;
            PublicAccount = new PublicAccount(PublicKey, networkType);
        }

        /// <summary>
        ///     The account address
        /// </summary>
        public Address Address { get; }

        /// <summary>
        ///     The account keyPair, public and private key.
        /// </summary>
        public KeyPair KeyPair { get; }

        /// <summary>
        ///     The private key
        /// </summary>
        public string PrivateKey => KeyPair.PrivateKeyString;

        /// <summary>
        ///     The public key
        /// </summary>
        public string PublicKey => KeyPair.PublicKeyString;

        /// <summary>
        ///     The public account
        /// </summary>
        public PublicAccount PublicAccount { get; set; }


        /// <summary>
        ///     Creates from private key.
        /// </summary>
        /// <param name="privateKey">The private key.</param>
        /// <param name="networkType">Type of the network.</param>
        /// <returns>Account</returns>
        public static Account CreateFromPrivateKey(string privateKey, NetworkType networkType)
        {
            var keyPair = KeyPair.CreateFromPrivateKey(privateKey);
            var address = Address.CreateFromPublicKey(keyPair.PublicKeyString, networkType);

            return new Account(address, keyPair);
        }

        /// <summary>
        ///     Generates a new account
        /// </summary>
        /// <param name="networkType">The network type</param>
        /// <returns>Account</returns>
        public static Account GenerateNewAccount(NetworkType networkType)
        {
            var provider = new RNGCryptoServiceProvider();

            var randomBytes = new byte[2048];
            provider.GetNonZeroBytes(randomBytes);

            var digestSha3 = new Sha3Digest(256);
            var bytes = new byte[32];
            digestSha3.BlockUpdate(randomBytes, 0, 32);
            digestSha3.DoFinal(bytes, 0);

            var keyPair = KeyPair.CreateFromPrivateKey(bytes.ToHexLower());
            var address = Address.CreateFromPublicKey(keyPair.PublicKeyString, networkType);

            return new Account(address, keyPair);
        }


        /// <summary>
        ///     Signs the specified transaction.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <returns>SignedTransaction.</returns>
        public SignedTransaction Sign(Transaction transaction)
        {
            return transaction.SignWith(this);
        }

        /// <summary>
        ///     ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return
                $"{nameof(Address)}: {Address}, {nameof(KeyPair)}: {KeyPair}, {nameof(PrivateKey)}: {PrivateKey}, {nameof(PublicKey)}: {PublicKey}, {nameof(PublicAccount)}: {PublicAccount}";
        }
    }
}