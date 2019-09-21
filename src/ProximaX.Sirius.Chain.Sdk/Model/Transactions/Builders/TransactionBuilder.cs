using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Fees;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions.Builders
{
    public abstract class TransactionBuilder<B, T> where B : TransactionBuilder<B, T> where T : Transaction
    {
        public EntityType EntityType { get; private set; }

        public int Version { get; private set; }
        public ulong Duration { get; private set; }

        public NetworkType NetworkType { get; private set; }

        public Deadline Deadline { get; private set; }
        public ulong? MaxFee { get; private set; }
        public string Signature { get; private set; }
        public PublicAccount Signer { get; private set; }
        public TransactionInfo TransactionInfo { get; private set; }
        public FeeCalculationStrategyType FeeCalculationStrategyType { get; private set; }

        protected abstract B Self();

        public abstract T Build();

        public TransactionBuilder(EntityType entityType, int version)
        {
            EntityType = entityType;
            Version = version;
            FeeCalculationStrategyType = FeeCalculationStrategyType.LOW;
        }


        protected ulong GetMaxFeeCalculation(int transactionSize)
        {
            var fcs = new FeeCalculationStrategy(FeeCalculationStrategyType);
            return fcs.CalculateFee(Transaction.HEADER_SIZE + transactionSize);
        }

        public B SetEntityType(EntityType entityType)
        {
            EntityType = entityType;
            return Self();
        }

        public B SetVersion(int version)
        {
            Version = version;
            return Self();
        }

        public B SetNetworkType(NetworkType networkType)
        {
            NetworkType = networkType;
            return Self();
        }

        public B SetDeadline(Deadline deadline)
        {
            Deadline = deadline;
            return Self();
        }

        public B SetDuration(ulong duration)
        {
            Duration = duration;
            return Self();
        }

        public B SetMaxFee(ulong maxFee)
        {
            MaxFee = maxFee;
            return Self();
        }

        public B SetSignature(string signature)
        {
            Signature = signature;
            return Self();
        }

        public B SetSigner(PublicAccount signer)
        {
            Signer = signer;
            return Self();
        }

        public B SetTransactionInfo(TransactionInfo transactionInfo)
        {
            TransactionInfo = transactionInfo;
            return Self();
        }

        public B SetFeeCalculationStrategy(FeeCalculationStrategyType feeCalculationStrategyType)
        {
            FeeCalculationStrategyType = feeCalculationStrategyType;
            return Self();
        }

    
    }
}
