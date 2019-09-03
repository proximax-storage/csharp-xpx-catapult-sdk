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
        public async Task Should_Announce_Transfer_Transaction_With_NetworkCurrencyMosaic_PlainMessage()
        {
            var account = Account.GenerateNewAccount(NetworkType);
            var mosaic = NetworkCurrencyMosaic.CreateRelative(10);
            var message = PlainMessage.Create("Test message");
            var result = await Transfer(SeedAccount, account.Address, mosaic, message, GenerationHash);
            Log.WriteLine($"Transaction confirmed {result.TransactionInfo.Hash}");
            result.TransactionInfo.Hash.Should().NotBeNullOrWhiteSpace();
            result.TransactionType.Should().Be(TransactionType.TRANSFER);
            ((TransferTransaction)result).Message.GetMessageType().Should().Be(MessageType.PLAIN_MESSAGE.GetValueInByte());
        }

        [Fact]
        public async Task Should_Announce_Aggregate_Transaction_Signed_Aggregate_Transaction()
        {
            var account = Account.GenerateNewAccount(NetworkType);
            var mosaic = NetworkCurrencyMosaic.CreateAbsolute(1);
            var message = PlainMessage.Create("c#__ SDK plain message test");
            var result = await AggregateTransfer(SeedAccount, account.Address, mosaic, message, GenerationHash);
            Log.WriteLine($"Transaction confirmed {result.TransactionInfo.Hash}");
            result.TransactionInfo.Hash.Should().NotBeNullOrWhiteSpace();
            result.TransactionType.Should().Be(TransactionType.AGGREGATE_COMPLETE);
        
        }

        [Fact]
        public async Task Should_Announce_Transfer_Transaction_With_NetworkCurrencyMosaic_SecureMessage()
        {
            var account = Account.GenerateNewAccount(NetworkType);
            var mosaic = NetworkCurrencyMosaic.CreateRelative(10);
            var message = SecureMessage.Create("Test secure message", SeedAccount.KeyPair.PrivateKeyString, account.PublicAccount.PublicKey);
            var result = await Transfer(SeedAccount, account.Address, mosaic, message, GenerationHash);
            Log.WriteLine($"Transaction confirmed {result.TransactionInfo.Hash}");
            result.TransactionInfo.Hash.Should().NotBeNullOrWhiteSpace();
            result.TransactionType.Should().Be(TransactionType.TRANSFER);
            ((TransferTransaction)result).Message.GetMessageType().Should().Be(MessageType.SECURED_MESSAGE.GetValueInByte());
        }

        private async Task<Transaction> Transfer(Account from,Address to, Mosaic mosaic, IMessage message, string generationHash)
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

        private async Task<Transaction> AggregateTransfer(Account from, Address to, Mosaic mosaic, IMessage message, string generationHash)
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

            var signedTransaction = from.Sign(aggregateTransaction, generationHash);

            WatchForFailure(signedTransaction);

            Log.WriteLine($"Going to announce signed aggregate transaction {signedTransaction.Hash}");

            var tx = SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(from.Address).Take(1);

            await SiriusClient.TransactionHttp.Announce(signedTransaction);

            var result = await tx;

            return result;
        }
    }
}
