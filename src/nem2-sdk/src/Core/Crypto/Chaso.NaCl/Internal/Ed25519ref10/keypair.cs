using System;
using Org.BouncyCastle.Crypto.Digests;

namespace io.nem2.sdk.Core.Crypto.Chaso.NaCl.Internal.Ed25519ref10
{
    internal static partial class Ed25519Operations
    {
        internal static void crypto_sign_keypair(byte[] pk, int pkoffset, byte[] sk, int skoffset, byte[] seed, int seedoffset)
        {
            GroupElementP3 A;
            int i;
            
            Array.Copy(seed, seedoffset, sk, skoffset, 32);
            var digest = new Sha3Digest(512); //new  // tried and failed -> new Sha3Digest(512);
            byte[] h = new byte[64];   // byte[] ha = Sha512.Hash(sk, skoffset, 32);//ToDo: Remove alloc
            digest.BlockUpdate(sk, skoffset, 32); // new
            digest.DoFinal(h, 0);  // new
         
            ScalarOperations.sc_clamp(h, 0);
            GroupOperations.ge_scalarmult_base(out A, h, 0);
            GroupOperations.ge_p3_tobytes(pk, pkoffset, ref A);

            for (i = 0; i < 32; ++i) sk[skoffset + 32 + i] = pk[pkoffset + i];
            CryptoBytes.Wipe(h);
        }
    }
}
