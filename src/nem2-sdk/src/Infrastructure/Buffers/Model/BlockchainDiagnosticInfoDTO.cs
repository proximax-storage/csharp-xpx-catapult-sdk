using Newtonsoft.Json;

namespace io.nem2.sdk.Infrastructure.Buffers.Model
{
    public class BlockchainDiagnosticStroageDTO
    {
        [JsonProperty("numBlocks")]
        public int NumBlocks { get; set; }

        [JsonProperty("numTransactions")]
        public int NumTransactions { get; set; }

        [JsonProperty("numAccounts")]
        public int NumAccounts { get; set; }
    }
}
