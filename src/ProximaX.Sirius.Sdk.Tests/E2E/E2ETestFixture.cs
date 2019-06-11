using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ProximaX.Sirius.Sdk.Client;
using ProximaX.Sirius.Sdk.Infrastructure;
using ProximaX.Sirius.Sdk.Infrastructure.Listener;
using ProximaX.Sirius.Sdk.Model.Accounts;
using ProximaX.Sirius.Sdk.Model.Mosaics;
using ProximaX.Sirius.Sdk.Model.Transactions;
using ProximaX.Sirius.Sdk.Model.Transactions.Messages;
using ProximaX.Sirius.Sdk.Tests.Utils;
using ProximaX.Sirius.Sdk.Utils;
using Xunit;

namespace ProximaX.Sirius.Sdk.Tests.E2E
{
    public class E2ETestFixture : IDisposable
    {
        public AccountHttp AccountHttp { get; set; }

        public TransactionHttp TransactionHttp { get; set; }

        public Listener Listener { get; set; }

        public NetworkHttp NetworkHttp { get; set; }

        public MosaicHttp MosaicHttp { get; set; }

        public NamespaceHttp NamespaceHttp { get; set; }

        public MetadataHttp MetadataHttp { get; set; }

        public Account SeedAccount { get; set; }

        public TimeSpan DefaultTimeout { get; set; }

        private TestEnvironment Environment { get; set; }


        public E2ETestFixture()
        {
            Task.Run(InitializeFixture);
        }

        private async Task InitializeFixture()
        {
            // Setup test environment
            Environment = GetEnvironment();

            // Initiate other services
   
            Listener = new Listener(Environment.Host, Environment.Port);
            var client = new SiriusClient(Environment.BaseUrl);
            NetworkHttp = client.NetworkHttp;
            AccountHttp = client.AccountHttp;
            TransactionHttp = client.TransactionHttp;
            NamespaceHttp = client.NamespaceHttp;
            MosaicHttp = client.MosaicHttp;
            MetadataHttp = client.MetadataHttp;
            /*
            NetworkHttp = new NetworkHttp(Environment.BaseUrl);
            AccountHttp = new AccountHttp(Environment.BaseUrl, NetworkHttp);
            TransactionHttp = new TransactionHttp(Environment.BaseUrl, NetworkHttp);
            NamespaceHttp = new NamespaceHttp(Environment.BaseUrl, NetworkHttp);
            MosaicHttp = new MosaicHttp(Environment.BaseUrl, NetworkHttp);
            MetadataHttp = new MetadataHttp(Environment.BaseUrl, NetworkHttp);
            */
            SeedAccount = await GetSeedAccount();
            //BobAccount = await GenerateAccountAndSendSomeMoney();
            //AliceAccount = await GenerateAccountAndSendSomeMoney();

            //set default timeout
            DefaultTimeout = TimeSpan.FromSeconds(100);
        }


        public void Dispose()
        {
            //Do clean up
            // Listener.Close();
        }


        private TestEnvironment GetEnvironment()
        {
            var environment = TestHelper.GetConfig()[$"ActiveEnvironment"] ?? "DEV";

            string env;
            switch (environment.ToUpper())
            {
                case "DEV":
                    env = "Dev";
                    break;
                case "BCSTAGE":
                    env = "BcStage";
                    break;
                case "BCTESTNET":
                    env = "BcTestNet";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Environment), environment, null);
            }

            var protocol = TestHelper.GetConfig()[$"Environments:{env}:Protocol"];
            var host = TestHelper.GetConfig()[$"Environments:{env}:Host"];
            var port = TestHelper.GetConfig()[$"Environments:{env}:Port"];

            return new TestEnvironment(host, protocol, Convert.ToInt32(port));

        }

        public async Task<Account> GetSeedAccount()
        {
            var privateKey = TestHelper.GetConfig()[$"Environments:{Environment.EnvironmentSelection.ToDescription()}:SeedAccountPrivateKey"];

            var networkType = await NetworkHttp.GetNetworkType();

            return Account.CreateFromPrivateKey(privateKey, networkType);

        }

        public async Task<Account> GenerateAccountAndSendSomeMoney(int amount)
        {
            var networkType = NetworkHttp.GetNetworkType().Wait();
            var account = Account.GenerateNewAccount(networkType);

            var money = (ulong)amount;

            var mosaicToTransfer = NetworkCurrencyMosaic.CreateRelative(money);

            var transferTransaction = TransferTransaction.Create(
                Deadline.Create(),
                account.Address,
                new List<Mosaic>()
                {
                    mosaicToTransfer
                },
                PlainMessage.Create("transferTest"),
                networkType);

            var signedTransaction = SeedAccount.Sign(transferTransaction);

            await Listener.Open();

            var tx = Listener.ConfirmedTransactionsGiven(account.Address).Take(1).Timeout(TimeSpan.FromSeconds(100));

            await TransactionHttp.Announce(signedTransaction);

            var result = await tx;

            if (result.IsConfirmed())
                return account;
            else
                throw new Exception($"Unable to send money to account {account.Address.Plain}");
        }

        public void WatchForFailure(SignedTransaction transaction)
        {
            Listener.TransactionStatus(Address.CreateFromPublicKey(transaction.Signer, NetworkHttp.GetNetworkType().Wait()))
                .Subscribe(
                    e =>
                    {
                        Console.WriteLine(e.Status);

                    });
        }
    }

    [CollectionDefinition("E2ETestFixtureCollection")]
    public class E2ETestFixtureCollection : ICollectionFixture<E2ETestFixture>
    {
        // This class has no code, and is never created. Its purpose is simply
        // to be the place to apply [CollectionDefinition] and all the
        // ICollectionFixture<> interfaces.
    }
}
