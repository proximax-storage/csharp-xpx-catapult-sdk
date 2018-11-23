//
// Copyright 2018 NEM
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
// 

using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using io.nem2.sdk.Core.Crypto;
using io.nem2.sdk.Core.Crypto.Chaso.NaCl;
using io.nem2.sdk.Core.Crypto.Chaso.NaCl.Internal.Ed25519ref10;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using io.nem2.sdk.Infrastructure.HttpRepositories;
using io.nem2.sdk.Infrastructure.Listeners;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Blockchain;
using io.nem2.sdk.Model.Mosaics;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.Model.Transactions.Messages;
using Org.BouncyCastle.Crypto.Digests;


namespace IntegrationTests.Infrastructure.Transactions
{
    [TestClass]
    public class TransferTransactionTests
    {
        private Listener listener { get; }

        public TransferTransactionTests()
        {
            listener = new Listener(Config.Domain);

//            listener.Open().Wait();
        }

        public async Task AnnounceTransaction(ulong amount = 10)
        {
            var keyPair =
                KeyPair.CreateFromPrivateKey(Config.PrivateKeyTransfer);

            var transaction =  TransferTransaction.Create(
                NetworkType.Types.MIJIN_TEST,
                Deadline.CreateHours(2), 
                Address.CreateFromRawAddress("SCEYFB35CYFF2U7UZ32RYXXZ5JTPCSKU4P6BRXZR"),
                new List<Mosaic> { Mosaic.CreateFromIdentifier("nem:xem", amount) }, 
                PlainMessage.Create("hello")  
                ).SignWith(keyPair);

             await new TransactionHttp("http://" + Config.Domain + ":3000").Announce(transaction);
        }
        
        [TestMethod, Timeout(40000)]
        public async Task AnnounceTransferTransactionWithMosaicWithMessage()
        {
            var keyPair = KeyPair.CreateFromPrivateKey(Config.PrivateKeyTransfer);

            var account = new Account(Config.PrivateKeyTransfer, NetworkType.Types.MIJIN_TEST);

            var transaction = TransferTransaction.Create(
                NetworkType.Types.MIJIN_TEST,
                Deadline.CreateHours(2),
                account.Address,
                new List<Mosaic> { Xem.CreateRelative(1) },
                PlainMessage.Create("111111111111111111111111111111111111111111111111111111111111111111111111111" +
                                    "111111111111111111111111111111111111111111111111111111111111111111111111111" +
                                    "111111111111111111111111111111111111111111111111111111111111111111111111111" +
                                    "111111111111111111111111111111")
            ).SignWith(keyPair);

            listener.TransactionStatus(Address.CreateFromPublicKey(transaction.Signer, NetworkType.Types.MIJIN_TEST))
                .Subscribe(e =>
                {
                    Console.WriteLine(e.Status);
                });

            await new TransactionHttp("http://" + Config.Domain + ":3000").Announce(transaction);
           
            var status = await listener.ConfirmedTransactionsGiven(Address.CreateFromPublicKey(transaction.Signer, NetworkType.Types.MIJIN_TEST)).Take(1);

            Assert.AreEqual(keyPair.PublicKeyString, status.Signer.PublicKey);
        }

        [TestMethod, Timeout(20000)]
        public async Task AnnounceTransferTransactionWithMosaicWithSecureMessage()
        {
            var keyPair = KeyPair.CreateFromPrivateKey(Config.PrivateKeyAggregate1);

            var transaction = TransferTransaction.Create(
                NetworkType.Types.MIJIN_TEST,
                Deadline.CreateHours(2),
                Address.CreateFromRawAddress("SAAA57-DREOPY-KUFX4O-G7IQXK-ITMBWK-D6KXTV-BBQP"),
                new List<Mosaic> { Mosaic.CreateFromIdentifier("nem:xem", 10) },
                SecureMessage.Create("hello2", Config.PrivateKeyAggregate1, "5D8BEBBE80D7EA3B0088E59308D8671099781429B449A0BBCA6D950A709BA068")
                ).SignWith(keyPair);

            await new TransactionHttp("http://" + Config.Domain + ":3000").Announce(transaction);

            listener.TransactionStatus(Address.CreateFromPublicKey(keyPair.PublicKeyString, NetworkType.Types.MIJIN_TEST))
                .Subscribe(e => Console.WriteLine(e.Status));

            var status = await listener.ConfirmedTransactionsGiven(Address.CreateFromPublicKey(transaction.Signer, NetworkType.Types.MIJIN_TEST)).Take(1);

            Assert.AreEqual(keyPair.PublicKeyString, status.Signer.PublicKey);
        }

        [TestMethod, Timeout(20000)]
        public async Task AnnounceTransferTransactionWithMultipleMosaicsWithSecureMessage()
        {
            var keyPair =
                KeyPair.CreateFromPrivateKey(Config.PrivateKeyAggregate1);

            var transaction = TransferTransaction.Create(
                NetworkType.Types.MIJIN_TEST,
                Deadline.CreateHours(2),
                Address.CreateFromRawAddress("SAOV4Y5W627UXLIYS5O43SVU23DD6VNRCFP222P2"),
                new List<Mosaic>()
                {
                    Mosaic.CreateFromIdentifier("happy:test4", 10),
                    Mosaic.CreateFromIdentifier("nem:xem", 100),
                    
                },
                SecureMessage.Create("hello2", Config.PrivateKeyMain, "5D8BEBBE80D7EA3B0088E59308D8671099781429B449A0BBCA6D950A709BA068")
                
            ).SignWith(keyPair);

            await new TransactionHttp("http://" + Config.Domain + ":3000").Announce(transaction);

            var status = await listener.ConfirmedTransactionsGiven(Address.CreateFromPublicKey(transaction.Signer, NetworkType.Types.MIJIN_TEST)).Take(1);

            Assert.AreEqual(keyPair.PublicKeyString, status.Signer.PublicKey);
        }

        [TestMethod, Timeout(20000)]
        public async Task AnnounceTransferTransactionWithMultipleMosaicsWithoutMessage()
        {
            var keyPair =
                KeyPair.CreateFromPrivateKey(Config.PrivateKeyAggregate1);

            var transaction = TransferTransaction.Create(
                NetworkType.Types.MIJIN_TEST,
                Deadline.CreateHours(2),
                Address.CreateFromRawAddress("SAAA57-DREOPY-KUFX4O-G7IQXK-ITMBWK-D6KXTV-BBQP"),
                new List<Mosaic>()
                {
                    
                    Mosaic.CreateFromIdentifier("happy:test2", 10),
                    Mosaic.CreateFromIdentifier("nem:xem", 10),
                },
                EmptyMessage.Create()        
            ).SignWith(keyPair);

            await new TransactionHttp("http://" + Config.Domain + ":3000").Announce(transaction);

            listener.TransactionStatus(Address.CreateFromPublicKey(transaction.Signer, NetworkType.Types.MIJIN_TEST))
                .Subscribe(e => Console.WriteLine(e.Status));

            var status = await listener.ConfirmedTransactionsGiven(Address.CreateFromPublicKey(transaction.Signer, NetworkType.Types.MIJIN_TEST)).Take(1);

            Assert.AreEqual(keyPair.PublicKeyString, status.Signer.PublicKey);
        }

        internal static TransferTransaction CreateInnerTransferTransaction(string mosaic, ulong amount = 10)
        {
            return TransferTransaction.Create(
                        NetworkType.Types.MIJIN_TEST,
                        Deadline.CreateHours(2),
                        Address.CreateFromRawAddress("SAAA57-DREOPY-KUFX4O-G7IQXK-ITMBWK-D6KXTV-BBQP"),
                        new List<Mosaic> {Mosaic.CreateFromIdentifier(mosaic, amount)}, 
                        PlainMessage.Create("hey")
                        
                    );
        }
        
        [TestMethod, Timeout(20000)]
        public async Task EncodeDecodeSecureMessage()
        {
            var privateKey1 = "2a91e1d5c110a8d0105aad4683f962c2a56663a3cad46666b16d243174673d90";
            var privateKey2 = "2618090794e9c9682f2ac6504369a2f4fb9fe7ee7746f9560aca228d355b1cb9";
            
            Console.WriteLine(Account.CreateFromPrivateKey(privateKey1, NetworkType.Types.MIJIN_TEST).PublicKey);
            Console.WriteLine(Account.CreateFromPrivateKey(privateKey2, NetworkType.Types.MIJIN_TEST).PublicKey);
            
            var secureMessage = SecureMessage.Create("hello2", privateKey1,
                Account.CreateFromPrivateKey(privateKey2, NetworkType.Types.MIJIN_TEST).PublicKey);

            Console.WriteLine("payload " + secureMessage.GetPayload().ToHexLower());
            
            var decodedPayload1 = secureMessage.GetDecodedPayload(privateKey1, 
                Account.CreateFromPrivateKey(privateKey2, NetworkType.Types.MIJIN_TEST).PublicKey);
            Assert.AreEqual(decodedPayload1, "hello2");

            var decodedPayload2 = secureMessage.GetDecodedPayload(privateKey2, 
                Account.CreateFromPrivateKey(privateKey1, NetworkType.Types.MIJIN_TEST).PublicKey);
            Assert.AreEqual(decodedPayload2, "hello2");

        }

        [TestMethod, Timeout(20000), ExpectedException(typeof(CryptographicException))]
        public async Task EncodeDecodeSecureMessageShouldFail()
        {
            var privateKey1 = "2a91e1d5c110a8d0105aad4683f962c2a56663a3cad46666b16d243174673d90";
            var privateKey2 = "2618090794e9c9682f2ac6504369a2f4fb9fe7ee7746f9560aca228d355b1cb9";
            
            Console.WriteLine(Account.CreateFromPrivateKey(privateKey1, NetworkType.Types.MIJIN_TEST).PublicKey);
            Console.WriteLine(Account.CreateFromPrivateKey(privateKey2, NetworkType.Types.MIJIN_TEST).PublicKey);
            
            var secureMessage = SecureMessage.Create("hello2", privateKey1,
                Account.CreateFromPrivateKey(privateKey2, NetworkType.Types.MIJIN_TEST).PublicKey);
            
            secureMessage.GetDecodedPayload(privateKey1, 
                Account.CreateFromPrivateKey(privateKey1, NetworkType.Types.MIJIN_TEST).PublicKey);
        }

        [TestMethod, Timeout(20000)]
        public async Task TestDerive()
        {
            var privateKey1 = "2a91e1d5c110a8d0105aad4683f962c2a56663a3cad46666b16d243174673d90";
            var privateKey2 = "2618090794e9c9682f2ac6504369a2f4fb9fe7ee7746f9560aca228d355b1cb9";

            Console.WriteLine(Account.CreateFromPrivateKey(privateKey1, NetworkType.Types.MIJIN_TEST).PublicKey);
            Console.WriteLine(Account.CreateFromPrivateKey(privateKey2, NetworkType.Types.MIJIN_TEST).PublicKey);

            var shared = new byte[32];
            var salt = "0a5103646209c911468723ea67095ed325e5faa35a319d338edcfc7e32c5e30c".FromHex();
            var secretKey = privateKey1.FromHex();
            var pubkey = Account.CreateFromPrivateKey(privateKey2, NetworkType.Types.MIJIN_TEST).PublicKey.FromHex();
            var longKeyHash = new byte[64];
            var shortKeyHash = new byte[32];

//            Array.Reverse(secretKey);

            // compute  Sha3(512) hash of secret key (as in prepareForScalarMultiply)
//            var digestSha3 = new KeccakDigest(512);
            var digestSha3 = new Sha3Digest(512);
            digestSha3.BlockUpdate(secretKey, 0, 32);
            digestSha3.DoFinal(longKeyHash, 0);

            longKeyHash[0] &= 248;
            longKeyHash[31] &= 127;
            longKeyHash[31] |= 64;

            Array.Copy(longKeyHash, 0, shortKeyHash, 0, 32);

            ScalarOperations.sc_clamp(shortKeyHash, 0);

            var p = new[] { new long[16], new long[16], new long[16], new long[16] };
            var q = new[] { new long[16], new long[16], new long[16], new long[16] };

            TweetNaCl.Unpackneg(q, pubkey); // returning -1 invalid signature
            TweetNaCl.Scalarmult(p, q, shortKeyHash, 0);
            TweetNaCl.Pack(shared, p);

            // for some reason the most significant bit of the last byte needs to be flipped.
            // doesnt seem to be any corrosponding action in nano/nem.core, so this may be an issue in one of the above 3 functions. i have no idea.
            shared[31] ^= (1 << 7);

            // salt
            for (var i = 0; i < salt.Length; i++)
            {
                shared[i] ^= salt[i];
            }

            Console.WriteLine();
            Console.WriteLine("shared key before salt " + BitConverter.ToString(shared).Replace("-",""));

            // hash salted shared key
//            var digestSha3Two = new KeccakDigest(256);
            var digestSha3Two = new Sha3Digest(256);
            digestSha3Two.BlockUpdate(shared, 0, 32);
            digestSha3Two.DoFinal(shared, 0);

            Console.WriteLine();
            Console.WriteLine("salted shared key " + BitConverter.ToString(shared).Replace("-",""));
        }        

    }
}
