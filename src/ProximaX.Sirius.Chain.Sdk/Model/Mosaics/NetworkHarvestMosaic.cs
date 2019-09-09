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
using ProximaX.Sirius.Chain.Sdk.Model.Namespaces;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;

namespace ProximaX.Sirius.Chain.Sdk.Model.Mosaics
{
    /// <summary>
    ///     NetworkHarvestMosaic
    /// </summary>
    public class NetworkHarvestMosaic : Mosaic
    {
        /// <summary>
        ///     NetworkHarvestMosaicName
        /// </summary>
        private const string NetworkHarvestMosaicName = "cat.harvest";

        /// <summary>
        ///     The network harvest mosaic
        /// </summary>
        public new static NamespaceId Id = new NamespaceId(NetworkHarvestMosaicName);

        /// <summary>
        ///     The network harvest mosaic divisibility
        /// </summary>
        public static int Divisibility = 3;

        /// <summary>
        ///     The network harvest mosaic initial supply
        /// </summary>
        public static ulong InitialSupply = 15000000L;

        /// <summary>
        ///     Allow transferable
        /// </summary>
        public static bool Transferable = true;

        /// <summary>
        ///     Allow supply mutable
        /// </summary>
        public static bool SupplyMutable = true;

        /// <summary>
        ///     Allow levy mutable
        /// </summary>
        public static bool LevyMutable = false;

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="amount"></param>
        public NetworkHarvestMosaic(ulong amount) : base(Id, amount)
        {
        }

        /// <summary>
        ///     CreateRelative
        /// </summary>
        /// <param name="amount">The amount</param>
        /// <returns>NetworkCurrencyMosaic</returns>
        public static NetworkCurrencyMosaic CreateRelative(ulong amount)
        {
            var relativeAmount = Convert.ToUInt64(Math.Pow(10, Divisibility)) * amount;

            return new NetworkCurrencyMosaic(relativeAmount);
        }

        /// <summary>
        ///     CreateAbsolute
        /// </summary>
        /// <param name="amount">The amount</param>
        /// <returns>NetworkCurrencyMosaic</returns>
        public static NetworkCurrencyMosaic CreateAbsolute(ulong amount)
        {
            return new NetworkCurrencyMosaic(amount);
        }
    }
}