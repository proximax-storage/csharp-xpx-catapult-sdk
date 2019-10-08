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
using System;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class CatapultUpgradeDTO : IEquatable<CatapultUpgradeDTO>
    { 
        /// <summary>
        /// Gets or Sets CatapultConfig
        /// </summary>
        [DataMember(Name="catapultConfig")]
        public UpgradeDTO CatapultConfig { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class CatapultUpgradeDTO {\n");
            sb.Append("  CatapultConfig: ").Append(CatapultConfig).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((CatapultUpgradeDTO)obj);
        }

        /// <summary>
        /// Returns true if CatapultUpgradeDTO instances are equal
        /// </summary>
        /// <param name="other">Instance of CatapultUpgradeDTO to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(CatapultUpgradeDTO other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    CatapultConfig == other.CatapultConfig ||
                    CatapultConfig != null &&
                    CatapultConfig.Equals(other.CatapultConfig)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                    if (CatapultConfig != null)
                    hashCode = hashCode * 59 + CatapultConfig.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(CatapultUpgradeDTO left, CatapultUpgradeDTO right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(CatapultUpgradeDTO left, CatapultUpgradeDTO right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
