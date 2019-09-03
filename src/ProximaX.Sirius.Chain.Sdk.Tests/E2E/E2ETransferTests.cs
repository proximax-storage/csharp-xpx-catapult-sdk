using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions.Messages;
using Xunit;
using Xunit.Abstractions;

namespace ProximaX.Sirius.Chain.Sdk.Tests.E2E
{
    
    public class E2ETransferTests: E2ETestBase, IDisposable
    {
       
        private readonly ITestOutputHelper Log;

        public E2ETransferTests(ITestOutputHelper log)
        {

            Log = log;
            SiriusWebSocketClient.Listener.Open().Wait();
        }

        public void Dispose()
        {
            SiriusWebSocketClient.Listener.Close();
        }

        [Fact]
        public async Task Should_Announce_Transfer_Transaction_With_NetworkCurrencyMosaic()
        {
       
            var account = Account.GenerateNewAccount(NetworkType);

            var transferTransaction = TransferTransaction.Create(
                Deadline.Create(),
                Recipient.From(account.Address),
                new List<Mosaic>()
                {
                  NetworkCurrencyMosaic.CreateRelative(10)
                },
                PlainMessage.Create("transferTest"),
                NetworkType);

            var signedTransaction = SeedAccount.Sign(transferTransaction, GenerationHash);

            WatchForFailure(signedTransaction);

            Log.WriteLine($"Going to announce transaction {signedTransaction.Hash}");

            var tx = SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(SeedAccount.Address).Take(1);

            await SiriusClient.TransactionHttp.Announce(signedTransaction);
            
            var result = await tx;

            result.TransactionInfo.Hash.Should().NotBeNullOrWhiteSpace();

            Log.WriteLine($"Request with transaction status {result.TransactionInfo.Hash}");

        }

    }
}
