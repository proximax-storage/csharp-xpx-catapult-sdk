using System;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class UpgradeDTO : IEquatable<UpgradeDTO>
    { 
        /// <summary>
        /// Gets or Sets Height
        /// </summary>
        [Required]
        [DataMember(Name="height")]
        public UInt64DTO Height { get; set; }

        /// <summary>
        /// Gets or Sets CatapultVersion
        /// </summary>
        [Required]
        [DataMember(Name="catapultVersion")]
        public UInt64DTO CatapultVersion { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class UpgradeDTO {\n");
            sb.Append("  Height: ").Append(Height).Append("\n");
            sb.Append("  CatapultVersion: ").Append(CatapultVersion).Append("\n");
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
            return obj.GetType() == GetType() && Equals((UpgradeDTO)obj);
        }

        /// <summary>
        /// Returns true if UpgradeDTO instances are equal
        /// </summary>
        /// <param name="other">Instance of UpgradeDTO to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(UpgradeDTO other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return 
                (
                    Height == other.Height ||
                    Height != null &&
                    Height.Equals(other.Height)
                ) && 
                (
                    CatapultVersion == other.CatapultVersion ||
                    CatapultVersion != null &&
                    CatapultVersion.Equals(other.CatapultVersion)
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
                    if (Height != null)
                    hashCode = hashCode * 59 + Height.GetHashCode();
                    if (CatapultVersion != null)
                    hashCode = hashCode * 59 + CatapultVersion.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(UpgradeDTO left, UpgradeDTO right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(UpgradeDTO left, UpgradeDTO right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
