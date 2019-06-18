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


namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions.Messages
{
    public class EmptyMessage : IMessage
    {
        /// <summary>
        ///     Prevents a default instance of the <see cref="EmptyMessage" /> class from being created.
        /// </summary>
        private EmptyMessage()
        {
            Type = MessageType.PLAIN_MESSAGE.GetValueInByte();
        }

        /// <summary>
        ///     Gets the type.
        /// </summary>
        /// <value>The type.</value>
        private byte Type { get; }

        public byte GetMessageType()
        {
            return Type;
        }

        public byte[] GetPayload()
        {
            return new byte[] { };
        }

        public uint GetLength()
        {
            return 1;
        }

        /// <summary>
        ///     Creates this instance.
        /// </summary>
        /// <returns>EmptyMessage.</returns>
        public static EmptyMessage Create()
        {
            return new EmptyMessage();
        }
    }
}