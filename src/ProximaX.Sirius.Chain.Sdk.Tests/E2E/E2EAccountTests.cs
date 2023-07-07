using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Newtonsoft.Json;
using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Fees;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Namespaces;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions.Builders;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions.Messages;
using ProximaX.Sirius.Chain.Sdk.Utils;
using Xunit;
using Xunit.Abstractions;

namespace ProximaX.Sirius.Chain.Sdk.Tests.E2E
{
    public class E2EAccountTests : IClassFixture<E2EBaseFixture>
    {
        private readonly E2EBaseFixture Fixture;
        private readonly ITestOutputHelper Log;

        public E2EAccountTests(E2EBaseFixture fixture, ITestOutputHelper log)
        {
            Fixture = fixture;
            Log = log;
        }

        [Fact]
        public async Task Should_Send_Some_Money_To_New_Account()
        {
            // var aliceAccount = await Fixture.GenerateAccountWithCurrency(100000);

            var aliceAccount = Account.CreateFromPrivateKeyV2("101a4dfcffe3d5abddee535c0c7fa7bb386b99eb7c19f0c73cdc3837c5844477", Fixture.NetworkType);

            var tx = Fixture.SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(aliceAccount.Address).Take(1);

            Log.WriteLine($"Alice Account {aliceAccount.Address.Plain} \r\n Private Key: {aliceAccount.PrivateKey} \r\n Public Key {aliceAccount.PublicKey}");

            const ulong amount = (ulong)1000;
            var mosaicToTransfer = NetworkCurrencyMosaic.CreateRelative(amount);

            var transferTransaction = TransferTransaction.Create(
                Deadline.Create(),
                aliceAccount.Address,
                new List<Mosaic>()
                {
                    mosaicToTransfer
                },
                PlainMessage.Create("transferTest"),
                Fixture.NetworkType);

            var signedTransaction = Fixture.SeedAccount.Sign(transferTransaction, Fixture.GenerationHash);
            Log.WriteLine($"Going to send {amount} XPP to {aliceAccount.Address.Pretty} with transaction {signedTransaction.Hash}");
            Fixture.WatchForFailure(signedTransaction);

            await Fixture.SiriusClient.TransactionHttp.Announce(signedTransaction);
            var result = await tx;

            Log.WriteLine($"Request confirmed with public key {result.Signer.PublicKey}");

            var aliceAccountInfo = await Fixture.SiriusClient.AccountHttp.GetAccountInfo(aliceAccount.Address);

            Log.WriteLine($"Alice Account Info: {aliceAccountInfo.Mosaics[0]}");

            aliceAccountInfo.Mosaics[0]?.Amount.Should().BeGreaterThan(0);

            var outgoingTxs = Fixture.SiriusClient.AccountHttp.OutgoingTransactions(PublicAccount.CreateFromPublicKey(result.Signer.PublicKey, Fixture.NetworkType)).Wait();
            outgoingTxs.Transactions.Count().Should().BeGreaterThan(0);
            Log.WriteLine($"Complete");
        }

        [Fact]
        public async Task Should_Add_Account_Filter_With_Block_Address()
        {
            var company = Account.GenerateNewAccount(Fixture.NetworkType);
            var blocked = Account.GenerateNewAccount(Fixture.NetworkType);

            var addressFilter = ModifyAccountPropertyTransaction<Address>.CreateForAddress(
                Deadline.Create(),
                (ulong)0,
                PropertyType.BLOCK_ADDRESS,
                new List<AccountPropertyModification<Address>>()
                {
                    new AccountPropertyModification<Address>(PropertyModificationType.ADD, blocked.Address)
                },
                Fixture.NetworkType);

            Log.WriteLine($"Going to filter the address {company.Address} ");

            await Fixture.SiriusWebSocketClient.Listener.Open();

            var tx = Fixture.SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(company.Address).Take(1)
                .Timeout(TimeSpan.FromSeconds(1000));

            var signedTransaction = addressFilter.SignWith(company, Fixture.GenerationHash);

            Log.WriteLine($"Going to announce transaction {signedTransaction.Hash}");

            await Fixture.SiriusClient.TransactionHttp.Announce(signedTransaction);

            var result = await tx;
            Log.WriteLine($"Request confirmed with transaction {result.TransactionInfo.Hash}");

            var accountProperties = await Fixture.SiriusClient.AccountHttp.GetAccountProperty(company.PublicAccount);
            accountProperties.Should().NotBeNull();
            var blockAddressProperty =
                accountProperties.AccountProperties.Properties.Single(ap =>
                    ap.PropertyType == PropertyType.BLOCK_ADDRESS);
            blockAddressProperty.Should().NotBeNull();
            var blockedAddress = blockAddressProperty.Values.Single(a => Address.CreateFromHex(a.ToString()).Plain == blocked.Address.Plain);
            blockedAddress.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_Add_Account_Filter_With_Allow_Mosaic()
        {
            var company = await Fixture.GenerateAccountWithCurrency(10000);

            Log.WriteLine($"Company Account {company.Address.Plain} \r\n Private Key: {company.PrivateKey} \r\n Public Key {company.PublicKey}");
            var allowedMosaic = await Fixture.CreateMosaic(company);

            Log.WriteLine($"Mosaic Id: {allowedMosaic.HexId} ");

            var accountFilter = ModifyAccountPropertyTransaction<IUInt64Id>.CreateForMosaic(
                Deadline.Create(),
                (ulong)0,
                PropertyType.ALLOW_MOSAIC,
                new List<AccountPropertyModification<IUInt64Id>>()
                {
                    new AccountPropertyModification<IUInt64Id>(PropertyModificationType.ADD, allowedMosaic)
                },
                Fixture.NetworkType);

            Log.WriteLine($"Going to filter the address {company.Address} ");

            await Fixture.SiriusWebSocketClient.Listener.Open();

            var tx = Fixture.SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(company.Address).Take(1).Timeout(TimeSpan.FromSeconds(1000));

            var signedTransaction = accountFilter.SignWith(company, Fixture.GenerationHash);

            Log.WriteLine($"Going to announce transaction {signedTransaction.Hash}");

            await Fixture.SiriusClient.TransactionHttp.Announce(signedTransaction);

            var result = await tx;
            Log.WriteLine($"Request confirmed with transaction {result.TransactionInfo.Hash}");

            var accountProperties = await Fixture.SiriusClient.AccountHttp.GetAccountProperty(company.Address);
            accountProperties.Should().NotBeNull();
            var allowMosaicProperty =
                accountProperties.AccountProperties.Properties.Single(ap =>
                    ap.PropertyType == PropertyType.ALLOW_MOSAIC);
            allowMosaicProperty.Should().NotBeNull();

            bool hasAllowMosaic = false;
            foreach (var am in allowMosaicProperty.Values)
            {
                var m = JsonConvert.DeserializeObject<UInt64DTO>(am.ToString());
                var mId = new MosaicId(m.FromUInt8Array());

                if (mId.Id.ToHex().Equals(allowedMosaic.HexId))
                {
                    hasAllowMosaic = true;
                }
            }
            hasAllowMosaic.Should().Be(true);
            var allowMosaic = allowMosaicProperty.Values
                .Single(a =>
                {
                    var m = JsonConvert.DeserializeObject<UInt64DTO>(a.ToString());
                    return (new MosaicId(m.ToUInt64())).HexId == allowedMosaic.HexId;
                });
            allowMosaic.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_Add_Account_Filter_With_Allow_Entity_Type()
        {
            // var company = await Fixture.GenerateAccountWithCurrency(10000);
            var company = Account.CreateFromPrivateKeyV2("101a4dfcffe3d5abddee535c0c7fa7bb386b99eb7c19f0c73cdc3837c5844477", Fixture.NetworkType);

            Log.WriteLine($"Company Account {company.Address.Plain} \r\n Private Key: {company.PrivateKey} \r\n Public Key {company.PublicKey}");
            //    var allowedMosaic = await Fixture.CreateMosaic(company);
            var allowedTransType = EntityType.TRANSFER;

            var accountFilter = ModifyAccountPropertyTransaction<EntityType>.CreateForEntityType(
                Deadline.Create(),
                (ulong)0,
                PropertyType.BLOCK_TRANSACTION,
                new List<AccountPropertyModification<EntityType>>()
                {
                    new AccountPropertyModification<EntityType>(PropertyModificationType.ADD, allowedTransType)
                },
                Fixture.NetworkType);

            Log.WriteLine($"Going to filter the address {company.Address} ");

            //   await Fixture.SiriusWebSocketClient.Listener.Open();

            //    var tx = Fixture.SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(company.Address).Take(1).Timeout(TimeSpan.FromSeconds(5000));

            var signedTransaction = accountFilter.SignWith(Fixture.SeedAccount, Fixture.GenerationHash);

            Log.WriteLine($"Going to announce transaction {signedTransaction.Hash}");
            Log.WriteLine($"Transaction Payload {signedTransaction.Payload}");

            await Fixture.SiriusClient.TransactionHttp.Announce(signedTransaction);

            //    var result = await tx;
            //   Log.WriteLine($"Request confirmed with transaction {result.TransactionInfo.Hash}");

            var accountProperties = await Fixture.SiriusClient.AccountHttp.GetAccountProperty(company.Address);
            accountProperties.Should().NotBeNull();
            var allowedTransactionProperty = accountProperties.AccountProperties.Properties.Single(ap => ap.PropertyType == PropertyType.BLOCK_TRANSACTION);
            var allowedTxType = allowedTransactionProperty.Values.Select(p =>
                EntityTypeExtension.GetRawValue((int)p) == allowedTransType);
            allowedTxType.Should().NotBeNull();
        }

        [Fact]
        public async Task Should_Link_Namespace_To_An_Account()
        {
            var company = Account.GenerateNewAccount(Fixture.NetworkType);

            await Fixture.SiriusWebSocketClient.Listener.Open();
            var tx = Fixture.SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(company.Address).Take(1);

            #region Create account and send some money to it

            Log.WriteLine($"Alice Account {company.Address.Plain} \r\n Private Key: {company.PrivateKey} \r\n Public Key {company.PublicKey}");

            const ulong amount = (ulong)150;
            var mosaicToTransfer = NetworkCurrencyMosaic.CreateRelative(amount);

            var transferTransaction = TransferTransaction.Create(
                Deadline.Create(),
                company.Address,
                new List<Mosaic>()
                {
                    mosaicToTransfer
                },
                PlainMessage.Create("transferTest"),
                Fixture.NetworkType);

            var signedTransaction = Fixture.SeedAccount.Sign(transferTransaction, Fixture.GenerationHash);
            Log.WriteLine($"Going to send {amount} XPP to {company.Address.Pretty} with transaction {signedTransaction.Hash}");

            await Fixture.SiriusClient.TransactionHttp.Announce(signedTransaction);
            var result = await tx;

            Log.WriteLine($"Request confirmed with public key {result.Signer.PublicKey}");

            var companyAccountInfo = await Fixture.SiriusClient.AccountHttp.GetAccountInfo(company.Address);

            Log.WriteLine($"Account {companyAccountInfo.Address.Plain} with initial mosaic {companyAccountInfo.Mosaics[0]}");

            companyAccountInfo.Mosaics[0]?.Amount.Should().BeGreaterThan(0);

            #endregion Create account and send some money to it

            #region register new namespace

            var namespaceName = "nsp" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);
            var registerNamespaceTransaction = RegisterNamespaceTransaction.CreateRootNamespace(
                Deadline.Create(),
                namespaceName,
                10000,
                Fixture.NetworkType
            );

            var registeredNsSignedTransaction = company.Sign(registerNamespaceTransaction, Fixture.GenerationHash);

            tx = Fixture.SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(company.Address).Take(1)
                .Timeout(TimeSpan.FromSeconds(3000));

            await Fixture.SiriusClient.TransactionHttp.Announce(registeredNsSignedTransaction);

            Log.WriteLine(
                $"Registered namespace {namespaceName} for account {company.Address.Plain} with transaction {registeredNsSignedTransaction.Hash}");

            result = await tx;

            Log.WriteLine($"Request confirmed with transaction {result.TransactionInfo.Hash}");

            var expectedId = new NamespaceId(namespaceName);

            var nsInfo = await Fixture.SiriusClient.NamespaceHttp.GetNamespace(expectedId);

            Log.WriteLine(
                $"Retrieved namespace {namespaceName} successfully. The namespace HexId {nsInfo.Id.HexId}");
            nsInfo.Should().NotBeNull();

            companyAccountInfo = await Fixture.SiriusClient.AccountHttp.GetAccountInfo(company.Address);

            Log.WriteLine($"Account {companyAccountInfo.Address.Plain} with mosaic {companyAccountInfo.Mosaics[0]} after registered namespace");

            #endregion register new namespace

            #region Link namespace to the address

            /*var addressAliasTransaction = AliasTransaction.CreateForAddress(
                company.Address,
                nsInfo.Id,
                AliasActionType.LINK,
                Deadline.Create(),
                Fixture.NetworkType
            );*/

            var builder = AliasTransactionBuilder.CreateForAddress();
            var addressAliasTransaction = builder
                .SetNamespaceId(nsInfo.Id)
                .SetDeadline(Deadline.Create())
                .SetNetworkType(Fixture.NetworkType)
                .SetFeeCalculationStrategy(FeeCalculationStrategyType.LOW)
                .Link(company.Address)
                .Build();

            tx = Fixture.SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(company.Address).Take(1);

            var aliasSignedTransaction = company.Sign(addressAliasTransaction, Fixture.GenerationHash);

            Fixture.WatchForFailure(aliasSignedTransaction);

            await Fixture.SiriusClient.TransactionHttp.Announce(aliasSignedTransaction);

            result = await tx;

            Log.WriteLine($"Request confirmed with aliasSignedTransaction transaction {aliasSignedTransaction.Hash}");

            nsInfo = await Fixture.SiriusClient.NamespaceHttp.GetNamespace(expectedId);

            nsInfo.Should().NotBeNull();
            nsInfo.HasAlias.Should().BeTrue();
            nsInfo.Alias.Address.Plain.Should().BeEquivalentTo(company.Address.Plain);

            #endregion Link namespace to the address

            #region Send mosaic to namespace instead of address

            transferTransaction = TransferTransaction.Create(
                Deadline.Create(),
                nsInfo.Id,
                new List<Mosaic>()
                {
                    NetworkCurrencyMosaic.CreateRelative(10)
                },
                PlainMessage.Create("Send to namespace"),
                Fixture.NetworkType);

            tx = Fixture.SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(Fixture.SeedAccount.Address).Take(1)
                .Timeout(TimeSpan.FromSeconds(3000));

            var nsSignedTransferTransaction = Fixture.SeedAccount.Sign(transferTransaction, Fixture.GenerationHash);

            Fixture.WatchForFailure(nsSignedTransferTransaction);

            await Fixture.SiriusClient.TransactionHttp.Announce(nsSignedTransferTransaction);

            var result2 = await tx;

            Log.WriteLine($"Request confirmed with transaction {result2.TransactionInfo.Hash}");

            companyAccountInfo = await Fixture.SiriusClient.AccountHttp.GetAccountInfo(company.Address);

            Log.WriteLine($"Account {companyAccountInfo.Address.Plain} with mosaic {companyAccountInfo.Mosaics[0]} after transfer to the alias");

            //var expectedMosaicAmount = Convert.ToUInt64(Math.Pow(10, 6)) * (150 - 100 + 10);

            companyAccountInfo.Mosaics[0]?.Amount.Should().BeGreaterThan(0);

            #endregion Send mosaic to namespace instead of address
        }
    }
}