// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 01-29-2018
// ***********************************************************************
// <copyright file="TransactionInfoTests.cs" company="Nem.io">
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

using io.nem2.sdk.Model.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test.Model.Transactions
{
    [TestClass]
    public class TransactionInfoTests
    {
        [TestMethod]
        public void CreateATransactionInfoWithStaticConstructorCreateForTransactionsGetUsingListener()
        {
            TransactionInfo transactionInfo = TransactionInfo.Create(121855,
                    "B6C7648A3DDF71415650805E9E7801424FE03BBEE7D21F9C57B60220D3E95B2F", "B6C7648A3DDF71415650805E9E7801424FE03BBEE7D21F9C57B60220D3E95B2F");

            Assert.AreEqual((ulong)121855, transactionInfo.Height);
            Assert.IsFalse(transactionInfo.Index.HasValue);
            Assert.IsNull(transactionInfo.Id);
            Assert.IsNull(transactionInfo.AggregateHash);
            Assert.IsNull(transactionInfo.AggregateId);
            Assert.IsNotNull(transactionInfo.Hash);
            Assert.IsNotNull(transactionInfo.MerkleComponentHash);

            Assert.AreEqual("B6C7648A3DDF71415650805E9E7801424FE03BBEE7D21F9C57B60220D3E95B2F", transactionInfo.Hash);
            Assert.AreEqual("B6C7648A3DDF71415650805E9E7801424FE03BBEE7D21F9C57B60220D3E95B2F", transactionInfo.MerkleComponentHash);
        }

        [TestMethod]
        public void CreateATransactionInfoWithStaticConstructorCreateForStandaloneTransactions()
        {
            TransactionInfo transactionInfo = TransactionInfo.Create(121855, 1, "5A3D23889CD1E800015929A9",
                    "B6C7648A3DDF71415650805E9E7801424FE03BBEE7D21F9C57B60220D3E95B2F", "B6C7648A3DDF71415650805E9E7801424FE03BBEE7D21F9C57B60220D3E95B2F");

            Assert.AreEqual((ulong)121855, transactionInfo.Height);
            Assert.IsNotNull(transactionInfo.Index);
            Assert.IsNotNull(transactionInfo.Id);
            Assert.IsNull(transactionInfo.AggregateHash);
            Assert.IsNull(transactionInfo.AggregateId);
            Assert.IsNotNull(transactionInfo.Hash);
            Assert.IsNotNull(transactionInfo.MerkleComponentHash);
            Assert.IsNotNull(1 == transactionInfo.Index);
            Assert.AreEqual("5A3D23889CD1E800015929A9", transactionInfo.Id);
            Assert.AreEqual("B6C7648A3DDF71415650805E9E7801424FE03BBEE7D21F9C57B60220D3E95B2F", transactionInfo.Hash);
            Assert.AreEqual("B6C7648A3DDF71415650805E9E7801424FE03BBEE7D21F9C57B60220D3E95B2F", transactionInfo.MerkleComponentHash);
        }


        [TestMethod]
        public void CreateATransactionInfoWithStaticConstructorCreateForAggregateInnerTransactions()
        {
            TransactionInfo transactionInfo = TransactionInfo.CreateAggregate(121855, 1, "5A3D23889CD1E800015929A9",
                    "3D28C804EDD07D5A728E5C5FFEC01AB07AFA5766AE6997B38526D36015A4D006", "5A0069D83F17CF0001777E55");

            Assert.AreEqual((ulong)121855, transactionInfo.Height);
            Assert.IsNotNull(transactionInfo.Index);
            Assert.IsNotNull(transactionInfo.Id);
            Assert.IsNull(transactionInfo.Hash);
            Assert.IsNull(transactionInfo.MerkleComponentHash);
            Assert.IsNotNull(transactionInfo.AggregateHash);
            Assert.IsNotNull(transactionInfo.AggregateId);
            Assert.IsTrue(1 == transactionInfo.Index);
            Assert.AreEqual("5A3D23889CD1E800015929A9", transactionInfo.Id);
            Assert.AreEqual("3D28C804EDD07D5A728E5C5FFEC01AB07AFA5766AE6997B38526D36015A4D006", transactionInfo.AggregateHash);
            Assert.AreEqual("5A0069D83F17CF0001777E55", transactionInfo.AggregateId);
        }
    }
}

