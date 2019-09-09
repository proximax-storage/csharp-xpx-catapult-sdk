using GuardNet;
using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ProximaX.Sirius.Chain.Sdk.Model.Blockchain
{
    public class BlockchainVersion
    {
        private static readonly int MAX_VALUE = 65535;

        private static readonly int MASK_2_BYTES = 0xFFFF;

        public BlockchainVersion(int major, int minor, int revision, int build)
        {
            Major = major;
            Minor = minor;
            Revision = revision;
            Build = build;
        }

        public int Major { get; }
        public int Minor { get; }
        public int Revision { get; }
        public int Build { get; }

        public static BlockchainVersion FromVersionValue(ulong version)
        {
            
            int build = (int)version & MASK_2_BYTES;
            int rev = (int)(version >> 16) & MASK_2_BYTES;
            int minor = (int)(version >> 32) & MASK_2_BYTES;
            int major = (int)(version >> 48) & MASK_2_BYTES;

            // create new instance
            return new BlockchainVersion(major, minor, rev, build);
        }

        public ulong GetVersionValue()
        {

            var version = new BigInteger(Major) << 16;
            version = (version + new BigInteger(Minor)) << 16;
            version = (version + new BigInteger(Revision)) << 16;
            version += new BigInteger(Build);

            return (ulong)version;
        }
    }
}
