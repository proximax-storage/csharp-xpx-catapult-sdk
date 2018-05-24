using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using io.nem2.sdk.Core.Crypto.Chaso.NaCl;
using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Infrastructure.Listeners;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Blockchain;
using io.nem2.sdk.Model.Transactions;
using Org.BouncyCastle.Crypto.Digests;

namespace IntegrationTests.Infrastructure.Transactions
{
    [TestClass]
    public class SecretProofTransactionTests
    {
        private readonly String host = "http://" + Config.Domain + ":3000";

        private Listener listener { get; }

        public SecretProofTransactionTests()
        {
            listener = new Listener(Config.Domain);

            listener.Open().Wait();
        }


        [TestMethod, Timeout(20000)]
        public async Task SecretProofTransactionTest()
        {
            var signer = KeyPair.CreateFromPrivateKey(Config.PrivateKeyMain);

            var secretHash = new byte[64];

            var digest = new Sha3Digest(512);
            digest.BlockUpdate("5D8BEBBE80D7EA3B0088E59308D8671099781429".FromHex(),0, "5D8BEBBE80D7EA3B0088E59308D8671099781429".FromHex().Length);
            digest.DoFinal(secretHash, 0);

            var trans = SecretProofTransaction.Create(
                    NetworkType.Types.MIJIN_TEST,
                    Deadline.CreateHours(2),
                    0,
                    HashType.Types.SHA3_512,
                    secretHash.ToHexLower(),
                    "5D8BEBBE80D7EA3B0088E59308D8671099781429")
                .SignWith(signer);

            listener.ConfirmedTransactionsGiven(Address.CreateFromPublicKey(signer.PublicKeyString,
                NetworkType.Types.MIJIN_TEST)).Subscribe(e => Console.WriteLine(e.TransactionInfo.Hash));

            await new TransactionHttp(host).Announce(trans);

            var status = await listener.TransactionStatus(Address.CreateFromPublicKey(signer.PublicKeyString, NetworkType.Types.MIJIN_TEST)).Where(e => e.Hash == trans.Hash).Take(1);

            Assert.AreEqual("Failure_Lock_Secret_Already_Used", status.Status);
        }
    }
}
