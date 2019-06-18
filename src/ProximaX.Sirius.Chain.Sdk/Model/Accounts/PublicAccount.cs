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
using System.Text.RegularExpressions;
using GuardNet;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;

namespace ProximaX.Sirius.Chain.Sdk.Model.Accounts
{
    /// <summary>
    ///     Class PublicAccount
    /// </summary>
    public class PublicAccount
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="networkType"></param>
        public PublicAccount(string publicKey, NetworkType networkType)
        {
            Guard.NotNullOrEmpty(publicKey, nameof(publicKey));

            if (!Regex.IsMatch(publicKey, @"\A\b[0-9a-fA-F]+\b\Z"))
                throw new FormatException("Invalid public key format. Public key must be in Hex");

            Guard.NotEqualTo(publicKey.Length, 64, new ArgumentOutOfRangeException(nameof(publicKey)));

            Address = Address.CreateFromPublicKey(publicKey, networkType);
            PublicKey = publicKey;
        }

        /// <summary>
        ///     Address
        /// </summary>
        public Address Address { get; }

        /// <summary>
        ///     PublicKey
        /// </summary>
        public string PublicKey { get; }

        /// <summary>
        ///     Creates from public key
        /// </summary>
        /// <param name="publicKey"></param>
        /// <param name="networkType"></param>
        /// <returns></returns>
        public static PublicAccount CreateFromPublicKey(string publicKey, NetworkType networkType)
        {
            return new PublicAccount(publicKey, networkType);
        }

        public override string ToString()
        {
            return $"{nameof(Address)}: {Address}, {nameof(PublicKey)}: {PublicKey}";
        }
    }
}