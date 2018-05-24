using Newtonsoft.Json;

namespace io.nem2.sdk.Infrastructure.Buffers.Model
{
    public class MultisigAccountInfoDTO
    {
        [JsonProperty("multisig")]
        public MultisigDTO Multisig { get; set; }
    }
}
