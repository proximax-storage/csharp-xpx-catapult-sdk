/*
 * Copyright 2014 Google Inc. All rights reserved.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *     http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */

using System;
using System.Text;

namespace io.nem2.sdk.Infrastructure.Imported.FlatBuffers
{
    /// <summary>
    /// All tables in the generated code derive from this struct, and add their own accessors.
    /// </summary>
    internal struct Table
    {
        internal int bb_pos;
        internal ByteBuffer bb;

        internal ByteBuffer ByteBuffer => bb;

        // Look up a field in the vtable, return an offset into the object, or 0 if the field is not
        // present.
        internal int __offset(int vtableOffset)
        {
            int vtable = bb_pos - bb.GetInt(bb_pos);
            return vtableOffset < bb.GetShort(vtable) ? (int)bb.GetShort(vtable + vtableOffset) : 0;
        }

        internal static int __offset(int vtableOffset, int offset, ByteBuffer bb)
        {
            int vtable = bb.Length - offset;
            return (int)bb.GetShort(vtable + vtableOffset - bb.GetInt(vtable)) + vtable;
        }

        // Retrieve the relative offset stored at "offset"
        internal int __indirect(int offset)
        {
            return offset + bb.GetInt(offset);
        }

        internal static int __indirect(int offset, ByteBuffer bb)
        {
            return offset + bb.GetInt(offset);
        }

        // Create a .NET String from UTF-8 data stored inside the flatbuffer.
        internal string __string(int offset)
        {
            offset += bb.GetInt(offset);
            var len = bb.GetInt(offset);
            var startPos = offset + sizeof(int);
            return Encoding.UTF8.GetString(bb.Data, startPos , len);
        }

        // Get the length of a vector whose offset is stored at "offset" in this object.
        internal int __vector_len(int offset)
        {
            offset += bb_pos;
            offset += bb.GetInt(offset);
            return bb.GetInt(offset);
        }

        // Get the start of data of a vector whose offset is stored at "offset" in this object.
        internal int __vector(int offset)
        {
            offset += bb_pos;
            return offset + bb.GetInt(offset) + sizeof(int);  // data starts after the length
        }

        // Get the data of a vector whoses offset is stored at "offset" in this object as an
        // ArraySegment&lt;byte&gt;. If the vector is not present in the ByteBuffer,
        // then a null value will be returned.
        internal ArraySegment<byte>? __vector_as_arraysegment(int offset)
        {
            var o = __offset(offset);
            if (0 == o)
            {
                return null;
            }

            var pos = __vector(o);
            var len = __vector_len(o);
            return new ArraySegment<byte>(bb.Data, pos, len);
        }

        // Initialize any Table-derived type to point to the union at the given offset.
        internal T __union<T>(int offset) where T : struct, IFlatbufferObject
        {
            offset += bb_pos;
            T t = new T();
            t.__init(offset + bb.GetInt(offset), bb);
            return t;
        }

        internal static bool __has_identifier(ByteBuffer bb, string ident)
        {
            if (ident.Length != FlatBufferConstants.FileIdentifierLength)
                throw new ArgumentException("FlatBuffers: file identifier must be length " + FlatBufferConstants.FileIdentifierLength, "ident");

            for (var i = 0; i < FlatBufferConstants.FileIdentifierLength; i++)
            {
                if (ident[i] != (char)bb.Get(bb.Position + sizeof(int) + i)) return false;
            }

            return true;
        }

        // Compare strings in the ByteBuffer.
        internal static int CompareStrings(int offset_1, int offset_2, ByteBuffer bb)
        {
            offset_1 += bb.GetInt(offset_1);
            offset_2 += bb.GetInt(offset_2);
            var len_1 = bb.GetInt(offset_1);
            var len_2 = bb.GetInt(offset_2);
            var startPos_1 = offset_1 + sizeof(int);
            var startPos_2 = offset_2 + sizeof(int);
            var len = Math.Min(len_1, len_2);
            byte[] bbArray = bb.Data;
            for(int i = 0; i < len; i++) {
                if (bbArray[i + startPos_1] != bbArray[i + startPos_2])
                    return bbArray[i + startPos_1] - bbArray[i + startPos_2];
            }
            return len_1 - len_2;
        }

        // Compare string from the ByteBuffer with the string object
        internal static int CompareStrings(int offset_1, byte[] key, ByteBuffer bb)
        {
            offset_1 += bb.GetInt(offset_1);
            var len_1 = bb.GetInt(offset_1);
            var len_2 = key.Length;
            var startPos_1 = offset_1 + sizeof(int);
            var len = Math.Min(len_1, len_2);
            byte[] bbArray = bb.Data;
            for (int i = 0; i < len; i++) {
                if (bbArray[i + startPos_1] != key[i])
                    return bbArray[i + startPos_1] - key[i];
            }
            return len_1 - len_2;
        }
    }
}
