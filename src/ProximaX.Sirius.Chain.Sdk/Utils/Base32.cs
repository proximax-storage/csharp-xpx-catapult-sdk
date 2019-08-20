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
using System.Text;

namespace ProximaX.Sirius.Chain.Sdk.Utils
{
    public static class Base32Coder
    {
        private const int InByteSize = 8;

        private const int OutByteSize = 5;

        private const string Base32Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ234567";

        public static string ToBase32String(this byte[] bytes)
        {
            if (bytes == null) return null;

            if (bytes.Length == 0) return string.Empty;

            var builder = new StringBuilder(bytes.Length * InByteSize / OutByteSize);

            var bytesPosition = 0;

            var bytesSubPosition = 0;

            byte outputBase32Byte = 0;

            var outputBase32BytePosition = 0;

            while (bytesPosition < bytes.Length)
            {
                var bitsAvailableInByte =
                    Math.Min(InByteSize - bytesSubPosition, OutByteSize - outputBase32BytePosition);

                outputBase32Byte <<= bitsAvailableInByte;

                outputBase32Byte |=
                    (byte) (bytes[bytesPosition] >> (InByteSize - (bytesSubPosition + bitsAvailableInByte)));

                bytesSubPosition += bitsAvailableInByte;

                if (bytesSubPosition >= InByteSize)
                {
                    bytesPosition++;
                    bytesSubPosition = 0;
                }

                outputBase32BytePosition += bitsAvailableInByte;

                if (outputBase32BytePosition >= OutByteSize)
                {
                    outputBase32Byte &= 0x1F;

                    builder.Append(Base32Alphabet[outputBase32Byte]);

                    outputBase32BytePosition = 0;
                }
            }

            if (outputBase32BytePosition > 0)
            {
                outputBase32Byte <<= OutByteSize - outputBase32BytePosition;

                outputBase32Byte &= 0x1F;

                builder.Append(Base32Alphabet[outputBase32Byte]);
            }

            return builder.ToString();
        }

        public static byte[] FromBase32String(this string base32String)
        {
            if (base32String == null) return null;

            if (base32String == string.Empty) return new byte[0];

            var base32StringUpperCase = base32String.ToUpperInvariant();

            var outputBytes = new byte[base32StringUpperCase.Length * OutByteSize / InByteSize];

            if (outputBytes.Length == 0)
                throw new ArgumentException(
                    "Specified string is not valid Base32 format because it doesn't have enough data to construct a complete byte array");

            var base32Position = 0;

            var base32SubPosition = 0;

            var outputBytePosition = 0;

            var outputByteSubPosition = 0;

            while (outputBytePosition < outputBytes.Length)
            {
                var currentBase32Byte = Base32Alphabet.IndexOf(base32StringUpperCase[base32Position]);

                if (currentBase32Byte < 0)
                    throw new ArgumentException(
                        $"Specified string is not valid Base32 format because character \"{base32String[base32Position]}\" does not exist in Base32 alphabet");

                var bitsAvailableInByte = Math.Min(OutByteSize - base32SubPosition, InByteSize - outputByteSubPosition);

                outputBytes[outputBytePosition] <<= bitsAvailableInByte;

                outputBytes[outputBytePosition] |=
                    (byte) (currentBase32Byte >> (OutByteSize - (base32SubPosition + bitsAvailableInByte)));

                outputByteSubPosition += bitsAvailableInByte;

                if (outputByteSubPosition >= InByteSize)
                {
                    outputBytePosition++;
                    outputByteSubPosition = 0;
                }

                base32SubPosition += bitsAvailableInByte;

                if (base32SubPosition >= OutByteSize)
                {
                    base32Position++;
                    base32SubPosition = 0;
                }
            }

            return outputBytes;
        }
    }
}