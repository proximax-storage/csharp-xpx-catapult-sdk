using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using ProximaX.Sirius.Chain.Sdk.Infrastructure;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Metadata;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Namespaces;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using Xunit;
using Xunit.Abstractions;

namespace ProximaX.Sirius.Chain.Sdk.Tests.E2E
{
    public class E2EMetadataTests : IClassFixture<E2EBaseFixture>
    {
        private readonly E2EBaseFixture Fixture;
        private readonly ITestOutputHelper Log;

        public E2EMetadataTests(E2EBaseFixture fixture, ITestOutputHelper log)
        {
            Fixture = fixture;
            Log = log;
        }

        [Fact]
        public async Task Should_Add_Metadata_To_Account()
        {
            var account = await Fixture.GenerateAccountWithCurrency(10000);
            Log.WriteLine($"Company Account {account.Address.Plain} \r\n Private Key: {account.PrivateKey} \r\n Public Key {account.PublicKey}");

            // await Fixture.SiriusWebSocketClient.Listener.Open();
            var metadatatrx = AccountMetadataTransaction.Create(Deadline.Create(), account.PublicAccount, "test", "test", "", Fixture.NetworkType);
            var aggregateTransaction = AggregateTransaction.CreateBonded(
               Deadline.Create(),
               new List<Transaction>
               {
                   metadatatrx.ToAggregate(account.PublicAccount),
               },
               Fixture.NetworkType);

            var aggregateTransactionsigned = account.Sign(aggregateTransaction, Fixture.GenerationHash);

            var hashLockTransaction = HashLockTransaction.Create(
                Deadline.Create(),
                //new Mosaic(new MosaicId("0BB1469B94E6FEF8"), 10),
                NetworkCurrencyMosaic.CreateRelative(10),
                (ulong)3650,
                aggregateTransactionsigned,
                Fixture.NetworkType);

            var hashLockTransactionSigned = account.Sign(hashLockTransaction, Fixture.GenerationHash);

            Log.WriteLine($"Going to announce hashlock transaction: {hashLockTransactionSigned.Hash}");
            await Fixture.SiriusClient.TransactionHttp.Announce(hashLockTransactionSigned);
            var hashLocktx = Fixture.SiriusWebSocketClient.Listener
                     .ConfirmedTransactionsGiven(account.Address).Take(1)
                     .Timeout(TimeSpan.FromSeconds(500));

            Fixture.WatchForFailure(hashLockTransactionSigned);
            var hashLockConfirmed = await hashLocktx;

            Log.WriteLine($"Request confirmed with hash lock transaction {hashLockConfirmed.TransactionInfo.Hash}");
            Thread.Sleep(4000);

            Log.WriteLine($"Going to announce aggregate transaction {aggregateTransactionsigned.Hash}");

            await Fixture.SiriusClient.TransactionHttp.AnnounceAggregateBonded(aggregateTransactionsigned);
            Fixture.WatchForFailure(aggregateTransactionsigned);
            var aggregateTransactiontx = Fixture.SiriusWebSocketClient.Listener
                    .ConfirmedTransactionsGiven(account.Address).Take(1)
                    .Timeout(TimeSpan.FromSeconds(500));

            //var aggregateTransactionConfirmed = await aggregateTransactiontx;
            //Log.WriteLine($" Request confirmed with aggregate transaction {aggregateTransactionConfirmed.TransactionInfo.Hash}");
        }

        [Fact]
        public async Task Should_Add_Metadata_To_Mosaic()
        {
            var account = await Fixture.GenerateAccountWithCurrency(10000);
            Log.WriteLine($"Company Account {account.Address.Plain} \r\n Private Key: {account.PrivateKey} \r\n Public Key {account.PublicKey}");
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
                   duration: 0
               ),
               Fixture.NetworkType);

            Log.WriteLine($"Going to create mosaic {mosaicDefinitionTransaction.MosaicId.HexId}");

            var mosaicSupplyChangeTransaction = MosaicSupplyChangeTransaction.Create(
              Deadline.Create(),
              mosaicDefinitionTransaction.MosaicId,
              MosaicSupplyType.INCREASE,
              1000,
               Fixture.NetworkType);
            var aggregateTransactiontx = AggregateTransaction.CreateComplete(
            Deadline.Create(),
            new List<Transaction>
            {
                mosaicDefinitionTransaction.ToAggregate(account.PublicAccount),
                mosaicSupplyChangeTransaction.ToAggregate(account.PublicAccount)
            },
            Fixture.NetworkType);
            var aggregateTransactionsigned = account.Sign(aggregateTransactiontx, Fixture.GenerationHash);
            await Fixture.SiriusWebSocketClient.Listener.Open();

            Log.WriteLine($"Going to announce aggregate transaction: {aggregateTransactionsigned.Hash}");
            await Fixture.SiriusClient.TransactionHttp.Announce(aggregateTransactionsigned);

            var metadatatrx = MosaicMetadataTransaction.Create(
                Deadline.Create(),
                account.PublicAccount,
                mosaicDefinitionTransaction.MosaicId,
                "test_mosaic",
                "1",
                "",
                Fixture.NetworkType);
            var aggregateTransaction = AggregateTransaction.CreateBonded(
               Deadline.Create(),
               new List<Transaction>
               {
                   metadatatrx.ToAggregate(account.PublicAccount),
               },
               Fixture.NetworkType);

            var aggregateTrxsigned = account.Sign(aggregateTransaction, Fixture.GenerationHash);

            Fixture.WatchForFailure(aggregateTransactionsigned);

            var hashLockTransaction = HashLockTransaction.Create(
                Deadline.Create(),
                //new Mosaic(new MosaicId("0BB1469B94E6FEF8"), 10),
                NetworkCurrencyMosaic.CreateRelative(10),
                (ulong)3650,
                aggregateTrxsigned,
                Fixture.NetworkType);

            var hashLockTransactionSigned = account.Sign(hashLockTransaction, Fixture.GenerationHash);

            Log.WriteLine($"Going to announce hashlock transaction: {hashLockTransactionSigned.Hash}");
            await Fixture.SiriusClient.TransactionHttp.Announce(hashLockTransactionSigned);
            var hashLocktx = Fixture.SiriusWebSocketClient.Listener
                    .ConfirmedTransactionsGiven(account.Address).Take(1)
                    .Timeout(TimeSpan.FromSeconds(500));

            Fixture.WatchForFailure(hashLockTransactionSigned);
            var hashLockConfirmed = await hashLocktx;

            Log.WriteLine($"Request confirmed with hash lock transaction {hashLockConfirmed.TransactionInfo.Hash}");

            Log.WriteLine($"Going to announce aggregate transaction {aggregateTransactionsigned.Hash}");

            await Fixture.SiriusClient.TransactionHttp.AnnounceAggregateBonded(aggregateTransactionsigned);
            Fixture.WatchForFailure(aggregateTransactionsigned);
            var aggregateTrntx = Fixture.SiriusWebSocketClient.Listener
                    .ConfirmedTransactionsGiven(account.Address).Take(1)
                    .Timeout(TimeSpan.FromSeconds(500));

            var MosaicMetadataDetail = await Fixture.SiriusClient.MetadataHttp.SearchMetadata(new MetadataQueryParams(1, Order.ASC, 1, Address.CreateFromRawAddress(account.Address.ToString()), null, null, null));
            Log.WriteLine($"Metadata mosaic {MosaicMetadataDetail}");
        }

        [Fact]
        public async Task Should_Add_Metadata_To_Namespace()
        {
            var account = await Fixture.GenerateAccountWithCurrency(10000);
            Log.WriteLine($"Company Account {account.Address.Plain} \r\n Private Key: {account.PrivateKey} \r\n Public Key {account.PublicKey}");
            var rootNs = "namespace_metadata_test";
            var rootId = new NamespaceId(rootNs);
            var registerRootTransaction = RegisterNamespaceTransaction.CreateRootNamespace(
                 Deadline.Create(),
                 rootNs,
                 3650,
                 Fixture.NetworkType);

            var aggregateTransactiontx = AggregateTransaction.CreateComplete(
            Deadline.Create(),
            new List<Transaction>
            {
                registerRootTransaction.ToAggregate(account.PublicAccount),
            },
            Fixture.NetworkType);
            var aggregateTransactionsigned = account.Sign(aggregateTransactiontx, Fixture.GenerationHash);
            await Fixture.SiriusWebSocketClient.Listener.Open();

            Log.WriteLine($"Going to announce aggregate transaction: {aggregateTransactionsigned.Hash}");
            await Fixture.SiriusClient.TransactionHttp.Announce(aggregateTransactionsigned);
            /*var aggregateTransactiontxs = Fixture.SiriusWebSocketClient.Listener
                    .ConfirmedTransactionsGiven(account.Address).Take(1)
                    .Timeout(TimeSpan.FromSeconds(500));

             var aggregateTransactionConfirmed = await aggregateTransactiontxs;
             Log.WriteLine($" Request confirmed with aggregate transaction {aggregateTransactionConfirmed.TransactionInfo.Hash}");*/

            var metadatatrx = NamespaceMetadataTransaction.Create(
                Deadline.Create(),
                account.PublicAccount,
                 rootId,
                "test_Namespace",
                "1",
                "",
                Fixture.NetworkType);
            var aggregateTransaction = AggregateTransaction.CreateBonded(
               Deadline.Create(),
               new List<Transaction>
               {
                   metadatatrx.ToAggregate(account.PublicAccount),
               },
               Fixture.NetworkType);

            var aggregateTrxsigned = account.Sign(aggregateTransaction, Fixture.GenerationHash);

            Fixture.WatchForFailure(aggregateTrxsigned);
            /* Log.WriteLine($" {aggregateTransactionConfirmed.TransactionType}Request confirmed with aggregate transaction {aggregateTransactionConfirmed.TransactionInfo.Hash}");*/
            var hashLockTransaction = HashLockTransaction.Create(
                Deadline.Create(),
                //new Mosaic(new MosaicId("0BB1469B94E6FEF8"), 10),
                NetworkCurrencyMosaic.CreateRelative(10),
                (ulong)3650,
                aggregateTrxsigned,
                Fixture.NetworkType);

            var hashLockTransactionSigned = account.Sign(hashLockTransaction, Fixture.GenerationHash);

            Log.WriteLine($"Going to announce hashlock transaction: {hashLockTransactionSigned.Hash}");
            await Fixture.SiriusClient.TransactionHttp.Announce(hashLockTransactionSigned);
            var hashLocktx = Fixture.SiriusWebSocketClient.Listener
                    .ConfirmedTransactionsGiven(account.Address).Take(1)
                    .Timeout(TimeSpan.FromSeconds(500));

            Fixture.WatchForFailure(hashLockTransactionSigned);
            var hashLockConfirmed = await hashLocktx;

            Log.WriteLine($"Request confirmed with hash lock transaction {hashLockConfirmed.TransactionInfo.Hash}");

            Log.WriteLine($"Going to announce aggregate transaction {aggregateTrxsigned.Hash}");

            await Fixture.SiriusClient.TransactionHttp.AnnounceAggregateBonded(aggregateTrxsigned);
            Fixture.WatchForFailure(aggregateTrxsigned);
            var aggregateTrntx = Fixture.SiriusWebSocketClient.Listener
                   .ConfirmedTransactionsGiven(account.Address).Take(1)
                   .Timeout(TimeSpan.FromSeconds(500));

            var aggregateTrxConfirmed = await aggregateTrntx;
            Log.WriteLine($"Request confirmed with aggregate transaction {aggregateTrxConfirmed.TransactionInfo.Hash}");
        }
    }
}