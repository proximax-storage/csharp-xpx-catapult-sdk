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
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;

namespace ProximaX.Sirius.Chain.Sdk.Utils
{
    public static class EnumExtensions
    {
        public static T GetEnumValue<T>(string str) where T : struct, IConvertible
        {
            var enumType = typeof(T);
            if (!enumType.IsEnum) throw new Exception("T must be an Enumeration type.");
            return Enum.TryParse(str, true, out T val) ? val : default;
        }

        public static T GetEnumValue<T>(int intValue) where T : struct, IConvertible
        {
            var enumType = typeof(T);
            if (!enumType.IsEnum) throw new Exception("T must be an Enumeration type.");

            return (T) Enum.ToObject(enumType, intValue);
        }

        public static string ToDescription<TEnum>(this TEnum enumValue) where TEnum : struct
        {
            return enumValue
                .GetType()
                .GetMember(enumValue.ToString())
                .FirstOrDefault()
                ?.GetCustomAttribute<DescriptionAttribute>()
                ?.Description;
        }

        public static string Repeat(this string value, int count)
        {
            return new StringBuilder(value.Length * count).Insert(0, value, count).ToString();
        }
    }
}