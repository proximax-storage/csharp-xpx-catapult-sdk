using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using io.nem2.sdk.Core.Crypto.Chaso.NaCl;
using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Infrastructure.Listeners;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Blockchain;
using io.nem2.sdk.Model.Mosaics;
using io.nem2.sdk.Model.Transactions;
using Org.BouncyCastle.Crypto.Digests;

namespace IntegrationTests.Infrastructure.Transactions
{
    [TestClass]
    public class SecretLockTransactionTests
    {
        private readonly String host = "http://" + Config.Domain + ":3000";

        private Listener listener { get; }

        public SecretLockTransactionTests()
        {
            listener = new Listener(Config.Domain);

            listener.Open().Wait();
        }

        [TestMethod, Timeout(20000)]
        public async Task SecretLockTransactionTest()
        {
            var signer = KeyPair.CreateFromPrivateKey(Config.PrivateKeyAggregate1);

            var secretHash = new byte[64];

            var digest = new Sha3Digest(512);
            digest.BlockUpdate("5D8BEBBE80D7EA3B0088E59308D8671099781429".FromHex(), 0, "5D8BEBBE80D7EA3B0088E59308D8671099781429".FromHex().Length);
            digest.DoFinal(secretHash, 0);

            var transaction = SecretLockTransaction.Create(
                    NetworkType.Types.MIJIN_TEST,
                    3,
                    Deadline.CreateHours(2),
                    0,
                    new Mosaic(new MosaicId("nem:xem"), 10000000),
                    10000,  
                    HashType.Types.SHA3_512,
                    secretHash.ToHexLower(), 
                    Address.CreateFromPublicKey("5D8BEBBE80D7EA3B0088E59308D8671099781429B449A0BBCA6D950A709BA068", NetworkType.Types.MIJIN_TEST)
                    )
                .SignWith(signer);

            listener.ConfirmedTransactionsGiven(Address.CreateFromPublicKey(transaction.Signer, NetworkType.Types.MIJIN_TEST))
                .Subscribe(
                    e =>
                    {
                        Console.WriteLine("Success");
                    });

            await new TransactionHttp(host).Announce(transaction);

            var status = await listener.TransactionStatus(Address.CreateFromPublicKey(signer.PublicKeyString, NetworkType.Types.MIJIN_TEST)).Where(e => e.Hash == transaction.Hash).Take(1);

            Assert.AreEqual("Failure_Lock_Hash_Exists", status.Status);
        }
    }
}
