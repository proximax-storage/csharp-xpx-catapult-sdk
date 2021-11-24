// Copyright 2021 ProximaX
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
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;
using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Model.Exchange
{
    public class AccountExchange
    {
        public AccountExchange(string owner, string ownerAddress, int version, List<OfferInfo> buyOffers, List<OfferInfo> sellOffers)
        {
            Owner = owner;
            OwnerAddress = ownerAddress;
            Version = version;
            BuyOffers = buyOffers;
            SellOffers = sellOffers;
        }

        /// <summary>
        /// Get and set Owner
        /// </summary>
        public string Owner { get; }

        /// <summary>
        /// Get and set OwnerAddress
        /// </summary>
        public string OwnerAddress { get; }

        /// <summary>
        /// Get and set Version
        /// </summary>
        public int Version { get; }

        /// <summary>
        /// Get and set BuyOffers
        /// </summary>
        public List<OfferInfo> BuyOffers { get; }

        /// <summary>
        /// Get and set SellOffers
        /// </summary>
        public List<OfferInfo> SellOffers { get; }

        public static AccountExchange FromDTO(JObject input)
        {
            var buyOffer = new List<OfferInfo>();
            var sellOffer = new List<OfferInfo>();

            var accountexchange = input["exchange"] as JObject;
            var owner = accountexchange["owner"].ToObject<string>();
            var ownerAddress = accountexchange["ownerAddress"].ToObject<string>();
            var version = accountexchange["version"].ToObject<int>();
            if (accountexchange["buyOffers"] != null)
            {
                for (var i = 0; i < accountexchange["buyOffers"].ToList().Count; i++)
                {
                    var trx = accountexchange["buyOffers"].ToList()[i] as JObject;
                    var MosaicId = trx["mosaicId"].ToObject<UInt64DTO>().ToUInt64();
                    var Amount = trx["amount"].ToObject<UInt64DTO>().ToUInt64();
                    var InitialAmount = trx["initialAmount"].ToObject<UInt64DTO>().ToUInt64();
                    var InitialCost = trx["initialCost"].ToObject<UInt64DTO>().ToUInt64();
                    var Deadline = trx["deadline"].ToObject<UInt64DTO>().ToUInt64();
                    var Price = trx["price"].ToObject<int>();

                    buyOffer.Add(new OfferInfo(MosaicId, Amount, InitialAmount, InitialCost, Deadline, Price));
                }
            }

            if (accountexchange["sellOffers"] != null)
            {
                for (var i = 0; i < accountexchange["sellOffers"].ToList().Count; i++)
                {
                    var trx = accountexchange["sellOffers"].ToList()[i] as JObject;
                    var MosaicId = trx["mosaicId"].ToObject<UInt64DTO>().ToUInt64();
                    var Amount = trx["amount"].ToObject<UInt64DTO>().ToUInt64();
                    var InitialAmount = trx["initialAmount"].ToObject<UInt64DTO>().ToUInt64();
                    var InitialCost = trx["initialCost"].ToObject<UInt64DTO>().ToUInt64();
                    var Deadline = trx["deadline"].ToObject<UInt64DTO>().ToUInt64();
                    var Price = trx["price"].ToObject<int>();

                    sellOffer.Add(new OfferInfo(MosaicId, Amount, InitialAmount, InitialCost, Deadline, Price));
                }
            }

            return new AccountExchange(owner, ownerAddress, version, buyOffer, sellOffer);
        }
    }
}