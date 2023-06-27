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
//using Chaos.NaCl;
using GuardNet;
using ProximaX.Sirius.Chain.Sdk.Utils;
using ProximaX.Sirius.Chain.Sdk.Crypto.Core.Chaso.NaCl;

namespace ProximaX.Sirius.Chain.Sdk.Model.Accounts
{
    /// <summary>
    ///     Class KeyPair
    /// </summary>
    public class KeyPair
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="KeyPair" /> class.
        /// </summary>
        /// <param name="privateKey">The private key</param>
        /// <param name="publicKey">The public key</param>
        public KeyPair(string privateKey, string publicKey)
        {
            Guard.NotNullOrEmpty(publicKey,nameof(publicKey));
            Guard.NotEqualTo(publicKey.Length,64, new ArgumentOutOfRangeException(nameof(publicKey)));
            PrivateKeyString = privateKey;
            PublicKeyString = publicKey;
            //PrivateKey = privateKey.FromHex();
            //PublicKey = publicKey.FromHex();
            PrivateKey = CryptoBytes.FromHexString(privateKey);
            PublicKey = CryptoBytes.FromHexString(publicKey);
        }

        /// <summary>
        ///     Private key
        /// </summary>
        public byte[] PrivateKey { get; }

        /// <summary>
        ///     Public key
        /// </summary>
        public byte[] PublicKey { get; }

        /// <summary>
        ///     Private Key in Hex
        /// </summary>
        public string PrivateKeyString { get; }

        /// <summary>
        ///     Public Key in Hex
        /// </summary>
        public string PublicKeyString { get; }

        /// <summary>
        ///     Create KeyPair from private key
        /// </summary>
        /// <param name="privateKey">The private key</param>
        /// <returns></returns>
        public static KeyPair CreateFromPrivateKey(string privateKey, DerivationScheme dScheme = DerivationScheme.Ed25519Sha3)
        {
            Guard.NotNullOrEmpty(privateKey, nameof(privateKey));
            Guard.NotEqualTo(privateKey.Length, 64, new ArgumentOutOfRangeException(nameof(privateKey)));

           // var privateKeyArray = privateKey.FromHex();
           // var publicKey = Ed25519.PublicKeyFromSeed(privateKeyArray).ToHexUpper();
            var privateKeyArray = CryptoBytes.FromHexString(privateKey);
            var publicKey = CryptoBytes.ToHexStringUpper(Ed25519.PublicKeyFromSeed(privateKeyArray, dScheme));
            return new KeyPair(privateKey, publicKey);
        }

        /// <summary>
        ///     Signs the specified data.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <returns>System.Byte[].</returns>
        /// <exception cref="ArgumentNullException">data</exception>
        public byte[] Sign(byte[] data, DerivationScheme dScheme = DerivationScheme.Ed25519Sha3)
        {
           /*
            if (data == null) throw new ArgumentNullException(nameof(data));
            var sig = new byte[64];
            var sk = new byte[64];

            Array.Copy(PrivateKey, sk, 32);

            Array.Copy(PublicKey, 0, sk, 32, 32);

            Ed25519.crypto_sign2(sig, data, sk, 32);

            CryptoBytes.Wipe(sk);

            return sig;
           */

            var sk = Ed25519.ExpandedPrivateKeyFromSeed(PrivateKey, dScheme);

            var sig = Ed25519.Sign(data, sk, dScheme);
           
            return sig;
        }

        /// <summary>
        ///     ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return
                $"{nameof(PrivateKey)}: {PrivateKey}, {nameof(PublicKey)}: {PublicKey}, {nameof(PrivateKeyString)}: {PrivateKeyString}, {nameof(PublicKeyString)}: {PublicKeyString}";
        }
    }
}