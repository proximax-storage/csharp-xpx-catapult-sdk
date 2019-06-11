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
using System.Collections;
using System.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ProximaX.Sirius.Sdk.Utils
{
    public static class ByteCoder
    {
        public static ulong ExtractBigInteger(this JToken input, string identifier)
        {
            return JsonConvert.DeserializeObject<uint[]>(input[identifier].ToString()).FromUInt8Array();
        }

        public static int ExtractInteger(this JObject input, string identifier)
        {
            return int.Parse(input[identifier].ToString());
        }

        public static int ExtractVersion(this int version)
        {
            return (int) Convert.ToInt64(version.ToString("X").Substring(2, 2), 16);
        }


        public static byte[] Take(this byte[] bytes, int start, int length)
        {
            var tempBytes = new byte[length];

            Array.Copy(bytes, start, tempBytes, 0, length);

            return tempBytes;
        }


        public static uint[] ToUInt8Array(this ulong src)
        {
            return new[]
            {
                (uint) src,
                (uint) (src >> 32)
            };
        }

        public static ulong FromUInt8Array(this uint[] src)
        {
            var s1 = Convert.ToUInt32(src[0]);
            var s2 = Convert.ToUInt32(src[1]);
            return BitConverter.ToUInt64(BitConverter.GetBytes(s1).Concat(BitConverter.GetBytes(s2)).ToArray(), 0);
        }

        public static ulong FromInt8Array(this int[] src)
        {
            return BitConverter.ToUInt64(BitConverter.GetBytes(src[0]).Concat(BitConverter.GetBytes(src[1])).ToArray(),
                0);
        }

        public static ulong[] FromUInt8ArrayArray(this uint[][] src)
        {
            var longArray = new ulong[src.Length];

            for (var i = 0; i < src.Length; i++) longArray[i] = FromUInt8Array(src[i]);

            return longArray;
        }

     
        public static byte[] ToByteArray(this BitArray bits)
        {
            const int BYTE = 8;
            var length = bits.Count / BYTE + (bits.Count % BYTE == 0 ? 0 : 1);
            var bytes = new byte[length];

            for (var i = 0; i < bits.Length; i++)
            {
                var bitIndex = i % BYTE;
                var byteIndex = i / BYTE;

                var mask = (bits[i] ? 1 : 0) << bitIndex;
                bytes[byteIndex] |= (byte) mask;
            } //for

            return bytes;
        } //ToByteArray
    }
}