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
using System.Text;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Newtonsoft.Json;

namespace ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO
{

    /// <summary>
    /// The hash algorithm used to hash te proof: * 0 (Op_Sha3_256)  - The proof is hashed using sha3 256. * 1 (Op_Keccak_256)  - The proof is hashed using Keccak (ETH compatibility). * 2 (Op_Hash_160)  - The proof is hashed twice: first with Sha-256 and then with RIPEMD-160 (bitcoin’s OP_HASH160). * 3 (Op_Hash_256)  - The proof is hashed twice with Sha-256 (bitcoin’s OP_HASH256). 
    /// </summary>
    [DataContract]
    public enum HashAlgorithmEnum : int
    {



    }
}
