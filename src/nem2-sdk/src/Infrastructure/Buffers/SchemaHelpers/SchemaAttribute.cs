using io.nem2.sdk.Core.Utils;

namespace io.nem2.sdk.Infrastructure.Buffers.SchemaHelpers
{
    internal abstract class SchemaAttribute
    {
        private string Name { get; }

        protected SchemaAttribute(string name)
        {
            Name = name;
        }

        internal abstract byte[] Serialize(byte[] buffer, int position, int innerObjectPosition);

        internal byte[] Serialize(byte[] buffer, int position)
        {
            return Serialize(buffer, position, buffer[0]);
        }

        internal string GetName()
        {
            return Name;
        }

        protected byte[] FindParam(int innerObjectPosition, int position, byte[] buffer, Constants.Value Typesize)
        {
            var offset = __offset(innerObjectPosition, position, buffer);
            
            return offset == 0 ? new byte[] { 0 } : buffer.Take(offset + innerObjectPosition, (int)Typesize);
        }

        protected byte[] FindVector(int innerObjectPosition, int position, byte[] buffer, Constants.Value Typesize)
        {          
            var offset = __offset(innerObjectPosition, position, buffer);
            var offsetLong = offset + innerObjectPosition;
            var vecStart = __vector(offsetLong, buffer);
            var vecLength = __vector_length(offsetLong, buffer) * (int)Typesize;
            return offset == 0 ? new byte[] { 0 } : buffer.Take(vecStart, vecLength);
        }

        protected int FindObjectStartPosition(int innerObjectPosition, int position, byte[] buffer)
        {
            var offset = __offset(innerObjectPosition, position, buffer);
            return __indirect(offset + innerObjectPosition, buffer);
        }

        protected int FindArrayLength(int innerObjectPosition, int position, byte[] buffer)
        {
            var offset = __offset(innerObjectPosition, position, buffer);
            return offset == 0 ? 0 : __vector_length(innerObjectPosition + offset, buffer);
        }

        protected int FindObjectArrayElementStartPosition(int innerObjectPosition, int position, byte[] buffer, int startPosition)
        {
            var offset = __offset(innerObjectPosition, position, buffer);
            var vector = __vector(innerObjectPosition + offset, buffer);
            return __indirect(vector + (startPosition * 4), buffer);
        }

        protected int __offset(int innerObjectPosition, int position, byte[] buffer)
        {
            var vtable = innerObjectPosition - ReadInt32(innerObjectPosition, buffer);
            return position < ReadInt16(vtable, buffer) ? ReadInt16(vtable + position, buffer) : 0;
        }

        protected int ReadInt32(int offset, byte[] buffer)
        {
            return buffer[offset] | buffer[offset + 1] << 8 | buffer[offset + 2] << 16 | buffer[offset + 3] << 24;
        }

        protected int  ReadInt16(int offset, byte[] bytes)
        {
              return bytes[offset] | (bytes[offset + 1] << 8);
         }
        
        protected int __vector_length(int offset, byte[] buffer)
        {
            return ReadInt32(offset + ReadInt32(offset, buffer), buffer);
        }

        protected int __indirect(int offset, byte[] buffer)
        {
            return offset + ReadInt32(offset, buffer);
        }

        protected int __vector(int offset, byte[] buffer)
        {
            return offset + ReadInt32(offset, buffer) + 4;
        }
    }
}
