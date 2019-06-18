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
    [Collection("E2ETestFixtureCollection")]
    public class E2ETransferTests: IDisposable
    {
        private readonly E2ETestFixture _fixture;

        private readonly ITestOutputHelper _output;

        public E2ETransferTests(E2ETestFixture fixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            _output = output;
            _fixture.WebSocket.Listener.Open().Wait();
        }

        public void Dispose()
        {
           // _fixture.WebSocket.Listener.Close();
        }

        [Fact]
        public async Task Should_Announce_Transfer_Transaction()
        {
            var networkType = _fixture.Client.NetworkHttp.GetNetworkType().Wait();

            var account = Account.GenerateNewAccount(networkType);

            const ulong money = (ulong)10;

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

            var signedTransaction = _fixture.SeedAccount.Sign(transferTransaction);
            _output.WriteLine($"Going to announce transaction {signedTransaction.Hash}");

            var tx = _fixture.WebSocket.Listener.ConfirmedTransactionsGiven(account.Address).Take(1);

            await _fixture.Client.TransactionHttp.Announce(signedTransaction);
            
            var result = await tx;
            result.TransactionInfo.Hash.Should().NotBeNullOrWhiteSpace();
            _output.WriteLine($"Request with transaction status {result.TransactionInfo.Hash}");

      

        }

    }
}
