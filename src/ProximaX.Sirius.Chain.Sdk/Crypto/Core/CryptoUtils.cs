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
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Security;
using ProximaX.Sirius.Chain.Sdk.Crypto.Core.Chaso.NaCl;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Crypto.Core
{
    public static class CryptoUtils
    {
        /// <summary>
        ///     Hash an array of bytes using Sha3 512 algorithm.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] Sha3_512(byte[] bytes)
        {
            var temp = new byte[64];
            var digest = new Sha3Digest(512);
            digest.BlockUpdate(bytes, 0, bytes.Length);
            digest.DoFinal(temp, 0);
            return temp;
        }

        /// <summary>
        ///     Hash an array of bytes using Sha3 256 algorithm.
        /// </summary>
        /// <param name="bytes">The bytes.</param>
        /// <returns>System.Byte[].</returns>
        public static byte[] Sha3_256(byte[] bytes)
        {
            var temp = new byte[32];
            var digest = new Sha3Digest(256);
            digest.BlockUpdate(bytes, 0, bytes.Length);
            digest.DoFinal(temp, 0);
            return temp;
        }

        /// <summary>
        ///     Encrypt a private key for mobile apps (AES_PBKF2)
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="privateKey">The private key.</param>
        /// <returns>System.String.</returns>
        public static string ToMobileKey(string password, string privateKey)
        {
            var random = new SecureRandom();

            var salt = new byte[256 / 8];
            random.NextBytes(salt);

            var maker = new Rfc2898DeriveBytes(password, salt, 2000);
            var key = maker.GetBytes(256 / 8);

            var ivData = new byte[16];
            random.NextBytes(ivData);

            return salt.ToHexLower() + AesEncryptor(key, ivData, privateKey);
        }


        /// <summary>
        ///     Decrypt a private key for mobile apps (AES_PBKF2)
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <param name="password">The password.</param>
        /// <returns>System.String.</returns>
        public static string FromMobileKey(string payload, string password)
        {
            var salt = payload.FromHex().Take(32).ToArray();
            var iv = payload.FromHex().Take(32, 16);
            var cypher = payload.FromHex().Take(48, payload.FromHex().Length - 48);

            var maker = new Rfc2898DeriveBytes(password, salt, 2000);
            var key = maker.GetBytes(256 / 8);

            return AesDecryptor(key, iv, cypher).ToUpper();
        }

        /// <summary>
        ///     Encodes the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="secretKey">The secret key.</param>
        /// <param name="publicKey">The public key.</param>
        /// <returns>System.String.</returns>
        public static string Encode(string text, string secretKey, string publicKey)
        {
            var random = new SecureRandom();

            var salt = new byte[32];
            random.NextBytes(salt);

            var ivData = new byte[16];
            random.NextBytes(ivData);

            return _Encode(
                secretKey.FromHex(),
                publicKey.FromHex(),
                text,
                ivData,
                salt);
        }

        /// <summary>
        ///     Decodes the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <param name="secretKey">The secret key.</param>
        /// <param name="publicKey">The public key.</param>
        /// <returns>System.String.</returns>
        public static string Decode(string text, string secretKey, string publicKey)
        {
            return _Decode(
                secretKey.FromHex(),
                publicKey.FromHex(),
                text.FromHex());
        }

        /// <summary>
        ///     Derive a private key from a password using count iterations of SHA3-256
        /// </summary>
        /// <param name="password">The password.</param>
        /// <param name="count">The count.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="ArgumentNullException">password</exception>
        /// <exception cref="ArgumentOutOfRangeException">count - must be positive</exception>
        internal static string DerivePassSha(byte[] password, int count)
        {
            if (password == null) throw new ArgumentNullException(nameof(password));
            if (count <= 0) throw new ArgumentOutOfRangeException(nameof(count), "must be positive");

            var sha3Hasher = new KeccakDigest(256);
            var hash = new byte[32];


            sha3Hasher.BlockUpdate(password, 0, password.Length);
            sha3Hasher.DoFinal(hash, 0);

            for (var i = 0; i < count - 1; ++i)
            {
                sha3Hasher.Reset();
                sha3Hasher.BlockUpdate(hash, 0, hash.Length);
                sha3Hasher.DoFinal(hash, 0);
            }

            return hash.ToHexUpper();
        }

        /// <summary>
        ///     Encodes the specified private key.
        /// </summary>
        /// <param name="privateKey">The private key.</param>
        /// <param name="publicKey">The pub key.</param>
        /// <param name="msg">The MSG.</param>
        /// <param name="iv">The iv.</param>
        /// <param name="salt">The salt.</param>
        /// <returns>System.String.</returns>
        internal static string _Encode(byte[] privateKey, byte[] publicKey, string msg, byte[] iv, byte[] salt)
        {
            var shared = new byte[32];

            Ed25519.key_derive(
                shared,
                salt,
                privateKey,
                publicKey);

            return salt.ToHexLower() + AesEncryptor(shared, iv, msg);
        }

        /// <summary>
        ///     Decodes the specified private key.
        /// </summary>
        /// <param name="privateKey">The private key.</param>
        /// <param name="publicKey">The pub key.</param>
        /// <param name="data">The data.</param>
        /// <returns>System.String.</returns>
        internal static string _Decode(byte[] privateKey, byte[] publicKey, byte[] data)
        {
            var salt = data.Take(0, 32).ToArray();
            var iv = data.Take(32, 16);
            var payload = data.Take(48, data.Length - 48);
            var shared = new byte[32];

            Ed25519.key_derive(
                shared,
                salt,
                privateKey,
                publicKey);

            return AesDecryptor(shared, iv, payload);
        }

        internal static string AesEncryptor(byte[] key, byte[] iv, string msg)
        {
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = key;

                aesAlg.IV = iv;

                aesAlg.Mode = CipherMode.CBC;

                var encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(msg);
                        }

                        return iv.ToHexLower() + msEncrypt.ToArray().ToHexLower();
                    }
                }
            }
        }

        internal static string AesDecryptor(byte[] key, byte[] iv, byte[] payload)
        {
            using (var aesAlg = Aes.Create())
            {
                aesAlg.Key = key;

                aesAlg.IV = iv;

                aesAlg.Mode = CipherMode.CBC;

                aesAlg.Padding = PaddingMode.PKCS7;

                var decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (var msDecrypt = new MemoryStream(payload))
                {
                    using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (var srDecrypt = new StreamReader(csDecrypt))
                        {
                            var a = srDecrypt.ReadToEnd();

                            return a;
                        }
                    }
                }
            }
        }
    }
}