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

// There are 2 #defines that have an impact on performance of this ByteBuffer implementation
//
//      UNSAFE_BYTEBUFFER 
//          This will use unsafe code to manipulate the underlying byte array. This
//          can yield a reasonable performance increase.
//
//      BYTEBUFFER_NO_BOUNDS_CHECK
//          This will disable the bounds check asserts to the byte array. This can
//          yield a small performance gain in normal code..
//
// Using UNSAFE_BYTEBUFFER and BYTEBUFFER_NO_BOUNDS_CHECK together can yield a
// performance gain of ~15% for some operations, however doing so is potentially 
// dangerous. Do so at your own risk!
//

using System;

namespace io.nem2.sdk.Infrastructure.Imported.FlatBuffers
{
    /// <summary>
    /// Class to mimic Java's ByteBuffer which is used heavily in Flatbuffers.
    /// </summary>
    internal class ByteBuffer
    {
        private readonly byte[] _buffer;
        private int _pos;  // Must track start of the buffer.

        internal int Length => _buffer.Length;

        internal byte[] Data => _buffer;

        internal ByteBuffer(byte[] buffer) : this(buffer, 0) { }

        internal ByteBuffer(byte[] buffer, int pos)
        {
            _buffer = buffer;
            _pos = pos;
        }

        internal int Position {
            get => _pos;
            set => _pos = value;
        }

        internal void Reset()
        {
            _pos = 0;
        }

        // Pre-allocated helper arrays for convertion.
        private readonly float[] floathelper = new[] { 0.0f };
        private readonly int[] inthelper = new[] { 0 };
        private readonly double[] doublehelper = new[] { 0.0 };
        private readonly ulong[] ulonghelper = new[] { 0UL };

        // Helper functions for the unsafe version.
        static internal ushort ReverseBytes(ushort input)
        {
            return (ushort)(((input & 0x00FFU) << 8) |
                            ((input & 0xFF00U) >> 8));
        }
        static internal uint ReverseBytes(uint input)
        {
            return ((input & 0x000000FFU) << 24) |
                   ((input & 0x0000FF00U) <<  8) |
                   ((input & 0x00FF0000U) >>  8) |
                   ((input & 0xFF000000U) >> 24);
        }
        static internal ulong ReverseBytes(ulong input)
        {
            return (((input & 0x00000000000000FFUL) << 56) |
                    ((input & 0x000000000000FF00UL) << 40) |
                    ((input & 0x0000000000FF0000UL) << 24) |
                    ((input & 0x00000000FF000000UL) <<  8) |
                    ((input & 0x000000FF00000000UL) >>  8) |
                    ((input & 0x0000FF0000000000UL) >> 24) |
                    ((input & 0x00FF000000000000UL) >> 40) |
                    ((input & 0xFF00000000000000UL) >> 56));
        }

#if !UNSAFE_BYTEBUFFER
        // Helper functions for the safe (but slower) version.
        protected void WriteLittleEndian(int offset, int count, ulong data)
        {
            if (BitConverter.IsLittleEndian)
            {
                for (int i = 0; i < count; i++)
                {
                    _buffer[offset + i] = (byte)(data >> i * 8);
                }
            }
            else
            {
                for (int i = 0; i < count; i++)
                {
                    _buffer[offset + count - 1 - i] = (byte)(data >> i * 8);
                }
            }
        }

        protected ulong ReadLittleEndian(int offset, int count)
        {
            AssertOffsetAndLength(offset, count);
            ulong r = 0;
            if (BitConverter.IsLittleEndian)
            {
                for (int i = 0; i < count; i++)
                {
                  r |= (ulong)_buffer[offset + i] << i * 8;
                }
            }
            else
            {
              for (int i = 0; i < count; i++)
              {
                r |= (ulong)_buffer[offset + count - 1 - i] << i * 8;
              }
            }
            return r;
        }
#endif // !UNSAFE_BYTEBUFFER


        private void AssertOffsetAndLength(int offset, int length)
        {
            #if !BYTEBUFFER_NO_BOUNDS_CHECK
            if (offset < 0 ||
                offset > _buffer.Length - length)
                throw new ArgumentOutOfRangeException();
            #endif
        }

        internal void PutSbyte(int offset, sbyte value)
        {
            AssertOffsetAndLength(offset, sizeof(sbyte));
            _buffer[offset] = (byte)value;
        }

        internal void PutByte(int offset, byte value)
        {
            AssertOffsetAndLength(offset, sizeof(byte));
            _buffer[offset] = value;
        }

        internal void PutByte(int offset, byte value, int count)
        {
            AssertOffsetAndLength(offset, sizeof(byte) * count);
            for (var i = 0; i < count; ++i)
                _buffer[offset + i] = value;
        }

        // this method exists in order to conform with Java ByteBuffer standards
        internal void Put(int offset, byte value)
        {
            PutByte(offset, value);
        }

#if UNSAFE_BYTEBUFFER
        // Unsafe but more efficient versions of Put*.
        internal void PutShort(int offset, short value)
        {
            PutUshort(offset, (ushort)value);
        }

        internal unsafe void PutUshort(int offset, ushort value)
        {
            AssertOffsetAndLength(offset, sizeof(ushort));
            fixed (byte* ptr = _buffer)
            {
                *(ushort*)(ptr + offset) = BitConverter.IsLittleEndian
                    ? value
                    : ReverseBytes(value);
            }
        }

        internal void PutInt(int offset, int value)
        {
            PutUint(offset, (uint)value);
        }

        internal unsafe void PutUint(int offset, uint value)
        {
            AssertOffsetAndLength(offset, sizeof(uint));
            fixed (byte* ptr = _buffer)
            {
                *(uint*)(ptr + offset) = BitConverter.IsLittleEndian
                    ? value
                    : ReverseBytes(value);
            }
        }

        internal unsafe void PutLong(int offset, long value)
        {
            PutUlong(offset, (ulong)value);
        }

        internal unsafe void PutUlong(int offset, ulong value)
        {
            AssertOffsetAndLength(offset, sizeof(ulong));
            fixed (byte* ptr = _buffer)
            {
                *(ulong*)(ptr + offset) = BitConverter.IsLittleEndian
                    ? value
                    : ReverseBytes(value);
            }
        }

        internal unsafe void PutFloat(int offset, float value)
        {
            AssertOffsetAndLength(offset, sizeof(float));
            fixed (byte* ptr = _buffer)
            {
                if (BitConverter.IsLittleEndian)
                {
                    *(float*)(ptr + offset) = value;
                }
                else
                {
                    *(uint*)(ptr + offset) = ReverseBytes(*(uint*)(&value));
                }
            }
        }

        internal unsafe void PutDouble(int offset, double value)
        {
            AssertOffsetAndLength(offset, sizeof(double));
            fixed (byte* ptr = _buffer)
            {
                if (BitConverter.IsLittleEndian)
                {
                    *(double*)(ptr + offset) = value;

                }
                else
                {
                    *(ulong*)(ptr + offset) = ReverseBytes(*(ulong*)(ptr + offset));
                }
            }
        }
#else // !UNSAFE_BYTEBUFFER
        // Slower versions of Put* for when unsafe code is not allowed.
        internal void PutShort(int offset, short value)
        {
            AssertOffsetAndLength(offset, sizeof(short));
            WriteLittleEndian(offset, sizeof(short), (ulong)value);
        }

        internal void PutUshort(int offset, ushort value)
        {
            AssertOffsetAndLength(offset, sizeof(ushort));
            WriteLittleEndian(offset, sizeof(ushort), (ulong)value);
        }

        internal void PutInt(int offset, int value)
        {
            AssertOffsetAndLength(offset, sizeof(int));
            WriteLittleEndian(offset, sizeof(int), (ulong)value);
        }

        internal void PutUint(int offset, uint value)
        {
            AssertOffsetAndLength(offset, sizeof(uint));
            WriteLittleEndian(offset, sizeof(uint), (ulong)value);
        }

        internal void PutLong(int offset, long value)
        {
            AssertOffsetAndLength(offset, sizeof(long));
            WriteLittleEndian(offset, sizeof(long), (ulong)value);
        }

        internal void PutUlong(int offset, ulong value)
        {
            AssertOffsetAndLength(offset, sizeof(ulong));
            WriteLittleEndian(offset, sizeof(ulong), value);
        }

        internal void PutFloat(int offset, float value)
        {
            AssertOffsetAndLength(offset, sizeof(float));
            floathelper[0] = value;
            Buffer.BlockCopy(floathelper, 0, inthelper, 0, sizeof(float));
            WriteLittleEndian(offset, sizeof(float), (ulong)inthelper[0]);
        }

        internal void PutDouble(int offset, double value)
        {
            AssertOffsetAndLength(offset, sizeof(double));
            doublehelper[0] = value;
            Buffer.BlockCopy(doublehelper, 0, ulonghelper, 0, sizeof(double));
            WriteLittleEndian(offset, sizeof(double), ulonghelper[0]);
        }

#endif // UNSAFE_BYTEBUFFER

        internal sbyte GetSbyte(int index)
        {
            AssertOffsetAndLength(index, sizeof(sbyte));
            return (sbyte)_buffer[index];
        }

        internal byte Get(int index)
        {
            AssertOffsetAndLength(index, sizeof(byte));
            return _buffer[index];
        }

#if UNSAFE_BYTEBUFFER
        // Unsafe but more efficient versions of Get*.
        internal short GetShort(int offset)
        {
            return (short)GetUshort(offset);
        }

        internal unsafe ushort GetUshort(int offset)
        {
            AssertOffsetAndLength(offset, sizeof(ushort));
            fixed (byte* ptr = _buffer)
            {
                return BitConverter.IsLittleEndian
                    ? *(ushort*)(ptr + offset)
                    : ReverseBytes(*(ushort*)(ptr + offset));
            }
        }

        internal int GetInt(int offset)
        {
            return (int)GetUint(offset);
        }

        internal unsafe uint GetUint(int offset)
        {
            AssertOffsetAndLength(offset, sizeof(uint));
            fixed (byte* ptr = _buffer)
            {
                return BitConverter.IsLittleEndian
                    ? *(uint*)(ptr + offset)
                    : ReverseBytes(*(uint*)(ptr + offset));
            }
        }

        internal long GetLong(int offset)
        {
            return (long)GetUlong(offset);
        }

        internal unsafe ulong GetUlong(int offset)
        {
            AssertOffsetAndLength(offset, sizeof(ulong));
            fixed (byte* ptr = _buffer)
            {
                return BitConverter.IsLittleEndian
                    ? *(ulong*)(ptr + offset)
                    : ReverseBytes(*(ulong*)(ptr + offset));
            }
        }

        internal unsafe float GetFloat(int offset)
        {
            AssertOffsetAndLength(offset, sizeof(float));
            fixed (byte* ptr = _buffer)
            {
                if (BitConverter.IsLittleEndian)
                {
                    return *(float*)(ptr + offset);
                }
                else
                {
                    uint uvalue = ReverseBytes(*(uint*)(ptr + offset));
                    return *(float*)(&uvalue);
                }
            }
        }

        internal unsafe double GetDouble(int offset)
        {
            AssertOffsetAndLength(offset, sizeof(double));
            fixed (byte* ptr = _buffer)
            {
                if (BitConverter.IsLittleEndian)
                {
                    return *(double*)(ptr + offset);
                }
                else
                {
                    ulong uvalue = ReverseBytes(*(ulong*)(ptr + offset));
                    return *(double*)(&uvalue);
                }
            }
        }
#else // !UNSAFE_BYTEBUFFER
        // Slower versions of Get* for when unsafe code is not allowed.
        internal short GetShort(int index)
        {
            return (short)ReadLittleEndian(index, sizeof(short));
        }

        internal ushort GetUshort(int index)
        {
            return (ushort)ReadLittleEndian(index, sizeof(ushort));
        }

        internal int GetInt(int index)
        {
            return (int)ReadLittleEndian(index, sizeof(int));
        }

        internal uint GetUint(int index)
        {
            return (uint)ReadLittleEndian(index, sizeof(uint));
        }

        internal long GetLong(int index)
        {
           return (long)ReadLittleEndian(index, sizeof(long));
        }

        internal ulong GetUlong(int index)
        {
            return ReadLittleEndian(index, sizeof(ulong));
        }

        internal float GetFloat(int index)
        {
            int i = (int)ReadLittleEndian(index, sizeof(float));
            inthelper[0] = i;
            Buffer.BlockCopy(inthelper, 0, floathelper, 0, sizeof(float));
            return floathelper[0];
        }

        internal double GetDouble(int index)
        {
            ulong i = ReadLittleEndian(index, sizeof(double));
            // There's Int64BitsToDouble but it uses unsafe code internally.
            ulonghelper[0] = i;
            Buffer.BlockCopy(ulonghelper, 0, doublehelper, 0, sizeof(double));
            return doublehelper[0];
        }
#endif // UNSAFE_BYTEBUFFER
    }
}
