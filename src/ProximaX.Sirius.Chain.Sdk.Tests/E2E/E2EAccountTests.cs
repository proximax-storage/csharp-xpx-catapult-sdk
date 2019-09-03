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
    public class E2EAccountTests
    {
        private readonly E2ETestFixture _fixture;

        private readonly ITestOutputHelper _output;

        public E2EAccountTests(E2ETestFixture fixture, ITestOutputHelper output)
        {
            _fixture = fixture;
            _output = output;

        }

     
        [Fact]
        public async Task Should_Send_Some_Money_To_New_Account()
        {
            var networkType = _fixture.Client.NetworkHttp.GetNetworkType().Wait();
            var aliceAccount = Account.GenerateNewAccount(networkType);

            await _fixture.WebSocket.Listener.Open();

            var tx = _fixture.WebSocket.Listener.ConfirmedTransactionsGiven(aliceAccount.Address).Take(1);

            _output.WriteLine($"Alice Account {aliceAccount.Address.Plain} \r\n Private Key: {aliceAccount.PrivateKey} \r\n Public Key {aliceAccount.PublicKey}");

            const ulong amount = (ulong)10;
            var mosaicToTransfer = NetworkCurrencyMosaic.CreateRelative(amount);

            var transferTransaction = TransferTransaction.Create(
                Deadline.Create(),
                aliceAccount.Address,
                new List<Mosaic>()
                {
                    mosaicToTransfer
                },
                PlainMessage.Create("transferTest"),
                networkType);

            var signedTransaction = _fixture.SeedAccount.Sign(transferTransaction, _fixture.Environment.GenerationHash);
            _output.WriteLine($"Going to send {amount} XPP to {aliceAccount.Address.Pretty} with transaction {signedTransaction.Hash}");
            WatchForFailure(signedTransaction);
            await _fixture.Client.TransactionHttp.Announce(signedTransaction);
            var result = await tx;

            _output.WriteLine($"Request confirmed with public key {result.Signer.PublicKey}");

            var aliceAccountInfo = await _fixture.Client.AccountHttp.GetAccountInfo(aliceAccount.Address);

            aliceAccountInfo.Mosaics[0]?.Amount.Should().BeGreaterThan(0);

            var outgoingTxs = _fixture.Client.AccountHttp.OutgoingTransactions(aliceAccountInfo.PublicAccount).Wait();

            var recipientAddress = "VDG4WG-FS7EQJ-KFQKXM-4IUCQG-PXUW5H-DJVIJB-OXJG";
            var address = Address.CreateFromRawAddress(recipientAddress);

            var mosaicAmounts = (from TransferTransaction t in outgoingTxs
                                 where t.TransactionType == TransactionType.TRANSFER &&
                                       t.Recipient.Address.Plain == address.Plain &&
                                       t.Mosaics.Count == 1 && 
                                       t.Mosaics[0].HexId == NetworkCurrencyMosaic.Id.HexId
                                 select (long)t.Mosaics[0].Amount).ToList();

            Console.WriteLine($"Total xpx send to account {address.Plain} is {mosaicAmounts.Sum()}");

        }


        [Fact]
        public async Task Should_Add_Account_Filter_With_Block_Address()
        {
            var networkType = _fixture.Client.NetworkHttp.GetNetworkType().Wait();
            var company = Account.GenerateNewAccount(networkType);
            var blocked = Account.GenerateNewAccount(networkType);

            var addressFilter = ModifyAccountPropertyTransaction<Address>.CreateForAddress(
                Deadline.Create(),
                (ulong)0,
                PropertyType.BLOCK_ADDRESS,
                new List<AccountPropertyModification<Address>>()
                {
                    new AccountPropertyModification<Address>(PropertyModificationType.ADD, blocked.Address)
                },
                networkType);

            _output.WriteLine($"Going to filter the address {company.Address} ");

            await _fixture.WebSocket.Listener.Open();

            var tx = _fixture.WebSocket.Listener.ConfirmedTransactionsGiven(company.Address).Take(1)
                .Timeout(TimeSpan.FromSeconds(1000));

     
            var signedTransaction = addressFilter.SignWith(company, _fixture.Environment.GenerationHash);


            _output.WriteLine($"Going to announce transaction {signedTransaction.Hash}");


            await _fixture.Client.TransactionHttp.Announce(signedTransaction);

            var result = await tx;
            _output.WriteLine($"Request confirmed with transaction {result.TransactionInfo.Hash}");


            var accountProperties = await _fixture.Client.AccountHttp.GetAccountProperty(company.PublicAccount);
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
            var networkType = _fixture.Client.NetworkHttp.GetNetworkType().Wait();
            var company = Account.GenerateNewAccount(networkType);

            var allowedMosaic = NetworkCurrencyMosaic.Id;

            var accountFilter = ModifyAccountPropertyTransaction<IUInt64Id>.CreateForMosaic(
                Deadline.Create(),
                (ulong)0,
                PropertyType.ALLOW_MOSAIC,
                new List<AccountPropertyModification<IUInt64Id>>()
                {
                    new AccountPropertyModification<IUInt64Id>(PropertyModificationType.ADD, allowedMosaic)
                },
                networkType);

            _output.WriteLine($"Going to filter the address {company.Address} ");

            await _fixture.WebSocket.Listener.Open();

            var tx = _fixture.WebSocket.Listener.ConfirmedTransactionsGiven(company.Address).Take(1)
                .Timeout(TimeSpan.FromSeconds(1000));

       
            var signedTransaction = accountFilter.SignWith(company, _fixture.Environment.GenerationHash);

            _output.WriteLine($"Going to announce transaction {signedTransaction.Hash}");

            await _fixture.Client.TransactionHttp.Announce(signedTransaction);

            var result = await tx;
            _output.WriteLine($"Request confirmed with transaction {result.TransactionInfo.Hash}");


            var accountProperties = await _fixture.Client.AccountHttp.GetAccountProperty(company.PublicAccount);
            accountProperties.Should().NotBeNull();
            var allowMosaicProperty =
                accountProperties.AccountProperties.Properties.Single(ap =>
                    ap.PropertyType == PropertyType.ALLOW_MOSAIC);
            allowMosaicProperty.Should().NotBeNull();
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
            var networkType = _fixture.Client.NetworkHttp.GetNetworkType().Wait();
            var company = Account.GenerateNewAccount(networkType);

            var allowedTransType = TransactionType.MODIFY_ACCOUNT_PROPERTY_ENTITY_TYPE;

            var accountFilter = ModifyAccountPropertyTransaction<TransactionType>.CreateForEntityType(
                Deadline.Create(),
                (ulong)0,
                PropertyType.ALLOW_TRANSACTION,
                new List<AccountPropertyModification<TransactionType>>()
                {
                    new AccountPropertyModification<TransactionType>(PropertyModificationType.ADD, allowedTransType)
                },
                networkType);

            _output.WriteLine($"Going to filter the address {company.Address} ");

            await _fixture.WebSocket.Listener.Open();

            var tx = _fixture.WebSocket.Listener.ConfirmedTransactionsGiven(company.Address).Take(1)
                .Timeout(TimeSpan.FromSeconds(1000));

          
            var signedTransaction = accountFilter.SignWith(company, _fixture.Environment.GenerationHash);

            _output.WriteLine($"Going to announce transaction {signedTransaction.Hash}");

            await _fixture.Client.TransactionHttp.Announce(signedTransaction);

            var result = await tx;
            _output.WriteLine($"Request confirmed with transaction {result.TransactionInfo.Hash}");


            var accountProperties = await _fixture.Client.AccountHttp.GetAccountProperty(company.PublicAccount);
            accountProperties.Should().NotBeNull();
            var allowedTransactionProperty =
                accountProperties.AccountProperties.Properties.Single(ap =>
                    ap.PropertyType == PropertyType.ALLOW_TRANSACTION);
            var allowedTxType = allowedTransactionProperty.Values.Select(p =>
                TransactionTypeExtension.GetRawValue((int)p) == allowedTransType);
            allowedTxType.Should().NotBeNull();

        }


        [Fact]
        public async Task Should_Link_Namespace_To_An_Account()
        {
            var networkType = _fixture.Client.NetworkHttp.GetNetworkType().Wait();
            var company = Account.GenerateNewAccount(networkType);

            await _fixture.WebSocket.Listener.Open();
            var tx = _fixture.WebSocket.Listener.ConfirmedTransactionsGiven(company.Address).Take(1);

            #region Create account and send some money to it
            _output.WriteLine($"Alice Account {company.Address.Plain} \r\n Private Key: {company.PrivateKey} \r\n Public Key {company.PublicKey}");

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
                networkType);

            var signedTransaction = _fixture.SeedAccount.Sign(transferTransaction,_fixture.Environment.GenerationHash);
            _output.WriteLine($"Going to send {amount} XPP to {company.Address.Pretty} with transaction {signedTransaction.Hash}");

            await _fixture.Client.TransactionHttp.Announce(signedTransaction);
            var result = await tx;

            _output.WriteLine($"Request confirmed with public key {result.Signer.PublicKey}");

            var companyAccountInfo = await _fixture.Client.AccountHttp.GetAccountInfo(company.Address);

            _output.WriteLine($"Account {companyAccountInfo.Address.Plain} with initial mosaic {companyAccountInfo.Mosaics[0]}");

            companyAccountInfo.Mosaics[0]?.Amount.Should().BeGreaterThan(0);
            #endregion

            #region register new namespace
            var namespaceName = "nsp" + Guid.NewGuid().ToString().Replace("-", "").Substring(0, 6);
            var registerNamespaceTransaction = RegisterNamespaceTransaction.CreateRootNamespace(
                Deadline.Create(),
                namespaceName,
                100,
                networkType
            );

            var registeredNsSignedTransaction = company.Sign(registerNamespaceTransaction, _fixture.Environment.GenerationHash);

            tx = _fixture.WebSocket.Listener.ConfirmedTransactionsGiven(company.Address).Take(1)
                .Timeout(TimeSpan.FromSeconds(3000));

            await _fixture.Client.TransactionHttp.Announce(registeredNsSignedTransaction);

            _output.WriteLine(
                $"Registered namespace {namespaceName} for account {company.Address.Plain} with transaction {registeredNsSignedTransaction.Hash}");

            result = await tx;

            _output.WriteLine($"Request confirmed with transaction {result.TransactionInfo.Hash}");

            var expectedId = new NamespaceId(namespaceName);

            var nsInfo = await _fixture.Client.NamespaceHttp.GetNamespace(expectedId).Timeout(_fixture.DefaultTimeout);

            _output.WriteLine(
                $"Retrieved namespace {namespaceName} successfully. The namespace HexId {nsInfo.Id.HexId}");
            nsInfo.Should().NotBeNull();

            companyAccountInfo = await _fixture.Client.AccountHttp.GetAccountInfo(company.Address);

            _output.WriteLine($"Account {companyAccountInfo.Address.Plain} with mosaic {companyAccountInfo.Mosaics[0]} after registered namespace");

            #endregion

            #region Link namespace to the address
            var addressAliasTransaction = AliasTransaction.CreateForAddress(
                company.Address,
                nsInfo.Id,
                AliasActionType.LINK,
                Deadline.Create(),
                networkType
            );

            tx = _fixture.WebSocket.Listener.ConfirmedTransactionsGiven(company.Address).Take(1);

            var aliasSignedTransaction = company.Sign(addressAliasTransaction, _fixture.Environment.GenerationHash);

            WatchForFailure(aliasSignedTransaction);

            await _fixture.Client.TransactionHttp.Announce(aliasSignedTransaction);

            result = await tx;

            _output.WriteLine($"Request confirmed with transaction {result.TransactionInfo.Hash}");


            nsInfo = await _fixture.Client.NamespaceHttp.GetNamespace(expectedId);

            nsInfo.Should().NotBeNull();
            nsInfo.HasAlias.Should().BeTrue();
            nsInfo.Alias.Address.Plain.Should().BeEquivalentTo(company.Address.Plain);

            #endregion

            #region Send mosaic to namespace instead of address
            transferTransaction = TransferTransaction.Create(
                Deadline.Create(),
                nsInfo.Id,
                new List<Mosaic>()
                {
                    NetworkCurrencyMosaic.CreateRelative(10)
                },
                PlainMessage.Create("Send to namespace"),
                networkType);

            tx = _fixture.WebSocket.Listener.ConfirmedTransactionsGiven(_fixture.SeedAccount.Address).Take(1)
                .Timeout(TimeSpan.FromSeconds(3000));

            var nsSignedTransferTransaction = _fixture.SeedAccount.Sign(transferTransaction, _fixture.Environment.GenerationHash);

            WatchForFailure(nsSignedTransferTransaction);

            await _fixture.Client.TransactionHttp.Announce(nsSignedTransferTransaction);

            var result2 = await tx;

            _output.WriteLine($"Request confirmed with transaction {result2.TransactionInfo.Hash}");

            companyAccountInfo = await _fixture.Client.AccountHttp.GetAccountInfo(company.Address);

            _output.WriteLine($"Account {companyAccountInfo.Address.Plain} with mosaic {companyAccountInfo.Mosaics[0]} after transfer to the alias");

            var expectedMosaicAmount = Convert.ToUInt64(Math.Pow(10, 6)) * (150 - 100 + 10);

            companyAccountInfo.Mosaics[0]?.Amount.Should().Be(expectedMosaicAmount);

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


        /*
        [Fact(Timeout = 30000)]
        public async Task Should_Convert_An_Account_To_MultiSig()
        {
            var networkType = await _networkHttp.GetNetworkType();
            const string privateKey = "FFCA7367B4EE8E14041091F438B7BC6C3206ADE2D312670B218C7516395C7D7F";
            var account = Account.CreateFromPrivateKey(privateKey, networkType);

            const string cosignatory1PublicKey = "7D08373CFFE4154E129E04F0827E5F3D6907587E348757B0F87D2F839BF88246";
            var cosignatory1 = PublicAccount.CreateFromPublicKey(cosignatory1PublicKey, networkType);
            const string cosignatory2PublicKey = "F82527075248B043994F1CAFD965F3848324C9ABFEC506BC05FBCF5DD7307C9D";
            var cosignatory2 = PublicAccount.CreateFromPublicKey(cosignatory2PublicKey, networkType);

            var modifications = new List<MultisigCosignatoryModification>
            {
                new MultisigCosignatoryModification(MultisigCosignatoryModificationType.ADD,cosignatory1),
                new MultisigCosignatoryModification(MultisigCosignatoryModificationType.ADD,cosignatory2)
            };

            var convertIntoMultisigTransaction = ModifyMultisigAccountTransaction.Create(
                Deadline.Create(),
                1,
                1,
                modifications,
                networkType
            );

            var aggregateTransaction = AggregateTransaction.CreateBonded(
                Deadline.Create(),
                new List<Transaction>
                {
                    convertIntoMultisigTransaction.ToAggregate(account.PublicAccount)
                }, 
                networkType);

            var signedTransaction = account.Sign(aggregateTransaction);
            _output.WriteLine($"Signed Transaction Hash {signedTransaction.Hash}");

            var hashLockTransaction = LockFundsTransaction.Create(
                Deadline.Create(),
                new Mosaic(
                    (new MosaicId("0DC67FBE1CAD29E3")).Id, //Replace with your network currency mosaic id
                    (ulong)10000000
                ),
                480L,
                signedTransaction,
                networkType);

            var hashLockTransactionSigned = account.Sign(hashLockTransaction);

            await _transactionHttp.Announce(hashLockTransactionSigned);

            await Task.Delay(15000);

            await _transactionHttp.AnnounceAggregateBonded(hashLockTransactionSigned);
        }*/

    }
}
