using System;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Utils;
using Xunit;
using Xunit.Abstractions;

namespace ProximaX.Sirius.Chain.Sdk.Tests.E2E
{
    
    public class E2EAccountLinkTests: E2ETestBase, IDisposable
    {
       

        public E2EAccountLinkTests(ITestOutputHelper log):base(log)
        { 
            SiriusWebSocketClient.Listener.Open().Wait();

        }

        public void Dispose()
        {
            SiriusWebSocketClient.Listener.Close();
        }

        [Fact]
        public async Task Should_Link_Account_To_Remote_Account()
        {
      
            var localAccount = Account.GenerateNewAccount(NetworkType);
            var remoteAccount = Account.GenerateNewAccount(NetworkType);


            var tx = SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(localAccount.Address).Take(1);

            Log.WriteLine($"Local Account {localAccount.Address.Plain} \r\n Private Key: {localAccount.PrivateKey} \r\n Public Key {localAccount.PublicKey}");
            Log.WriteLine($"Remote Account {remoteAccount.Address.Plain} \r\n Private Key: {remoteAccount.PrivateKey} \r\n Public Key {localAccount.PublicKey}");

            var accountLinkTransaction = AccountLinkTransaction.Create(remoteAccount.PublicAccount, AccountLinkAction.LINK, Deadline.Create(), (ulong)0, NetworkType);

            var signedTransaction = localAccount.Sign(accountLinkTransaction,GenerationHash);
            Log.WriteLine($"Going to announce account link transaction {signedTransaction} with hash {signedTransaction.Hash}");

            WatchForFailure(signedTransaction);

            await SiriusClient.TransactionHttp.Announce(signedTransaction);

            var result = await tx;

            Log.WriteLine($"Transaction confirmed with public key {result.Signer.PublicKey}");

            var localAccountInfo = await SiriusClient.AccountHttp.GetAccountInfo(localAccount.Address);
            var remoteAccountInfo = await SiriusClient.AccountHttp.GetAccountInfo(remoteAccount.Address);
            
            localAccountInfo.PublicKey.Should().BeEquivalentTo(remoteAccountInfo.LinkedAccountKey);
            remoteAccountInfo.PublicKey.Should().BeEquivalentTo(localAccountInfo.LinkedAccountKey);
        }

       
        [Fact]
        public async Task Should_UnLink_Account_From_Remote_Account()
        {
            var localAccount = Account.GenerateNewAccount(NetworkType);
            var remoteAccount = Account.GenerateNewAccount(NetworkType);


            var tx = SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(localAccount.Address).Take(1);

            Log.WriteLine($"Local Account {localAccount.Address.Plain} \r\n Private Key: {localAccount.PrivateKey} \r\n Public Key {localAccount.PublicKey}");
            Log.WriteLine($"Remote Account {remoteAccount.Address.Plain} \r\n Private Key: {remoteAccount.PrivateKey} \r\n Public Key {localAccount.PublicKey}");

            var accountLinkTransaction = AccountLinkTransaction.Create(remoteAccount.PublicAccount, AccountLinkAction.LINK, Deadline.Create(), (ulong)0, NetworkType);

            var signedTransaction = localAccount.Sign(accountLinkTransaction, GenerationHash);
            Log.WriteLine($"Going to announce account link transaction {signedTransaction} with hash {signedTransaction.Hash}");

            WatchForFailure(signedTransaction);

            await SiriusClient.TransactionHttp.Announce(signedTransaction);

            var result = await tx;

            Log.WriteLine($"Transaction confirmed with public key {result.Signer.PublicKey}");

            var localAccountInfo = await SiriusClient.AccountHttp.GetAccountInfo(localAccount.Address);
            var remoteAccountInfo = await SiriusClient.AccountHttp.GetAccountInfo(remoteAccount.Address);

            localAccountInfo.PublicKey.Should().BeEquivalentTo(remoteAccountInfo.LinkedAccountKey);
            remoteAccountInfo.PublicKey.Should().BeEquivalentTo(localAccountInfo.LinkedAccountKey);

            var accountUnLinkTransaction = AccountLinkTransaction.Create(remoteAccount.PublicAccount, AccountLinkAction.UNLINK, Deadline.Create(), (ulong)0, NetworkType);

            var signedTransaction2 = localAccount.Sign(accountUnLinkTransaction, GenerationHash);

            Log.WriteLine($"Going to announce account link transaction {signedTransaction2} with hash {signedTransaction2.Hash}");

            WatchForFailure(signedTransaction2);

            await SiriusClient.TransactionHttp.Announce(signedTransaction2);
            var result2 = await tx;

            Log.WriteLine($"Request confirmed with public key {result2.Signer.PublicKey}");

            var localAccountInfo2 = await SiriusClient.AccountHttp.GetAccountInfo(localAccount.Address);
            var remoteAccountInfo2 = await SiriusClient.AccountHttp.GetAccountInfo(remoteAccount.Address);

            localAccountInfo2.LinkedAccountKey.Should().BeEquivalentTo("0000000000000000000000000000000000000000000000000000000000000000");
            remoteAccountInfo2.LinkedAccountKey.Should().BeEquivalentTo("0000000000000000000000000000000000000000000000000000000000000000");

        }




    }
}
