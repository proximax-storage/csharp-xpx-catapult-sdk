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

using ProximaX.Sirius.Sdk.Infrastructure.Listener;

namespace ProximaX.Sirius.Sdk.Client
{
    /// <summary>
    ///     Class SiriusWebSocket
    /// </summary>
    public class SiriusWebSocketClient : IClient
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="SiriusWebSocketClient" /> class.
        /// </summary>
        /// <param name="host">The node host without</param>
        /// <param name="port"></param>
        /// <param name="useSsl"></param>
        public SiriusWebSocketClient(string host = @"localhost", int port = 3000, bool useSsl = false)
        {
            Host = host;
            Port = port;
            UseSsl = useSsl;
            Listener = new Listener(host, port, useSsl);
        }

        /// <summary>
        ///     The websocket port
        /// </summary>
        public int Port { get; set; }

        /// <summary>
        ///     Use secure protocol
        /// </summary>
        public bool UseSsl { get; }

        /// <summary>
        ///     The listener
        /// </summary>
        public Listener Listener { get; }

        /// <summary>
        ///     The websocket host
        /// </summary>
        public string Host { get; set; }
    }
}