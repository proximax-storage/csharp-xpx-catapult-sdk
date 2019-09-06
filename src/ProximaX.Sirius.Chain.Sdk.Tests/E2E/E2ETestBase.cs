using ProximaX.Sirius.Chain.Sdk.Client;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions.Messages;
using ProximaX.Sirius.Chain.Sdk.Tests.Utils;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace ProximaX.Sirius.Chain.Sdk.Tests.E2E
{
    public class E2ETestBase
    {
        protected readonly SiriusClient SiriusClient;

        protected readonly SiriusWebSocketClient SiriusWebSocketClient;

        protected readonly NetworkType NetworkType;

        protected readonly Account SeedAccount;

        protected readonly string GenerationHash;

        protected readonly ITestOutputHelper Log;

        public E2ETestBase(ITestOutputHelper log)
        {
            Log = log;
            var env = GetEnvironment();

            SiriusClient = new SiriusClient(env.BaseUrl);
            SiriusWebSocketClient = new SiriusWebSocketClient(env.Host, env.Port);
            NetworkType = SiriusClient.NetworkHttp.GetNetworkType().Wait();
            SeedAccount = Account.CreateFromPrivateKey(env.SeedAccountPK, NetworkType);
            GenerationHash = SiriusClient.BlockHttp.GetGenerationHash().Wait();
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
            var generationHash = TestHelper.GetConfig()[$"Environments:{env}:Hash"];
            var seedAccountPK = TestHelper.GetConfig()[$"Environments:{env}:SeedAccountPrivateKey"]; 
            return new TestEnvironment(host, protocol, Convert.ToInt32(port), generationHash,seedAccountPK);

        }

        protected async Task<Account> GenerateAccountWithCurrency(ulong amount)
        {
            var account = Account.GenerateNewAccount(NetworkType);
            var mosaic = NetworkCurrencyMosaic.CreateRelative(amount);
            var message = PlainMessage.Create("Send some money");
            var tx = await Transfer(SeedAccount, account.Address, mosaic, message, GenerationHash);

            return account;
        }

        protected async Task<Transaction> Transfer(Account from, Address to, Mosaic mosaic, IMessage message, string generationHash)
        {

            var transferTransaction = TransferTransaction.Create(
                Deadline.Create(),
                Recipient.From(to),
                new List<Mosaic>()
                {
                 mosaic
                },
                message,
                NetworkType);

            var signedTransaction = from.Sign(transferTransaction, generationHash);

            WatchForFailure(signedTransaction);

            Log.WriteLine($"Going to announce transaction {signedTransaction.Hash}");

            var tx = SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(from.Address).Take(1);

            await SiriusClient.TransactionHttp.Announce(signedTransaction);

            var result = await tx;

            return result;
        }

        protected void WatchForFailure(SignedTransaction transaction)
        {
            SiriusWebSocketClient.Listener.TransactionStatus(Address.CreateFromPublicKey(transaction.Signer, NetworkType))
                .Subscribe(
                    e =>
                    {
                        Console.WriteLine(e.Status);

                    });
        }
    }
}
