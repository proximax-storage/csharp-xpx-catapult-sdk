using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
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
        readonly E2EBaseFixture Fixture;
        readonly ITestOutputHelper Log;

        public E2EMetadataTests(E2EBaseFixture fixture, ITestOutputHelper log)
        {
            Fixture = fixture;
            Log = log;
        }


        [Fact]
        public async Task Should_Add_Metadata_To_Address()
        {
          
            var account = Account.GenerateNewAccount(Fixture.NetworkType);

            await Fixture.SiriusWebSocketClient.Listener.Open();

            var tx = Fixture.SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(account.Address).Take(1)
                .Timeout(TimeSpan.FromSeconds(1000));

            Log.WriteLine($"Generated account {account}");

            var modifications = new List<MetadataModification>
            {
                MetadataModification.Add("productId", "123"),
                MetadataModification.Add("productName", "TestProduct")
            };

            var modifyMetadataTransaction = ModifyMetadataTransaction.CreateForAddress(
                Deadline.Create(),
                account.Address,
                modifications,
                Fixture.NetworkType);

            var signedTransaction = account.Sign(modifyMetadataTransaction,Fixture.GenerationHash);

            Log.WriteLine($"Going to announce transaction {signedTransaction.Hash}");

          

            await Fixture.SiriusClient.TransactionHttp.Announce(signedTransaction);

            var result = await tx;
            Log.WriteLine($"Request confirmed with transaction {result.TransactionInfo.Hash}");


            var metaInfo = await Fixture.SiriusClient.MetadataHttp.GetMetadataFromAddress(account.Address);
            metaInfo.Fields.Should().HaveCount(2);
            metaInfo.Type.Should().BeEquivalentTo(MetadataType.ADDRESS);
        }


        [Fact]
        public async Task Should_Remove_Metadata_From_Address()
        {
            var account = Account.GenerateNewAccount(Fixture.NetworkType);

            await Fixture.SiriusWebSocketClient.Listener.Open();

            var tx = Fixture.SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(account.Address).Take(1).Timeout(TimeSpan.FromSeconds(500));

            Log.WriteLine($"Generated account {account}");

            var modifications = new List<MetadataModification>
            {
                MetadataModification.Add("productId", "123"),
                MetadataModification.Add("productName", "TestProduct")
            };

            var modifyMetadataTransaction = ModifyMetadataTransaction.CreateForAddress(
                Deadline.Create(),
                account.Address,
                modifications,
                Fixture.NetworkType);

            var signedTransaction = account.Sign(modifyMetadataTransaction,Fixture.GenerationHash);

            Log.WriteLine($"Going to announce transaction {signedTransaction.Hash}");


            await Fixture.SiriusClient.TransactionHttp.Announce(signedTransaction);

            var result = await tx;

            Log.WriteLine($"Request confirmed with transaction {result.TransactionInfo.Hash}");


            var metaInfo = await Fixture.SiriusClient.MetadataHttp.GetMetadataFromAddress(account.Address);
            metaInfo.Fields.Should().HaveCount(2);
            metaInfo.Type.Should().BeEquivalentTo(MetadataType.ADDRESS);

            modifications = new List<MetadataModification>
            {
                MetadataModification.Remove("productName")
            };

            modifyMetadataTransaction = ModifyMetadataTransaction.CreateForAddress(
                Deadline.Create(),
                account.Address,
                modifications,
                Fixture.NetworkType);

            signedTransaction = account.Sign(modifyMetadataTransaction,Fixture.GenerationHash);
            Log.WriteLine($"Going to announce transaction {signedTransaction.Hash}");


            await Fixture.SiriusClient.TransactionHttp.Announce(signedTransaction);

            result = await tx;

            Log.WriteLine($"Request confirmed with transaction {result.TransactionInfo.Hash}");

            metaInfo = await Fixture.SiriusClient.MetadataHttp.GetMetadataFromAddress(account.Address);
            metaInfo.Fields.Should().HaveCount(1);
            metaInfo.Type.Should().BeEquivalentTo(MetadataType.ADDRESS);

        }

        [Fact]
        public async Task Should_Add_Metadata_To_Namespace()
        {
            var namespaceName = "nsp" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);
            var namespaceId = new NamespaceId(namespaceName);
            var account = Fixture.SeedAccount;

            await Fixture.SiriusWebSocketClient.Listener.Open();

            var tx = Fixture.SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(account.Address).Take(1).Timeout(TimeSpan.FromSeconds(1000));


            Log.WriteLine($"Going to generate namespace {namespaceId}");

            var registerRootTransaction = RegisterNamespaceTransaction.CreateRootNamespace(
                Deadline.Create(),
                namespaceName,
                100,
                Fixture.NetworkType
            );

            var signedTransaction = account.Sign(registerRootTransaction,Fixture.GenerationHash);

            await Fixture.SiriusClient.TransactionHttp.Announce(signedTransaction);


            var result = await tx;

            Log.WriteLine($"Request register namespace confirmed with transaction {result.TransactionInfo.Hash}");


            var modifications = new List<MetadataModification>
            {
                MetadataModification.Add("company", "ProximaX"),
                MetadataModification.Add("department", "IT")
            };

            var modifyMetadataTransaction = ModifyMetadataTransaction.CreateForNamespace(
                Deadline.Create(),
                namespaceId,
                modifications,
                Fixture.NetworkType);

            signedTransaction = account.Sign(modifyMetadataTransaction,Fixture.GenerationHash);

            await Fixture.SiriusClient.TransactionHttp.Announce(signedTransaction);

            result = await tx;
            Log.WriteLine($"Request add metadata to namespace confirmed with transaction {result.TransactionInfo.Hash}");


            var metaInfo = await Fixture.SiriusClient.MetadataHttp.GetMetadataFromNamespace(namespaceId);
            metaInfo.Fields.Should().HaveCount(2);
            metaInfo.Type.Should().BeEquivalentTo(MetadataType.NAMESPACE);
        }

        [Fact]
        public async Task Should_Remove_Metadata_From_Namespace()
        {
            var namespaceName = "nsp" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);
            var namespaceId = new NamespaceId(namespaceName);
            var account =  Fixture.SeedAccount;
            await Fixture.SiriusWebSocketClient.Listener.Open();

            var tx = Fixture.SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(account.Address).Take(1).Timeout(TimeSpan.FromSeconds(500));


            Log.WriteLine($"Going to generate namespace {namespaceId}");

            var registerRootTransaction = RegisterNamespaceTransaction.CreateRootNamespace(
                Deadline.Create(),
                namespaceName,
                100,
                Fixture.NetworkType
            );

            var signedTransaction = account.Sign(registerRootTransaction,Fixture.GenerationHash);

            await Fixture.SiriusClient.TransactionHttp.Announce(signedTransaction);


            var result = await tx;

            Log.WriteLine($"Request register namespace confirmed with transaction {result.TransactionInfo.Hash}");


            var modifications = new List<MetadataModification>
            {
                MetadataModification.Add("company", "ProximaX"),
                MetadataModification.Add("department", "IT")
            };

            var modifyMetadataTransaction = ModifyMetadataTransaction.CreateForNamespace(
                Deadline.Create(),
                namespaceId,
                modifications,
                Fixture.NetworkType);

            signedTransaction = account.Sign(modifyMetadataTransaction,Fixture.GenerationHash);

            await Fixture.SiriusClient.TransactionHttp.Announce(signedTransaction);

            result = await tx;
            Log.WriteLine($"Request add metadata to namespace confirmed with transaction {result.TransactionInfo.Hash}");

            modifications = new List<MetadataModification>
            {
                MetadataModification.Remove("department")
            };

            modifyMetadataTransaction = ModifyMetadataTransaction.CreateForNamespace(
               Deadline.Create(),
               namespaceId,
               modifications,
               Fixture.NetworkType);

            signedTransaction = account.Sign(modifyMetadataTransaction,Fixture.GenerationHash);

            await Fixture.SiriusClient.TransactionHttp.Announce(signedTransaction);
            result = await tx;
            Log.WriteLine($"Request remove metadata from namespace confirmed with transaction {result.TransactionInfo.Hash}");

            var metaInfo = await Fixture.SiriusClient.MetadataHttp.GetMetadataFromNamespace(namespaceId);
            metaInfo.Fields.Should().HaveCount(1);
            metaInfo.Type.Should().BeEquivalentTo(MetadataType.NAMESPACE);
        }

        [Fact]
        public async Task Should_Add_Metadata_To_Mosaic()
        {
           var account =  Fixture.SeedAccount;

            await Fixture.SiriusWebSocketClient.Listener.Open();
            var tx = Fixture.SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(account.Address).Take(1).Timeout(TimeSpan.FromSeconds(500));

            var nonce = MosaicNonce.CreateRandom();
            var mosaicId = MosaicId.CreateFromNonce(nonce, account.PublicAccount.PublicKey);

            Log.WriteLine($"Going to generate mosaicId {mosaicId}");

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

       
            var signedTransaction = account.Sign(mosaicDefinitionTransaction,Fixture.GenerationHash);

            await Fixture.SiriusClient.TransactionHttp.Announce(signedTransaction);


            var result = await tx;

            Log.WriteLine($"Request create mosaic confirmed with transaction {result.TransactionInfo.Hash}");

            var modifications = new List<MetadataModification>
            {
                MetadataModification.Add("company", "ProximaX"),
                MetadataModification.Add("department", "IT")
            };

            var modifyMetadataTransaction = ModifyMetadataTransaction.CreateForMosaic(
                Deadline.Create(),
                mosaicId,
                modifications,
                Fixture.NetworkType);

            signedTransaction = account.Sign(modifyMetadataTransaction,Fixture.GenerationHash);

            await Fixture.SiriusClient.TransactionHttp.Announce(signedTransaction);

            result = await tx;
            Log.WriteLine($"Request add metadata to mosaic confirmed with transaction {result.TransactionInfo.Hash}");


            var metaInfo = await Fixture.SiriusClient.MetadataHttp.GetMetadataFromMosaic(mosaicId);
            metaInfo.Fields.Should().HaveCount(2);
            metaInfo.Type.Should().BeEquivalentTo(MetadataType.MOSAIC);
        }

        [Fact]
        public async Task Should_Remove_Metadata_From_Mosaic()
        {
            var account =  Fixture.SeedAccount;

            await Fixture.SiriusWebSocketClient.Listener.Open();
            var tx = Fixture.SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(account.Address).Take(1).Timeout(TimeSpan.FromSeconds(500));

            var nonce = MosaicNonce.CreateRandom();
            var mosaicId = MosaicId.CreateFromNonce(nonce, account.PublicAccount.PublicKey);

            Log.WriteLine($"Going to generate mosaicId {mosaicId}");

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


            var signedTransaction = account.Sign(mosaicDefinitionTransaction,Fixture.GenerationHash);

            await Fixture.SiriusClient.TransactionHttp.Announce(signedTransaction);


            var result = await tx;

            Log.WriteLine($"Request create mosaic confirmed with transaction {result.TransactionInfo.Hash}");

            var modifications = new List<MetadataModification>
            {
                MetadataModification.Add("company", "ProximaX"),
                MetadataModification.Add("department", "IT")
            };

            var modifyMetadataTransaction = ModifyMetadataTransaction.CreateForMosaic(
                Deadline.Create(),
                mosaicId,
                modifications,
                Fixture.NetworkType);

            signedTransaction = account.Sign(modifyMetadataTransaction,Fixture.GenerationHash);

            await Fixture.SiriusClient.TransactionHttp.Announce(signedTransaction);

            result = await tx;
            Log.WriteLine($"Request add metadata to mosaic confirmed with transaction {result.TransactionInfo.Hash}");

             modifications = new List<MetadataModification>
            {
               
                MetadataModification.Remove("department")
            };

             modifyMetadataTransaction = ModifyMetadataTransaction.CreateForMosaic(
                Deadline.Create(),
                mosaicId,
                modifications,
                Fixture.NetworkType);

            signedTransaction = account.Sign(modifyMetadataTransaction,Fixture.GenerationHash);

            await Fixture.SiriusClient.TransactionHttp.Announce(signedTransaction);

            result = await tx;
            Log.WriteLine($"Request remove metadata from mosaic confirmed with transaction {result.TransactionInfo.Hash}");


            var metaInfo = await Fixture.SiriusClient.MetadataHttp.GetMetadataFromMosaic(mosaicId);
            metaInfo.Fields.Should().HaveCount(1);
            metaInfo.Type.Should().BeEquivalentTo(MetadataType.MOSAIC);
        }

     
    }
}
