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
using ProximaX.Sirius.Chain.Sdk.Utils;
using ProximaX.Sirius.Chain.Sdk.Crypto.Core.Chaso.NaCl.Internal.Ed25519ref10;

namespace ProximaX.Sirius.Chain.Sdk.Crypto.Core.Chaso.NaCl
{
    public static class Ed25519
    {
        public static readonly int PublicKeySizeInBytes = 32;
        public static readonly int SignatureSizeInBytes = 64;
        public static readonly int ExpandedPrivateKeySizeInBytes = 32 * 2;
        public static readonly int PrivateKeySeedSizeInBytes = 32;
        public static readonly int SharedKeySizeInBytes = 32;

        public static bool Verify(ArraySegment<byte> signature, ArraySegment<byte> message, ArraySegment<byte> publicKey, DerivationScheme dScheme = DerivationScheme.Ed25519Sha3)
        {
            if (signature.Count != SignatureSizeInBytes)
                throw new ArgumentException(string.Format("Signature size must be {0}", SignatureSizeInBytes), "signature.Count");
            if (publicKey.Count != PublicKeySizeInBytes)
                throw new ArgumentException(string.Format("Public key size must be {0}", PublicKeySizeInBytes), "publicKey.Count");
            return Ed25519Operations.crypto_sign_verify(signature.Array, signature.Offset, message.Array, message.Offset, message.Count, publicKey.Array, publicKey.Offset, dScheme);
        }

        public static bool Verify(byte[] signature, byte[] message, byte[] publicKey, DerivationScheme dScheme = DerivationScheme.Ed25519Sha3)
        {
            if (signature == null)
                throw new ArgumentNullException("signature");
            if (message == null)
                throw new ArgumentNullException("message");
            if (publicKey == null)
                throw new ArgumentNullException("publicKey");
            if (signature.Length != SignatureSizeInBytes)
                throw new ArgumentException(string.Format("Signature size must be {0}", SignatureSizeInBytes), "signature.Length");
            if (publicKey.Length != PublicKeySizeInBytes)
                throw new ArgumentException(string.Format("Public key size must be {0}", PublicKeySizeInBytes), "publicKey.Length");
            return Ed25519Operations.crypto_sign_verify(signature, 0, message, 0, message.Length, publicKey, 0, dScheme);
        }

        public static void Sign(ArraySegment<byte> signature, ArraySegment<byte> message, ArraySegment<byte> expandedPrivateKey, DerivationScheme dScheme = DerivationScheme.Ed25519Sha3)
        {
            if (signature.Array == null)
                throw new ArgumentNullException("signature.Array");
            if (signature.Count != SignatureSizeInBytes)
                throw new ArgumentException("signature.Count");
            if (expandedPrivateKey.Array == null)
                throw new ArgumentNullException("expandedPrivateKey.Array");
            if (expandedPrivateKey.Count != ExpandedPrivateKeySizeInBytes)
                throw new ArgumentException("expandedPrivateKey.Count");
            if (message.Array == null)
                throw new ArgumentNullException("message.Array");
            Ed25519Operations.crypto_sign(signature.Array, signature.Offset, message.Array, message.Offset, message.Count, expandedPrivateKey.Array, expandedPrivateKey.Offset, dScheme);
        }

        public static byte[] Sign(byte[] message, byte[] expandedPrivateKey, DerivationScheme dScheme = DerivationScheme.Ed25519Sha3)
        {
            var signature = new byte[SignatureSizeInBytes];
            Sign(new ArraySegment<byte>(signature), new ArraySegment<byte>(message), new ArraySegment<byte>(expandedPrivateKey), dScheme);
            return signature;
        }

        public static byte[] PublicKeyFromSeed(byte[] privateKeySeed, DerivationScheme dScheme = DerivationScheme.Ed25519Sha3)
        {
            byte[] privateKey;
            byte[] publicKey;
            KeyPairFromSeed(out publicKey, out privateKey, privateKeySeed, dScheme);
            CryptoBytes.Wipe(privateKey);
            return publicKey;
        }

        public static byte[] ExpandedPrivateKeyFromSeed(byte[] privateKeySeed, DerivationScheme dScheme = DerivationScheme.Ed25519Sha3)
        {
            byte[] privateKey;
            byte[] publicKey;
            KeyPairFromSeed(out publicKey, out privateKey, privateKeySeed, dScheme);
            CryptoBytes.Wipe(publicKey);
            return privateKey;
        }

        public static void KeyPairFromSeed(out byte[] publicKey, out byte[] expandedPrivateKey, byte[] privateKeySeed, DerivationScheme dScheme = DerivationScheme.Ed25519Sha3)
        {
            if (privateKeySeed == null)
                throw new ArgumentNullException("privateKeySeed");
            if (privateKeySeed.Length != PrivateKeySeedSizeInBytes)
                throw new ArgumentException("privateKeySeed");
            var pk = new byte[PublicKeySizeInBytes];
            var sk = new byte[ExpandedPrivateKeySizeInBytes];
            Ed25519Operations.crypto_sign_keypair(pk, 0, sk, 0, privateKeySeed, 0, dScheme);
            publicKey = pk;
            expandedPrivateKey = sk;
        }

        public static void KeyPairFromSeed(ArraySegment<byte> publicKey, ArraySegment<byte> expandedPrivateKey, ArraySegment<byte> privateKeySeed, DerivationScheme dScheme = DerivationScheme.Ed25519Sha3)
        {
            if (publicKey.Array == null)
                throw new ArgumentNullException("publicKey.Array");
            if (expandedPrivateKey.Array == null)
                throw new ArgumentNullException("expandedPrivateKey.Array");
            if (privateKeySeed.Array == null)
                throw new ArgumentNullException("privateKeySeed.Array");
            if (publicKey.Count != PublicKeySizeInBytes)
                throw new ArgumentException("publicKey.Count");
            if (expandedPrivateKey.Count != ExpandedPrivateKeySizeInBytes)
                throw new ArgumentException("expandedPrivateKey.Count");
            if (privateKeySeed.Count != PrivateKeySeedSizeInBytes)
                throw new ArgumentException("privateKeySeed.Count");
            Ed25519Operations.crypto_sign_keypair(
                publicKey.Array, publicKey.Offset,
                expandedPrivateKey.Array, expandedPrivateKey.Offset,
                privateKeySeed.Array, privateKeySeed.Offset, dScheme);
        }

    }
}