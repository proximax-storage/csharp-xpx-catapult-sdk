namespace io.nem2.sdk.Model.Blockchain
{
    public class BlockchainStorageInfo
    {
        /// <summary>
        /// Returns number of accounts published in the blockchain
        /// </summary>
        /// <value>The number accounts.</value>
        public int NumAccounts { get; }

        /// <summary>
        /// Returns number of confirmed blocks.
        /// </summary>
        /// <value>The number blocks.</value>
        public int NumBlocks { get; }

        /// <summary>
        /// Returns number of confirmed transactions.
        /// </summary>
        /// <value>The number transactions.</value>
        public int NumTransactions { get; }

        public BlockchainStorageInfo(int numAccounts, int numBlocks, int numTransactions)
        {
            NumAccounts = numAccounts;
            NumBlocks = numBlocks;
            NumTransactions = numTransactions;
        }
    }
}
