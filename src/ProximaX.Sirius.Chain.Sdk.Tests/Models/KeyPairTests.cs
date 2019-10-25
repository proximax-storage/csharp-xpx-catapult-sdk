
using FluentAssertions;
using ProximaX.Sirius.Chain.Sdk.Crypto.Core.Chaso.NaCl;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace ProximaX.Sirius.Chain.Sdk.Tests.Models
{
    public class KeyPairTests
    {
        [Fact]
        public void Should_Create_KeyPair_From_Private_Key()
        {
            var pk = "A574ECE8F79DE11A39C6BADF0EF87C2C88730A5EA4CF2C0BD7E27103390BC4F4";
            var pubKey = "2B0FF0CADE0D945A23D1AF7AF266A0BAB7E07B163756F5F16CC75F24A4EEF23B";
            var keyPair = KeyPair.CreateFromPrivateKey(pk);

            keyPair.PublicKeyString.Should().Be(pubKey);
            var puk = Chaos.NaCl.Ed25519.PublicKeyFromSeed(keyPair.PublicKey);
            var sk = Chaos.NaCl.Ed25519.ExpandedPrivateKeyFromSeed(keyPair.PrivateKey);
            var skHex = sk.ToHexUpper();

            var message = "Test";

            var data = Encoding.ASCII.GetBytes(message);

            var sig = Chaos.NaCl.Ed25519.Sign(data, sk);
            var hex1 = sig.ToHexUpper();
         


            var sig2 = new byte[64];
            var sk2 = new byte[64];

            Array.Copy(keyPair.PrivateKey, sk2, 32);

            Array.Copy(keyPair.PublicKey, 0, sk2, 32, 32);

            Ed25519.crypto_sign2(sig2, data, sk2, 32);

            CryptoBytes.Wipe(sk2);

            var hex = sig2.ToHexUpper();

            var isValid = Chaos.NaCl.Ed25519.Verify(sig, data, keyPair.PublicKey);
            var isValid2 = Chaos.NaCl.Ed25519.Verify(sig2, data, keyPair.PublicKey);
            var t = "";
        }

    }
}
