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
using System.Collections.Generic;
using Newtonsoft.Json;
using ProximaX.Sirius.Chain.Sdk.Crypto.Core.Chaso.NaCl;
using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Model.Namespaces;

namespace ProximaX.Sirius.Chain.Sdk.Utils
{
    public static class Converter
    {
        public static ulong ToUInt64(this UInt64DTO input)
        {
            var arrayInput = JsonConvert.SerializeObject(input, Formatting.Indented);
            return JsonConvert.DeserializeObject<uint[]>(arrayInput).FromUInt8Array();
        }

        public static ulong FromUInt8Array(this List<uint> input)
        {
            return input.ToArray().FromUInt8Array();
        }

        public static ulong FromUInt8Array(this UInt64DTO input)
        {
            return input.ToArray().FromUInt8Array();
        }

        public static ulong[] ToUInt64Array(this List<UInt64DTO> input)
        {
            var arrayInput = JsonConvert.SerializeObject(input, Formatting.Indented);
            return JsonConvert.DeserializeObject<uint[][]>(arrayInput).FromUInt8ArrayArray();
        }

        public static string ToHex(this ulong input)
        {
            var hexLength = input.ToString("X").Length;

            return hexLength % 2 == 0 ? input.ToString("X") : input.ToString("X" + (hexLength + 1));
        }

        public static string ToHex(this int input)
        {
            var hexLength = input.ToString("X").Length;

            return hexLength % 2 == 0 ? input.ToString("X") : input.ToString("X" + (hexLength + 1));
        }


        public static byte[] AliasToRecipient(this NamespaceId id)
        {
            // 0x91 | namespaceId on 8 bytes | 16 bytes 0-pad = 25 bytes
            const int length = 1 + 8 + 16;

            var paddedZero = "00".Repeat(16);

            var aliasBytes = new List<byte> { 0x91 };
            aliasBytes.AddRange(BitConverter.GetBytes(id.Id));
            aliasBytes.AddRange(paddedZero.FromHex());

            if (aliasBytes.ToArray().Length == length)
                return aliasBytes.ToArray();

            throw new ArgumentOutOfRangeException($"Unable to convert to namespace alias");
        }

    }
}