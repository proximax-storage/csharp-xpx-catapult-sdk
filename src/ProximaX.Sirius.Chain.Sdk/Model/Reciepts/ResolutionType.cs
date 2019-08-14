using ProximaX.Sirius.Chain.Sdk.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProximaX.Sirius.Chain.Sdk.Model.Reciepts
{
    public enum ResolutionType
    {
        Address = 0,
        Mosaic = 1,
    }

    public static class ResolutionTypeExtension
    {
        /// <summary>
        ///     Get raw value extension
        /// </summary>
        /// <param name="value">The resolution type</param>
        /// <returns>ResolutionType</returns>
        public static ResolutionType GetRawValue(int? value)
        {
            return EnumExtensions.GetEnumValue<ResolutionType>(value.Value);
        }

        /// <summary>
        ///     Get value extension
        /// </summary>
        /// <param name="type">The resolution typ</param>
        /// <returns>int</returns>
        public static int GetValue(this ResolutionType type)
        {
            return (int)type;
        }
    }
}
