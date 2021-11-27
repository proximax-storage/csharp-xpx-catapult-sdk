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
    ///     The mosaic name info structure describes basic information of a mosaic and name.
    /// </summary>
    public class MosaicModifyLevy
    {
        private static int MosaicLevyDecimalPlace = 100000;

        /// <summary>
        ///     Initializes a new instance of the <see cref="Mosaic" /> class.
        /// </summary>
        /// <param name="id">The mosaic identifier</param>
        /// <param name="amount">The mosaic account</param>
        public MosaicModifyLevy(MosaicLevyType levytype, Address recipent, IUInt64Id mosaicid, ulong fee)
        {
            Levytype = levytype;
            Recipent = recipent;
            Mosaicid = mosaicid;
            Fee = fee;
        }

        public MosaicLevyType Levytype { get; private set; }
        public Address Recipent { get; private set; }
        public IUInt64Id Mosaicid { get; private set; }
        public ulong Fee { get; private set; }

        public float CreateMosaicLevyFeePercentile(float percent)
        {
            return (percent * MosaicLevyDecimalPlace);
        }

        public override string ToString()
        {
            return $"{nameof(Levytype)}: {Levytype}, {nameof(Recipent)}: {Recipent}, {nameof(Mosaicid)}: {Mosaicid}, {nameof(Fee)}: {Fee}";
        }
    }
}