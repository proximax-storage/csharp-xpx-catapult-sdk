﻿// Copyright 2019 ProximaX
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

using System.Collections.Generic;
using ProximaX.Sirius.Chain.Sdk.Model.Lock;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;

namespace ProximaX.Sirius.Chain.Sdk.Model.Accounts
{
    /// <summary>
    ///     Class AccountInfo
    /// </summary>
    public class SecretLockWithMetaInfo
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="metaLock"></param>
        /// <param name="hashLockInfo"></param>
        public SecretLockWithMetaInfo(MetaLock meta, SecretLockInfo secretLock)
        {
            Meta = meta;
            SecretLock = secretLock;
        }

        /// <summary>
        ///     Meta
        /// </summary>
        public MetaLock Meta { get; }

        /// <summary>
        ///     SecretLock
        /// </summary>
        public SecretLockInfo SecretLock { get; }

        public override string ToString()
        {
            return
                $"{nameof(Meta)}: {Meta}, {nameof(SecretLock)}: {SecretLock}";
        }
    }
}