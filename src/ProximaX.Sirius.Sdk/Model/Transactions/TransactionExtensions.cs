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
using System.Linq;
using Org.BouncyCastle.Crypto.Digests;
using ProximaX.Sirius.Sdk.Crypto.Core.Chaso.NaCl;
using ProximaX.Sirius.Sdk.Model.Accounts;
using ProximaX.Sirius.Sdk.Utils;

namespace ProximaX.Sirius.Sdk.Model.Transactions
{
    public static class TransactionExtensions
    {
        /// <summary>
        ///     Hashers the specified payload.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <returns>The transaction hash.</returns>
        public static byte[] Hasher(byte[] payload)
        {
            var sigAndKey = payload.Take(4, 32)
                .Concat(
                    payload.Take(4 + 64, payload.Length - (4 + 64))
                ).ToArray();

            var hash = new byte[32];
            var sha3Hasher = new Sha3Digest(256);
            sha3Hasher.BlockUpdate(sigAndKey, 0, sigAndKey.Length);
            sha3Hasher.DoFinal(hash, 0);

            return hash;
        }

        /// <summary>
        ///     Signs the hash.
        /// </summary>
        /// <param name="account">The account.</param>
        /// <param name="hash">The hash.</param>
        /// <returns>The signature.</returns>
        public static byte[] SignHash(KeyPair account, byte[] hash)
        {
            return account.Sign(hash);
        }

        /// <summary>
        ///     Signs the transaction.
        /// </summary>
        /// <param name="keyPair">The key pair.</param>
        /// <param name="payload">The payload.</param>
        /// <returns>The signature.</returns>
        public static byte[] SignTransaction(KeyPair keyPair, byte[] payload)
        {
            var sig = new byte[64];
            var sk = new byte[64];

            Array.Copy(keyPair.PrivateKey, sk, 32);

            Array.Copy(keyPair.PublicKey, 0, sk, 32, 32);

            Ed25519.crypto_sign2(sig, Subset(payload, 4 + 64 + 32, payload.Length - (4 + 64 + 32)).ToArray(), sk, 32);

            CryptoBytes.Wipe(sk);

            return sig;
        }

        /// <summary>
        ///     Extracts a subset of an array given a start index and length.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array">The array.</param>
        /// <param name="start">The start index.</param>
        /// <param name="count">The count.</param>
        /// <returns>The extracted subset of the given array.</returns>
        private static T[] Subset<T>(T[] array, int start, int count)
        {
            var result = new T[count];

            Array.Copy(array, start, result, 0, count);

            return result;
        }
    }
}