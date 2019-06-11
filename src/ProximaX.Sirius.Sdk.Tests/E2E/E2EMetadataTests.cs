using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using ProximaX.Sirius.Sdk.Model.Accounts;
using ProximaX.Sirius.Sdk.Model.Metadata;
using ProximaX.Sirius.Sdk.Model.Mosaics;
using ProximaX.Sirius.Sdk.Model.Namespaces;
using ProximaX.Sirius.Sdk.Model.Transactions;
using Xunit;
using Xunit.Abstractions;

namespace ProximaX.Sirius.Sdk.Tests.E2E
{
    [Collection("E2ETestFixtureCollection")]
    public class E2EMetadataTests
    {
        private readonly E2ETestFixture _fixture;

        private readonly ITestOutputHelper _output;

        public E2EMetadataTests(E2ETestFixture fixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            _output = output;
        }

        [Fact]
        public async Task Should_Add_Metadata_To_Address()
        {
            var networkType = _fixture.Client.NetworkHttp.GetNetworkType().Wait();
            var account = Account.GenerateNewAccount(networkType);

            await _fixture.WebSocket.Listener.Open();

            var tx = _fixture.WebSocket.Listener.ConfirmedTransactionsGiven(account.Address).Take(1)
                .Timeout(TimeSpan.FromSeconds(1000));

            _output.WriteLine($"Generated account {account}");

            var modifications = new List<MetadataModification>
            {
                MetadataModification.Add("productId", "123"),
                MetadataModification.Add("productName", "TestProduct")
            };

            var modifyMetadataTransaction = ModifyMetadataTransaction.CreateForAddress(
                Deadline.Create(),
                account.Address,
                modifications,
                networkType);

            var signedTransaction = account.Sign(modifyMetadataTransaction);

            _output.WriteLine($"Going to announce transaction {signedTransaction.Hash}");

          

            await _fixture.Client.TransactionHttp.Announce(signedTransaction);

            var result = await tx;
            _output.WriteLine($"Request confirmed with transaction {result.TransactionInfo.Hash}");


            var metaInfo = await _fixture.Client.MetadataHttp.GetMetadataFromAddress(account.Address);
            metaInfo.Fields.Should().HaveCount(2);
            metaInfo.Type.Should().BeEquivalentTo(MetadataType.ADDRESS);
        }


        [Fact]
        public async Task Should_Remove_Metadata_From_Address()
        {
            var networkType = _fixture.Client.NetworkHttp.GetNetworkType().Wait();
            var account = Account.GenerateNewAccount(networkType);

            await _fixture.WebSocket.Listener.Open();

            var tx = _fixture.WebSocket.Listener.ConfirmedTransactionsGiven(account.Address).Take(1).Timeout(TimeSpan.FromSeconds(500));

            _output.WriteLine($"Generated account {account}");

            var modifications = new List<MetadataModification>
            {
                MetadataModification.Add("productId", "123"),
                MetadataModification.Add("productName", "TestProduct")
            };

            var modifyMetadataTransaction = ModifyMetadataTransaction.CreateForAddress(
                Deadline.Create(),
                account.Address,
                modifications,
                networkType);

            var signedTransaction = account.Sign(modifyMetadataTransaction);

            _output.WriteLine($"Going to announce transaction {signedTransaction.Hash}");


            await _fixture.Client.TransactionHttp.Announce(signedTransaction);

            var result = await tx;

            _output.WriteLine($"Request confirmed with transaction {result.TransactionInfo.Hash}");


            var metaInfo = await _fixture.Client.MetadataHttp.GetMetadataFromAddress(account.Address);
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
                networkType);

            signedTransaction = account.Sign(modifyMetadataTransaction);
            _output.WriteLine($"Going to announce transaction {signedTransaction.Hash}");


            await _fixture.Client.TransactionHttp.Announce(signedTransaction);

            result = await tx;

            _output.WriteLine($"Request confirmed with transaction {result.TransactionInfo.Hash}");

            metaInfo = await _fixture.Client.MetadataHttp.GetMetadataFromAddress(account.Address);
            metaInfo.Fields.Should().HaveCount(1);
            metaInfo.Type.Should().BeEquivalentTo(MetadataType.ADDRESS);

        }

        [Fact]
        public async Task Should_Add_Metadata_To_Namespace()
        {
            var networkType = _fixture.Client.NetworkHttp.GetNetworkType().Wait();
            var namespaceName = "nsp" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);
            var namespaceId = new NamespaceId(namespaceName);
            var account = await _fixture.GetSeedAccount();

            await _fixture.WebSocket.Listener.Open();

            var tx = _fixture.WebSocket.Listener.ConfirmedTransactionsGiven(account.Address).Take(1).Timeout(TimeSpan.FromSeconds(1000));


            _output.WriteLine($"Going to generate namespace {namespaceId}");

            var registerRootTransaction = RegisterNamespaceTransaction.CreateRootNamespace(
                Deadline.Create(),
                namespaceName,
                100,
                networkType
            );

            var signedTransaction = account.Sign(registerRootTransaction);

            await _fixture.Client.TransactionHttp.Announce(signedTransaction);


            var result = await tx;

            _output.WriteLine($"Request register namespace confirmed with transaction {result.TransactionInfo.Hash}");


            var modifications = new List<MetadataModification>
            {
                MetadataModification.Add("company", "ProximaX"),
                MetadataModification.Add("department", "IT")
            };

            var modifyMetadataTransaction = ModifyMetadataTransaction.CreateForNamespace(
                Deadline.Create(),
                namespaceId,
                modifications,
                networkType);

            signedTransaction = account.Sign(modifyMetadataTransaction);

            await _fixture.Client.TransactionHttp.Announce(signedTransaction);

            result = await tx;
            _output.WriteLine($"Request add metadata to namespace confirmed with transaction {result.TransactionInfo.Hash}");


            var metaInfo = await _fixture.Client.MetadataHttp.GetMetadataFromNamespace(namespaceId);
            metaInfo.Fields.Should().HaveCount(2);
            metaInfo.Type.Should().BeEquivalentTo(MetadataType.NAMESPACE);
        }

        [Fact]
        public async Task Should_Remove_Metadata_From_Namespace()
        {
            var networkType = _fixture.Client.NetworkHttp.GetNetworkType().Wait();
            var namespaceName = "nsp" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);
            var namespaceId = new NamespaceId(namespaceName);
            var account = await _fixture.GetSeedAccount();
            await _fixture.WebSocket.Listener.Open();

            var tx = _fixture.WebSocket.Listener.ConfirmedTransactionsGiven(account.Address).Take(1).Timeout(TimeSpan.FromSeconds(500));


            _output.WriteLine($"Going to generate namespace {namespaceId}");

            var registerRootTransaction = RegisterNamespaceTransaction.CreateRootNamespace(
                Deadline.Create(),
                namespaceName,
                100,
                networkType
            );

            var signedTransaction = account.Sign(registerRootTransaction);

            await _fixture.Client.TransactionHttp.Announce(signedTransaction);


            var result = await tx;

            _output.WriteLine($"Request register namespace confirmed with transaction {result.TransactionInfo.Hash}");


            var modifications = new List<MetadataModification>
            {
                MetadataModification.Add("company", "ProximaX"),
                MetadataModification.Add("department", "IT")
            };

            var modifyMetadataTransaction = ModifyMetadataTransaction.CreateForNamespace(
                Deadline.Create(),
                namespaceId,
                modifications,
                networkType);

            signedTransaction = account.Sign(modifyMetadataTransaction);

            await _fixture.Client.TransactionHttp.Announce(signedTransaction);

            result = await tx;
            _output.WriteLine($"Request add metadata to namespace confirmed with transaction {result.TransactionInfo.Hash}");

            modifications = new List<MetadataModification>
            {
                MetadataModification.Remove("department")
            };

            modifyMetadataTransaction = ModifyMetadataTransaction.CreateForNamespace(
               Deadline.Create(),
               namespaceId,
               modifications,
               networkType);

            signedTransaction = account.Sign(modifyMetadataTransaction);

            await _fixture.Client.TransactionHttp.Announce(signedTransaction);
            result = await tx;
            _output.WriteLine($"Request remove metadata from namespace confirmed with transaction {result.TransactionInfo.Hash}");

            var metaInfo = await _fixture.Client.MetadataHttp.GetMetadataFromNamespace(namespaceId);
            metaInfo.Fields.Should().HaveCount(1);
            metaInfo.Type.Should().BeEquivalentTo(MetadataType.NAMESPACE);
        }

        [Fact]
        public async Task Should_Add_Metadata_To_Mosaic()
        {
            var networkType = _fixture.Client.NetworkHttp.GetNetworkType().Wait();
            var account = await _fixture.GetSeedAccount();

            await _fixture.WebSocket.Listener.Open();
            var tx = _fixture.WebSocket.Listener.ConfirmedTransactionsGiven(account.Address).Take(1).Timeout(TimeSpan.FromSeconds(500));

            var nonce = MosaicNonce.CreateRandom();
            var mosaicId = MosaicId.CreateFromNonce(nonce, account.PublicAccount.PublicKey);

            _output.WriteLine($"Going to generate mosaicId {mosaicId}");

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

       
            var signedTransaction = account.Sign(mosaicDefinitionTransaction);

            await _fixture.Client.TransactionHttp.Announce(signedTransaction);


            var result = await tx;

            _output.WriteLine($"Request create mosaic confirmed with transaction {result.TransactionInfo.Hash}");

            var modifications = new List<MetadataModification>
            {
                MetadataModification.Add("company", "ProximaX"),
                MetadataModification.Add("department", "IT")
            };

            var modifyMetadataTransaction = ModifyMetadataTransaction.CreateForMosaic(
                Deadline.Create(),
                mosaicId,
                modifications,
                networkType);

            signedTransaction = account.Sign(modifyMetadataTransaction);

            await _fixture.Client.TransactionHttp.Announce(signedTransaction);

            result = await tx;
            _output.WriteLine($"Request add metadata to mosaic confirmed with transaction {result.TransactionInfo.Hash}");


            var metaInfo = await _fixture.Client.MetadataHttp.GetMetadataFromMosaic(mosaicId);
            metaInfo.Fields.Should().HaveCount(2);
            metaInfo.Type.Should().BeEquivalentTo(MetadataType.MOSAIC);
        }

        [Fact]
        public async Task Should_Remove_Metadata_From_Mosaic()
        {
            var networkType = _fixture.Client.NetworkHttp.GetNetworkType().Wait();
            var account = await _fixture.GetSeedAccount();

            await _fixture.WebSocket.Listener.Open();
            var tx = _fixture.WebSocket.Listener.ConfirmedTransactionsGiven(account.Address).Take(1).Timeout(TimeSpan.FromSeconds(500));

            var nonce = MosaicNonce.CreateRandom();
            var mosaicId = MosaicId.CreateFromNonce(nonce, account.PublicAccount.PublicKey);

            _output.WriteLine($"Going to generate mosaicId {mosaicId}");

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


            var signedTransaction = account.Sign(mosaicDefinitionTransaction);

            await _fixture.Client.TransactionHttp.Announce(signedTransaction);


            var result = await tx;

            _output.WriteLine($"Request create mosaic confirmed with transaction {result.TransactionInfo.Hash}");

            var modifications = new List<MetadataModification>
            {
                MetadataModification.Add("company", "ProximaX"),
                MetadataModification.Add("department", "IT")
            };

            var modifyMetadataTransaction = ModifyMetadataTransaction.CreateForMosaic(
                Deadline.Create(),
                mosaicId,
                modifications,
                networkType);

            signedTransaction = account.Sign(modifyMetadataTransaction);

            await _fixture.Client.TransactionHttp.Announce(signedTransaction);

            result = await tx;
            _output.WriteLine($"Request add metadata to mosaic confirmed with transaction {result.TransactionInfo.Hash}");

             modifications = new List<MetadataModification>
            {
               
                MetadataModification.Remove("department")
            };

             modifyMetadataTransaction = ModifyMetadataTransaction.CreateForMosaic(
                Deadline.Create(),
                mosaicId,
                modifications,
                networkType);

            signedTransaction = account.Sign(modifyMetadataTransaction);

            await _fixture.Client.TransactionHttp.Announce(signedTransaction);

            result = await tx;
            _output.WriteLine($"Request remove metadata from mosaic confirmed with transaction {result.TransactionInfo.Hash}");


            var metaInfo = await _fixture.Client.MetadataHttp.GetMetadataFromMosaic(mosaicId);
            metaInfo.Fields.Should().HaveCount(1);
            metaInfo.Type.Should().BeEquivalentTo(MetadataType.MOSAIC);
        }

        
    }
}
