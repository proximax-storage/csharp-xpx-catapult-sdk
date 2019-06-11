﻿using System;
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
  
        public Account SeedAccount { get; set; }

        public TimeSpan DefaultTimeout { get; set; }

        private TestEnvironment Environment { get; set; }

        public SiriusClient Client { get; set; }

        public SiriusWebSocketClient WebSocket { get; set; }

        public E2ETestFixture()
        {
            Task.Run(InitializeFixture);
        }

        private async Task InitializeFixture()
        {
            // Setup test environment
            Environment = GetEnvironment();

            // Initiate other services

            WebSocket = new SiriusWebSocketClient(Environment.Host, Environment.Port);
            Client = new SiriusClient(Environment.BaseUrl);
          
            SeedAccount = await GetSeedAccount();
          

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

            var networkType = await Client.NetworkHttp.GetNetworkType();

            return Account.CreateFromPrivateKey(privateKey, networkType);

        }

        public async Task<Account> GenerateAccountAndSendSomeMoney(int amount)
        {
            var networkType = Client.NetworkHttp.GetNetworkType().Wait();
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

            await WebSocket.Listener.Open();

            var tx = WebSocket.Listener.ConfirmedTransactionsGiven(account.Address).Take(1).Timeout(TimeSpan.FromSeconds(100));

            await Client.TransactionHttp.Announce(signedTransaction);

            var result = await tx;

            if (result.IsConfirmed())
                return account;
            else
                throw new Exception($"Unable to send money to account {account.Address.Plain}");
        }

        public void WatchForFailure(SignedTransaction transaction)
        {
            WebSocket.Listener.TransactionStatus(Address.CreateFromPublicKey(transaction.Signer, Client.NetworkHttp.GetNetworkType().Wait()))
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
