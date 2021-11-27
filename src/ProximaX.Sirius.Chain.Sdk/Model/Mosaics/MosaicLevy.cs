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

using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Namespaces;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Model.Mosaics
{
    /// <summary>
    ///     Class MosaicLevy
    /// </summary>
    public class MosaicLevy
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="levytype"></param>
        /// <param name="recipent"></param>
        /// <param name="mosaicid"></param>
        /// <param name="amount"></param>
        public MosaicLevy(MosaicLevyType levytype, Recipient recipent, MosaicId mosaicid, ulong amount)
        {
            Levytype = levytype;
            Recipent = recipent;
            Mosaic = mosaicid;
            Amount = amount;
        }

        /// <summary>
        ///     Levytype
        /// </summary>
        public MosaicLevyType Levytype { get; }

        /// <summary>
        ///     Recipent
        /// </summary>
        public Recipient Recipent { get; }

        /// <summary>
        ///     Mosaic
        /// </summary>
        public MosaicId Mosaic { get; }

        /// <summary>
        ///     Amount
        /// </summary>
        public ulong Amount { get; }

        /// <summary>
        ///     Creates from Percentage Fee
        /// </summary>
        /// <param name="recipent">The recipent's address.</param>
        /// <param name="mosaicid">The mosaic id.</param>
        /// <param name="percent">The precent.</param>
        /// <param name="amount">The amount.</param>
        /// <returns>MosaicLevy</returns>
        public static MosaicLevy CreateWithPercentageFee(Recipient recipent, MosaicId mosaicid, float percent, ulong? amount = null)
        {
            return new MosaicLevy(MosaicLevyType.LevyPercentileFee, recipent, mosaicid, CreateLevyFeePercentile(percent, amount));
        }

        /// <summary>
        ///     Creates from Absolute Fee
        /// </summary>
        /// <param name="recipent">The recipent's address.</param>
        /// <param name="mosaicid">The mosaic id.</param>
        /// <param name="amount">The amount.</param>
        /// <returns>MosaicLevy</returns>
        public static MosaicLevy CreateWithAbsoluteFee(Recipient recipent, MosaicId mosaicid, ulong amount)
        {
            return new MosaicLevy(MosaicLevyType.LevyAbsoluteFee, recipent, mosaicid, amount);
        }

        /// <summary>
        ///     Creates Levy Fee Percentile
        /// </summary>
        /// <param name="percent">The recipent's address.</param>
        /// <param name="amount">The amount.</param>
        /// <returns>Percentile Fee</returns>
        private static ulong CreateLevyFeePercentile(float percent, ulong? amount)
        {
            int MosaicLevyDecimalPlace = 100000;
            if (amount != null)
            {
                return ((ulong)(percent * amount));
            }
            else
            {
                return ((ulong)(percent * MosaicLevyDecimalPlace));
            }
        }

        /// <summary>
        ///     ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{nameof(Levytype)}: {Levytype}, {nameof(Recipent)}: {Recipent}, {nameof(Mosaic)}: {Mosaic}, {nameof(Amount)}: {Amount}";
        }
    }
}