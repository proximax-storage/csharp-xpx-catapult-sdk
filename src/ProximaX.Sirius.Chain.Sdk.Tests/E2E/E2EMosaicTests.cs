using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using Xunit;
using Xunit.Abstractions;

namespace ProximaX.Sirius.Chain.Sdk.Tests.E2E
{

    public class E2EMosaicTests : E2ETestBase, IDisposable
    {

        public E2EMosaicTests(ITestOutputHelper log) : base(log)
        {
            SiriusWebSocketClient.Listener.Open().Wait();
        }

        public void Dispose()
        {
            SiriusWebSocketClient.Listener.Close();
        }


        [Fact]
        public async Task Should_Create_Mosaic()
        {

            var account = SeedAccount;
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
            Log.WriteLine($"Going to create mosaic {mosaicId}");

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

            Log.WriteLine($"Going to announce transaction {signedTransaction.Hash}");

            var tx = SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(account.Address).Take(1)
                .Timeout(TimeSpan.FromSeconds(3000));

            await SiriusClient.TransactionHttp.Announce(signedTransaction);

            var result = await tx;
            Log.WriteLine($"Request confirmed with transaction {result.TransactionInfo.Hash}");

        
           var mosaicInfo = await SiriusClient.MosaicHttp.GetMosaic(mosaicDefinitionTransaction.MosaicId);

           Log.WriteLine($"Mosaic created {mosaicInfo}");

           mosaicInfo.Should().NotBeNull();
           mosaicInfo.Divisibility.Should().Be(0);
           mosaicInfo.Duration.Should().Be(1000);
           mosaicInfo.IsLevyMutable.Should().BeFalse();
           mosaicInfo.IsSupplyMutable.Should().BeTrue();
           mosaicInfo.IsTransferable.Should().BeTrue();
           mosaicInfo.Supply.Should().Be(1000000);
           
        }

      
        [Fact]
        public async Task Should_Decrease_Mosaic_Supply()
        {

            var account = SeedAccount;

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
                    divisibility: 6,
                    duration: 1000
                ),
                NetworkType);

            Log.WriteLine($"Going to create mosaic {mosaicId}");

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

            Log.WriteLine($"Going to announce transaction {signedTransaction.Hash}");

            var tx = SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(account.Address).Take(1)
                .Timeout(TimeSpan.FromSeconds(3000));

            await SiriusClient.TransactionHttp.Announce(signedTransaction);

            var result = await tx;
            Log.WriteLine($"Request confirmed with transaction {result.TransactionInfo.Hash}");



            var mosaicDecreaseSupplyChangeTransaction = MosaicSupplyChangeTransaction.Create(
                Deadline.Create(),
                mosaicDefinitionTransaction.MosaicId,
                MosaicSupplyType.DECREASE,
                500000,
                NetworkType);

            const ulong expectedAmount = 1000000 - 500000;

            var signedDecreaseTransaction = account.Sign(mosaicDecreaseSupplyChangeTransaction, GenerationHash);

            await SiriusClient.TransactionHttp.Announce(signedDecreaseTransaction);

            var result2 = await tx;

            Log.WriteLine($"Request confirmed with transaction {result2.TransactionInfo.Hash}");

            var mosaicInfo = await SiriusClient.MosaicHttp.GetMosaic(mosaicDefinitionTransaction.MosaicId);
            mosaicInfo.Should().NotBeNull();
            mosaicInfo.Divisibility.Should().Be(6);
            mosaicInfo.Duration.Should().Be(1000);
            mosaicInfo.IsLevyMutable.Should().BeFalse();
            mosaicInfo.IsSupplyMutable.Should().BeTrue();
            mosaicInfo.IsTransferable.Should().BeTrue();
            mosaicInfo.Supply.Should().Be(expectedAmount);

        }
        
        [Fact]
        public async Task Should_Increase_Mosaic_Supply()
        {

            var account = SeedAccount;

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
                    divisibility: 3,
                    duration: 1000
                ),
                NetworkType);
            Log.WriteLine($"Going to create mosaic {mosaicId}");

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
            Log.WriteLine($"Going to announce transaction {signedTransaction.Hash}");

            var tx = SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(account.Address).Take(1)
                .Timeout(TimeSpan.FromSeconds(3000));

            await SiriusClient.TransactionHttp.Announce(signedTransaction);

            var result = await tx;
            Log.WriteLine($"Request confirmed with transaction {result.TransactionInfo.Hash}");



            var mosaicDecreaseSupplyChangeTransaction = MosaicSupplyChangeTransaction.Create(
                Deadline.Create(),
                mosaicDefinitionTransaction.MosaicId,
                MosaicSupplyType.INCREASE,
                500000,
                NetworkType);

            const ulong expectedAmount = 1000000 + 500000;

            var signedDecreaseTransaction = account.Sign(mosaicDecreaseSupplyChangeTransaction, GenerationHash);
            await SiriusClient.TransactionHttp.Announce(signedDecreaseTransaction);

            var result2 = await tx;

            Log.WriteLine($"Request confirmed with transaction {result2.TransactionInfo.Hash}");

            var mosaicInfo = await SiriusClient.MosaicHttp.GetMosaic(mosaicDefinitionTransaction.MosaicId);
            mosaicInfo.Should().NotBeNull();
            mosaicInfo.Divisibility.Should().Be(3);
            mosaicInfo.Duration.Should().Be(1000);
            mosaicInfo.IsLevyMutable.Should().BeFalse();
            mosaicInfo.IsSupplyMutable.Should().BeTrue();
            mosaicInfo.IsTransferable.Should().BeTrue();
            mosaicInfo.Supply.Should().Be(expectedAmount);

        }
        /*
        [Fact]
        public async Task Should_Link_Namespace_To_Mosaic()
        {
            #region Create mosaic
            var NetworkType = SiriusClient.NetworkHttp.GetNetworkType().Wait();

            var account = await _fixture.GenerateAccountAndSendSomeMoney(1000);

            var nonce = MosaicNonce.CreateRandom();

            var mosaicDefinitionTransaction = MosaicDefinitionTransaction.Create(
                nonce,
                MosaicId.CreateFromNonce(nonce, account.PublicAccount.PublicKey),
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

            Log.WriteLine($"Going to announce transaction {signedTransaction.Hash}");

            var tx = SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(account.Address).Take(1)
                .Timeout(TimeSpan.FromSeconds(3000));

            await SiriusClient.TransactionHttp.Announce(signedTransaction);

            var result = await tx;
            Log.WriteLine($"Request confirmed with transaction {result.TransactionInfo.Hash}");

            var mosaicInfo = await SiriusClient.MosaicHttp.GetMosaic(mosaicDefinitionTransaction.MosaicId);
            mosaicInfo.Should().NotBeNull();
            mosaicInfo.Divisibility.Should().Be(0);
            mosaicInfo.Duration.Should().Be(1000);
            mosaicInfo.IsLevyMutable.Should().BeFalse();
            mosaicInfo.IsSupplyMutable.Should().BeTrue();
            mosaicInfo.IsTransferable.Should().BeTrue();
            mosaicInfo.Supply.Should().Be(1000000);
            #endregion

            #region register new namespace
            var namespaceName = "nsp" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);
            var registerNamespaceTransaction = RegisterNamespaceTransaction.CreateRootNamespace(
                Deadline.Create(),
                namespaceName,
                100,
                NetworkType
            );

            var registeredNsSignedTransaction = account.Sign(registerNamespaceTransaction, GenerationHash);

            tx = SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(account.Address).Take(1)
                .Timeout(TimeSpan.FromSeconds(3000));

            await SiriusClient.TransactionHttp.Announce(registeredNsSignedTransaction);

            Log.WriteLine(
                $"Registered namespace {namespaceName} for account {account.Address.Plain} with transaction {registeredNsSignedTransaction.Hash}");

            result = await tx;

            Log.WriteLine($"Request confirmed with transaction {result.TransactionInfo.Hash}");

            var expectedId = new NamespaceId(namespaceName);

            var nsInfo = await SiriusClient.NamespaceHttp.GetNamespace(expectedId).Timeout(_fixture.DefaultTimeout);

            Log.WriteLine(
                $"Retrieved namespace {namespaceName} successfully. The namespace HexId {nsInfo.Id.HexId}");
            nsInfo.Should().NotBeNull();
            #endregion

            #region Link namespace to the mosaic
            var mosaicAliasTransaction = AliasTransaction.CreateForMosaic(
                mosaicInfo.MosaicId,
                nsInfo.Id,
                AliasActionType.LINK,
                Deadline.Create(),
                NetworkType
            );

            tx = SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(account.Address).Take(1);

            var aliasSignedTransaction = account.Sign(mosaicAliasTransaction, GenerationHash);

            WatchForFailure(aliasSignedTransaction);

            await SiriusClient.TransactionHttp.Announce(aliasSignedTransaction);

            result = await tx;

            Log.WriteLine($"Request confirmed with transaction {result.TransactionInfo.Hash}");


            nsInfo = await SiriusClient.NamespaceHttp.GetNamespace(expectedId);

            nsInfo.Should().NotBeNull();
            nsInfo.HasAlias.Should().BeTrue();
            nsInfo.Alias.Type.Should().BeEquivalentTo(AliasType.MOSAIC_ID);
            nsInfo.Alias.MosaicId.HexId.Should().BeEquivalentTo(mosaicInfo.MosaicId.HexId);

            #endregion

            #region Send mosaic using namespace alias to recipient

            var newAccount = Account.GenerateNewAccount(NetworkType);

            var transferTransaction = TransferTransaction.Create(
                Deadline.Create(),
                Recipient.From(newAccount.Address),
                new List<Mosaic>()
                {
                    new Mosaic(nsInfo.Id, 10)
                },
                PlainMessage.Create("Send some mosaic to new address"),
                NetworkType);

            var tx2 = SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(newAccount.Address).Take(1)
                .Timeout(TimeSpan.FromSeconds(3000));

            var nsSignedTransferTransaction = account.Sign(transferTransaction, GenerationHash);

            WatchForFailure(nsSignedTransferTransaction);

            await SiriusClient.TransactionHttp.Announce(nsSignedTransferTransaction);

            var result2 = await tx2;

            Log.WriteLine($"Request confirmed with transaction {result2.TransactionInfo.Hash}");

            var newAccountInfo = await SiriusClient.AccountHttp.GetAccountInfo(newAccount.Address);

            Log.WriteLine($"Account {newAccountInfo.Address.Plain} with mosaic {newAccountInfo.Mosaics[0]} after transfer to the namespace alias");

            var expectedMosaicAmount = Convert.ToUInt64(10);
            newAccountInfo.Mosaics[0].Id.Should().Be(mosaicInfo.MosaicId.Id);
            newAccountInfo.Mosaics[0]?.Amount.Should().Be(expectedMosaicAmount);
            #endregion
        }

        internal void WatchForFailure(SignedTransaction transaction)
        {
            SiriusWebSocketClient.Listener.TransactionStatus(Address.CreateFromPublicKey(transaction.Signer, SiriusClient.NetworkHttp.GetNetworkType().Wait()))
                .Subscribe(
                    e =>
                    {
                        Log.WriteLine($"Transaction status {e.Hash} - {e.Status}");
                    },
                    err =>
                    {
                        Log.WriteLine($"Transaction error - {err}");
                    });
        }

        internal void WatchForNewBlock()
        {
            SiriusWebSocketClient.Listener.NewBlock()
                .Timeout(TimeSpan.FromSeconds(3000))
                .Subscribe(
                    block =>
                    {
                        Log.WriteLine($"New block is created {block.Height}");
                    },
                    err =>
                    {
                        Log.WriteLine($"Transaction error - {err}");
                        SiriusWebSocketClient.Listener.Close();

                    });
        }*/
    }
}
