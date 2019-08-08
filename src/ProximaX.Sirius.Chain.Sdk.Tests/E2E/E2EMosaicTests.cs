using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Namespaces;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions.Messages;
using Xunit;
using Xunit.Abstractions;

namespace ProximaX.Sirius.Chain.Sdk.Tests.E2E
{
    [Collection("E2ETestFixtureCollection")]
    public class E2EMosaicTests 
    {

        private readonly E2ETestFixture _fixture;

        private readonly ITestOutputHelper _output;


        public E2EMosaicTests(E2ETestFixture fixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            _output = output;


        }


        [Fact]
        public async Task Should_Create_Mosaic()
        {
            var networkType = _fixture.Client.NetworkHttp.GetNetworkType().Wait();

            var account = await _fixture.GenerateAccountAndSendSomeMoney(1000);

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
                networkType);

            _output.WriteLine($"Going to create mosaic {mosaicId}");

            var mosaicSupplyChangeTransaction = MosaicSupplyChangeTransaction.Create(
                Deadline.Create(),
                mosaicDefinitionTransaction.MosaicId,
                MosaicSupplyType.INCREASE,
                1000000,
                networkType);

            var aggregateTransaction = AggregateTransaction.CreateComplete(
                Deadline.Create(),
                new List<Transaction>
                {
                    mosaicDefinitionTransaction.ToAggregate(account.PublicAccount),
                    mosaicSupplyChangeTransaction.ToAggregate(account.PublicAccount)
                },
                networkType);

            var signedTransaction = account.Sign(aggregateTransaction,_fixture.Environment.GenerationHash);
            _output.WriteLine($"Going to announce transaction {signedTransaction.Hash}");

            var tx = _fixture.WebSocket.Listener.ConfirmedTransactionsGiven(account.Address).Take(1)
                .Timeout(TimeSpan.FromSeconds(3000));

            await _fixture.Client.TransactionHttp.Announce(signedTransaction);

            var result = await tx;
            _output.WriteLine($"Request confirmed with transaction {result.TransactionInfo.Hash}");

            var mosaicInfo = await _fixture.Client.MosaicHttp.GetMosaic(mosaicDefinitionTransaction.MosaicId);

            _output.WriteLine($"Mosaic created {mosaicInfo}");

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
            var networkType = _fixture.Client.NetworkHttp.GetNetworkType().Wait();

            var account = await _fixture.GenerateAccountAndSendSomeMoney(1000);

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
                networkType);
            _output.WriteLine($"Going to create mosaic {mosaicId}");

            var mosaicSupplyChangeTransaction = MosaicSupplyChangeTransaction.Create(
                Deadline.Create(),
                mosaicDefinitionTransaction.MosaicId,
                MosaicSupplyType.INCREASE,
                1000000,
                networkType);

            var aggregateTransaction = AggregateTransaction.CreateComplete(
                Deadline.Create(),
                new List<Transaction>
                {
                    mosaicDefinitionTransaction.ToAggregate(account.PublicAccount),
                    mosaicSupplyChangeTransaction.ToAggregate(account.PublicAccount)
                },
                networkType);

            var signedTransaction = account.Sign(aggregateTransaction, _fixture.Environment.GenerationHash);
            _output.WriteLine($"Going to announce transaction {signedTransaction.Hash}");

            var tx = _fixture.WebSocket.Listener.ConfirmedTransactionsGiven(account.Address).Take(1)
                .Timeout(TimeSpan.FromSeconds(3000));

            await _fixture.Client.TransactionHttp.Announce(signedTransaction);

            var result = await tx;
            _output.WriteLine($"Request confirmed with transaction {result.TransactionInfo.Hash}");

        

            var mosaicDecreaseSupplyChangeTransaction = MosaicSupplyChangeTransaction.Create(
                Deadline.Create(),
                mosaicDefinitionTransaction.MosaicId,
                MosaicSupplyType.DECREASE,
                500000,
                networkType);

            const ulong expectedAmount = 1000000 - 500000;

            var signedDecreaseTransaction = account.Sign(mosaicDecreaseSupplyChangeTransaction, _fixture.Environment.GenerationHash);
            await _fixture.Client.TransactionHttp.Announce(signedDecreaseTransaction);

            var result2 = await tx;

            _output.WriteLine($"Request confirmed with transaction {result2.TransactionInfo.Hash}");

            var mosaicInfo = await _fixture.Client.MosaicHttp.GetMosaic(mosaicDefinitionTransaction.MosaicId);
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
            var networkType = _fixture.Client.NetworkHttp.GetNetworkType().Wait();

            var account = await _fixture.GenerateAccountAndSendSomeMoney(1000);

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
                networkType);
            _output.WriteLine($"Going to create mosaic {mosaicId}");

            var mosaicSupplyChangeTransaction = MosaicSupplyChangeTransaction.Create(
                Deadline.Create(),
                mosaicDefinitionTransaction.MosaicId,
                MosaicSupplyType.INCREASE,
                1000000,
                networkType);

            var aggregateTransaction = AggregateTransaction.CreateComplete(
                Deadline.Create(),
                new List<Transaction>
                {
                    mosaicDefinitionTransaction.ToAggregate(account.PublicAccount),
                    mosaicSupplyChangeTransaction.ToAggregate(account.PublicAccount)
                },
                networkType);

            var signedTransaction = account.Sign(aggregateTransaction, _fixture.Environment.GenerationHash);
            _output.WriteLine($"Going to announce transaction {signedTransaction.Hash}");

            var tx = _fixture.WebSocket.Listener.ConfirmedTransactionsGiven(account.Address).Take(1)
                .Timeout(TimeSpan.FromSeconds(3000));

            await _fixture.Client.TransactionHttp.Announce(signedTransaction);

            var result = await tx;
            _output.WriteLine($"Request confirmed with transaction {result.TransactionInfo.Hash}");



            var mosaicDecreaseSupplyChangeTransaction = MosaicSupplyChangeTransaction.Create(
                Deadline.Create(),
                mosaicDefinitionTransaction.MosaicId,
                MosaicSupplyType.INCREASE,
                500000,
                networkType);

            const ulong expectedAmount = 1000000 + 500000;

            var signedDecreaseTransaction = account.Sign(mosaicDecreaseSupplyChangeTransaction, _fixture.Environment.GenerationHash);
            await _fixture.Client.TransactionHttp.Announce(signedDecreaseTransaction);

            var result2 = await tx;

            _output.WriteLine($"Request confirmed with transaction {result2.TransactionInfo.Hash}");

            var mosaicInfo = await _fixture.Client.MosaicHttp.GetMosaic(mosaicDefinitionTransaction.MosaicId);
            mosaicInfo.Should().NotBeNull();
            mosaicInfo.Divisibility.Should().Be(3);
            mosaicInfo.Duration.Should().Be(1000);
            mosaicInfo.IsLevyMutable.Should().BeFalse();
            mosaicInfo.IsSupplyMutable.Should().BeTrue();
            mosaicInfo.IsTransferable.Should().BeTrue();
            mosaicInfo.Supply.Should().Be(expectedAmount);

        }

        [Fact]
        public async Task  Should_Link_Namespace_To_Mosaic()
        {
            #region Create mosaic
            var networkType = _fixture.Client.NetworkHttp.GetNetworkType().Wait();

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
                networkType);

            var mosaicSupplyChangeTransaction = MosaicSupplyChangeTransaction.Create(
                Deadline.Create(),
                mosaicDefinitionTransaction.MosaicId,
                MosaicSupplyType.INCREASE,
                1000000,
                networkType);

            var aggregateTransaction = AggregateTransaction.CreateComplete(
                Deadline.Create(),
                new List<Transaction>
                {
                    mosaicDefinitionTransaction.ToAggregate(account.PublicAccount),
                    mosaicSupplyChangeTransaction.ToAggregate(account.PublicAccount)
                },
                networkType);

            var signedTransaction = account.Sign(aggregateTransaction, _fixture.Environment.GenerationHash);

            _output.WriteLine($"Going to announce transaction {signedTransaction.Hash}");

            var tx = _fixture.WebSocket.Listener.ConfirmedTransactionsGiven(account.Address).Take(1)
                .Timeout(TimeSpan.FromSeconds(3000));

            await _fixture.Client.TransactionHttp.Announce(signedTransaction);

            var result = await tx;
            _output.WriteLine($"Request confirmed with transaction {result.TransactionInfo.Hash}");

            var mosaicInfo = await _fixture.Client.MosaicHttp.GetMosaic(mosaicDefinitionTransaction.MosaicId);
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
                networkType
            );

            var registeredNsSignedTransaction = account.Sign(registerNamespaceTransaction, _fixture.Environment.GenerationHash);

            tx = _fixture.WebSocket.Listener.ConfirmedTransactionsGiven(account.Address).Take(1)
                .Timeout(TimeSpan.FromSeconds(3000));

            await _fixture.Client.TransactionHttp.Announce(registeredNsSignedTransaction);

            _output.WriteLine(
                $"Registered namespace {namespaceName} for account {account.Address.Plain} with transaction {registeredNsSignedTransaction.Hash}");

            result = await tx;

            _output.WriteLine($"Request confirmed with transaction {result.TransactionInfo.Hash}");

            var expectedId = new NamespaceId(namespaceName);

            var nsInfo = await _fixture.Client.NamespaceHttp.GetNamespace(expectedId).Timeout(_fixture.DefaultTimeout);

            _output.WriteLine(
                $"Retrieved namespace {namespaceName} successfully. The namespace HexId {nsInfo.Id.HexId}");
            nsInfo.Should().NotBeNull();
            #endregion

            #region Link namespace to the mosaic
            var mosaicAliasTransaction = AliasTransaction.CreateForMosaic(
                mosaicInfo.MosaicId,
                nsInfo.Id,
                AliasActionType.LINK,
                Deadline.Create(),
                networkType
            );

            tx = _fixture.WebSocket.Listener.ConfirmedTransactionsGiven(account.Address).Take(1);

            var aliasSignedTransaction = account.Sign(mosaicAliasTransaction, _fixture.Environment.GenerationHash);

            WatchForFailure(aliasSignedTransaction);

            await _fixture.Client.TransactionHttp.Announce(aliasSignedTransaction);

            result = await tx;

            _output.WriteLine($"Request confirmed with transaction {result.TransactionInfo.Hash}");


            nsInfo = await _fixture.Client.NamespaceHttp.GetNamespace(expectedId);

            nsInfo.Should().NotBeNull();
            nsInfo.HasAlias.Should().BeTrue();
            nsInfo.Alias.Type.Should().BeEquivalentTo(AliasType.MOSAIC_ID);
            nsInfo.Alias.MosaicId.HexId.Should().BeEquivalentTo(mosaicInfo.MosaicId.HexId);

            #endregion

            #region Send mosaic using namespace alias to recipient

            var newAccount = Account.GenerateNewAccount(networkType);

            var transferTransaction = TransferTransaction.Create(
                Deadline.Create(),
                newAccount.Address, 
                new List<Mosaic>()
                {
                    new Mosaic(nsInfo.Id, 10)
                },
                PlainMessage.Create("Send some mosaic to new address"),
                networkType);

            var tx2 = _fixture.WebSocket.Listener.ConfirmedTransactionsGiven(newAccount.Address).Take(1)
                .Timeout(TimeSpan.FromSeconds(3000));

            var nsSignedTransferTransaction = account.Sign(transferTransaction,_fixture.Environment.GenerationHash);

            WatchForFailure(nsSignedTransferTransaction);

            await _fixture.Client.TransactionHttp.Announce(nsSignedTransferTransaction);

            var result2 = await tx2;

            _output.WriteLine($"Request confirmed with transaction {result2.TransactionInfo.Hash}");

            var newAccountInfo = await _fixture.Client.AccountHttp.GetAccountInfo(newAccount.Address);

            _output.WriteLine($"Account {newAccountInfo.Address.Plain} with mosaic {newAccountInfo.Mosaics[0]} after transfer to the namespace alias");

            var expectedMosaicAmount = Convert.ToUInt64(10);
            newAccountInfo.Mosaics[0].Id.Should().Be(mosaicInfo.MosaicId.Id);
            newAccountInfo.Mosaics[0]?.Amount.Should().Be(expectedMosaicAmount);
            #endregion
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

        internal void WatchForNewBlock()
        {
            _fixture.WebSocket.Listener.NewBlock()
                .Timeout(TimeSpan.FromSeconds(3000))  
                .Subscribe(
                    block =>
                    {
                        _output.WriteLine($"New block is created {block.Height}");
                    },
                    err =>
                    {
                        _output.WriteLine($"Transaction error - {err}");
                        _fixture.WebSocket.Listener.Close();

                    });
        }
    }
}
