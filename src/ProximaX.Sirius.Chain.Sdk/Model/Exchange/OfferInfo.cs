using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProximaX.Sirius.Chain.Sdk.Model.Exchange
{
    /// <summary>
    /// Class of OfferInfo
    /// </summary>
    public class OfferInfo
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="mosaicId"></param>
        /// <param name="amount"></param>
        /// <param name="initialAmount"></param>
        /// <param name="initialCost"></param>
        /// <param name="deadline"></param>
        /// <param name="price"></param>
        public OfferInfo(ulong mosaicId, ulong amount, ulong initialAmount, ulong initialCost, ulong deadline, int price)
        {
            MosaicId = mosaicId;
            Amount = amount;
            InitialAmount = initialAmount;
            InitialCost = initialCost;
            Deadline = deadline;
            Price = price;
        }

        /// <summary>
        /// Get and set MosaicId
        /// </summary>
        public ulong MosaicId { get; private set; }

        /// <summary>
        /// Get and set Amount
        /// </summary>
        public ulong Amount { get; private set; }

        /// <summary>
        /// Get and set InitialAmount
        /// </summary>
        public ulong InitialAmount { get; private set; }

        /// <summary>
        /// Get and set InitialCost
        /// </summary>
        public ulong InitialCost { get; private set; }

        /// <summary>
        /// Get and set Deadline
        /// </summary>
        public ulong Deadline { get; private set; }

        /// <summary>
        /// Get and set Price
        /// </summary>
        public int Price { get; private set; }

        /// <summary>
        ///     ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return
                $"{nameof(MosaicId)}: {MosaicId}, {nameof(Amount)}: {Amount}, {nameof(InitialAmount)}: {InitialAmount}, {nameof(InitialCost)}: {InitialCost},{nameof(Deadline)}: {Deadline},{nameof(Price)}: {Price}";
        }
    }
}