using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Namespaces;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions.Messages;
using ProximaX.Sirius.Chain.Sdk.Utils;
using Xunit;
using Xunit.Abstractions;

namespace ProximaX.Sirius.Chain.Sdk.Tests.E2E
{
    [Collection("E2ETestFixtureCollection")]
    public class E2EAccountLinkTests
    {
        private readonly E2ETestFixture _fixture;

        private readonly ITestOutputHelper _output;

        public E2EAccountLinkTests(E2ETestFixture fixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            _output = output;

        }

     
        [Fact]
        public async Task Should_Link_Account_To_Remote_Account()
        {
            var networkType = _fixture.Client.NetworkHttp.GetNetworkType().Wait();
            var localAccount = Account.GenerateNewAccount(networkType);
            var remoteAccount = Account.GenerateNewAccount(networkType);

            await _fixture.WebSocket.Listener.Open();

            var tx = _fixture.WebSocket.Listener.ConfirmedTransactionsGiven(localAccount.Address).Take(1);

            _output.WriteLine($"Local Account {localAccount.Address.Plain} \r\n Private Key: {localAccount.PrivateKey} \r\n Public Key {localAccount.PublicKey}");
            _output.WriteLine($"Remote Account {remoteAccount.Address.Plain} \r\n Private Key: {remoteAccount.PrivateKey} \r\n Public Key {localAccount.PublicKey}");

            var accountLinkTransaction = AccountLinkTransaction.Create(remoteAccount.PublicAccount, AccountLinkAction.LINK, Deadline.Create(), (ulong)0, networkType);

            var signedTransaction = localAccount.Sign(accountLinkTransaction, _fixture.Environment.GenerationHash);
            _output.WriteLine($"Going to announce account link transaction {signedTransaction} with hash {signedTransaction.Hash}");
            WatchForFailure(signedTransaction);
            await _fixture.Client.TransactionHttp.Announce(signedTransaction);
            var result = await tx;

            _output.WriteLine($"Request confirmed with public key {result.Signer.PublicKey}");

            var localAccountInfo = await _fixture.Client.AccountHttp.GetAccountInfo(localAccount.Address);
            var remoteAccountInfo = await _fixture.Client.AccountHttp.GetAccountInfo(remoteAccount.Address);
            
            localAccountInfo.PublicKey.Should().BeEquivalentTo(remoteAccountInfo.LinkedAccountKey);
            remoteAccountInfo.PublicKey.Should().BeEquivalentTo(localAccountInfo.LinkedAccountKey);
        }


        [Fact]
        public async Task Should_UnLink_Account_From_Remote_Account()
        {
            var networkType = _fixture.Client.NetworkHttp.GetNetworkType().Wait();
            var localAccount = Account.GenerateNewAccount(networkType);
            var remoteAccount = Account.GenerateNewAccount(networkType);

            await _fixture.WebSocket.Listener.Open();

            var tx = _fixture.WebSocket.Listener.ConfirmedTransactionsGiven(localAccount.Address).Take(1);

            _output.WriteLine($"Local Account {localAccount.Address.Plain} \r\n Private Key: {localAccount.PrivateKey} \r\n Public Key {localAccount.PublicKey}");
            _output.WriteLine($"Remote Account {remoteAccount.Address.Plain} \r\n Private Key: {remoteAccount.PrivateKey} \r\n Public Key {localAccount.PublicKey}");

            var accountLinkTransaction = AccountLinkTransaction.Create(remoteAccount.PublicAccount, AccountLinkAction.LINK, Deadline.Create(), (ulong)0, networkType);

            var signedTransaction = localAccount.Sign(accountLinkTransaction, _fixture.Environment.GenerationHash);
            _output.WriteLine($"Going to announce account link transaction {signedTransaction} with hash {signedTransaction.Hash}");
            WatchForFailure(signedTransaction);
            await _fixture.Client.TransactionHttp.Announce(signedTransaction);
            var result = await tx;

            _output.WriteLine($"Request confirmed with public key {result.Signer.PublicKey}");

            var localAccountInfo = await _fixture.Client.AccountHttp.GetAccountInfo(localAccount.Address);
            var remoteAccountInfo = await _fixture.Client.AccountHttp.GetAccountInfo(remoteAccount.Address);

            localAccountInfo.PublicKey.Should().BeEquivalentTo(remoteAccountInfo.LinkedAccountKey);
            remoteAccountInfo.PublicKey.Should().BeEquivalentTo(localAccountInfo.LinkedAccountKey);

            var accountUnLinkTransaction = AccountLinkTransaction.Create(remoteAccount.PublicAccount, AccountLinkAction.UNLINK, Deadline.Create(), (ulong)0, networkType);

            var signedTransaction2 = localAccount.Sign(accountUnLinkTransaction, _fixture.Environment.GenerationHash);
            _output.WriteLine($"Going to announce account link transaction {signedTransaction2} with hash {signedTransaction2.Hash}");
            WatchForFailure(signedTransaction2);
            await _fixture.Client.TransactionHttp.Announce(signedTransaction2);
            var result2 = await tx;

            _output.WriteLine($"Request confirmed with public key {result2.Signer.PublicKey}");

            var localAccountInfo2 = await _fixture.Client.AccountHttp.GetAccountInfo(localAccount.Address);
            var remoteAccountInfo2 = await _fixture.Client.AccountHttp.GetAccountInfo(remoteAccount.Address);
            localAccountInfo2.LinkedAccountKey.Should().BeEquivalentTo("0000000000000000000000000000000000000000000000000000000000000000");
            remoteAccountInfo2.LinkedAccountKey.Should().BeEquivalentTo("0000000000000000000000000000000000000000000000000000000000000000");

        }






        internal void WatchForFailure(SignedTransaction transaction)
        {
            _fixture.WebSocket.Listener.TransactionStatus(Address.CreateFromPublicKey(transaction.Signer, _fixture.Client.NetworkHttp.GetNetworkType().Wait()))
                 .Subscribe(
                     e =>
                     {
                         _output.WriteLine($"Transaction status {e.Hash} - {e.Status}");
                     },
                     err =>
                     {
                         _output.WriteLine($"Transaction error - {err}");
                     });
        }


    }
}
