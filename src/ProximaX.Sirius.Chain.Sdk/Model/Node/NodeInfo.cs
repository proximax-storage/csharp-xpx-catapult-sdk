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


using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Utils;


namespace ProximaX.Sirius.Chain.Sdk.Model.Node
{
    /// <summary>
    /// NodeInfo class
    /// </summary>
    public class NodeInfo
    {
        /// <summary>
        /// NodeInfo constructor
        /// </summary>
        /// <param name="publicKey">The public key</param>
        /// <param name="port">The port number</param>
        /// <param name="networkIdentifier">The network identifier</param>
        /// <param name="version">The version</param>
        /// <param name="roles">The node role</param>
        /// <param name="host">The host</param>
        /// <param name="friendlyName">The friendly name</param>
        public NodeInfo(string publicKey, int port, int networkIdentifier, int version, RoleType roles,
       string host, string friendlyName)
        {
            PublicKey = publicKey;
            Port = port;
            NetworkIdentifier = networkIdentifier;
            Version = version;
            Roles = roles;
            Host = host;
            FriendlyName = friendlyName;
        }

        public string PublicKey { get; }
        public int Port { get; }
        public int NetworkIdentifier { get; }
        public int Version { get; }
        public RoleType Roles { get; }
        public string Host { get; }
        public string FriendlyName { get; }

        /// <summary>
        /// NodeInfo from Dto object
        /// </summary>
        /// <param name="dto">The dto object</param>
        /// <returns></returns>
        public static NodeInfo FromDto(NodeInfoDTO dto)
        {
            return new NodeInfo(dto.PublicKey, dto.Port.Value, dto.NetworkIdentifier.Value,
                dto.Version.Value, EnumExtensions.GetEnumValue<RoleType>(dto.Roles.ToString()), dto.Host, dto.FriendlyName);
        }
    }
}
