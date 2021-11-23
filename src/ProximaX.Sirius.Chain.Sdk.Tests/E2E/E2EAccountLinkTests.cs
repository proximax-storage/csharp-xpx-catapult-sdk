using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Namespaces;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Utils;
using Xunit;
using Xunit.Abstractions;

namespace ProximaX.Sirius.Chain.Sdk.Tests.E2E
{
    
    public class E2EAccountLinkTests: IClassFixture<E2EBaseFixture>
    {
        readonly E2EBaseFixture Fixture;
        readonly ITestOutputHelper Log;

        public E2EAccountLinkTests(E2EBaseFixture fixture, ITestOutputHelper log)
        {
            Fixture = fixture;
            Log = log;
        }




        [Fact]
        public async Task Should_Link_Account_To_Remote_Account()
        {
      
            var localAccount = Account.GenerateNewAccount(Fixture.NetworkType);
            var remoteAccount = Account.GenerateNewAccount(Fixture.NetworkType);


            var tx = Fixture.SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(localAccount.Address).Take(1);

            Log.WriteLine($"Local Account {localAccount.Address.Plain} \r\n Private Key: {localAccount.PrivateKey} \r\n Public Key {localAccount.PublicKey}");
            Log.WriteLine($"Remote Account {remoteAccount.Address.Plain} \r\n Private Key: {remoteAccount.PrivateKey} \r\n Public Key {localAccount.PublicKey}");

            var accountLinkTransaction = AccountLinkTransaction.Create(remoteAccount.PublicAccount, AccountLinkAction.LINK, Deadline.Create(), (ulong)0, Fixture.NetworkType);

            var signedTransaction = localAccount.Sign(accountLinkTransaction, Fixture.GenerationHash);
            Log.WriteLine($"Going to announce account link transaction {signedTransaction} with hash {signedTransaction.Hash}");

           Fixture.WatchForFailure(signedTransaction);

            await Fixture.SiriusClient.TransactionHttp.Announce(signedTransaction);

            var result = await tx;

            Log.WriteLine($"Transaction confirmed with public key {result.Signer.PublicKey}");

            var localAccountInfo = await Fixture.SiriusClient.AccountHttp.GetAccountInfo(localAccount.Address);
            var remoteAccountInfo = await Fixture.SiriusClient.AccountHttp.GetAccountInfo(remoteAccount.Address);
            Log.WriteLine($" Local Account info {localAccountInfo}");
            Log.WriteLine($" Remote Account info {remoteAccountInfo}");

            localAccountInfo.PublicKey.Should().Equals(localAccountInfo.LinkedAccountKey);
            remoteAccountInfo.PublicKey.Should().Equals(remoteAccountInfo.LinkedAccountKey);

            Log.WriteLine($" Local Account info {localAccountInfo.LinkedAccountKey}");
            Log.WriteLine($" Remote Account info {remoteAccountInfo.LinkedAccountKey}");

        }


        [Fact]
        public async Task Should_UnLink_Account_From_Remote_Account()
        {
            var localAccount = Account.GenerateNewAccount(Fixture.NetworkType);
            var remoteAccount = Account.GenerateNewAccount(Fixture.NetworkType);


            var tx = Fixture.SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(localAccount.Address).Take(1);

            Log.WriteLine($"Local Account {localAccount.Address.Plain} \r\n Private Key: {localAccount.PrivateKey} \r\n Public Key {localAccount.PublicKey}");
            Log.WriteLine($"Remote Account {remoteAccount.Address.Plain} \r\n Private Key: {remoteAccount.PrivateKey} \r\n Public Key {localAccount.PublicKey}");

            var accountLinkTransaction = AccountLinkTransaction.Create(remoteAccount.PublicAccount, AccountLinkAction.LINK, Deadline.Create(), (ulong)0, Fixture.NetworkType);

            var signedTransaction = localAccount.Sign(accountLinkTransaction, Fixture.GenerationHash);
            Log.WriteLine($"Going to announce account link transaction {signedTransaction} with hash {signedTransaction.Hash}");

            Fixture.WatchForFailure(signedTransaction);

            await Fixture.SiriusClient.TransactionHttp.Announce(signedTransaction);

            var result = await tx;

            Log.WriteLine($"Transaction confirmed with public key {result.Signer.PublicKey}");

            var localAccountInfo = await Fixture.SiriusClient.AccountHttp.GetAccountInfo(localAccount.Address);
            var remoteAccountInfo = await Fixture.SiriusClient.AccountHttp.GetAccountInfo(remoteAccount.Address);

            localAccountInfo.PublicKey.Should().BeEquivalentTo(remoteAccountInfo.LinkedAccountKey);
            remoteAccountInfo.PublicKey.Should().BeEquivalentTo(localAccountInfo.LinkedAccountKey);

            var accountUnLinkTransaction = AccountLinkTransaction.Create(remoteAccount.PublicAccount, AccountLinkAction.UNLINK, Deadline.Create(), (ulong)0, Fixture.NetworkType);

            var signedTransaction2 = localAccount.Sign(accountUnLinkTransaction, Fixture.GenerationHash);

            Log.WriteLine($"Going to announce account link transaction {signedTransaction2} with hash {signedTransaction2.Hash}");

            Fixture.WatchForFailure(signedTransaction2);

            await Fixture.SiriusClient.TransactionHttp.Announce(signedTransaction2);
            var result2 = await tx;

            Log.WriteLine($"Request confirmed with public key {result2.Signer.PublicKey}");

            var localAccountInfo2 = await Fixture.SiriusClient.AccountHttp.GetAccountInfo(localAccount.Address);
            var remoteAccountInfo2 = await Fixture.SiriusClient.AccountHttp.GetAccountInfo(remoteAccount.Address);

            localAccountInfo2.LinkedAccountKey.Should().BeEquivalentTo("0000000000000000000000000000000000000000000000000000000000000000");
            remoteAccountInfo2.LinkedAccountKey.Should().BeEquivalentTo("0000000000000000000000000000000000000000000000000000000000000000");

        }


       

    }
}
