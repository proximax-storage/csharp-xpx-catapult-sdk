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
using System.Linq;
using System.Text.RegularExpressions;
using GuardNet;
using Org.BouncyCastle.Crypto.Digests;
using ProximaX.Sirius.Chain.Sdk.Crypto.Core.Chaso.NaCl;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Exceptions;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Model.Accounts
{
    /// <summary>
    ///     Address
    /// </summary>
    public class Address
    {
        private const int Ripemd160 = 20;
        private const int AddressDecoded = 25;
        private const int Key = 32;
        private const int Checksum = 4;

        /// <summary>
        ///     Network Type
        /// </summary>
        public NetworkType NetworkType;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="address"></param>
        /// <param name="networkType"></param>
        public Address(string address, NetworkType networkType)
        {
            Plain = Regex.Replace(address.Replace("-", ""), @"\s+", "").ToUpper();
            NetworkType = networkType;
        }

        /// <summary>
        ///     Plain address
        /// </summary>
        public string Plain { get; }

        /// <summary>
        ///     Pretty address
        /// </summary>
        public string Pretty => Regex.Replace(Plain, ".{6}", "$0-");

        /// <summary>
        ///     Create address from raw
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static Address CreateFromRawAddress(string address)
        {
            NetworkType networkType;

            var modAddress = address
                .Trim()
                .ToUpper()
                .Replace("-", "");

            Guard.NotEqualTo(modAddress.Length,40,
                new ArgumentOutOfRangeException($"The address has to be 40 characters long"));

            var addPrefix = modAddress.ToCharArray()[0];
            switch (addPrefix)
            {
                case 'S':
                    networkType = NetworkType.MIJIN_TEST;
                    break;
                case 'M':
                    networkType = NetworkType.MIJIN;
                    break;
                case 'X':
                    networkType = NetworkType.MAIN_NET;
                    break;
                case 'V':
                    networkType = NetworkType.TEST_NET;
                    break;
                case 'Z':
                    networkType = NetworkType.PRIVATE;
                    break;
                case 'W':
                    networkType = NetworkType.PRIVATE_TEST;
                    break;
                default:
                    throw new TypeNotSupportException($"Network type value {addPrefix} is not support.");


            }

            return new Address(modAddress, networkType);
        }

        /// <summary>
        ///     Creates address from hex
        /// </summary>
        /// <param name="address"></param>
        /// <returns></returns>
        public static Address CreateFromHex(string address)
        {
            return CreateFromRawAddress(address.FromHex().ToBase32String());
        }

        /// <summary>
        ///     Creates address from public key
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="networkType"></param>
        /// <returns></returns>
        public static Address CreateFromPublicKey(string publicKey, NetworkType networkType, number version)
        {
            // step 1) sha-3(256) public key
            var digestSha;
            if(version == 1){
                digestSha = new Sha3Digest(256);
            }else{
                digestSha = new SHA256Digest();
            }
            
            var stepOne = new byte[Key];

            digestSha.BlockUpdate(publicKey.FromHex(), 0, Key);
            digestSha.DoFinal(stepOne, 0);

            // step 2) perform ripemd160 on previous step
            var digestRipeMd160 = new RipeMD160Digest();
            var stepTwo = new byte[Ripemd160];
            digestRipeMd160.BlockUpdate(stepOne, 0, Key);
            digestRipeMd160.DoFinal(stepTwo, 0);

            // step3) prepend network byte    
            var stepThree = new[] {networkType.GetValueInByte()}.Concat(stepTwo).ToArray();

            // step 4) perform sha3 on previous step
            var stepFour = new byte[Key];
            digestSha.BlockUpdate(stepThree, 0, Ripemd160 + 1);
            digestSha.DoFinal(stepFour, 0);

            // step 5) retrieve checksum
            var stepFive = new byte[Checksum];
            Array.Copy(stepFour, 0, stepFive, 0, Checksum);

            // step 6) append stepFive to result of stepThree
            var stepSix = new byte[AddressDecoded];
            Array.Copy(stepThree, 0, stepSix, 0, Ripemd160 + 1);
            Array.Copy(stepFive, 0, stepSix, Ripemd160 + 1, Checksum);

            // step 7) return base 32 encode address byte array
            return CreateFromRawAddress(stepSix.ToBase32String());
        }


        /// <summary>
        ///     ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{nameof(NetworkType)}: {NetworkType}, {nameof(Plain)}: {Plain}, {nameof(Pretty)}: {Pretty}";
        }
    }

}