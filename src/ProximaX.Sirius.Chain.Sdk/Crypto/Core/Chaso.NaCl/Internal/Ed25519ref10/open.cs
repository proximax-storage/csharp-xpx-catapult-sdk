
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

using Org.BouncyCastle.Crypto.Digests;
using System;
namespace ProximaX.Sirius.Chain.Sdk.Crypto.Core.Chaso.NaCl.Internal.Ed25519ref10
{
    internal static partial class Ed25519Operations
    {
     
        public static bool crypto_sign_verify(
            byte[] sig, int sigoffset,
            byte[] m, int moffset, int mlen,
            byte[] pk, int pkoffset)
        {
      
            byte[] checkr = new byte[32];
            GroupElementP3 A;
            GroupElementP2 R;

            if ((sig[sigoffset + 63] & 224) != 0) return false;
            if (GroupOperations.ge_frombytes_negate_vartime(out A, pk, pkoffset) != 0)
                return false;
            var hash = new Sha3Digest(512);

            hash.BlockUpdate(sig, sigoffset, 32);
            hash.BlockUpdate(pk, pkoffset, 32);
            hash.BlockUpdate(m, moffset, mlen);
            var b = new byte[64];
            hash.DoFinal(b, 0);


            ScalarOperations.sc_reduce(b);

            var sm32 = new byte[32];//todo: remove allocation
            Array.Copy(sig, sigoffset + 32, sm32, 0, 32);
            GroupOperations.ge_double_scalarmult_vartime(out R, b, ref A, sm32);
            GroupOperations.ge_tobytes(checkr, 0, ref R);
            var result = CryptoBytes.ConstantTimeEquals(checkr, 0, sig, sigoffset, 32);
            CryptoBytes.Wipe(b);
            CryptoBytes.Wipe(checkr);
            return result;
        }
    }
}