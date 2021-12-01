using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProximaX.Sirius.Chain.Sdk.Model.Lock
{
    public class SecretLockInfo
    {
        public SecretLockInfo(string account, string accountAddress, ulong mosaicId, ulong amount, ulong height, string status, HashType hashAlgorithm, string secret, string recipient, string compositeHash)
        {
            Account = account;
            AccountAddress = accountAddress;
            MosaicId = mosaicId;
            Amount = amount;
            Height = height;
            Status = status;
            HashAlgorithm = hashAlgorithm;
            Secret = secret;
            Recipient = recipient;
            CompositeHash = compositeHash;
        }
        public string Account { get;}

        public string AccountAddress { get;}

        public ulong MosaicId { get; }

        public ulong Amount { get;}

        public ulong Height { get;}

        public string Status { get; }

        public HashType HashAlgorithm { get; }
        public string Secret { get; }

        public string Recipient { get; }

        public string CompositeHash { get; }
    }
}
