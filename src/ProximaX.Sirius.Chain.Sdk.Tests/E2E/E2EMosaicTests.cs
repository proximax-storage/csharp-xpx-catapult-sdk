﻿using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Namespaces;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions.Builders;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions.Messages;
using Xunit;
using Xunit.Abstractions;

namespace ProximaX.Sirius.Chain.Sdk.Tests.E2E
{
    public class E2EMosaicTests : IClassFixture<E2EBaseFixture>
    {
        private readonly E2EBaseFixture Fixture;
        private readonly ITestOutputHelper Log;

        public E2EMosaicTests(E2EBaseFixture fixture, ITestOutputHelper log)
        {
            Fixture = fixture;
            Log = log;
        }

        [Fact]
        public async Task Should_Create_Mosaic()
        {
            var account = Fixture.SeedAccount;
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
                Fixture.NetworkType);

            Log.WriteLine($"Going to create mosaic {mosaicDefinitionTransaction.MosaicId.HexId}");
            Log.WriteLine($"{account.PublicAccount.PublicKey}");
            Log.WriteLine($"{account.PublicAccount.Address}");

            var mosaicSupplyChangeTransaction = MosaicSupplyChangeTransaction.Create(
              Deadline.Create(),
              mosaicDefinitionTransaction.MosaicId,
              MosaicSupplyType.INCREASE,
              1000000,
              Fixture.NetworkType);

            var aggregateTransaction = AggregateTransaction.CreateComplete(
               Deadline.Create(),
               new List<Transaction>
               {
                       mosaicDefinitionTransaction.ToAggregate(account.PublicAccount),
                       mosaicSupplyChangeTransaction.ToAggregate(account.PublicAccount)
               },
               Fixture.NetworkType);

            var signedTransaction = account.Sign(aggregateTransaction, Fixture.GenerationHash);

            Fixture.WatchForFailure(signedTransaction);

            Log.WriteLine($"Going to announce transaction {signedTransaction.Hash}");

            var tx = Fixture.SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(account.Address).Take(1)
                .Timeout(TimeSpan.FromSeconds(3000));

            await Fixture.SiriusClient.TransactionHttp.Announce(signedTransaction);

            var result = await tx;
            Log.WriteLine($"Request confirmed with transaction {result.TransactionInfo.Hash}");

            var mosaic_Id = new MosaicId(mosaicId.HexId);

            var mosaicInfo = await Fixture.SiriusClient.MosaicHttp.GetMosaic(mosaic_Id);

            Log.WriteLine($"Mosaic created {mosaicInfo.MosaicId.HexId}");

            mosaicInfo.Should().NotBeNull();
            mosaicInfo.Divisibility.Should().Be(0);
            mosaicInfo.Duration.Should().Be(1000);
            mosaicInfo.IsLevyMutable.Should().BeFalse();
            mosaicInfo.IsSupplyMutable.Should().BeTrue();
            mosaicInfo.IsTransferable.Should().BeTrue();
            mosaicInfo.Supply.Should().Be(1000000);
        }

        [Fact]
        public async Task Should_Create_Mosaic_With_Builder()
        {
            var account = Fixture.SeedAccount;
            var nonce = MosaicNonce.CreateRandom();
            var mosaicId = MosaicId.CreateFromNonce(nonce, account.PublicAccount.PublicKey);
            var builder = new MosaicDefinitionTransactionBuilder();
            builder.SetNonce(nonce)
                .SetMosaicId(mosaicId)
                .SetMosaicProperties(MosaicProperties.Create(
                    supplyMutable: true,
                    transferable: true,
                    levyMutable: false,
                    divisibility: 0,
                    duration: 1000
                ))
                .SetDeadline(Deadline.Create())
                .SetNetworkType(Fixture.NetworkType);

            /*
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
                Fixture.NetworkType);
            */
            var mosaicDefinitionTransaction = builder.Build();

            Log.WriteLine($"Going to create mosaic {mosaicId}");

            var supplyBuilder = new MosaicSupplyChangeTransactionBuilder();
            supplyBuilder
                .SetDeadline(Deadline.Create())
                .SetNetworkType(Fixture.NetworkType)
                .IncreaseSupplyFor(mosaicDefinitionTransaction.MosaicId)
                .SetDelta(1000000);

            var mosaicSupplyChangeTransaction = supplyBuilder.Build();
            /*
            var mosaicSupplyChangeTransaction = MosaicSupplyChangeTransaction.Create(
              Deadline.Create(),
              mosaicDefinitionTransaction.MosaicId,
              MosaicSupplyType.INCREASE,
              1000000,
              Fixture.NetworkType);
              */

            var aggregateTransaction = AggregateTransaction.CreateComplete(
               Deadline.Create(),
               new List<Transaction>
               {
                       mosaicDefinitionTransaction.ToAggregate(account.PublicAccount),
                       mosaicSupplyChangeTransaction.ToAggregate(account.PublicAccount)
               },
               Fixture.NetworkType);

            var signedTransaction = account.Sign(aggregateTransaction, Fixture.GenerationHash);

            Fixture.WatchForFailure(signedTransaction);

            Log.WriteLine($"Going to announce transaction {signedTransaction.Hash}");

            var tx = Fixture.SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(account.Address).Take(1)
                .Timeout(TimeSpan.FromSeconds(3000));

            await Fixture.SiriusClient.TransactionHttp.Announce(signedTransaction);

            var result = await tx;
            Log.WriteLine($"Request confirmed with transaction {result.TransactionInfo.Hash}");

            var mosaicInfo = await Fixture.SiriusClient.MosaicHttp.GetMosaic(mosaicDefinitionTransaction.MosaicId);

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
            var account = Fixture.SeedAccount;

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
                Fixture.NetworkType);

            Log.WriteLine($"Going to create mosaic {mosaicId}");

            var mosaicSupplyChangeTransaction = MosaicSupplyChangeTransaction.Create(
                Deadline.Create(),
                mosaicDefinitionTransaction.MosaicId,
                MosaicSupplyType.INCREASE,
                1000000,
                Fixture.NetworkType);

            var aggregateTransaction = AggregateTransaction.CreateComplete(
                Deadline.Create(),
                new List<Transaction>
                {
                    mosaicDefinitionTransaction.ToAggregate(account.PublicAccount),
                    mosaicSupplyChangeTransaction.ToAggregate(account.PublicAccount)
                },
                Fixture.NetworkType);

            var signedTransaction = account.Sign(aggregateTransaction, Fixture.GenerationHash);

            Log.WriteLine($"Going to announce transaction {signedTransaction.Hash}");

            var tx = Fixture.SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(account.Address).Take(1)
                .Timeout(TimeSpan.FromSeconds(3000));

            await Fixture.SiriusClient.TransactionHttp.Announce(signedTransaction);

            var result = await tx;
            Log.WriteLine($"Request confirmed with transaction {result.TransactionInfo.Hash}");

            var mosaicDecreaseSupplyChangeTransaction = MosaicSupplyChangeTransaction.Create(
                Deadline.Create(),
                mosaicDefinitionTransaction.MosaicId,
                MosaicSupplyType.DECREASE,
                500000,
                Fixture.NetworkType);

            const ulong expectedAmount = 1000000 - 500000;

            var signedDecreaseTransaction = account.Sign(mosaicDecreaseSupplyChangeTransaction, Fixture.GenerationHash);

            await Fixture.SiriusClient.TransactionHttp.Announce(signedDecreaseTransaction);

            var result2 = await tx;

            Log.WriteLine($"Request confirmed with transaction {result2.TransactionInfo.Hash}");

            var mosaicInfo = await Fixture.SiriusClient.MosaicHttp.GetMosaic(mosaicId);

            Log.WriteLine($"Mosaic: {mosaicId}");

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
            var account = Fixture.SeedAccount;

            var nonce = MosaicNonce.CreateRandom();
            var mosaicId = MosaicId.CreateFromNonce(nonce, account.PublicAccount.PublicKey);
            Log.WriteLine($"SeedAccount Pk {account.PrivateKey}");

            var mosaicDefinitionTransaction = MosaicDefinitionTransaction.Create(
                nonce,
                mosaicId,
                Deadline.Create(),
                MosaicProperties.Create(
                    supplyMutable: true,
                    transferable: true,
                    levyMutable: false,
                    divisibility: 5,
                    duration: 1000
                ),
                Fixture.NetworkType);
            Log.WriteLine($"Going to create mosaic {mosaicId}");

            var mosaicSupplyChangeTransaction = MosaicSupplyChangeTransaction.Create(
                Deadline.Create(),
                mosaicDefinitionTransaction.MosaicId,
                MosaicSupplyType.INCREASE,
                1000000,
                Fixture.NetworkType);

            var aggregateTransaction = AggregateTransaction.CreateComplete(
                Deadline.Create(),
                new List<Transaction>
                {
                    mosaicDefinitionTransaction.ToAggregate(account.PublicAccount),
                    mosaicSupplyChangeTransaction.ToAggregate(account.PublicAccount)
                },
                Fixture.NetworkType);

            var signedTransaction = account.Sign(aggregateTransaction, Fixture.GenerationHash);
            Log.WriteLine($"Going to announce transaction {signedTransaction.Hash}");

            var tx = Fixture.SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(account.Address).Take(1)
                .Timeout(TimeSpan.FromSeconds(3000));

            await Fixture.SiriusClient.TransactionHttp.Announce(signedTransaction);

            var result = await tx;
            Log.WriteLine($"Request confirmed with transaction {result.TransactionInfo.Hash}");

            var mosaicDecreaseSupplyChangeTransaction = MosaicSupplyChangeTransaction.Create(
                Deadline.Create(),
                mosaicDefinitionTransaction.MosaicId,
                MosaicSupplyType.INCREASE,
                500000,
                Fixture.NetworkType);

            const ulong expectedAmount = 1000000 + 500000;

            var signedDecreaseTransaction = account.Sign(mosaicDecreaseSupplyChangeTransaction, Fixture.GenerationHash);
            await Fixture.SiriusClient.TransactionHttp.Announce(signedDecreaseTransaction);

            var result2 = await tx;

            Log.WriteLine($"Request confirmed with transaction {result2.TransactionInfo.Hash}");

            var mosaicInfo = await Fixture.SiriusClient.MosaicHttp.GetMosaic(mosaicDefinitionTransaction.MosaicId);
            mosaicInfo.Should().NotBeNull();
            //  mosaicInfo.Divisibility.Should().Be(3);
            mosaicInfo.Duration.Should().Be(1000);
            mosaicInfo.IsLevyMutable.Should().BeFalse();
            mosaicInfo.IsSupplyMutable.Should().BeTrue();
            mosaicInfo.IsTransferable.Should().BeTrue();
            mosaicInfo.Supply.Should().Be(expectedAmount);
        }

        [Fact]
        public async Task Should_Link_Namespace_To_Mosaic()
        {
            #region Create mosaic

            var account = await Fixture.GenerateAccountWithCurrency(150000);

            Log.WriteLine($"account {account}");

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
                Fixture.NetworkType);

            var mosaicSupplyChangeTransaction = MosaicSupplyChangeTransaction.Create(
                Deadline.Create(),
                mosaicDefinitionTransaction.MosaicId,
                MosaicSupplyType.INCREASE,
                1000000,
                Fixture.NetworkType);

            var aggregateTransaction = AggregateTransaction.CreateComplete(
                Deadline.Create(),
                new List<Transaction>
                {
                    mosaicDefinitionTransaction.ToAggregate(account.PublicAccount),
                    mosaicSupplyChangeTransaction.ToAggregate(account.PublicAccount)
                },
                Fixture.NetworkType);

            var signedTransaction = account.Sign(aggregateTransaction, Fixture.GenerationHash);

            Log.WriteLine($"Going to announce transaction {signedTransaction.Hash}");

            var tx = Fixture.SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(account.Address).Take(1)
                .Timeout(TimeSpan.FromSeconds(3000));

            await Fixture.SiriusClient.TransactionHttp.Announce(signedTransaction);

            var result = await tx;
            Log.WriteLine($"Request confirmed with transaction {result.TransactionInfo.Hash}");

            var mosaicInfo = await Fixture.SiriusClient.MosaicHttp.GetMosaic(mosaicDefinitionTransaction.MosaicId);
            mosaicInfo.Should().NotBeNull();
            mosaicInfo.Divisibility.Should().Be(0);
            mosaicInfo.Duration.Should().Be(1000);
            // mosaicInfo.IsLevyMutable.Should().BeFalse();
            mosaicInfo.IsSupplyMutable.Should().BeTrue();
            mosaicInfo.IsTransferable.Should().BeTrue();
            mosaicInfo.Supply.Should().Be(1000000);

            #endregion Create mosaic

            #region register new namespace

            var namespaceName = "haieng";
            //var namespaceName = "nsp" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);
            var registerNamespaceTransaction = RegisterNamespaceTransaction.CreateRootNamespace(
                Deadline.Create(),
                namespaceName,
                100,
                Fixture.NetworkType
            );

            var registeredNsSignedTransaction = account.Sign(registerNamespaceTransaction, Fixture.GenerationHash);

            tx = Fixture.SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(account.Address).Take(1)
                .Timeout(TimeSpan.FromSeconds(3000));

            await Fixture.SiriusClient.TransactionHttp.Announce(registeredNsSignedTransaction);

            Log.WriteLine(
                $"Registered namespace {namespaceName} for account {account.Address.Plain} with transaction {registeredNsSignedTransaction.Hash}");

            result = await tx;

            Log.WriteLine($"Request confirmed with transaction {result.TransactionInfo.Hash}");

            var expectedId = new NamespaceId(namespaceName);

            var nsInfo = await Fixture.SiriusClient.NamespaceHttp.GetNamespace(expectedId);

            Log.WriteLine(
                $"Retrieved namespace {namespaceName} successfully. The namespace HexId {nsInfo.Id.HexId}");
            nsInfo.Should().NotBeNull();

            #endregion register new namespace

            #region Link namespace to the mosaic

            var mosaicAliasTransaction = AliasTransaction.CreateForMosaic(
                mosaicInfo.MosaicId,
                nsInfo.Id,
                AliasActionType.LINK,
                Deadline.Create(),
                Fixture.NetworkType
            );

            tx = Fixture.SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(account.Address).Take(1);

            var aliasSignedTransaction = account.Sign(mosaicAliasTransaction, Fixture.GenerationHash);

            Fixture.WatchForFailure(aliasSignedTransaction);

            await Fixture.SiriusClient.TransactionHttp.Announce(aliasSignedTransaction);

            result = await tx;

            Log.WriteLine($"Request confirmed with transaction {result.TransactionInfo.Hash}");

            nsInfo = await Fixture.SiriusClient.NamespaceHttp.GetNamespace(expectedId);

            nsInfo.Should().NotBeNull();
            nsInfo.HasAlias.Should().BeTrue();
            nsInfo.Alias.Type.Should().BeEquivalentTo(AliasType.MOSAIC_ID);
            nsInfo.Alias.MosaicId.HexId.Should().BeEquivalentTo(mosaicInfo.MosaicId.HexId);

            #endregion Link namespace to the mosaic

            #region Send mosaic using namespace alias to recipient

            var newAccount = Account.GenerateNewAccount(Fixture.NetworkType);

            var transferTransaction = TransferTransaction.Create(
                Deadline.Create(),
                Recipient.From(newAccount.Address),
                new List<Mosaic>()
                {
                    new Mosaic(nsInfo.Id, 10)
                },
                PlainMessage.Create("Send some mosaic to new address"),
                Fixture.NetworkType);

            var tx2 = Fixture.SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(newAccount.Address).Take(1)
                .Timeout(TimeSpan.FromSeconds(3000));

            var nsSignedTransferTransaction = account.Sign(transferTransaction, Fixture.GenerationHash);

            Fixture.WatchForFailure(nsSignedTransferTransaction);

            await Fixture.SiriusClient.TransactionHttp.Announce(nsSignedTransferTransaction);

            var result2 = await tx2;

            Log.WriteLine($"Request confirmed with transaction {result2.TransactionInfo.Hash}");

            var newAccountInfo = await Fixture.SiriusClient.AccountHttp.GetAccountInfo(newAccount.Address);

            Log.WriteLine($"Account {newAccountInfo.Address.Plain} with mosaic {newAccountInfo.Mosaics[0]} after transfer to the namespace alias");

            var expectedMosaicAmount = Convert.ToUInt64(10);
            newAccountInfo.Mosaics[0].Id.Id.Should().Be(mosaicInfo.MosaicId.Id);
            newAccountInfo.Mosaics[0]?.Amount.Should().Be(expectedMosaicAmount);

            #endregion Send mosaic using namespace alias to recipient
        }

        #region MosaicLevy

        [Fact]
        public async Task Should_Create_Levy_WithAbsoluteFee()
        {
            var account = Fixture.SeedAccount;
            var recipient = Account.GenerateNewAccount(Fixture.NetworkType);

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
                    duration: 0
                ),
                Fixture.NetworkType);
            Log.WriteLine($"Going to create mosaic {mosaicDefinitionTransaction.MosaicId.HexId}");
            Log.WriteLine($"{account.PublicAccount.PublicKey}");
            Log.WriteLine($"{account.PublicAccount.Address}");

            var mosaicSupplyChangeTransaction = MosaicSupplyChangeTransaction.Create(
              Deadline.Create(),
              mosaicDefinitionTransaction.MosaicId,
              MosaicSupplyType.INCREASE,
              1000000,
              Fixture.NetworkType);

            var aggregateTransaction = AggregateTransaction.CreateComplete(
               Deadline.Create(),
               new List<Transaction>
               {
                       mosaicDefinitionTransaction.ToAggregate(account.PublicAccount),
                       mosaicSupplyChangeTransaction.ToAggregate(account.PublicAccount)
               },
               Fixture.NetworkType);

            var signedTransaction = account.Sign(aggregateTransaction, Fixture.GenerationHash);

            Fixture.WatchForFailure(signedTransaction);

            Log.WriteLine($"Going to announce transaction {signedTransaction.Hash}");

            await Fixture.SiriusClient.TransactionHttp.Announce(signedTransaction);

            var mosaic_Id = new MosaicId(mosaicId.HexId);
            Log.WriteLine($"Mosaic Id: {mosaic_Id.HexId}");
            var mosaicLevy = ModifyMosaicLevyTransaction.Create(
                 Deadline.Create(),
                 mosaicDefinitionTransaction.MosaicId,
                 MosaicLevy.CreateWithAbsoluteFee(
                 Recipient.From(recipient.Address),
                 mosaicDefinitionTransaction.MosaicId, 200),
                 Fixture.NetworkType);
            var signedTransaction_mosaicLevy = account.Sign(mosaicLevy, Fixture.GenerationHash);

            Fixture.WatchForFailure(signedTransaction_mosaicLevy);
            await Fixture.SiriusClient.TransactionHttp.Announce(signedTransaction_mosaicLevy);

            var tx = Fixture.SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(account.Address).Take(1).Timeout(TimeSpan.FromSeconds(1000));
            var transaction = await tx;
            Log.WriteLine($"Request confirmed with transaction {transaction.TransactionInfo.Hash}");
        }

        [Fact]
        public async Task Should_Create_Levy_WithPercentageFee()
        {
            var account = Fixture.SeedAccount;
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
                    duration: 0
                ),
                Fixture.NetworkType);
            Log.WriteLine($"Going to create mosaic {mosaicDefinitionTransaction.MosaicId.HexId}");
            Log.WriteLine($"{account.PublicAccount.PublicKey}");
            Log.WriteLine($"{account.PublicAccount.Address}");

            var mosaicSupplyChangeTransaction = MosaicSupplyChangeTransaction.Create(
              Deadline.Create(),
              mosaicDefinitionTransaction.MosaicId,
              MosaicSupplyType.INCREASE,
              1000000,
              Fixture.NetworkType);

            var aggregateTransaction = AggregateTransaction.CreateComplete(
               Deadline.Create(),
               new List<Transaction>
               {
                       mosaicDefinitionTransaction.ToAggregate(account.PublicAccount),
                       mosaicSupplyChangeTransaction.ToAggregate(account.PublicAccount)
               },
               Fixture.NetworkType);

            var signedTransaction = account.Sign(aggregateTransaction, Fixture.GenerationHash);

            Fixture.WatchForFailure(signedTransaction);

            Log.WriteLine($"Going to announce transaction {signedTransaction.Hash}");

            await Fixture.SiriusClient.TransactionHttp.Announce(signedTransaction);

            var recipient = Account.GenerateNewAccount(Fixture.NetworkType);

            var mosaic_Id = new MosaicId(mosaicId.HexId);
            Log.WriteLine($"Mosaic Id: {mosaic_Id.HexId}");
            var mosaicLevy = ModifyMosaicLevyTransaction.Create(
                 Deadline.Create(),
                 mosaicDefinitionTransaction.MosaicId,
                 MosaicLevy.CreateWithPercentageFee(
                 Recipient.From(recipient.Address),
                 mosaicDefinitionTransaction.MosaicId, 200),
                 Fixture.NetworkType);
            var signedTransaction_mosaicLevy = account.Sign(mosaicLevy, Fixture.GenerationHash);
            Fixture.WatchForFailure(signedTransaction_mosaicLevy);
            var tx_Levy = Fixture.SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(account.Address).Take(1).Timeout(TimeSpan.FromSeconds(3000));
            await Fixture.SiriusClient.TransactionHttp.Announce(signedTransaction_mosaicLevy);
            var result_levy = await tx_Levy;
            Log.WriteLine($"Request confirmed with transaction {result_levy.TransactionInfo.Hash}");
        }

        #endregion MosaicLevy

        internal void WatchForNewBlock()
        {
            Fixture.SiriusWebSocketClient.Listener.NewBlock()
                .Timeout(TimeSpan.FromSeconds(3000))
                .Subscribe(
                    block =>
                    {
                        Log.WriteLine($"New block is created {block.Height}");
                    },
                    err =>
                    {
                        Log.WriteLine($"Transaction error - {err}");
                        Fixture.SiriusWebSocketClient.Listener.Close();
                    });
        }
    }
}