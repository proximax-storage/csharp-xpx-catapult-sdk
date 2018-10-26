using System;
using System.Linq;
using System.Runtime.CompilerServices;
using io.nem2.sdk.Model.Blockchain;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleJson;

[assembly: InternalsVisibleTo("integration-test")]
[assembly: InternalsVisibleTo("test")]

namespace io.nem2.sdk.Core.Utils
{
    public static class ByteCoder
    {
        internal static ulong ExtractBigInteger(this JToken input, string identifier)
        {
            return JsonConvert.DeserializeObject<uint[]>(input[identifier].ToString()).FromUInt8Array();
        }

        internal static int ExtractInteger(this JsonObject input, string identifier)
        {

            return int.Parse(input[identifier].ToString());
        }

        internal static int ExtractVersion(this int version)
        {
            return (int)Convert.ToInt64(version.ToString("X").Substring(2, 2), 16);
        }

        internal static NetworkType.Types ExtractNetworkType(this int version)
        {
            var networkType = (int)Convert.ToInt64(version.ToString("X").Substring(0, 2), 16);

            return NetworkType.GetRawValue(networkType);
        }

        internal static byte[] Take(this byte[] bytes, int start, int length)
        {
            var tempBytes = new byte[length];

            Array.Copy(bytes, start, tempBytes, 0, length);

            return tempBytes;
        }

        internal static uint[] ToUInt8Array(this ulong src)
        {
            return new[]
            {
                (uint) src,
                (uint) (src >> 32),
            };
        }

        internal static ulong FromUInt8Array(this uint[] src)
        {
            return BitConverter.ToUInt64(BitConverter.GetBytes(src[0]).Concat(BitConverter.GetBytes(src[1])).ToArray(), 0);
        }

        internal static ulong FromInt8Array(this int[] src)
        {
            return BitConverter.ToUInt64(BitConverter.GetBytes(src[0]).Concat(BitConverter.GetBytes(src[1])).ToArray(), 0);
        }

        internal static ulong[] FromUInt8ArrayArray(this uint[][] src)
        {
            var longArray = new ulong[src.Length];

            for (var i = 0; i < src.Length; i++)
            {
                longArray[i] = FromUInt8Array(src[i]);
            }

            return longArray;
        }
    }
}