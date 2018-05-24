namespace io.nem2.sdk.Infrastructure.Buffers.SchemaHelpers
{
    internal class Constants
    {
        private readonly int _value;

        internal Constants(int value)
        {
            _value = value;
        }

        internal int getValue()
        {
            return _value;
        }

        internal enum Value
        {
            SIZEOF_BYTE = 1,

            SIZEOF_SHORT = 2,

            SIZEOF_INT = 4,
       
            SIZEOF_FLOAT = 4,

            SIZEOF_LONG = 8,

            SIZEOF_DOUBLE = 8,

            FILE_IDENTIFIER_LENGTH = 4
        }      
    } 
}
