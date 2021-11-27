using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProximaX.Sirius.Chain.Sdk.Model.Lock
{
    public class HashLockInfo
    {
        public HashLockInfo(string account, string accountAddress, ulong mosaicId, ulong amount, ulong height, string status, string hash)
        {
            Account = account;
            AccountAddress = accountAddress;
            MosaicId = mosaicId;
            Amount = amount;
            Height = height;
            Status = status;
            Hash = hash;
        }
        public string Account { get;}
        public string AccountAddress { get; set; }
        public ulong MosaicId { get; set; }
        public ulong Amount { get; set; }
        public ulong Height { get; set; }
        public string Status { get; set; }
        public string Hash { get; set; }

    }
}
