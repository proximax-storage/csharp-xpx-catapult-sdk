using System;

namespace ProximaX.Sirius.Chain.Sdk.Model.Fees
{
    public class FeeCalculationStrategy
    {
        public FeeCalculationStrategy(FeeCalculationStrategyType type = FeeCalculationStrategyType.ZERO)
        {
            FeeCalculationStrategyType = type;
           
        }

        public FeeCalculationStrategyType FeeCalculationStrategyType { get; }
  
        public readonly int MAX_FEE = 5000000;

        public ulong CalculateFee(int transactionSize)
        {
            return (ulong)Math.Min(MAX_FEE, transactionSize * (int)FeeCalculationStrategyType);
        }
    }
}
