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
using Org.BouncyCastle.Crypto.Digests;
using ProximaX.Sirius.Chain.Sdk.Crypto.Core.Chaso.NaCl.Internal.Ed25519ref10;

namespace ProximaX.Sirius.Chain.Sdk.Crypto.Core.Chaso.NaCl
{
    public static class Ed25519
    {
        internal static readonly int internalKeySizeInBytes = 32;
        internal static readonly int SignatureSizeInBytes = 64;
        internal static readonly int ExpandedPrivateKeySizeInBytes = 32 * 2;
        internal static readonly int PrivateKeySeedSizeInBytes = 32;
        internal static readonly int LongPrivateKeySizeInBytes = 33;
        internal static readonly int SharedKeySizeInBytes = 32;

        public static void crypto_sign2(
            byte[] sig,
            byte[] m,
            byte[] sk,
            int keylen)
        {
            var privHash = new byte[64];
            var seededHash = new byte[64];
            var result = new byte[64];
            var R = new GroupElementP3();
            var hasher = new Sha3Digest(512);
            {
                hasher.BlockUpdate(sk, 0, keylen);
                hasher.DoFinal(privHash, 0);

                ScalarOperations.sc_clamp(privHash, 0);

                hasher.Reset();
                hasher.BlockUpdate(privHash, 32, 32);
                hasher.BlockUpdate(m, 0, m.Length);
                hasher.DoFinal(seededHash, 0);

                ScalarOperations.sc_reduce(seededHash);

                GroupOperations.ge_scalarmult_base(out R, seededHash, 0);
                GroupOperations.ge_p3_tobytes(sig, 0, ref R);

                hasher.Reset();
                hasher.BlockUpdate(sig, 0, 32);
                hasher.BlockUpdate(sk, keylen, 32);
                hasher.BlockUpdate(m, 0, m.Length);
                hasher.DoFinal(result, 0);

                ScalarOperations.sc_reduce(result);

                var s = new byte[32];
                Array.Copy(sig, 32, s, 0, 32);
                ScalarOperations.sc_muladd(s, result, privHash, seededHash);
                Array.Copy(s, 0, sig, 32, 32);

                CryptoBytes.Wipe(s);
            }
        }

        public static void key_derive(byte[] shared, byte[] salt, byte[] secretKey, byte[] pubkey)
        {
            var longKeyHash = new byte[64];
            var shortKeyHash = new byte[32];

//            Array.Reverse(secretKey);

            // compute  Sha3(512) hash of secret key (as in prepareForScalarMultiply)
            var digestSha3 = new Sha3Digest(512);
            digestSha3.BlockUpdate(secretKey, 0, 32);
            digestSha3.DoFinal(longKeyHash, 0);

            longKeyHash[0] &= 248;
            longKeyHash[31] &= 127;
            longKeyHash[31] |= 64;

            Array.Copy(longKeyHash, 0, shortKeyHash, 0, 32);

            ScalarOperations.sc_clamp(shortKeyHash, 0);

            var p = new[] {new long[16], new long[16], new long[16], new long[16]};
            var q = new[] {new long[16], new long[16], new long[16], new long[16]};

            TweetNaCl.Unpackneg(q, pubkey); // returning -1 invalid signature
            TweetNaCl.Scalarmult(p, q, shortKeyHash, 0);
            TweetNaCl.Pack(shared, p);

            // for some reason the most significant bit of the last byte needs to be flipped.
            // doesnt seem to be any corrosponding action in nano/nem.core, so this may be an issue in one of the above 3 functions. i have no idea.
            shared[31] ^= 1 << 7;

            // salt
            for (var i = 0; i < salt.Length; i++) shared[i] ^= salt[i];

            // hash salted shared key
            var digestSha3Two = new Sha3Digest(256);
            digestSha3Two.BlockUpdate(shared, 0, 32);
            digestSha3Two.DoFinal(shared, 0);
        }

        public static byte[] PublicKeyFromSeed(byte[] privateKeySeed)
        {
            KeyPairFromSeed(out var publicKey, out var privateKey, privateKeySeed);
            CryptoBytes.Wipe(privateKey);
            return publicKey;
        }


        public static void KeyPairFromSeed(out byte[] internalKey, out byte[] expandedPrivateKey, byte[] privateKeySeed)
        {
            if (privateKeySeed == null)
                throw new ArgumentNullException(nameof(privateKeySeed));
            if (privateKeySeed.Length != 32 && privateKeySeed.Length != 33)
                throw new ArgumentException("privateKeySeed");
            var pk = new byte[internalKeySizeInBytes];
            var sk = new byte[ExpandedPrivateKeySizeInBytes];
            Ed25519Operations.crypto_sign_keypair(pk, 0, sk, 0, privateKeySeed, 0);
            internalKey = pk;
            expandedPrivateKey = sk;
        }
    }
}