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

namespace ProximaX.Sirius.Chain.Sdk.Model.Blockchain
{
    public class BlockchainConfig
    {
        public BlockchainConfig(ulong height, string config, string supportedEntityVersions)
        {
            Height = height;
            Config = config;
            SupportedEntityVersions = supportedEntityVersions;
        }

        public ulong Height { get; }
        public string Config { get; }
        public string SupportedEntityVersions { get; }

        public static BlockchainConfig FromDto(NetworkConfigDTO dto)
        {
            return new BlockchainConfig(dto.NetworkConfig.Height.ToUInt64(), dto.NetworkConfig.NetworkConfig, dto.NetworkConfig.SupportedEntityVersions);
        }
    }
}
