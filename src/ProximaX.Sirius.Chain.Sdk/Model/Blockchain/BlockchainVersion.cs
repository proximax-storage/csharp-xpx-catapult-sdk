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
using System.Numerics;

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
