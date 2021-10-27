using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO
{/// <summary>
 ///
 /// </summary>
    [DataContract]
    public class MosaicLevyDTO
    {
        /// <summary>
        /// Levy Type. LevyNone = 0x0, LevyAbsoluteFee = 0x1, LevyPercentileFee = 0x2.
        /// </summary>
        /// <value>Levy Type</value>
        [DataMember(Name = "type", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "type")]
        public MosaicLevyType Type { get; set; }

        /// <summary>
        /// Recipient
        /// </summary>
        /// <value>Recipient</value>
        [DataMember(Name = "recipient", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "recipient")]
        public string Recipient { get; set; }

        /// <summary>
        /// MosaicId
        /// </summary>
        /// <value>MosaicId</value>
        [DataMember(Name = "mosaicId", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "mosaicId")]
        public UInt64DTO MosaicId { get; set; }

        /// <summary>
        /// Recipient
        /// </summary>
        /// <value>Recipient</value>
        [DataMember(Name = "recipient", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "recipient")]
        public UInt64DTO Fee { get; set; }

        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class AccountDTO {\n");
            sb.Append("  Type: ").Append(Type).Append("\n");
            sb.Append("  Recipient: ").Append(Recipient).Append("\n");
            sb.Append("  MosaicId: ").Append(MosaicId).Append("\n");
            sb.Append("  Fee: ").Append(Fee).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Get the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}