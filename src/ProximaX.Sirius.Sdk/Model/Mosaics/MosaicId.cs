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
using ProximaX.Sirius.Sdk.Utils;

namespace ProximaX.Sirius.Sdk.Model.Mosaics
{
    /// <summary>
    ///     A mosaic describes an instance of a mosaic definition.
    ///     Mosaics can be transferred by means of a transfer transaction.
    /// </summary>
    public class MosaicId
    {
        private MosaicId(MosaicNonce nonce, string publicKeyHex)
        {
            Id = IdGenerator.GenerateMosaicId(BitConverter.ToUInt32(nonce.Nonce, 0), publicKeyHex);
            Nonce = nonce;
            OwnerPublicKeyHex = publicKeyHex;
        }

        public MosaicId(ulong id)
        {
            Id = id;
            Nonce = null;
            OwnerPublicKeyHex = null;
        }

        public MosaicId(string fromHexId)
        {
            Id = Hex.Hex2UInt64(fromHexId);
            Nonce = null;
            OwnerPublicKeyHex = null;
        }

        /// <summary>
        ///     The mosaic id.
        /// </summary>
        /// <value>The identifier.</value>
        public ulong Id { get; }

        /// <summary>
        /// The mosaic nonce
        /// </summary>
        public MosaicNonce Nonce { get; }

        /// <summary>
        /// The owner public key in hex
        /// </summary>
        public string OwnerPublicKeyHex { get; }

        /// <summary>
        /// The mosaic id in hex
        /// </summary>
        public string HexId => Id.ToHex();

        /// <summary>
        /// Creates mosaicId from nonce
        /// </summary>
        /// <param name="nonce">The mosaic nonce</param>
        /// <param name="publicKeyHex">The public key in hex format</param>
        /// <returns></returns>
        public static MosaicId CreateFromNonce(MosaicNonce nonce, string publicKeyHex)
        {
            return new MosaicId(nonce, publicKeyHex);
        }

        public override string ToString()
        {
            return
                $"{nameof(Id)}: {Id}, {nameof(Nonce)}: {Nonce}, {nameof(OwnerPublicKeyHex)}: {OwnerPublicKeyHex}, {nameof(HexId)}: {HexId}";
        }
    }
}