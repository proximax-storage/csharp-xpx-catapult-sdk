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
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Org.BouncyCastle.Crypto.Digests;

namespace ProximaX.Sirius.Sdk.Utils
{
    public static class IdGenerator
    {
        /// <summary>
        ///     Generates an in based on parentId and name
        /// </summary>
        /// <param name="parentId">The 64 bit id of the parent namespace.</param>
        /// <param name="name">The name of the bottom most namespace or mosaic.</param>
        /// <returns type="long">The 64 bit id.</returns>
        public static ulong GenerateId(ulong parentId, string name)
        {
            var p = BitConverter.GetBytes(parentId);
            var n = Encoding.UTF8.GetBytes(name);
            var hash = new Sha3Digest(256);

            hash.BlockUpdate(p.Concat(n).ToArray(), 0, p.Length + n.Length);

            var result = new byte[32];
            hash.DoFinal(result, 0);

            return (ulong) BitConverter.ToInt64(result, 0);
        }

        public static ulong GenerateNsId(ulong parentId, string name)
        {
            var pBytes = BitConverter.GetBytes(parentId);
            var nBytes = Encoding.UTF8.GetBytes(name);

            var hash = new Sha3Digest(256);
            hash.BlockUpdate(pBytes.Concat(nBytes).ToArray(), 0, pBytes.Length + nBytes.Length);

            var result = new byte[32];
            hash.DoFinal(result, 0);

            return (ulong) BitConverter.ToInt64(result, 0) | 0x8000000000000000;
        }

        private static void ThrowInvalidFqn(string reason, string name)
        {
            throw new Exception(string.Concat("Fully qualified id is invalid due to {0} {1}", reason, name));
        }

        public static int FindMosaicSeperatorIndex(string name)
        {
            var mosaicSeparatorIndex = name.LastIndexOf(':');
            if (0 > mosaicSeparatorIndex)
                ThrowInvalidFqn("missing mosaic", name);

            if (0 == mosaicSeparatorIndex)
                ThrowInvalidFqn("empty part", name);

            return mosaicSeparatorIndex;
        }

        public static string ExtractPartName(string name, int start, int size)
        {
            if (0 == size)
                ThrowInvalidFqn("empty part", name);

            var partName = name.Substring(start, size);
            if (Regex.IsMatch(partName, Constants.NamePattern))
                ThrowInvalidFqn("invalid part name ", name);

            return partName;
        }

        public static string Append(string path, string id, string name)
        {
            if (Constants.NamespaceMaxDepth == path.Length)
                ThrowInvalidFqn("too many parts", name);

            return string.Concat(path, id);
        }

        /// <summary>
        ///     Generate MosaicId
        /// </summary>
        /// <param name="nonce"></param>
        /// <param name="ownerPublicKeyHex"></param>
        /// <returns></returns>
        public static ulong GenerateMosaicId(uint nonce, string ownerPublicKeyHex)
        {
            var nBytes = BitConverter.GetBytes(nonce);
            var pkBytes = ownerPublicKeyHex.DecodeHexString();

            var hash = new Sha3Digest(256);
            hash.BlockUpdate(nBytes.Concat(pkBytes).ToArray(), 0, nBytes.Length + pkBytes.Length);
            var result = new byte[32];
            hash.DoFinal(result, 0);

            // 64 bits
            result = result.Take(0, 8);

            // Convert to bit array and remove the last bit
            var bitArray = new BitArray(result) {[63] = false};

            return BitConverter.ToUInt64(bitArray.ToByteArray(), 0);
        }

        public static ulong GenerateNamespaceId(string name)
        {
            var paths = GenerateNamespacePath(name);
            return paths[paths.Count - 1];
        }

        public static List<ulong> GenerateNamespacePath(string name)
        {
            var path = new List<ulong>();
            var parts = name.Split('.');

            if (parts.Length == 0)
                throw new InvalidDataException($"Invalid name space name {name}");

            if (parts.Length > 3)
                throw new IndexOutOfRangeException($"Too many parts of the namespace (max 3) {parts.Length}");

            ulong namespaceId = 0;

            var r = new Regex("^[a-z0-9][a-z0-9-_]*$");

            foreach (var p in parts)
            {
                if (!r.IsMatch(p)) throw new InvalidDataException($"Invalid name space name {name}");

                namespaceId = GenerateNsId(namespaceId, p);
                path.Add(namespaceId);
            }

            return path;
        }

        /// <summary>
        ///     Generate subnamespaceid from parent id
        /// </summary>
        /// <param name="parentId">The parent id</param>
        /// <param name="namespaceName">The namespace name</param>
        /// <returns></returns>
        public static ulong GenerateSubNamespaceIdFromParentId(ulong parentId, string namespaceName)
        {
            return GenerateNsId(parentId, namespaceName);
        }

        public struct Constants
        {
            internal static long NamespaceBaseId = 0;
            internal static int NamespaceMaxDepth = 3;
            internal static string NamePattern = "/^[a-z0-9] [a-z0-9-_]*$/";
        }
    }
}