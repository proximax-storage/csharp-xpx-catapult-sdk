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
using System.Text;
using GuardNet;

namespace ProximaX.Sirius.Sdk.Model.Transactions.Messages
{
    /// <summary>
    ///     Class PlainMessage.
    /// </summary>
    /// <seealso cref="IMessage" />
    public class PlainMessage : IMessage
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="PlainMessage" /> class.
        /// </summary>
        /// <param name="payload">The payload.</param>
        public PlainMessage(byte[] payload)
        {
            Type = MessageType.PLAIN_MESSAGE.GetValueInByte();
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
        ///     Gets the type of the message.
        /// </summary>
        /// <returns>byte</returns>
        public byte GetMessageType()
        {
            return Type;
        }

        /// <summary>
        ///     Creates the specified payload.
        /// </summary>
        /// <param name="payload">The payload.</param>
        /// <returns>PlainMessage.</returns>
        /// <exception cref="ArgumentNullException">payload</exception>
        public static PlainMessage Create(string payload)
        {
            Guard.NotNullOrEmpty(payload, "Payload could not be null or empty");

            return new PlainMessage(Encoding.UTF8.GetBytes(payload));
        }

        /// <summary>
        ///     Gets the String payload.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetStringPayload()
        {
            return Encoding.UTF8.GetString(Payload);
        }
    }
}