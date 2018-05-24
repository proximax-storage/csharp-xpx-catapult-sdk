using System;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Org.BouncyCastle.Crypto.Digests;

namespace io.nem2.sdk.Core.Utils
{
    internal static class IdGenerator
    {
        internal struct Constants
        {
            internal static long NamespaceBaseId = 0;
            internal static int NamespaceMaxDepth = 3;
            internal static string NamePattern = "/^[a-z0-9] [a-z0-9-_]*$/";
        }



        /// <summary>
        /// Generates an in based on parentId and name
        /// </summary>
        /// <param name="parentId">The 64 bit id of the parent namespace.</param>
        /// <param name="name">The name of the bottom most namespace or mosaic.</param>
        /// <returns type="long">The 64 bit id.</returns>
        internal static ulong GenerateId(ulong parentId, string name)
        {
            var p = BitConverter.GetBytes(parentId);
            var n = Encoding.UTF8.GetBytes(name);
            var hash = new Sha3Digest(256);

            hash.BlockUpdate(p.Concat(n).ToArray(), 0, p.Length + n.Length);     
            
            var result = new byte[32];
            hash.DoFinal(result, 0);

            return (ulong)BitConverter.ToInt64(result, 0);
        }

        private static void ThrowInvalidFqn(string reason, string name)
        {
            throw new Exception(string.Concat("Fully qualified id is invalid due to {0} {1}", reason, name));
        }

        internal static int FindMosaicSeperatorIndex(string name)
        {
            var mosaicSeparatorIndex = name.LastIndexOf(':');
            if (0 > mosaicSeparatorIndex)
                ThrowInvalidFqn("missing mosaic", name);

            if (0 == mosaicSeparatorIndex)
                ThrowInvalidFqn("empty part", name);

            return mosaicSeparatorIndex;
        }

        internal static string ExtractPartName(string name, int start, int size)
        {
            if (0 == size)
                ThrowInvalidFqn("empty part", name);

            var partName = name.Substring(start, size);
            if (Regex.IsMatch(partName, Constants.NamePattern))
                ThrowInvalidFqn("invalid part name ", name);

            return partName;
        }

        internal static string Append(string path, string id, string name)
        {
            if (Constants.NamespaceMaxDepth == path.Length)
                ThrowInvalidFqn("too many parts", name);

            return string.Concat(path, id);
        }
    }
}
