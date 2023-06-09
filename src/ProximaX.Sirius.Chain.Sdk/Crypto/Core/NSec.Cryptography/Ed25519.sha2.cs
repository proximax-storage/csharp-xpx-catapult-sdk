
using System;
using System.Runtime.CompilerServices;
using NSec.Cryptography;


namespace ProximaX.Sirius.Chain.Sdk.Crypto.Core.Nsec.Cryptography
{
    public static class Ed25519Sha2
    {
        public static byte[] PublicKeyFromSeedSha2(byte[] privateKeySeed){
            // byte[] privateKeyBytes = Convert.FromBase64String("..."); // Replace "..." with your private key in Base64 format
            var publicKey64Str;
            using (var privateKey = Key.Import(SignatureAlgorithm.Ed25519, privateKeySeed, KeyBlobFormat.RawPrivateKey))
            using (var publicKey = privateKey.PublicKey)
            {
                var publicKeyBytes = publicKey.Export(KeyBlobFormat.RawPublicKey);

                publicKey64Str = Convert.ToBase64String(publicKeyBytes);
            }
            return publicKey64Str;
        }
    }
}