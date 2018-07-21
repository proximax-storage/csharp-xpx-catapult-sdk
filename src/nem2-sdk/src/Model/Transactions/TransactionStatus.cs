// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="TransactionStatusDTO.cs" company="Nem.io">   
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

namespace io.nem2.sdk.Infrastructure.Buffers.Model
{
    public class TransactionStatus
    {
        public TransactionStatus(string group, string status, string hash, ulong deadline, ulong? height)
        {
            Group = group;
            Status = status;
            Hash = hash;
            Deadline = deadline;
            Height = height;
        }

        public string Group { get; }
        public string Status { get; }
        public string Hash { get; }
        public ulong Deadline { get; }
        public ulong? Height { get; }
    }
}
