/*
 *This Source Code Form is subject to the terms of the Mozilla internal
 * License, v. 2.0. If a copy of the MPL was not distributed with this
 * file, You can obtain one at http://mozilla.org/MPL/2.0/.
 *
 */

using System.Security.Cryptography;

namespace io.nem2.sdk.Core.Crypto
{
    internal class TweetNaCl
    {
        #region internal-key cryptography - Implementation of curve25519-xsalsa20-poly1305

        /// <summary>
        /// crypto_scalarmult_curve25519
        /// </summary>
        internal static readonly int ScalarmultBytes = 32;

        /// <summary>
        /// crypto_scalarmult_curve25519
        /// </summary>
        internal static readonly int ScalarBytes = 32;

        /// <summary>
        /// crypto_box_beforenm computed shared key size 
        /// </summary>
        internal static readonly int BoxBeforenmBytes = 32;
        internal static readonly int BoxinternalKeyBytes = 32;
        internal static readonly int BoxSecretKeyBytes = 32;
        internal static readonly int BoxNonceBytes = 24;
        internal static readonly int BoxZeroBytes = 32;
        internal static readonly int BoxBoxZeroBytes = 16;

        internal static readonly int SecretBoxNonceBytes = 24;
        internal static readonly int SecretBoxKeyBytes = 32;

        /// <summary>
        /// SHA-512 hash bytes
        /// </summary>
        internal static readonly int HashBytes = 64;

        internal static readonly int SigninternalKeyBytes = 32;
        internal static readonly int SignSecretKeyBytes = 64;

        internal static readonly int SignBytes = 64;


        internal class InvalidSignatureException : CryptographicException { }
        internal class InvalidCipherTextException : CryptographicException { }
        internal class InvalidEncryptionKeypair : CryptographicException { }
      
        #endregion

        #region private methods

        #region Curve25519


        private static void Set25519(long[] /*gf*/ r, long[] /*gf*/ a)
        {
            for (var i = 0; i < 16; ++i)
            {
                r[i] = a[i];
            }
        }

        private static void Car25519(long[] /*gf*/ o, int oOffset)
        {
            for (var i = 0; i < 16; ++i)
            {
                o[oOffset + i] += (1 << 16);
                long c = o[oOffset + i] >> 16;
                o[oOffset + (i + 1) * (i < 15 ? 1 : 0)] += c - 1 + 37 * (c - 1) * (i == 15 ? 1 : 0);
                o[oOffset + i] -= c << 16;
            }
        }

        private static void Sel25519(long[] /*gf*/ p, long[] /*gf*/ q, int b)
        {
            long t, c = ~(b - 1);
            for (var i = 0; i < 16; ++i)
            {
                t = c & (p[i] ^ q[i]);
                p[i] ^= t;
                q[i] ^= t;
            }
        }

        private static void Pack25519(byte[] o, long[] /*gf*/ n, int nOffset)
        {
            int b = 0, i, j;
            long[] /*gf*/ m = new long[GF_LEN], t = new long[GF_LEN];

            for (i = 0; i < 16; ++i)
            {
                t[i] = n[nOffset + i];
            }

            Car25519(t, 0);
            Car25519(t, 0);
            Car25519(t, 0);

            for (j = 0; j < 2; ++j)
            {
                m[0] = t[0] - 0xffed;

                for (i = 1; i < 15; i++)
                {
                    m[i] = t[i] - 0xffff - ((m[i - 1] >> 16) & 1);
                    m[i - 1] &= 0xffff;
                }

                m[15] = t[15] - 0x7fff - ((m[14] >> 16) & 1);
                b = (int)((m[15] >> 16) & 1);
                m[14] &= 0xffff;
                Sel25519(t, m, 1 - b);
            }

            for (i = 0; i < 16; ++i)
            {
                o[2 * i] = (byte)t[i];
                o[2 * i + 1] = (byte)(t[i] >> 8);
            }
        }

        private static int Neq25519(long[] /*gf*/ a, long[] /*gf*/ b)
        {
            byte[] c = new byte[32], d = new byte[32];
            Pack25519(c, a, 0);
            Pack25519(d, b, 0);
            return CryptoVerify32(c, d);
        }

        private static byte Par25519(long[] /*gf*/ a)
        {
            byte[] d = new byte[32];

            Pack25519(d, a, 0);

            return (byte)(d[0] & 1);
        }

        private static void Unpack25519(long[] /*gf*/ o, byte[] n)
        {
            for (var i = 0; i < 16; ++i)
            {
                o[i] = (0xff & n[2 * i]) + ((0xffL & n[2 * i + 1]) << 8);
            }

            o[15] &= 0x7fff;
        }

        private static void A(long[] /*gf*/ o, long[] /*gf*/ a, long[] /*gf*/ b)
        {
            for (var i = 0; i < 16; ++i)
            {
                o[i] = a[i] + b[i];
            }
        }

        private static void Z(long[] /*gf*/ o, long[] /*gf*/ a, long[] /*gf*/ b)
        {
            for (var i = 0; i < 16; ++i)
            {
                o[i] = a[i] - b[i];
            }
        }

        private static void M(long[] /*gf*/ o, int oOffset, long[] /*gf*/ a, int aOffset, long[] /*gf*/ b, int bOffset)
        {
            long[] t = new long[31];

            for (var i = 0; i < 31; ++i)
            {
                t[i] = 0;
            }

            for (var i = 0; i < 16; ++i)
            {
                for (var j = 0; j < 16; ++j)
                {
                    t[i + j] += a[aOffset + i] * b[bOffset + j];
                }
            }

            for (var i = 0; i < 15; ++i)
            {
                t[i] += 38 * t[i + 16];
            }

            for (var i = 0; i < 16; ++i)
            {
                o[oOffset + i] = t[i];
            }

            Car25519(o, oOffset);
            Car25519(o, oOffset);
        }

        private static void S(long[] /*gf*/ o, long[] /*gf*/ a)
        {
            M(o, 0, a, 0, a, 0);
        }

        private static void Inv25519(long[] /*gf*/ o, int oOffset, long[] /*gf*/ i, int iOffset)
        {
            long[] /*gf*/ c = new long[GF_LEN];

            for (var a = 0; a < 16; ++a)
            {
                c[a] = i[iOffset + a];
            }

            for (var a = 253; a >= 0; a--)
            {
                S(c, c);
                if (a != 2 && a != 4)
                {
                    M(c, 0, c, 0, i, iOffset);
                }
            }

            for (var a = 0; a < 16; ++a)
            {
                o[oOffset + a] = c[a];
            }
        }

        private static void Pow2523(long[] /*gf*/ o, long[] /*gf*/ i)
        {
            long[] /*gf*/ c = new long[GF_LEN];

            for (var a = 0; a < 16; ++a)
            {
                c[a] = i[a];
            }

            for (var a = 250; a >= 0; a--)
            {
                S(c, c);

                if (a != 1)
                {
                    M(c, 0, c, 0, i, 0);
                }
            }

            for (var a = 0; a < 16; ++a)
            {
                o[a] = c[a];
            }
        }

        #endregion

        #region Ed25519

        internal static void Pack(byte[] r, long[][] /*gf*/ p/*[4]*/)
        {
            long[] /*gf*/ tx = new long[GF_LEN], ty = new long[GF_LEN], zi = new long[GF_LEN];

            Inv25519(zi, 0, p[2], 0);
            M(tx, 0, p[0], 0, zi, 0);
            M(ty, 0, p[1], 0, zi, 0);

            Pack25519(r, ty, 0);

            r[31] ^= (byte)(Par25519(tx) << 7);
        }

        private static void Add(long[][] p, long[][] q)
        {
            long[] a = new long[GF_LEN],
                    b = new long[GF_LEN],
                    c = new long[GF_LEN],
                    d = new long[GF_LEN],
                    t = new long[GF_LEN],
                    e = new long[GF_LEN],
                    f = new long[GF_LEN],
                    g = new long[GF_LEN],
                    h = new long[GF_LEN]
                    ;

            Z(a, p[1], p[0]);
            Z(t, q[1], q[0]);
            M(a, 0, a, 0, t, 0);
            A(b, p[0], p[1]);
            A(t, q[0], q[1]);
            M(b, 0, b, 0, t, 0);
            M(c, 0, p[3], 0, q[3], 0);
            M(c, 0, c, 0, D2, 0);
            M(d, 0, p[2], 0, q[2], 0);
            A(d, d, d);
            Z(e, b, a);
            Z(f, d, c);
            A(g, d, c);
            A(h, b, a);

            M(p[0], 0, e, 0, f, 0);
            M(p[1], 0, h, 0, g, 0);
            M(p[2], 0, g, 0, f, 0);
            M(p[3], 0, e, 0, h, 0);
        }

        private static void Cswap(long[][] /*gf*/ p/*[4]*/, long[][] /*gf*/ q/*[4]*/, byte b)
        {
            for (var i = 0; i < 4; i++)
                Sel25519(p[i], q[i], b & 0xff);
        }

        internal static void Scalarmult(long[][] /*gf*/ p /*[4]*/ , long[][] /*gf*/ q /*[4]*/, byte[] s, int sOffset)
        {
            Set25519(p[0], GF0);
            Set25519(p[1], GF1);
            Set25519(p[2], GF1);
            Set25519(p[3], GF0);

            for (var i = 255; i >= 0; --i)
            {
                byte b = (byte)(((0xff & s[sOffset + i / 8]) >> (i & 7)) & 1);
                Cswap(p, q, b);
                Add(q, p);
                Add(p, p);
                Cswap(p, q, b);
            }
        }

        #endregion


        private static int Vn(byte[] x, byte[] y, int n, int xOffset = 0)
        {
            int d = 0;
            for (var i = 0; i < n; ++i) d |= x[i + xOffset] ^ y[i];
            return (1 & ((d - 1) >> 8)) - 1;
        }

        private static int CryptoVerify32(byte[] x, byte[] y)
        {
            return Vn(x, y, 32);
        }

       

        internal static int Unpackneg(long[][] /*gf*/ r/*[4]*/, byte[] p/*[32]*/)
        {
            long[] /*gf*/ t = new long[GF_LEN],
                chk = new long[GF_LEN],
                num = new long[GF_LEN],
                den = new long[GF_LEN],
                den2 = new long[GF_LEN],
                den4 = new long[GF_LEN],
                den6 = new long[GF_LEN];

            Set25519(r[2], GF1);
            Unpack25519(r[1], p);
            S(num, r[1]);
            M(den, 0, num, 0, D, 0);
            Z(num, num, r[2]);
            A(den, r[2], den);

            S(den2, den);
            S(den4, den2);
            M(den6, 0, den4, 0, den2, 0);
            M(t, 0, den6, 0, num, 0);
            M(t, 0, t, 0, den, 0);

            Pow2523(t, t);
            M(t, 0, t, 0, num, 0);
            M(t, 0, t, 0, den, 0);
            M(t, 0, t, 0, den, 0);
            M(r[0], 0, t, 0, den, 0);

            S(chk, r[0]);
            M(chk, 0, chk, 0, den, 0);
            if (Neq25519(chk, num) != 0)
            {
                M(r[0], 0, r[0], 0, I, 0);
            }

            S(chk, r[0]);
            M(chk, 0, chk, 0, den, 0);
            if (Neq25519(chk, num) != 0)
            {
                return -1;
            }

            if (Par25519(r[0]) == ((0xff & p[31]) >> 7))
            {
                Z(r[0], GF0, r[0]);
            }

            M(r[3], 0, r[0], 0, r[1], 0);

            return 0;
        }

        #endregion

        #region Secret-key cryptography

        #endregion


        private const int GF_LEN = 16;

        private static readonly long[] GF0 = new long[GF_LEN];

        private static readonly long[] GF1 = new long[GF_LEN] { 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        private static long[] _121665 = new long[GF_LEN] { 0xDB41, 1, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

        private static readonly long[] D = new long[] { 0x78a3, 0x1359, 0x4dca, 0x75eb, 0xd8ab, 0x4141, 0x0a4d, 0x0070, 0xe898, 0x7779, 0x4079, 0x8cc7, 0xfe73, 0x2b6f, 0x6cee, 0x5203 };

        private static readonly long[] D2 = new long[] { 0xf159, 0x26b2, 0x9b94, 0xebd6, 0xb156, 0x8283, 0x149a, 0x00e0, 0xd130, 0xeef3, 0x80f2, 0x198e, 0xfce7, 0x56df, 0xd9dc, 0x2406 };

        private static long[] X = new long[] { 0xd51a, 0x8f25, 0x2d60, 0xc956, 0xa7b2, 0x9525, 0xc760, 0x692c, 0xdc5c, 0xfdd6, 0xe231, 0xc0a4, 0x53fe, 0xcd6e, 0x36d3, 0x2169 };

        private static long[] Y = new long[] { 0x6658, 0x6666, 0x6666, 0x6666, 0x6666, 0x6666, 0x6666, 0x6666, 0x6666, 0x6666, 0x6666, 0x6666, 0x6666, 0x6666, 0x6666, 0x6666 };

        private static readonly long[] I = new long[] { 0xa0b0, 0x4a0e, 0x1b27, 0xc4ee, 0xe478, 0xad2f, 0x1806, 0x2f43, 0xd7a7, 0x3dfb, 0x0099, 0x2b4d, 0xdf0b, 0x4fc1, 0x2480, 0x2b83 };

        private static ulong[] K = new ulong[80]
        {
          0x428a2f98d728ae22, 0x7137449123ef65cd, 0xb5c0fbcfec4d3b2f, 0xe9b5dba58189dbbc,
          0x3956c25bf348b538, 0x59f111f1b605d019, 0x923f82a4af194f9b, 0xab1c5ed5da6d8118,
          0xd807aa98a3030242, 0x12835b0145706fbe, 0x243185be4ee4b28c, 0x550c7dc3d5ffb4e2,
          0x72be5d74f27b896f, 0x80deb1fe3b1696b1, 0x9bdc06a725c71235, 0xc19bf174cf692694,
          0xe49b69c19ef14ad2, 0xefbe4786384f25e3, 0x0fc19dc68b8cd5b5, 0x240ca1cc77ac9c65,
          0x2de92c6f592b0275, 0x4a7484aa6ea6e483, 0x5cb0a9dcbd41fbd4, 0x76f988da831153b5,
          0x983e5152ee66dfab, 0xa831c66d2db43210, 0xb00327c898fb213f, 0xbf597fc7beef0ee4,
          0xc6e00bf33da88fc2, 0xd5a79147930aa725, 0x06ca6351e003826f, 0x142929670a0e6e70,
          0x27b70a8546d22ffc, 0x2e1b21385c26c926, 0x4d2c6dfc5ac42aed, 0x53380d139d95b3df,
          0x650a73548baf63de, 0x766a0abb3c77b2a8, 0x81c2c92e47edaee6, 0x92722c851482353b,
          0xa2bfe8a14cf10364, 0xa81a664bbc423001, 0xc24b8b70d0f89791, 0xc76c51a30654be30,
          0xd192e819d6ef5218, 0xd69906245565a910, 0xf40e35855771202a, 0x106aa07032bbd1b8,
          0x19a4c116b8d2d0c8, 0x1e376c085141ab53, 0x2748774cdf8eeb99, 0x34b0bcb5e19b48a8,
          0x391c0cb3c5c95a63, 0x4ed8aa4ae3418acb, 0x5b9cca4f7763e373, 0x682e6ff3d6b2b8a3,
          0x748f82ee5defb2fc, 0x78a5636f43172f60, 0x84c87814a1f0ab72, 0x8cc702081a6439ec,
          0x90befffa23631e28, 0xa4506cebde82bde9, 0xbef9a3f7b2c67915, 0xc67178f2e372532b,
          0xca273eceea26619c, 0xd186b8c721c0c207, 0xeada7dd6cde0eb1e, 0xf57d4f7fee6ed178,
          0x06f067aa72176fba, 0x0a637dc5a2c898a6, 0x113f9804bef90dae, 0x1b710b35131c471b,
          0x28db77f523047d84, 0x32caab7b40c72493, 0x3c9ebe0a15c9bebc, 0x431d67c49c100d4c,
          0x4cc5d4becb3e42b6, 0x597f299cfc657e2a, 0x5fcb6fab3ad6faec, 0x6c44198c4a475817
        };

        private static byte[] iv = new byte[64]
        {
          0x6a,0x09,0xe6,0x67,0xf3,0xbc,0xc9,0x08,
          0xbb,0x67,0xae,0x85,0x84,0xca,0xa7,0x3b,
          0x3c,0x6e,0xf3,0x72,0xfe,0x94,0xf8,0x2b,
          0xa5,0x4f,0xf5,0x3a,0x5f,0x1d,0x36,0xf1,
          0x51,0x0e,0x52,0x7f,0xad,0xe6,0x82,0xd1,
          0x9b,0x05,0x68,0x8c,0x2b,0x3e,0x6c,0x1f,
          0x1f,0x83,0xd9,0xab,0xfb,0x41,0xbd,0x6b,
          0x5b,0xe0,0xcd,0x19,0x13,0x7e,0x21,0x79
        };
    }
}