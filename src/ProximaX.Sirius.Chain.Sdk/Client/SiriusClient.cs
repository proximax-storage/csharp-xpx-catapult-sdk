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

using ProximaX.Sirius.Chain.Sdk.Infrastructure;

namespace ProximaX.Sirius.Chain.Sdk.Client
{
    /// <summary>
    ///     Class SiriusClient
    /// </summary>
    public class SiriusClient : IClient
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SiriusClient" /> class.
        /// </summary>
        /// <param name="host">The network host</param>
        public SiriusClient(string host = @"http://localhost:3000")
        {
            Host = host;
            NetworkHttp = new NetworkHttp(host);
            AccountHttp = new AccountHttp(host, NetworkHttp);
            BlockHttp = new BlockHttp(host, NetworkHttp);
            ChainHttp = new ChainHttp(host, NetworkHttp);
            MetadataHttp = new MetadataHttp(host, NetworkHttp);
            MosaicHttp = new MosaicHttp(host, NetworkHttp);
            NamespaceHttp = new NamespaceHttp(host, NetworkHttp);
            TransactionHttp = new TransactionHttp(host, NetworkHttp);
            NodeHttp = new NodeHttp(host, NetworkHttp);

        }

        /// <summary>
        ///     The network host
        /// </summary>
        public string Host { get; }

        /// <summary>
        ///     The network http
        /// </summary>
        public NetworkHttp NetworkHttp { get; }

        /// <summary>
        ///     The account http
        /// </summary>
        public AccountHttp AccountHttp { get; }

        /// <summary>
        ///     The metadata http
        /// </summary>
        public MetadataHttp MetadataHttp { get; }

        /// <summary>
        ///     The mosaic http
        /// </summary>
        public MosaicHttp MosaicHttp { get; }

        /// <summary>
        ///     The namespace http
        /// </summary>
        public NamespaceHttp NamespaceHttp { get; }

        /// <summary>
        ///     The transaction http
        /// </summary>
        public TransactionHttp TransactionHttp { get; }

        /// <summary>
        /// The Node Http
        /// </summary>
        public NodeHttp NodeHttp { get; }

        /// <summary>
        /// The Block http
        /// </summary>
        public BlockHttp BlockHttp { get; }

        /// <summary>
        /// The Chain http
        /// </summary>
        public ChainHttp ChainHttp { get; }
    }
}