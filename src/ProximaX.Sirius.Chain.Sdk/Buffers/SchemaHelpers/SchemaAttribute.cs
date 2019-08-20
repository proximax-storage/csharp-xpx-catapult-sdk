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

using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Buffers.SchemaHelpers
{
    internal abstract class SchemaAttribute
    {
        protected SchemaAttribute(string name)
        {
            Name = name;
        }

        private string Name { get; }

        internal abstract byte[] Serialize(byte[] buffer, int position, int innerObjectPosition);

        internal byte[] Serialize(byte[] buffer, int position)
        {
            return Serialize(buffer, position, buffer[0]);
        }

        internal string GetName()
        {
            return Name;
        }

        protected byte[] FindParam(int innerObjectPosition, int position, byte[] buffer, Constants.Value typeSize)
        {
            var offset = __offset(innerObjectPosition, position, buffer);

            return offset == 0 ? new byte[] {0} : buffer.Take(offset + innerObjectPosition, (int) typeSize);
        }

        protected byte[] FindVector(int innerObjectPosition, int position, byte[] buffer, Constants.Value typeSize)
        {
            var offset = __offset(innerObjectPosition, position, buffer);
            var offsetLong = offset + innerObjectPosition;
            var vecStart = Vector(offsetLong, buffer);
            var vecLength = Vector_length(offsetLong, buffer) * (int) typeSize;
            return offset == 0 ? new byte[] {0} : buffer.Take(vecStart, vecLength);
        }

        protected int FindObjectStartPosition(int innerObjectPosition, int position, byte[] buffer)
        {
            var offset = __offset(innerObjectPosition, position, buffer);
            return Indirect(offset + innerObjectPosition, buffer);
        }

        protected int FindArrayLength(int innerObjectPosition, int position, byte[] buffer)
        {
            var offset = __offset(innerObjectPosition, position, buffer);
            return offset == 0 ? 0 : Vector_length(innerObjectPosition + offset, buffer);
        }

        protected int FindObjectArrayElementStartPosition(int innerObjectPosition, int position, byte[] buffer,
            int startPosition)
        {
            var offset = __offset(innerObjectPosition, position, buffer);
            var vector = Vector(innerObjectPosition + offset, buffer);
            return Indirect(vector + startPosition * 4, buffer);
        }

        protected int __offset(int innerObjectPosition, int position, byte[] buffer)
        {
            var vTable = innerObjectPosition - ReadInt32(innerObjectPosition, buffer);
            return position < ReadInt16(vTable, buffer) ? ReadInt16(vTable + position, buffer) : 0;
        }

        protected int ReadInt32(int offset, byte[] buffer)
        {
            return buffer[offset] | (buffer[offset + 1] << 8) | (buffer[offset + 2] << 16) | (buffer[offset + 3] << 24);
        }

        protected int ReadInt16(int offset, byte[] bytes)
        {
            return bytes[offset] | (bytes[offset + 1] << 8);
        }

        protected int Vector_length(int offset, byte[] buffer)
        {
            return ReadInt32(offset + ReadInt32(offset, buffer), buffer);
        }

        protected int Indirect(int offset, byte[] buffer)
        {
            return offset + ReadInt32(offset, buffer);
        }

        protected int Vector(int offset, byte[] buffer)
        {
            return offset + ReadInt32(offset, buffer) + 4;
        }
    }
}