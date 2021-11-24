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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProximaX.Sirius.Chain.Sdk.Model.Diagnostic
{
    /// <summary>
    ///     Class of ServerInfo
    /// </summary>
    public class ServerInfo
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="ServerInfo" /> class.
        /// </summary>
        /// <param name="restVersion">The server rest version</param>
        /// <param name="sdkVersion">The server SDK version</param>

        public ServerInfo(string restVersion, string sdkVersion)
        {
            RestVersion = restVersion;
            SdkVersion = sdkVersion;
        }

        /// <summary>
        /// The server rest version
        /// </summary>
        public string RestVersion { get; }

        /// <summary>
        /// The server SDK Version
        /// </summary>
        public string SdkVersion { get; }

        /// <summary>
        ///     ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"{nameof(RestVersion)}: {RestVersion}, {nameof(SdkVersion)}: {SdkVersion}";
        }
    }
}