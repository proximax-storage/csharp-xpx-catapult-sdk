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
    public class E2EBaseFixture: IDisposable
    {
        public readonly SiriusClient SiriusClient;

        public readonly SiriusWebSocketClient SiriusWebSocketClient;

        public readonly NetworkType NetworkType;

        public readonly Account SeedAccount;
        public Account MultiSigAccount;
        public Account Cosignatory1;
        public Account Cosignatory2;
        public Account Cosignatory3;
        public Account Cosignatory4;
        public readonly string GenerationHash;

   

        public E2EBaseFixture()
        {
          
            var env = GetEnvironment();

            SiriusClient = new SiriusClient(env.BaseUrl);
            GenerationHash = SiriusClient.BlockHttp.GetGenerationHash().Wait();
            bool useSSL = false;
            if(env.Protocol.Equals("https",StringComparison.InvariantCultureIgnoreCase))
            {
                useSSL = true;
            }
            SiriusWebSocketClient = new SiriusWebSocketClient(env.Host, env.Port, useSSL);
            SiriusWebSocketClient.Listener.Open().Wait();

            NetworkType = SiriusClient.NetworkHttp.GetNetworkType().Wait();

            SeedAccount = Account.CreateFromPrivateKey(env.SeedAccountPK, NetworkType);

            Task.Run(() => InitializeAccounts()).Wait();

        }

        public void Dispose()
        {
            SiriusWebSocketClient.Listener.Close();
        }

        private async Task InitializeAccounts()
        {
            MultiSigAccount = await GenerateAccountWithCurrency(1000);
            Cosignatory1 = await GenerateAccountWithCurrency(1000);
            Cosignatory2 = Account.GenerateNewAccount(NetworkType);
            Cosignatory3 = Account.GenerateNewAccount(NetworkType);
            Cosignatory4 = Account.GenerateNewAccount(NetworkType);
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
                case "BCDEV":
                    env = "BcDev";
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

        public async Task<Account> GenerateAccountWithCurrency(ulong amount)
        {
            var account = Account.GenerateNewAccount(NetworkType);
            var mosaic = NetworkCurrencyMosaic.CreateRelative(amount);
            var message = PlainMessage.Create("Send some money");
            var tx = await Transfer(SeedAccount, account.Address, mosaic, message, GenerationHash);

            return account;
        }

        public async Task<Transaction> Transfer(Account from, Address to, Mosaic mosaic, IMessage message, string generationHash)
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

            //Log.WriteLine($"Going to announce transaction {signedTransaction.Hash}");

            var tx = SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(from.Address).Take(1);

            await SiriusClient.TransactionHttp.Announce(signedTransaction);

            var result = await tx;

            return result;
        }

        public async Task<Transaction> AggregateTransfer(Account from, Address to, Mosaic mosaic, IMessage message, string GenerationHash)
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

            var aggregateTransaction = AggregateTransaction.CreateComplete(
                Deadline.Create(),
                new List<Transaction>
                {
                    transferTransaction.ToAggregate(from.PublicAccount)
                }, NetworkType);

            var signedTransaction = from.Sign(aggregateTransaction, GenerationHash);

            WatchForFailure(signedTransaction);

         
            var tx = SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(from.Address).Take(1);

            await  SiriusClient.TransactionHttp.Announce(signedTransaction);

            var result = await tx;

            return result;
        }

        public async Task<MosaicId> CreateMosaic(Account account)
        {
            var nonce = MosaicNonce.CreateRandom();
            var mosaicId = MosaicId.CreateFromNonce(nonce, account.PublicAccount.PublicKey);
               
            var mosaicDefinitionTransaction = MosaicDefinitionTransaction.Create(
                nonce,
                mosaicId,
                Deadline.Create(),
                MosaicProperties.Create(
                    supplyMutable: true,
                    transferable: true,
                    levyMutable: false,
                    divisibility: 0,
                    duration: 1000
                ),
                NetworkType);
        
            var mosaicSupplyChangeTransaction = MosaicSupplyChangeTransaction.Create(
              Deadline.Create(),
              mosaicDefinitionTransaction.MosaicId,
              MosaicSupplyType.INCREASE,
              1000000,
              NetworkType);

            var aggregateTransaction = AggregateTransaction.CreateComplete(
               Deadline.Create(),
               new List<Transaction>
               {
                       mosaicDefinitionTransaction.ToAggregate(account.PublicAccount),
                       mosaicSupplyChangeTransaction.ToAggregate(account.PublicAccount)
               },
               NetworkType);

            var signedTransaction = account.Sign(aggregateTransaction, GenerationHash);

            WatchForFailure(signedTransaction);

      

            var tx = SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(account.Address).Take(1)
                .Timeout(TimeSpan.FromSeconds(3000));

            await SiriusClient.TransactionHttp.Announce(signedTransaction);

            var result = await tx;


            return mosaicId;
        }


        public void WatchForFailure(SignedTransaction transaction)
        {
            SiriusWebSocketClient.Listener.TransactionStatus(Address.CreateFromPublicKey(transaction.Signer, NetworkType))
                .Subscribe(
                    e =>
                    {
                        Console.WriteLine(e.Status);

                    });
        }

        public void WatchForFailure(CosignatureSignedTransaction transaction)
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
