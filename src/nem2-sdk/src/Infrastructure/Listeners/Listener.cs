// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="Listener.cs" company="Nem.io">   
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
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using io.nem2.sdk.Infrastructure.Buffers.Model;
using io.nem2.sdk.Infrastructure.Mapping;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Blockchain;
using io.nem2.sdk.Model.Transactions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace io.nem2.sdk.Infrastructure.Listeners
{
    /// <summary>
    /// Websocket Listener
    /// </summary>
    public class Listener 
    {
        /// <summary>
        /// Gets the uid.
        /// </summary>
        /// <value>The uid.</value>
        private WebsocketUID Uid { get; set; }
        /// <summary>
        /// Gets the client socket.
        /// </summary>
        /// <value>The client socket.</value>
        private ClientWebSocket ClientSocket { get; }
        /// <summary>
        /// Gets the loop reads.
        /// </summary>
        /// <value>The loop reads.</value>
        private Task LoopReads { get; set; }
        /// <summary>
        /// The subject
        /// </summary>
        private readonly Subject<string> _subject = new Subject<string>();

        /// <summary>
        /// Gets or sets the domain.
        /// </summary>
        /// <value>The domain.</value>
        public string Domain { get; set; }

        public int Port { get; set; }
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="domain">The host domain to listen to.</param>
        /// <param name="port">The port to listen on.</param>
        public Listener(string domain, int port = 3000)
        {
            ClientSocket = new ClientWebSocket();

            Domain = domain;

            Port = port;
        }

        /// <summary>
        /// Opens the websocket connection.
        /// </summary>
        /// <returns>IObservable&lt;System.Boolean&gt;.</returns>
        public IObservable<bool> Open()
        {
            return Observable.Start(() =>
            {
                ClientSocket.ConnectAsync(new Uri(string.Concat("ws://", Domain, ":", Port, "/ws")), CancellationToken.None)
                    .GetAwaiter()
                    .GetResult();

                Uid = JsonConvert.DeserializeObject<WebsocketUID>(ReadSocket().Result);

                LoopReads = Task.Run(() => LoopRead());

                Console.WriteLine("Connected to websocket via domain: " + Domain);

                return Uid != null;
            });         
        }

        /// <summary>
        /// Loops the read.
        /// </summary>
        internal async void LoopRead()
        {
            while (true)
            {
                _subject.OnNext(await ReadSocket());
            }
        }

        /// <summary>
        /// Reads the socket.
        /// </summary>
        /// <returns>Task&lt;System.String&gt;.</returns>
        internal async Task<string> ReadSocket()
        {
            var buffer = new ArraySegment<byte>(new byte[8192]);

            using (var stream = new MemoryStream())
            {
                WebSocketReceiveResult result;

                do
                {
                    result = await ClientSocket.ReceiveAsync(buffer, CancellationToken.None);

                    stream.Write(buffer.Array, buffer.Offset, result.Count);
                }
                while (!result.EndOfMessage);

                stream.Seek(0, SeekOrigin.Begin);

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Subscribes to channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        internal void SubscribeToChannel(string channel)
        {
            var encoded = Encoding.UTF8.GetBytes(string.Concat("{ \"uid\": \"", Uid.UID, "\", \"subscribe\":\"", channel, "\"}"));

            var buffer = new ArraySegment<byte>(encoded, 0, encoded.Length);

            ClientSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);

            Console.WriteLine("Subscribed to channel: " + channel + ", UID: " + Uid.UID);
        }

        /// <summary>
        /// Subscribe to new block messages.
        /// </summary>
        /// <returns>IObservable of type BlockInfoDTO</returns>
        public IObservable<BlockInfo> NewBlock()
        {
            SubscribeToChannel("block");
            
            return _subject.Where(e => JObject.Parse(e).Properties().ToArray().Any(i => i.Value.ToString().Contains("block")))
               .Select(e =>
                {
                    var block = JsonConvert.DeserializeObject<BlockInfoDTO>(e);
                    var network = (int)Convert.ToInt64(block.Block.Version.ToString("X").Substring(0, 2), 16);
                    return new BlockInfo(
                        block.Meta.Hash,
                        block.Meta.GenerationHash,
                        block.Meta.TotalFee,
                        block.Meta.NumTransactions,
                        block.Block.Signature,
                        new PublicAccount(block.Block.Signer, NetworkType.GetRawValue(network)),
                        NetworkType.GetRawValue(network),
                        block.Block.Version,
                        block.Block.Type,
                        block.Block.Height,
                        block.Block.Timestamp,
                        block.Block.Difficulty,
                        block.Block.PreviousBlockHash,
                        block.Block.BlockTransactionsHash);
                });
        }

        /// <summary>
        /// Listen for confirmed transactions added to an account.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>IObservable&lt;TransactionInfoDTO&gt;.</returns>
        /// <exception cref="ArgumentNullException">address</exception>
        public IObservable<Transaction> ConfirmedTransactionsGiven(Address address)
        {
            if (address == null) throw new ArgumentNullException(nameof(address));

            SubscribeToChannel(string.Concat("confirmedAdded/", address.Plain));
           
            return _subject.Where(e => JObject.Parse(e).Properties().ToArray().Any(i => i.Value.ToString().Contains("confirmedAdded")))
               .Where(e => TransactionFromAddress(new TransactionMapping().Apply(e), address))          
               .Select(new TransactionMapping().Apply);
        }

        /// <summary>
        /// Listen for unconfirmed transactions added to an account.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>IObservable&lt;TransactionInfoDTO&gt;.</returns>
        /// <exception cref="ArgumentNullException">address</exception>
        public IObservable<Transaction> UnconfirmedTransactionsAdded(Address address)
        {
            if (address == null) throw new ArgumentNullException(nameof(address));

            SubscribeToChannel(string.Concat("unconfirmedAdded/", address.Plain));

            return _subject.Where(e => JObject.Parse(e).Properties().ToArray().Any(i => i.Value.ToString().Contains("unconfirmedAdded")))
                .Where(e => TransactionFromAddress(new TransactionMapping().Apply(e), address))
                .Select(new TransactionMapping().Apply);
        }

        /// <summary>
        /// Listen for unconfirmed transactions removed from an account.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>IObservable&lt;TransactionInfoDTO&gt;.</returns>
        /// <exception cref="ArgumentNullException">address</exception>
        public IObservable<Transaction> UnconfirmedTransactionsRemoved(Address address)
        {
            if (address == null) throw new ArgumentNullException(nameof(address));

            SubscribeToChannel(string.Concat("unconfirmedRemoved/", address.Plain));

            return _subject.Where(e => JObject.Parse(e).Properties().ToArray().Any(i => i.Value.ToString().Contains("unconfirmedRemoved")))
                .Where(e => TransactionFromAddress(new TransactionMapping().Apply(e), address))
                .Select(new TransactionMapping().Apply);
        }

        /// <summary>
        /// Listen for partial transactions added to account.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>IObservable&lt;TransactionInfoDTO&gt;.</returns>
        /// <exception cref="ArgumentNullException">address</exception>
        public IObservable<Transaction> AggregateBondedAdded(Address address)
        {
            if (address == null) throw new ArgumentNullException(nameof(address));

            SubscribeToChannel(string.Concat("partialAdded/", address.Plain));

            return _subject.Where(e => JObject.Parse(e).Properties().ToArray().Any(i => i.Value.ToString().Contains("partialAdded")))
                .Where(e => TransactionFromAddress(new TransactionMapping().Apply(e), address))
                .Select(new TransactionMapping().Apply);
        }

        /// <summary>
        /// Listen for partial transactions removed from an account.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>IObservable&lt;TransactionInfoDTO&gt;.</returns>
        /// <exception cref="ArgumentNullException">address</exception>
        public IObservable<Transaction> AggregateBondedRemoved(Address address)
        {
            if (address == null) throw new ArgumentNullException(nameof(address));

            SubscribeToChannel(string.Concat("partialRemoved/", address.Plain));

            return _subject.Where(e => JObject.Parse(e).Properties().ToArray().Any(i => i.Value.ToString().Contains("partialRemoved")))
                .Where(e => TransactionFromAddress(new TransactionMapping().Apply(e), address))
                .Select(new TransactionMapping().Apply);
        }

        /// <summary>
        /// Listen for transaction status messages for an account.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>IObservable&lt;TransactionStatusDTO&gt;.</returns>
        /// <exception cref="ArgumentNullException">address</exception>
        public IObservable<TransactionStatusDTO> TransactionStatus(Address address)
        {
            if (address == null) throw new ArgumentNullException(nameof(address));

            SubscribeToChannel(string.Concat("status/", address.Plain));

            return _subject.Where(e => JObject.Parse(e).Properties().ToArray().Any(i => i.Name.ToString().Contains("status")))          
                .Select(JsonConvert.DeserializeObject<TransactionStatusDTO>);
        }

        /// <summary>
        /// Listen for cosignature transactions added to an account.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>IObservable&lt;CosignatureSignedTransactionDTO&gt;.</returns>
        /// <exception cref="ArgumentNullException">address</exception>
        public IObservable<CosignatureSignedTransactionDTO> CosignatureAdded(Address address)
        {
            if (address == null) throw new ArgumentNullException(nameof(address));

            SubscribeToChannel(string.Concat("cosignature/", address.Plain));

            return _subject.Where(e => JObject.Parse(e).Properties().ToArray().Any(i => i.Value.ToString().Contains("cosignature")))
                .Select(JsonConvert.DeserializeObject<CosignatureSignedTransactionDTO>);
        }

        /// <summary>
        /// Transactions from address.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="address">The address.</param>
        /// <returns><c>true</c> if the given transaction is from the given address, <c>false</c> otherwise.</returns>
        private bool TransactionFromAddress(Transaction transaction, Address address)
        {
            var transactionFromAddress = TransactionHasSignerOrReceptor(transaction, address);

            if (!transactionFromAddress && transaction.TransactionType.GetValue() == TransactionTypes.Types.AggregateComplete.GetValue() && ((AggregateTransaction)transaction).Cosignatures != null)
            {
                transactionFromAddress = ((AggregateTransaction)transaction).Cosignatures.Any(e => Address.CreateFromPublicKey(e.Signer.PublicKey, address.NetworkByte).Plain == address.Plain);
            }


            return transactionFromAddress;
        }

        /// <summary>
        /// Transactions the has signer or receptor.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="address">The address.</param>
        /// <returns><c>true</c> if the transaction has the given address as signer or receptor, <c>false</c> otherwise.</returns>
        private bool TransactionHasSignerOrReceptor(Transaction transaction, Address address)
        {
            var isReceptor = false;

            if (transaction.TransactionType.GetValue() == TransactionTypes.Types.Transfer.GetValue())
            {
                isReceptor = ((TransferTransaction)transaction).Address.Plain == address.Plain;
            }

            return Address.CreateFromPublicKey(transaction.Signer.PublicKey, address.NetworkByte).Plain == address.Plain || isReceptor;
        }

        /// <summary>
        /// Close the websocket connection.
        /// </summary>
        public void Close()
        {
            ClientSocket.Abort();
            LoopReads.Dispose();
        }

        /// <summary>
        /// Get the websocket UID.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetUid()
        {
            return Uid.UID;
        }
    }
}
