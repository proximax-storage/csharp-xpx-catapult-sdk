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
using ProximaX.Sirius.Chain.Sdk.Crypto.Core.Chaso.NaCl;

namespace ProximaX.Sirius.Chain.Sdk.Model.Mosaics
{
    /// <summary>
    ///     Before a mosaic can be created or transferred, a corresponding definition of the mosaic has to be created and
    ///     published to the network. This is done via a mosaic definition transaction.
    /// </summary>
    public class MosaicNonce
    {
        /// <summary>
        ///     The nonce bytes length
        /// </summary>
        private const int NonceBytes = 4;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="nonce"></param>
        public MosaicNonce(byte[] nonce)
        {
            if (nonce == null || nonce.Length != NonceBytes)
                throw new ArgumentNullException(nameof(nonce));

            Nonce = nonce;
        }

        /// <summary>
        ///     The nonce
        /// </summary>
        public byte[] Nonce { get; set; }

        /// <summary>
        ///     Create a random MosaicNonce
        /// </summary>
        /// <returns>MosaicNonce</returns>
        public static MosaicNonce CreateRandom()
        {
            var bytes = new byte[NonceBytes];
            var rnd = new Random();
            rnd.NextBytes(bytes);
            return new MosaicNonce(bytes);
        }

        /// <summary>
        ///     Create nonce from value
        /// </summary>
        /// <param name="value">The value</param>
        /// <returns>MosaicNonce</returns>
        public static MosaicNonce Create(uint value)
        {
            return new MosaicNonce(BitConverter.GetBytes(value));
        }

        /// <summary>
        ///     Create mosaic nonce from hex
        /// </summary>
        /// <param name="hex">The hex string</param>
        /// <returns>MosaicNonce</returns>
        public static MosaicNonce Create(string hex)
        {
            var bytes = hex.FromHex();

            if (bytes.Length != NonceBytes)
                throw new ArgumentNullException($"Expected 4 bytes for Nonce but got {bytes.Length} instead.");

            return new MosaicNonce(bytes);
        }
    }
}