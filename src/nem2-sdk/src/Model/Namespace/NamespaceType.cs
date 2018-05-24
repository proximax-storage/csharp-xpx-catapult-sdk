using System;
using System.ComponentModel;

namespace io.nem2.sdk.Model.Namespace
{
    public static class NamespaceTypes
    {
        /// <summary>
        /// Enum Types
        /// </summary>
        public enum Types
        {
            /// <summary>
            /// The transfer type
            /// </summary>
            SubNamespace = 0x01,

            /// <summary>
            /// The namespace creation type
            /// </summary>
            RootNamespace = 0x00,          
        }

        /// <summary>
        /// Gets the value of the type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The int16 value of the type.</returns>
        /// <exception cref="InvalidEnumArgumentException">type</exception>
        public static byte GetValue(this Types type)
        {
            if (!Enum.IsDefined(typeof(Types), type))
                throw new InvalidEnumArgumentException(nameof(type), (int)type, typeof(Types));

            return (byte)type;
        }

        /// <summary>
        /// Gets the type for the given value.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>The Type associated with the given int16 value.</returns>
        /// <exception cref="InvalidEnumArgumentException">type</exception>
        public static Types GetRawValue(byte type)
        {
            switch (type)
            {
                case 0x01:
                    return Types.SubNamespace;
                case 0x00:
                    return Types.RootNamespace;
                default:
                    throw new ArgumentException("invalid transaction type.");
            }
        }
    }
}
