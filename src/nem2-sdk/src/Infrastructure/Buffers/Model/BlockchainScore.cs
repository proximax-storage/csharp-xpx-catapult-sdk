// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="BlockchainScore.cs" company="Nem.io">   
// Copyright 2018 NEM
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
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using io.nem2.sdk.Core.Utils;
using io.nem2.sdk.Infrastructure.Buffers.Model.JsonConverters;
using Newtonsoft.Json;

namespace io.nem2.sdk.Infrastructure.Buffers.Model
{
    /// <summary>
    /// Class BlockchainScore.
    /// </summary>
    public class BlockchainScore
    {
        /// <summary>
        /// Gets or sets the score high.
        /// </summary>
        /// <value>The score high.</value>
        [JsonProperty("scoreHigh")]
        [JsonConverter(typeof(UInt32ArrayToLong))]
        public ulong ScoreHigh { get; set; }

        /// <summary>
        /// Gets or sets the score low.
        /// </summary>
        /// <value>The score low.</value>
        [JsonProperty("scoreLow")]
        [JsonConverter(typeof(UInt32ArrayToLong))]
        public ulong ScoreLow { get; set; }

        internal ulong Extract()
        {
            return new[] {  BitConverter.ToInt32(BitConverter.GetBytes(ScoreLow), 0), BitConverter.ToInt32(BitConverter.GetBytes(ScoreHigh), 0) }.FromInt8Array();
        }
    }
}
