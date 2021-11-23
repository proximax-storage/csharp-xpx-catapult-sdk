using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Security.Cryptography;
using System.Threading;
using System.Threading.Tasks;
using FluentAssertions;
using ProximaX.Sirius.Chain.Sdk.Crypto.Core;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Fees;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions.Builders;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions.Messages;
using ProximaX.Sirius.Chain.Sdk.Utils;
using Xunit;
using Xunit.Abstractions;

namespace ProximaX.Sirius.Chain.Sdk.Tests.E2E
{
    public class E2ETransferTests : IClassFixture<E2EBaseFixture>
    {
        private readonly E2EBaseFixture Fixture;
        private readonly ITestOutputHelper Log;

        public E2ETransferTests(E2EBaseFixture fixture, ITestOutputHelper log)
        {
            Fixture = fixture;
            Log = log;
        }

        [Fact]
        public async Task Should_Announce_Transfer_Transaction_With_NetworkCurrencyMosaic_PlainMessage()
        {
            var account = Account.GenerateNewAccount(Fixture.NetworkType);
            var mosaic = NetworkCurrencyMosaic.CreateRelative(10);
            var message = PlainMessage.Create("Test message");
            var result = await Fixture.Transfer(Fixture.SeedAccount, account.Address, mosaic, message, Fixture.GenerationHash);
            Log.WriteLine($"Transaction confirmed {result.TransactionInfo.Hash}");
            result.TransactionInfo.Hash.Should().NotBeNullOrWhiteSpace();
            //result.TransactionType.Should().Be(EntityType.TRANSFER);
            ((TransferTransaction)result).Message.GetMessageType().Should().Be(MessageType.PLAIN_MESSAGE.GetValueInByte());
        }

        [Fact]
        public async Task Should_Announce_Transfer_Transaction_With_Default_Fee()
        {
            var account = Account.GenerateNewAccount(Fixture.NetworkType);
            var mosaic = NetworkCurrencyMosaic.CreateRelative(10);
            var message = PlainMessage.Create("Test message");

            var builder = new TransferTransactionBuilder();

            var transferTransaction = builder
                .SetNetworkType(Fixture.NetworkType)
                .SetDeadline(Deadline.Create())
                .SetMosaics(new List<Mosaic>() { mosaic })
                .SetRecipient(Recipient.From(account.Address))
                .SetMessage(message)
                .SetFeeCalculationStrategy(FeeCalculationStrategyType.LOW)
                .Build();

            var signedTransaction = Fixture.SeedAccount.Sign(transferTransaction, Fixture.GenerationHash);

            Fixture.WatchForFailure(signedTransaction);

            //Log.WriteLine($"Going to announce transaction {signedTransaction.Hash}");

            var tx = Fixture.SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(Fixture.SeedAccount.Address).Take(1);

            await Fixture.SiriusClient.TransactionHttp.Announce(signedTransaction);

            var result = await tx;

            Log.WriteLine($"Transaction confirmed {result.TransactionInfo.Hash}");
            result.TransactionInfo.Hash.Should().NotBeNullOrWhiteSpace();
            result.TransactionType.Should().Be(EntityType.TRANSFER);
            //((TransferTransaction)result).Message.GetMessageType().Should().Be(MessageType.PLAIN_MESSAGE.GetValueInByte());
        }

        [Fact]
        public async Task Should_Announce_Aggregate_Transaction_Signed_Aggregate_Transaction()
        {
            var account = Account.GenerateNewAccount(Fixture.NetworkType);
            Log.WriteLine($"Account Address: {account.Address}");

            var mosaic = NetworkCurrencyMosaic.CreateAbsolute(1);
            var message = PlainMessage.Create("c#__ SDK plain message test");
            var result = await Fixture.AggregateTransfer(Fixture.SeedAccount, account.Address, mosaic, message, Fixture.GenerationHash);
            Log.WriteLine($"Transaction confirmed {result.TransactionInfo.Hash}");
            result.TransactionInfo.Hash.Should().NotBeNullOrWhiteSpace();
            result.TransactionType.Should().Be(EntityType.AGGREGATE_COMPLETE);
        }

        [Fact]
        public async Task Should_Announce_Transfer_Transaction_With_NetworkCurrencyMosaic_SecureMessage()
        {
            var account1 = PublicAccount.CreateFromPublicKey("42B85DF37E6349B20E48F82ADA20F53E0EED60FA190CDAC792A8E1C02EFEFB85", Fixture.NetworkType);
            var account = Account.GenerateNewAccount(Fixture.NetworkType);
            Log.WriteLine($"Reciever private key {account.KeyPair.PrivateKeyString}, reciever public key {account.PublicAccount.PublicKey}");
            var mosaic = NetworkCurrencyMosaic.CreateRelative(1000000);
            Log.WriteLine($"Sender private key {Fixture.SeedAccount.KeyPair.PrivateKeyString}, sender public key {Fixture.SeedAccount.PublicAccount.PublicKey}");

            var message = SecureMessage.Create("Test secure message", Fixture.SeedAccount.KeyPair.PrivateKeyString, account.PublicAccount.PublicKey);
            var result = await Fixture.Transfer(Fixture.SeedAccount, account1.Address, mosaic, message, Fixture.GenerationHash);
            Log.WriteLine($"Transaction confirmed {result.TransactionInfo.Hash}");
            result.TransactionInfo.Hash.Should().NotBeNullOrWhiteSpace();
            result.TransactionType.Should().Be(EntityType.TRANSFER);
            ((TransferTransaction)result).Message.GetMessageType().Should().Be(MessageType.SECURED_MESSAGE.GetValueInByte());
        }

        [Fact]
        public async Task Should_Announce_Secret_Hash()
        {
            var bob = await Fixture.GenerateAccountWithCurrency(10000);
            /*  var priv_bob = await Fixture.GenerateAccountWithCurrency(10000);

              var priv_alice = await Fixture.GenerateAccountWithCurrency(10000);*/
            var alice = await Fixture.GenerateAccountWithCurrency(10000);
            Log.WriteLine($"Bob Account Address: {bob.Address.Plain} \r\n Private Key: {bob.PrivateKey} \r\n Public Key {bob.PublicKey}");
            Log.WriteLine($"Alice Account Address: {alice.Address.Plain} \r\n Private Key: {alice.PrivateKey} \r\n Public Key {alice.PublicKey}");
            //var secret = "8ee6bb9dc6dbf61db3b2c01086976ffac34b0820c1a4c8aeff92f59c33cb49fa";
            byte[] Seed = new byte[40];
            // Random random = new Random();
            // random.NextBytes(Seed);
            var secret_ = CryptoUtils.Sha3_256(Seed);
            var secret = BitConverter.ToString(secret_);
            secret = secret.Replace("-", "").ToLowerInvariant();
            var csprng = new RNGCryptoServiceProvider();
            csprng.GetNonZeroBytes(Seed);
            var proof = BitConverter.ToString(Seed);
            //var proof = string.Join("", Seed.Select(b => b.ToString("X")));
            proof = proof.Replace("-", "");
            /*   Random rdm = new Random();
               string proof = string.Empty;
               int num;

               for (int i = 0; i < 4; i++)
               {
                   num = rdm.Next(0, int.MaxValue);
                   proof += num.ToString("X4");
               }*/

            //proof = Hex.EncodeHexString(Seed);
            //proof = proof.Replace("-", "");

            //Random random = new Random();
            //string proof = random.NextBytes(secretSeed);

            var nonce = MosaicNonce.CreateRandom();
            var mosaic_id = MosaicId.CreateFromNonce(nonce, Fixture.GenerationHash);
            var secretLockTransaction = SecretLockTransaction.Create(Deadline.Create(),
                // NetworkCurrencyMosaic.CreateRelative(10),
                new Mosaic(mosaic_id, 10),
                (ulong)100,
                HashType.SHA3_256,
                secret,
                bob.Address,
                NetworkType.TEST_NET);

            /*   var aggregateTransaction = AggregateTransaction.CreateComplete(
                   Deadline.Create(),
                   new List<Transaction>
                   {
                       secretLockTransaction.ToAggregate(Fixture.SeedAccount.PublicAccount)
                   },
                   Fixture.NetworkType);*/
            var signedTransaction = alice.Sign(secretLockTransaction, Fixture.GenerationHash);
            Fixture.WatchForFailure(signedTransaction);
            Log.WriteLine($"Going to announce transaction {signedTransaction.Hash}");
            Log.WriteLine($"Proof {proof}");
            Log.WriteLine($"Secret {secret}");
            Log.WriteLine($"proof length {proof.DecodeHexString().Length}");

            await Fixture.SiriusClient.TransactionHttp.Announce(signedTransaction);
            //Log.WriteLine($"Going to announce Secret Lock Transaction {signedTransaction.Hash}");

            var secretProofTransaction = SecretProofTransaction.Create(
                    Deadline.Create(),
                    HashType.SHA3_256,
                    Recipient.From(bob.Address),
                    secret,
                    proof,
                    Fixture.NetworkType
                );
            var secretProofsignedTransaction = bob.Sign(secretProofTransaction, Fixture.GenerationHash);
            Fixture.WatchForFailure(secretProofsignedTransaction);
            Thread.Sleep(8000);
            //   Log.WriteLine($"Going to announce Secret Lock Transaction {signedTransaction.Hash}");

            //  var tx = Fixture.SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(account.Address).Take(1).Timeout(TimeSpan.FromSeconds(3000));

            // var tx = Fixture.SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(alice.Address).Take(1).Timeout(TimeSpan.FromSeconds(3000));
            await Fixture.SiriusClient.TransactionHttp.Announce(secretProofsignedTransaction);
            Thread.Sleep(8000);
            Log.WriteLine($"Going to announce Secret Proof Transaction {secretProofsignedTransaction.Hash}");
            //var tx = Fixture.SiriusWebSocketClient.Listener.ConfirmedTransactionsGiven(alice.Address).Take(1).Timeout(TimeSpan.FromSeconds(3000));
            //var secretLocksignedTransaction = account.Sign(secretLockTransaction, Fixture.GenerationHash);

            // await Fixture.SiriusClient.TransactionHttp.Announce(secretLocksignedTransaction);

            //  var result = await tx;
            //  Log.WriteLine($"Request confirmed with transaction {result.TransactionInfo.Hash}");
        }

        private Task Transfer(object seedAccount, Address address, NetworkCurrencyMosaic mosaic, PlainMessage message, object generationHash)
        {
            throw new NotImplementedException();
        }
    }
}