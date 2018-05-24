using System;
using io.nem2.sdk.Model.Accounts;

namespace io.nem2.sdk.Model.Blockchain
{
    public class BlockInfo
    {
        public string Hash { get; }
        public string GenerationHash { get; }
        public ulong TotalFee { get; }
        public int? NumTransactions { get; }
        public string Signature { get; }
        public PublicAccount Signer { get; }
        public NetworkType.Types NetworkType { get; }
        public int Version { get; }
        public int Type { get; }
        public ulong Height { get; }
        public ulong Timestamp { get; }
        public ulong Difficulty { get; }
        public string PreviousBlockHash { get; }
        public string BlockTransactionsHash { get; }


        public BlockInfo(string hash, string generationHash, ulong totalFee,
            int numTransactions, string signature, PublicAccount signer, NetworkType.Types networkType,
            int version, int type, ulong height, ulong timestamp, ulong difficulty,
            String previousBlockHash, String blockTransactionsHash)
        {
            Hash = hash;
            GenerationHash = generationHash;
            TotalFee = totalFee;
            NumTransactions = numTransactions;
            Signature = signature;
            Signer = signer;
            NetworkType = networkType;
            Version = version;
            Type = type;
            Height = height;
            Timestamp = timestamp;
            Difficulty = difficulty;
            PreviousBlockHash = previousBlockHash;
            BlockTransactionsHash = blockTransactionsHash;
        }

        public override String ToString()
        {
            return "BlockInfo{" +
                   "hash='" + Hash + '\'' +
                   ", generationHash='" + GenerationHash + '\'' +
                   ", totalFee=" + TotalFee +
                   ", numTransactions=" + NumTransactions +
                   ", signature='" + Signature + '\'' +
                   ", signer=" + Signer +
                   ", networkType=" + NetworkType +
                   ", version=" + Version +
                   ", type=" + Type +
                   ", height=" + Height +
                   ", timestamp=" + Timestamp +
                   ", difficulty=" + Difficulty +
                   ", previousBlockHash='" + PreviousBlockHash + '\'' +
                   ", blockTransactionsHash='" + BlockTransactionsHash + '\'' +
                   '}';
        }
    }
}
