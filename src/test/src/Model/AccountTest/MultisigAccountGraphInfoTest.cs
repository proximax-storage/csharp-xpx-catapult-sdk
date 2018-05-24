//
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
// 

using io.nem2.sdk.Infrastructure.Buffers.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;

namespace test.Model.AccountTest
{
    [TestClass]
    public class MultisigAccountGraphInfoTest
    {
   
        [TestMethod]
        public void ReturnTheLevels()
        {
            var data = "[{\"level\":0,\"multisigEntries\":[{\"multisig\":{\"account\":\"8A2184589237395307FDF9FB6884211BE25EF5FF13ACFBADC430BB01C96485C9\",\"minApproval\":2,\"minRemoval\":2,\"cosignatories\":[\"BEF0589BBD6D9D54C2705D1A04B301DCE654CF3A166EFC84CAF6CF390F6C9AF2\",\"011DDF08E21E3CF7C01F63724F48562F0E5029E0192E00844511A15414191BEA\",\"C72EDA7484D264243B034E975DF21FEAD1938A3B4990368FEA891548BE231FE6\"],\"multisigAccounts\":[]}}]},{\"level\":1,\"multisigEntries\":[{\"multisig\":{\"account\":\"011DDF08E21E3CF7C01F63724F48562F0E5029E0192E00844511A15414191BEA\",\"minApproval\":2,\"minRemoval\":2,\"cosignatories\":[\"28B4C3AA79E5C6409024FB80A4F28489934A269D50CF70E29E7A1221B1BD6F8B\",\"BEF0589BBD6D9D54C2705D1A04B301DCE654CF3A166EFC84CAF6CF390F6C9AF2\",\"C72EDA7484D264243B034E975DF21FEAD1938A3B4990368FEA891548BE231FE6\"],\"multisigAccounts\":[\"610BC5D73525E83DB55EBFE71F6326C813006539983EA7A5BD3EC7BD83C74787\",\"8A2184589237395307FDF9FB6884211BE25EF5FF13ACFBADC430BB01C96485C9\"]}},{\"multisig\":{\"account\":\"BEF0589BBD6D9D54C2705D1A04B301DCE654CF3A166EFC84CAF6CF390F6C9AF2\",\"minApproval\":0,\"minRemoval\":0,\"cosignatories\":[],\"multisigAccounts\":[\"011DDF08E21E3CF7C01F63724F48562F0E5029E0192E00844511A15414191BEA\",\"8A2184589237395307FDF9FB6884211BE25EF5FF13ACFBADC430BB01C96485C9\"]}},{\"multisig\":{\"account\":\"C72EDA7484D264243B034E975DF21FEAD1938A3B4990368FEA891548BE231FE6\",\"minApproval\":0,\"minRemoval\":0,\"cosignatories\":[],\"multisigAccounts\":[\"011DDF08E21E3CF7C01F63724F48562F0E5029E0192E00844511A15414191BEA\",\"8A2184589237395307FDF9FB6884211BE25EF5FF13ACFBADC430BB01C96485C9\"]}}]},{\"level\":2,\"multisigEntries\":[{\"multisig\":{\"account\":\"28B4C3AA79E5C6409024FB80A4F28489934A269D50CF70E29E7A1221B1BD6F8B\",\"minApproval\":0,\"minRemoval\":0,\"cosignatories\":[],\"multisigAccounts\":[\"011DDF08E21E3CF7C01F63724F48562F0E5029E0192E00844511A15414191BEA\"]}},{\"multisig\":{\"account\":\"BEF0589BBD6D9D54C2705D1A04B301DCE654CF3A166EFC84CAF6CF390F6C9AF2\",\"minApproval\":0,\"minRemoval\":0,\"cosignatories\":[],\"multisigAccounts\":[\"011DDF08E21E3CF7C01F63724F48562F0E5029E0192E00844511A15414191BEA\",\"8A2184589237395307FDF9FB6884211BE25EF5FF13ACFBADC430BB01C96485C9\"]}},{\"multisig\":{\"account\":\"C72EDA7484D264243B034E975DF21FEAD1938A3B4990368FEA891548BE231FE6\",\"minApproval\":0,\"minRemoval\":0,\"cosignatories\":[],\"multisigAccounts\":[\"011DDF08E21E3CF7C01F63724F48562F0E5029E0192E00844511A15414191BEA\",\"8A2184589237395307FDF9FB6884211BE25EF5FF13ACFBADC430BB01C96485C9\"]}}]}]";

            var graphInfo = JsonConvert.DeserializeObject<MultisigAccountGraphInfoDTO[]>(data);

            Assert.AreEqual(0, graphInfo[0].Level);
            Assert.AreEqual("8A2184589237395307FDF9FB6884211BE25EF5FF13ACFBADC430BB01C96485C9", graphInfo[0].MultisigEntries[0].Multisig.Account);
            Assert.AreEqual("BEF0589BBD6D9D54C2705D1A04B301DCE654CF3A166EFC84CAF6CF390F6C9AF2", graphInfo[0].MultisigEntries[0].Multisig.Cosignatories[0]);
            Assert.AreEqual(2, graphInfo[0].MultisigEntries[0].Multisig.MinApproval);
            Assert.AreEqual(2, graphInfo[0].MultisigEntries[0].Multisig.MinApproval);
            Assert.IsTrue(graphInfo[0].MultisigEntries[0].Multisig.MultisigAccounts.Length == 0);
        }
    }
}
