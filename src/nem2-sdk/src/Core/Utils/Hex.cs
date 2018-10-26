using io.nem2.sdk.Core.Crypto.Chaso.NaCl;
using Org.BouncyCastle.Crypto.Digests;

namespace io.nem2.sdk.Core.Utils
{
    public static class Hex
    {
        public static string EncodeHexString(this byte[] bytes)
        {
            return bytes.ToHexUpper();
        }

        public static byte[] DecodeHexString(this string bytes)
        {
            return bytes.FromHex();
        }    
    }
}
