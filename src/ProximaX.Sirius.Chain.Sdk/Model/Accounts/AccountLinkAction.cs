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
using System;


namespace ProximaX.Sirius.Chain.Sdk.Model.Accounts
{
    public enum AccountLinkAction
    {
        LINK = 0,
        UNLINK = 1
    }


    public static class AccountLinkActionExtension
    {
        public static AccountLinkAction GetRawValue(int? value)
        {
            return value.HasValue
                ? EnumExtensions.GetEnumValue<AccountLinkAction>(value.Value)
                : throw new Exception("Unsupported AccountLinkAction");
        }

        /// <summary>
        ///     Get value extension
        /// </summary>
        /// <param name="type">The alias action type</param>
        /// <returns>AliasActionType</returns>
        public static byte GetValueInByte(this AccountLinkAction type)
        {
            return (byte)type;
        }
    }
}
