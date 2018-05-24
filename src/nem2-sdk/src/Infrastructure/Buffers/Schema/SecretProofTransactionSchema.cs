// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-25-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="CosignatureTransaction.cs" company="Nem.io">
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

using System.Collections.Generic;
using io.nem2.sdk.Infrastructure.Buffers.SchemaHelpers;

namespace io.nem2.sdk.Infrastructure.Buffers.Schema
{
    internal class SecretProofTransactionSchema : SchemaHelpers.Schema
    {
        internal SecretProofTransactionSchema() : base(
            new List<SchemaAttribute> {
                new ScalarAttribute("size", Constants.Value.SIZEOF_INT),
                new ArrayAttribute("signature", Constants.Value.SIZEOF_BYTE),
                new ArrayAttribute("signer", Constants.Value.SIZEOF_BYTE),
                new ScalarAttribute("version", Constants.Value.SIZEOF_SHORT),
                new ScalarAttribute("type", Constants.Value.SIZEOF_SHORT),
                new ArrayAttribute("fee", Constants.Value.SIZEOF_INT),
                new ArrayAttribute("deadline", Constants.Value.SIZEOF_INT),
                new ScalarAttribute("hashAlgorithm", Constants.Value.SIZEOF_BYTE),
                new ArrayAttribute("secret", Constants.Value.SIZEOF_BYTE),
                new ScalarAttribute("proofSize", Constants.Value.SIZEOF_SHORT),
                new ArrayAttribute("proof", Constants.Value.SIZEOF_BYTE),
             
            })
        { }
    }
}
