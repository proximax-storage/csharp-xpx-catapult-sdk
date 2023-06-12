
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
using ProximaX.Sirius.Chain.Sdk.Utils;
namespace ProximaX.Sirius.Chain.Sdk.Crypto.Core.Chaso.NaCl.Internal.Ed25519ref10
{
	internal static partial class Ed25519Operations
	{
		public static void crypto_sign2(
			byte[] sig, int sigoffset,
			byte[] m, int moffset, int mlen,
			byte[] sk, int skoffset,
            DerivationScheme dScheme = DerivationScheme.Ed25519Sha3)
		{
            byte[] az = new byte[64];
            byte[] r = new byte[64];
            byte[] hram = new byte[64];
            GroupElementP3 R;
 
            if(dScheme == DerivationScheme.Ed25519Sha3){
                var hasher3 = new Sha3Digest(512);
                {
                    hasher3.BlockUpdate(sk, 0, 32);
                    hasher3.DoFinal(az, 0);
                    ScalarOperations.sc_clamp(az, 0);

                    hasher3.Reset();
                    hasher3.BlockUpdate(az, 32, 32);
                    hasher3.BlockUpdate(m, moffset, mlen);
                    hasher3.DoFinal(r, 0);

                    ScalarOperations.sc_reduce(r);
                    GroupOperations.ge_scalarmult_base(out R, r, 0);
                    GroupOperations.ge_p3_tobytes(sig, sigoffset, ref R);

                    hasher3.Reset();
                    hasher3.BlockUpdate(sig, sigoffset, 32);
                    hasher3.BlockUpdate(sk, skoffset + 32, 32);
                    hasher3.BlockUpdate(m, moffset, mlen);
                    hasher3.DoFinal(hram, 0);

                    ScalarOperations.sc_reduce(hram);
                    var s = new byte[32];
                    Array.Copy(sig, sigoffset + 32, s, 0, 32);
                    ScalarOperations.sc_muladd(s, hram, az, r);
                    Array.Copy(s, 0, sig, sigoffset + 32, 32);

                    CryptoBytes.Wipe(s);
                }
            }else if(dScheme == DerivationScheme.Ed25519Sha2){
                var hasher2 = new SHA256Digest();
                {
                    hasher2.BlockUpdate(sk, 0, 32);
                    hasher2.DoFinal(az, 0);
                    ScalarOperations.sc_clamp(az, 0);

                    hasher2.Reset();
                    hasher2.BlockUpdate(az, 32, 32);
                    hasher2.BlockUpdate(m, moffset, mlen);
                    hasher2.DoFinal(r, 0);

                    ScalarOperations.sc_reduce(r);
                    GroupOperations.ge_scalarmult_base(out R, r, 0);
                    GroupOperations.ge_p3_tobytes(sig, sigoffset, ref R);

                    hasher2.Reset();
                    hasher2.BlockUpdate(sig, sigoffset, 32);
                    hasher2.BlockUpdate(sk, skoffset + 32, 32);
                    hasher2.BlockUpdate(m, moffset, mlen);
                    hasher2.DoFinal(hram, 0);

                    ScalarOperations.sc_reduce(hram);
                    var s = new byte[32];
                    Array.Copy(sig, sigoffset + 32, s, 0, 32);
                    ScalarOperations.sc_muladd(s, hram, az, r);
                    Array.Copy(s, 0, sig, sigoffset + 32, 32);

                    CryptoBytes.Wipe(s);
                }
            }
            
        }
	}
}