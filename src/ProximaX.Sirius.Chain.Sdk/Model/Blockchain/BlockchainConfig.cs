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
