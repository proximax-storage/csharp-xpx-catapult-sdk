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


using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.Model.Node
{
    /// <summary>
    /// NodeTime class
    /// </summary>
    public class NodeTime
    {
        /// <summary>
        /// NodeTime constructor
        /// </summary>
        /// <param name="sendTimestamp">The send timestamp</param>
        /// <param name="receiveTimestamp">The recieve timestamp</param>
        public NodeTime(ulong sendTimestamp, ulong receiveTimestamp)
        {
           SendTimestamp = sendTimestamp;
           ReceiveTimestamp = receiveTimestamp;
        }

        public ulong SendTimestamp { get; }
        public ulong ReceiveTimestamp { get; }

        /// <summary>
        /// NodeTime from DTO object
        /// </summary>
        /// <param name="dto">The DTO object</param>
        /// <returns>The NodeTime</returns>
        public static NodeTime FromDto(NodeTimeDTO dto)
        {
            var ts = dto.CommunicationTimestamps;
           
            return new NodeTime(ts.SendTimestamp.ToUInt64(),
                  ts.ReceiveTimestamp.ToUInt64());
        }
    }
}
