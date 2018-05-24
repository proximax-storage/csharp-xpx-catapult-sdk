using System;
using System.Collections.Generic;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Blockchain;
using io.nem2.sdk.Model.Mosaics;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.Model.Transactions.Messages;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace test.Model.Transactions
{
    [TestClass]
    public class AggregateTransactionTest
    {
       [TestMethod]
        public void CreateAAggregateTransactionViaStaticConstructor()
        {

            TransferTransaction transferTx = TransferTransaction.Create(
                    NetworkType.Types.MIJIN_TEST,
                    Deadline.CreateHours(2),
                    new Address("SDGLFW-DSHILT-IUHGIB-H5UGX2-VYF5VN-JEKCCD-BR26", NetworkType.Types.MIJIN_TEST),
                    new List<Mosaic>(), 
                    EmptyMessage.Create()
            );

            AggregateTransaction aggregateTx = AggregateTransaction.CreateComplete(
                    NetworkType.Types.MIJIN_TEST,
                    Deadline.CreateHours(2),
                    new List<Transaction>(){transferTx.ToAggregate(new PublicAccount("9A49366406ACA952B88BADF5F1E9BE6CE4968141035A60BE503273EA65456B24", NetworkType.Types.MIJIN_TEST))});

            Assert.AreEqual(NetworkType.Types.MIJIN_TEST, aggregateTx.NetworkType);
            Assert.IsTrue(144 == aggregateTx.NetworkType.GetNetworkByte());
            Assert.IsTrue(DateTime.Now > aggregateTx.Deadline.GetLocalDateTime());
            Assert.AreEqual((ulong)0, aggregateTx.Fee);
            Assert.AreEqual(1, aggregateTx.InnerTransactions.Count);
        }
  
         [TestMethod]
         public void ShouldCreateAggregateTransactionAndSignWithMultipleCosignatories()
         {
    
             TransferTransaction transferTx = TransferTransaction.Create(
                 NetworkType.Types.MIJIN_TEST,
                     FakeDeadline.Create(),
                     new Address("SBILTA367K2LX2FEXG5TFWAS7GEFYAGY7QLFBYKC", NetworkType.Types.MIJIN_TEST),
                     new List<Mosaic>(), 
                     PlainMessage.Create("test-message")
                     
             );
    
             AggregateTransaction aggregateTx = AggregateTransaction.CreateComplete(
                 NetworkType.Types.MIJIN_TEST,
                     FakeDeadline.Create(),
                     new List<Transaction>(){transferTx.ToAggregate(new PublicAccount("B694186EE4AB0558CA4AFCFDD43B42114AE71094F5A1FC4A913FE9971CACD21D", NetworkType.Types.MIJIN_TEST))});
    
             Account cosignatoryAccount = new Account("2a2b1f5d366a5dd5dc56c3c757cf4fe6c66e2787087692cf329d7a49a594658b", NetworkType.Types.MIJIN_TEST);
             Account cosignatoryAccount2 = new Account("b8afae6f4ad13a1b8aad047b488e0738a437c7389d4ff30c359ac068910c1d59", NetworkType.Types.MIJIN_TEST); 
    
             SignedTransaction signedTransaction = cosignatoryAccount.SignTransactionWithCosignatories(aggregateTx, new List<Account>(){ cosignatoryAccount2 });
    
             Assert.AreEqual("2D010000", signedTransaction.Payload.Substring(0, 8));
             Assert.AreEqual("5100000051000000", signedTransaction.Payload.Substring(240, 16));
             
         }
    }
}
