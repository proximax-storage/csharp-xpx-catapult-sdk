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

using ProximaX.Sirius.Sdk.Crypto.Core;
using ProximaX.Sirius.Sdk.Crypto.Core.Chaso.NaCl;

namespace ProximaX.Sirius.Sdk.Model.Transactions.Messages
{
    /// <summary>
    /// The SecureMessage
    /// </summary>
    public class SecureMessage : IMessage
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="payload"></param>
        public SecureMessage(byte[] payload)
        {
            Type = MessageType.ENCRYPTED_MESSAGE.GetValueInByte();
            Payload = payload;
        }

        /// <summary>
        ///     Gets the type.
        /// </summary>
        /// <value>The type.</value>
        private byte Type { get; }

        /// <summary>
        ///     Gets the payload.
        /// </summary>
        /// <value>The payload.</value>
        private byte[] Payload { get; }


        /// <summary>
        ///     Gets the type of the message.
        /// </summary>
        /// <returns>byte</returns>
        public byte GetMessageType()
        {
            return Type;
        }

        /// <summary>
        ///     Gets the payload.
        /// </summary>
        /// <returns>byte[]</returns>
        public byte[] GetPayload()
        {
            return Payload;
        }

        /// <summary>
        ///     Gets the length.
        /// </summary>
        /// <returns>uint</returns>
        public uint GetLength()
        {
            return (uint) Payload.Length + 1;
        }

        /// <summary>
        ///     Creates secure message
        /// </summary>
        /// <param name="payload"></param>
        /// <param name="senderPrivateKey"></param>
        /// <param name="receiverPublicKey"></param>
        /// <returns></returns>
        public static SecureMessage Create(string payload, string senderPrivateKey, string receiverPublicKey)
        {
            return new SecureMessage(CryptoUtils.Encode(payload, senderPrivateKey, receiverPublicKey).FromHex());
        }

        /// <summary>
        ///     Creates secure message from payload
        /// </summary>
        /// <param name="payload"></param>
        /// <returns></returns>
        public static SecureMessage CreateFromEncodedPayload(byte[] payload)
        {
            return new SecureMessage(payload);
        }

        /// <summary>
        ///     Decrypt payload
        /// </summary>
        /// <param name="receiverPrivateKey"></param>
        /// <param name="senderPublicKey"></param>
        /// <returns></returns>
        public string DecryptPayload(string receiverPrivateKey, string senderPublicKey)
        {
            return CryptoUtils.Decode(Payload.ToHexLower(), receiverPrivateKey, senderPublicKey);
        }
    }
}