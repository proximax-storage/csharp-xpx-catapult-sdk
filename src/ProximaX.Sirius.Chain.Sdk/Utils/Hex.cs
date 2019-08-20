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
using ProximaX.Sirius.Chain.Sdk.Crypto.Core.Chaso.NaCl;

namespace ProximaX.Sirius.Chain.Sdk.Utils
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

        public static ulong Hex2UInt64(string hex)
        {
            if (string.IsNullOrEmpty(hex)) throw new ArgumentException("hex");

            var i = hex.Length > 1 && hex[0] == '0' && (hex[1] == 'x' || hex[1] == 'X') ? 2 : 0;
            ulong value = 0;

            while (i < hex.Length)
            {
                uint x = hex[i++];

                if (x >= '0' && x <= '9') x -= '0';
                else if (x >= 'A' && x <= 'F') x = x - 'A' + 10;
                else if (x >= 'a' && x <= 'f') x = x - 'a' + 10;
                else throw new ArgumentOutOfRangeException(nameof(hex));

                value = 16 * value + x;
            }

            return value;
        }

        public static bool IsHexString(this string str)
        {
            foreach (var c in str)
            {
                var isHex = c >= '0' && c <= '9' ||
                            c >= 'a' && c <= 'f' ||
                            c >= 'A' && c <= 'F';

                if (!isHex) return false;
            }

            return true;
        }

        public static bool IsParseableToByteArray(this string str)
        {
            return IsHexString(str) && str.Length % 2 == 0;
        }
    }
}