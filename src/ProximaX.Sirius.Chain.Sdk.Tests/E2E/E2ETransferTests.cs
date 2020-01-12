using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Fees;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions.Builders;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions.Messages;
using Xunit;
using Xunit.Abstractions;

namespace ProximaX.Sirius.Chain.Sdk.Tests.E2E
{
    
    public class E2ETransferTests : IClassFixture<E2EBaseFixture>
    {
        readonly E2EBaseFixture Fixture;
        readonly ITestOutputHelper Log;

        public E2ETransferTests(E2EBaseFixture fixture, ITestOutputHelper log)
        {
            Fixture = fixture;
            Log = log;
        }





        [Fact]
        public async Task Should_Announce_Transfer_Transaction_With_NetworkCurrencyMosaic_PlainMessage()
        {
            var account = Account.GenerateNewAccount(Fixture.NetworkType);
            var mosaic = NetworkCurrencyMosaic.CreateRelative(10);
            var message = PlainMessage.Create("Test message");
            var result = await Fixture.Transfer(Fixture.SeedAccount, account.Address, mosaic, message, Fixture.GenerationHash);
            Log.WriteLine($"Transaction confirmed {result.TransactionInfo.Hash}");
            result.TransactionInfo.Hash.Should().NotBeNullOrWhiteSpace();
            //result.TransactionType.Should().Be(EntityType.TRANSFER);
            ((TransferTransaction)result).Message.GetMessageType().Should().Be(MessageType.PLAIN_MESSAGE.GetValueInByte());
        }

        [Fact]
        public async Task Should_Announce_Transfer_Transaction_With_Default_Fee()
        {
            var account = Account.GenerateNewAccount(Fixture.NetworkType);
            var mosaic = NetworkCurrencyMosaic.CreateRelative(10);
            var message = PlainMessage.Create("Test message");

            var builder = new TransferTransactionBuilder();

            var transferTransaction = builder
                .SetNetworkType(Fixture.NetworkType)
                .SetDeadline(Deadline.Create())
                .SetMosaics(new List<Mosaic>() { mosaic })
                .SetRecipient(Recipient.From(account.Address))
                .SetMessage(message)
                .SetFeeCalculationStrategy(FeeCalculationStrategyType.LOW)
                .Build();


            var signedTransaction = Fixture.SeedAccount.Sign(transferTransaction, Fixture.GenerationHash);

            Fixture.WatchForFailure(signedTransaction);

            //Log.WriteLine($"Going to announce transaction {signedTransaction.Hash}");

            var tx = Fixture.SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(Fixture.SeedAccount.Address).Take(1);

            await Fixture.SiriusClient.TransactionHttp.Announce(signedTransaction);

            var result = await tx;

            Log.WriteLine($"Transaction confirmed {result.TransactionInfo.Hash}");
            result.TransactionInfo.Hash.Should().NotBeNullOrWhiteSpace();
            //result.TransactionType.Should().Be(EntityType.TRANSFER);
            ((TransferTransaction)result).Message.GetMessageType().Should().Be(MessageType.PLAIN_MESSAGE.GetValueInByte());
        }

        [Fact]
        public async Task Should_Announce_Aggregate_Transaction_Signed_Aggregate_Transaction()
        {
            var account = Account.GenerateNewAccount(Fixture.NetworkType);
            var mosaic = NetworkCurrencyMosaic.CreateAbsolute(1);
            var message = PlainMessage.Create("c#__ SDK plain message test");
            var result = await Fixture.AggregateTransfer(Fixture.SeedAccount, account.Address, mosaic, message, Fixture.GenerationHash);
            Log.WriteLine($"Transaction confirmed {result.TransactionInfo.Hash}");
            result.TransactionInfo.Hash.Should().NotBeNullOrWhiteSpace();
            result.TransactionType.Should().Be(EntityType.AGGREGATE_COMPLETE);
        
        }

        [Fact]
        public async Task Should_Announce_Transfer_Transaction_With_NetworkCurrencyMosaic_SecureMessage()
        {
            var account = Account.GenerateNewAccount(Fixture.NetworkType);
            Log.WriteLine($"Reciever private key {account.KeyPair.PrivateKeyString}, reciever public key {account.PublicAccount.PublicKey}");
            var mosaic = NetworkCurrencyMosaic.CreateRelative(10);
            Log.WriteLine($"Sender private key {Fixture.SeedAccount.KeyPair.PrivateKeyString}, sender public key {Fixture.SeedAccount.PublicAccount.PublicKey}");
            var message = SecureMessage.Create("Test secure message", Fixture.SeedAccount.KeyPair.PrivateKeyString, account.PublicAccount.PublicKey);
            var result = await Fixture.Transfer(Fixture.SeedAccount, account.Address, mosaic, message, Fixture.GenerationHash);
            Log.WriteLine($"Transaction confirmed {result.TransactionInfo.Hash}");
            result.TransactionInfo.Hash.Should().NotBeNullOrWhiteSpace();
            //result.TransactionType.Should().Be(EntityType.TRANSFER);
            ((TransferTransaction)result).Message.GetMessageType().Should().Be(MessageType.SECURED_MESSAGE.GetValueInByte());
        }

        /*private async Task<Transaction> Transfer(Account from,Address to, Mosaic mosaic, IMessage message, string Fixture.GenerationHash)
        {
           
            var transferTransaction = TransferTransaction.Create(
                Deadline.Create(),
                Recipient.From(to),
                new List<Mosaic>()
                {
                 mosaic
                },
                message,
                Fixture.NetworkType);

            var signedTransaction = from.Sign(transferTransaction, Fixture.GenerationHash);

            Fixture.WatchForFailure(signedTransaction);

            Log.WriteLine($"Going to announce transaction {signedTransaction.Hash}");

            var tx = Fixture.SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(from.Address).Take(1);

            await Fixture.SiriusClient.TransactionHttp.Announce(signedTransaction);

            var result = await tx;

            return result;
        }*/

        
    }
}
