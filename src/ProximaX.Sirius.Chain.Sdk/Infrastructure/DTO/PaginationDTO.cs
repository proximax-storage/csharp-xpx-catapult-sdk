// Copyright 2021 ProximaX
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
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO
{
    /// <summary>
    /// Class of PaginationDTO
    /// </summary>
    [DataContract]
    public class PaginationDTO
    {
        /// <summary>
        /// Gets or Sets Total Entries
        /// </summary>
        [DataMember(Name = "totalEntries", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "totalEntries")]
        public int TotalEntries { get; set; }

        /// <summary>
        /// Gets or Sets Page Number
        /// </summary>
        [DataMember(Name = "pageNumber", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "pageNumber")]
        public int PageNumber { get; set; }

        /// <summary>
        /// Gets or Sets Page Size
        /// </summary>
        [DataMember(Name = "pageSize", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "pageSize")]
        public int PageSize { get; set; }

        /// <summary>
        /// Gets or Sets Total Pages
        /// </summary>
        [DataMember(Name = "totalPages", EmitDefaultValue = false)]
        [JsonProperty(PropertyName = "totalPages")]
        public int TotalPages { get; set; }

        /// <summary>
        /// Get the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Pagination {\n");
            sb.Append("  TotalEntries: ").Append(TotalEntries).Append("\n");
            sb.Append("  PageNumber: ").Append(PageNumber).Append("\n");
            sb.Append("  PageSize: ").Append(PageSize).Append("\n");
            sb.Append("  TotalPages: ").Append(TotalPages).Append("\n");
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