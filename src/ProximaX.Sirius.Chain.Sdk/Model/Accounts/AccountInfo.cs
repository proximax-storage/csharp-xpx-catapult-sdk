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

using System.Collections.Generic;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;

namespace ProximaX.Sirius.Chain.Sdk.Model.Accounts
{
    /// <summary>
    ///     Class AccountInfo
    /// </summary>
    public class AccountInfo
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="address"></param>
        /// <param name="addressHeight"></param>
        /// <param name="publicKey"></param>
        /// <param name="publicKeyHeight"></param>
        /// <param name="mosaics"></param>
        /// <param name="meta"></param>
        public AccountInfo(Address address, ulong addressHeight, string publicKey,
            ulong publicKeyHeight, IList<Mosaic> mosaics, object meta = null)
        {
            Address = address;
            AddressHeight = addressHeight;
            PublicKey = publicKey;
            PublicKeyHeight = publicKeyHeight;
            Mosaics = mosaics;
            Meta = meta;
        }

        /// <summary>
        ///     Address
        /// </summary>
        public Address Address { get; }

        /// <summary>
        ///     AddressHeight
        /// </summary>
        public ulong AddressHeight { get; }

        /// <summary>
        ///     PublicKey
        /// </summary>
        public string PublicKey { get; }

        /// <summary>
        ///     PublicKeyHeight
        /// </summary>
        public ulong PublicKeyHeight { get; }

        /// <summary>
        ///     Mosaics
        /// </summary>
        public IList<Mosaic> Mosaics { get; }

        /// <summary>
        ///     PublicAccount
        /// </summary>
        public PublicAccount PublicAccount => new PublicAccount(PublicKey, Address.NetworkType);

        /// <summary>
        ///     Meta
        /// </summary>
        public object Meta { get; }

        /// <summary>
        ///     ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return
                $"{nameof(Address)}: {Address}, {nameof(AddressHeight)}: {AddressHeight}, {nameof(PublicKey)}: {PublicKey}, {nameof(PublicKeyHeight)}: {PublicKeyHeight}, {nameof(Mosaics)}: {Mosaics}, {nameof(PublicAccount)}: {PublicAccount}, {nameof(Meta)}: {Meta}";
        }
    }
}