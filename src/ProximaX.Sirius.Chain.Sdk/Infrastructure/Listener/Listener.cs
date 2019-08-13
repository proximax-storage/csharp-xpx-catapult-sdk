// Copyright 2019 ProximaX
// 
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

using System;
using System.IO;
using System.Linq;
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ProximaX.Sirius.Chain.Sdk.Infrastructure.Mapping;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Utils;


namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.Listener
{
    public class Listener
    {
        private readonly CancellationTokenSource _cToken = new CancellationTokenSource();

        private readonly bool UseSsl;

        /// <summary>
        ///     The subject
        /// </summary>
        private readonly Subject<string> _subject = new Subject<string>();

        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="domain">The host domain to listen to.</param>
        /// <param name="port">The port to listen on.</param>
        /// <param name="useSsl"></param>
        public Listener(string domain, int port = 3000, bool useSsl = false)
        {
            ClientSocket = new ClientWebSocket();

            Domain = domain;

            Port = port;

            UseSsl = useSsl;
        }

        /// <summary>
        ///     Gets the uid.
        /// </summary>
        /// <value>The uid.</value>
        private WebsocketUid Uid { get; set; }

        /// <summary>
        ///     Gets the client socket.
        /// </summary>
        /// <value>The client socket.</value>
        private ClientWebSocket ClientSocket { get; }

        /// <summary>
        ///     Gets the loop reads.
        /// </summary>
        /// <value>The loop reads.</value>
        private Task LoopReads { get; set; }

        /// <summary>
        ///     Gets or sets the domain.
        /// </summary>
        /// <value>The domain.</value>
        public string Domain { get; set; }

        public int Port { get; set; }

        /// <summary>
        ///     Opens the websocket connection.
        /// </summary>
        /// <returns>IObservable&lt;System.Boolean&gt;.</returns>
        public IObservable<bool> Open()
        {
            return Observable.Start(() =>
            {
                if (ClientSocket.State != WebSocketState.Open)
                {
                    var protocol = UseSsl ? "wss://" : "ws://";
                    var protocolPath = UseSsl ? "/wss" : "/ws";

                    ClientSocket.ConnectAsync(new Uri(string.Concat(protocol, Domain, ":", Port, protocolPath)),
                            CancellationToken.None)
                        .GetAwaiter()
                        .GetResult();

                    Uid = JsonConvert.DeserializeObject<WebsocketUid>(ReadSocket().Result);

                    LoopReads = Task.Run(() => LoopRead(), _cToken.Token);

                    Console.WriteLine("Connected to websocket via domain: " + Domain);
                }

                return Uid != null;
            });
        }

        /// <summary>
        ///     Loops the read.
        /// </summary>
        internal async void LoopRead()
        {
            while (true) _subject.OnNext(await ReadSocket());
        }

        /// <summary>
        ///     Reads the socket.
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

                    stream.Write(buffer.Array ?? throw new InvalidOperationException(), buffer.Offset, result.Count);
                } while (!result.EndOfMessage);

                stream.Seek(0, SeekOrigin.Begin);

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        ///     Subscribes to channel.
        /// </summary>
        /// <param name="channel">The channel.</param>
        internal void SubscribeToChannel(string channel)
        {
            var encoded =
                Encoding.UTF8.GetBytes(string.Concat("{ \"uid\": \"", Uid.Uid, "\", \"subscribe\":\"", channel, "\"}"));

            var buffer = new ArraySegment<byte>(encoded, 0, encoded.Length);

            ClientSocket.SendAsync(buffer, WebSocketMessageType.Text, true, CancellationToken.None);

            Console.WriteLine("Subscribed to channel: " + channel + ", UID: " + Uid.Uid);
        }

        /// <summary>
        ///     Subscribe to new block messages.
        /// </summary>
        /// <returns>IObservable of type BlockInfoDTO</returns>
        public IObservable<BlockInfo> NewBlock()
        {
            SubscribeToChannel("block");

            return _subject.Where(e =>
                    JObject.Parse(e).Properties().ToArray().Any(i => i.Value.ToString().Contains("block")))
                .Select(JObject.Parse)
                .Select(i => new BlockInfo(
                    i["meta"]["hash"].ToString(),
                    i["meta"]["generationHash"].ToString(),
                    i["meta"]["totalFee"] == null ? 0 : i["meta"].ExtractBigInteger("totalFee"),
                    i["meta"]["numTransactions"] == null ? 0 : int.Parse(i["meta"]["numTransactions"].ToString()),
                    i["block"]["signature"].ToString(),
                    new PublicAccount(i["block"]["signer"].ToString(),
                    int.Parse(i["block"]["version"].ToString()).ExtractNetworkType()),
                    int.Parse(i["block"]["version"].ToString()).ExtractNetworkType(),
                    int.Parse(i["block"]["version"].ToString()).ExtractVersion(),
                    int.Parse(i["block"]["type"].ToString()),
                    i["block"].ExtractBigInteger("height"),
                    i["block"].ExtractBigInteger("timestamp"),
                    i["block"].ExtractBigInteger("difficulty"),
                    i["block"]["previousBlockHash"].ToString(),
                    i["block"]["blockTransactionsHash"].ToString(),
                    i["block"]["blockReceiptsHash"].ToString()
                    ));
        }

        /// <summary>
        ///     Listen for confirmed transactions added to an account.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>IObservable&lt;TransactionInfoDTO&gt;.</returns>
        /// <exception cref="ArgumentNullException">address</exception>
        public IObservable<Transaction> ConfirmedTransactionsGiven(Address address)
        {
            if (address == null) throw new ArgumentNullException(nameof(address));

            SubscribeToChannel(string.Concat("confirmedAdded/", address.Plain));

            return _subject.Where(e =>
                    JObject.Parse(e).Properties().ToArray().Any(i => i.Value.ToString().Contains("confirmedAdded")))
                .Where(e => TransactionFromAddress(new TransactionMapping().Apply(JObject.Parse(e)), address))
                .Select(i => new TransactionMapping().Apply(JObject.Parse(i)));
        }

        /// <summary>
        ///     Listen for unconfirmed transactions added to an account.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>IObservable&lt;TransactionInfoDTO&gt;.</returns>
        /// <exception cref="ArgumentNullException">address</exception>
        public IObservable<Transaction> UnconfirmedTransactionsAdded(Address address)
        {
            if (address == null) throw new ArgumentNullException(nameof(address));

            SubscribeToChannel(string.Concat("unconfirmedAdded/", address.Plain));

            return _subject.Where(e =>
                    JObject.Parse(e).Properties().ToArray().Any(i => i.Value.ToString().Contains("unconfirmedAdded")))
                .Where(e => TransactionFromAddress(new TransactionMapping().Apply(JObject.Parse(e)), address))
                .Select(i => new TransactionMapping().Apply(JObject.Parse(i)));
        }

        /// <summary>
        ///     Listen for unconfirmed transactions removed from an account.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>IObservable&lt;TransactionInfoDTO&gt;.</returns>
        /// <exception cref="ArgumentNullException">address</exception>
        public IObservable<Transaction> UnconfirmedTransactionsRemoved(Address address)
        {
            if (address == null) throw new ArgumentNullException(nameof(address));

            SubscribeToChannel(string.Concat("unconfirmedRemoved/", address.Plain));

            return _subject
                .Where(e => JObject.Parse(e).Properties().ToArray()
                    .Any(i => i.Value.ToString().Contains("unconfirmedRemoved")))
                .Where(e => TransactionFromAddress(new TransactionMapping().Apply(JObject.Parse(e)), address))
                .Select(i => new TransactionMapping().Apply(JObject.Parse(i)));
        }

        /// <summary>
        ///     Listen for partial transactions added to account.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>IObservable&lt;TransactionInfoDTO&gt;.</returns>
        /// <exception cref="ArgumentNullException">address</exception>
        public IObservable<Transaction> AggregateBondedAdded(Address address)
        {
            if (address == null) throw new ArgumentNullException(nameof(address));

            SubscribeToChannel(string.Concat("partialAdded/", address.Plain));

            return _subject.Where(e =>
                    JObject.Parse(e).Properties().ToArray().Any(i => i.Value.ToString().Contains("partialAdded")))
                .Where(e => TransactionFromAddress(new TransactionMapping().Apply(JObject.Parse(e)), address))
                .Select(i => new TransactionMapping().Apply(JObject.Parse(i)));
        }

        /// <summary>
        ///     Listen for partial transactions removed from an account.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>IObservable&lt;TransactionInfoDTO&gt;.</returns>
        /// <exception cref="ArgumentNullException">address</exception>
        public IObservable<Transaction> AggregateBondedRemoved(Address address)
        {
            if (address == null) throw new ArgumentNullException(nameof(address));

            SubscribeToChannel(string.Concat("partialRemoved/", address.Plain));

            return _subject.Where(e =>
                    JObject.Parse(e).Properties().ToArray().Any(i => i.Value.ToString().Contains("partialRemoved")))
                .Where(e => TransactionFromAddress(new TransactionMapping().Apply(JObject.Parse(e)), address))
                .Select(i => new TransactionMapping().Apply(JObject.Parse(i)));
        }

        /// <summary>
        ///     Listen for transaction status messages for an account.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>IObservable&lt;TransactionStatusDTO&gt;.</returns>
        /// <exception cref="ArgumentNullException">address</exception>
        public IObservable<TransactionStatus> TransactionStatus(Address address)
        {
            if (address == null) throw new ArgumentNullException(nameof(address));

            SubscribeToChannel(string.Concat("status/", address.Plain));

            return _subject.Where(e =>
                    JObject.Parse(e).Properties().ToArray().Any(i => i.Name.ToString().Contains("status")))
                .Select(i => new TransactionStatus(null,
                    JObject.Parse(i)["status"].ToString(),
                    JObject.Parse(i)["hash"].ToString(),
                    JObject.Parse(i).ExtractBigInteger("deadline"),
                    null));
        }

        /// <summary>
        ///     Listen for cosignature transactions added to an account.
        /// </summary>
        /// <param name="address">The address.</param>
        /// <returns>IObservable&lt;CosignatureSignedTransactionDTO&gt;.</returns>
        /// <exception cref="ArgumentNullException">address</exception>
        public IObservable<CosignatureSignedTransaction> CosignatureAdded(Address address)
        {
            if (address == null) throw new ArgumentNullException(nameof(address));

            SubscribeToChannel(string.Concat("cosignature/", address.Plain));

            return _subject.Where(e =>
                    JObject.Parse(e).Properties().ToArray().Any(i => i.Value.ToString().Contains("cosignature")))
                .Select(JsonConvert.DeserializeObject<CosignatureSignedTransaction>);
        }

        /// <summary>
        ///     Transactions from address.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="address">The address.</param>
        /// <returns><c>true</c> if the given transaction is from the given address, <c>false</c> otherwise.</returns>
        private bool TransactionFromAddress(Transaction transaction, Address address)
        {
            var transactionFromAddress = TransactionHasSignerOrReceptor(transaction, address);

            if (!transactionFromAddress && typeof(AggregateTransaction) == transaction.GetType())
            {
                ((AggregateTransaction) transaction).Cosignatures?.ForEach(e =>
                {
                    if (Address.CreateFromPublicKey(e.Signer.PublicKey, address.NetworkType).Plain == address.Plain)
                        transactionFromAddress = true;
                });
                ((AggregateTransaction) transaction).InnerTransactions.ForEach(e =>
                {
                    if (Address.CreateFromPublicKey(e.Signer.PublicKey, address.NetworkType).Plain == address.Plain)
                        transactionFromAddress = true;
                });
            }

            return transactionFromAddress;
        }

        /// <summary>
        ///     Transactions the has signer or receptor.
        /// </summary>
        /// <param name="transaction">The transaction.</param>
        /// <param name="address">The address.</param>
        /// <returns><c>true</c> if the transaction has the given address as signer or receptor, <c>false</c> otherwise.</returns>
        private static bool TransactionHasSignerOrReceptor(Transaction transaction, Address address)
        {
            var isReceptor = false;

            if (transaction.TransactionType.GetValue() == TransactionType.TRANSFER.GetValue())
                isReceptor = ((TransferTransaction) transaction).Address.Plain == address.Plain;

            return Address.CreateFromPublicKey(transaction.Signer.PublicKey, address.NetworkType).Plain ==
                   address.Plain || isReceptor;
        }

        /// <summary>
        ///     Close the websocket connection.
        /// </summary>
        public void Close()
        {
            _cToken.Cancel();
            LoopReads.Dispose();
            ClientSocket.Abort();
        }

        /// <summary>
        ///     Get the websocket UID.
        /// </summary>
        /// <returns>System.String.</returns>
        public string GetUid()
        {
            return Uid.Uid;
        }
    }
}