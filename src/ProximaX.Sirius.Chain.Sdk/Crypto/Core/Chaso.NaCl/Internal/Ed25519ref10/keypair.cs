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
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Crypto.Core.Chaso.NaCl.Internal.Ed25519ref10
{
    internal static partial class Ed25519Operations
    {
        internal static void crypto_sign_keypair(byte[] pk, int pkoffset, byte[] sk, int skoffset, byte[] seed,
            int seedoffset, DerivationScheme dScheme = DerivationScheme.Ed25519Sha3)
        {
            GroupElementP3 A;
            int i;

            Array.Copy(seed, seedoffset, sk, skoffset, 32);
            if(dScheme == DerivationScheme.Ed25519Sha3){
                var digest = new Sha3Digest(512); //new  // tried and failed -> new Sha3Digest(512);
                var h = new byte[64]; // byte[] ha = Sha512.Hash(sk, skoffset, 32);//ToDo: REMOVE alloc
                digest.BlockUpdate(sk, skoffset, 32); // new
                digest.DoFinal(h, 0); // new

                ScalarOperations.sc_clamp(h, 0);
                GroupOperations.ge_scalarmult_base(out A, h, 0);
                GroupOperations.ge_p3_tobytes(pk, pkoffset, ref A);

                for (i = 0; i < 32; ++i) sk[skoffset + 32 + i] = pk[pkoffset + i];
                CryptoBytes.Wipe(h);
            }else if(dScheme == DerivationScheme.Ed25519Sha2){
                var digest = new Sha512Digest();
                var h = new byte[64]; // byte[] ha = Sha512.Hash(sk, skoffset, 32);//ToDo: REMOVE alloc
                digest.BlockUpdate(sk, skoffset, 32); // new
                digest.DoFinal(h, 0); // new

                ScalarOperations.sc_clamp(h, 0);
                GroupOperations.ge_scalarmult_base(out A, h, 0);
                GroupOperations.ge_p3_tobytes(pk, pkoffset, ref A);

                for (i = 0; i < 32; ++i) sk[skoffset + 32 + i] = pk[pkoffset + i];
                CryptoBytes.Wipe(h);
            }
        }


        internal static void key_derive(byte[] shared, byte[] salt, byte[] secretKey, byte[] pubkey)
        {
            var longKeyHash = new byte[64];
            var shortKeyHash = new byte[32];

            // Array.Reverse(secretKey);

            // compute  Sha3(512) hash of secret key (as in prepareForScalarMultiply)
            var digestSha3 = new Sha3Digest(512);
            digestSha3.BlockUpdate(secretKey, 0, 32);
            digestSha3.DoFinal(longKeyHash, 0);

            longKeyHash[0] &= 248;
            longKeyHash[31] &= 127;
            longKeyHash[31] |= 64;

            Array.Copy(longKeyHash, 0, shortKeyHash, 0, 32);

            ScalarOperations.sc_clamp(shortKeyHash, 0);

            var p = new[] { new long[16], new long[16], new long[16], new long[16] };
            var q = new[] { new long[16], new long[16], new long[16], new long[16] };

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
    }
}