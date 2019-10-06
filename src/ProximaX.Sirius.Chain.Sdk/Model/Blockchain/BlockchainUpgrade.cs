using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Model.Blockchain
{
    public class BlockchainUpgrade
    {
        public BlockchainUpgrade(ulong height, BlockchainVersion version)
        {
            Height = height;
            Version = version;
        }

        public ulong Height { get; }
        public BlockchainVersion Version { get; }

        public static BlockchainUpgrade FromDto(BlockchainUpgradeDTO dto)
        {
            return new BlockchainUpgrade(dto.BlockchainUpgrade.Height.ToUInt64(), BlockchainVersion.FromVersionValue(dto.BlockchainUpgrade.BlockChainVersion.ToUInt64()));
        }
    }
}
