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

namespace ProximaX.Sirius.Sdk.Crypto.Core.Chaso.NaCl.Internal.Ed25519ref10
{
    internal static class Ed25519Operations
    {
        internal static void crypto_sign_keypair(byte[] pk, int pkoffset, byte[] sk, int skoffset, byte[] seed,
            int seedoffset)
        {
            GroupElementP3 A;
            int i;

            Array.Copy(seed, seedoffset, sk, skoffset, 32);
            var digest = new Sha3Digest(512); //new  // tried and failed -> new Sha3Digest(512);
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
}