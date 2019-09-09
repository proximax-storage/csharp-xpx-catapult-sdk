

using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;

namespace ProximaX.Sirius.Chain.Sdk.Utils
{
    /// <summary>
    ///  Utility class to help with serialization and deserialization of transaction data
    /// </summary>
    public static class TransactionMappingUtils
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transactionVersion"></param>
        /// <param name="networkType"></param>
        /// <returns></returns>
        public static int SerializeVersion(int transactionVersion, int networkType)
        {
            return (networkType << 24) + transactionVersion;
        }

        public static int ExtractTransactionVersion(int version)
        {
            return version & 0xFFFFFF;
        }

        public static NetworkType ExtractNetworkType(int version)
        {
           
            var value = (uint)version >> 24;
            return NetworkTypeExtension.GetRawValue((int)value);
        }

    }
}
