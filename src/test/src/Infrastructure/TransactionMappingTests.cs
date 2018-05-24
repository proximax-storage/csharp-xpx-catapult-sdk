using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using io.nem2.sdk.Infrastructure.Buffers.Model;
using io.nem2.sdk.Core.Crypto.Chaso.NaCl;
using io.nem2.sdk.Infrastructure.Mapping;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Blockchain;
using io.nem2.sdk.Model.Mosaics;
using io.nem2.sdk.Model.Namespace;
using io.nem2.sdk.Model.Transactions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace test.Infrastructure
{
    [TestClass]
    public class TransactionMappingTests
    {
        [TestMethod]
        public void ShouldCreateStandaloneTransferTransaction()
        {
            var transferTransactionDto =  "{\"meta\":{\"hash\":\"18C036C20B32348D63684E09A13128A2C18F6A75650D3A5FB43853D716E5E219\",\"height\":[1,0],\"id\":\"59FDA0733F17CF0001772CA7\",\"index\":19,\"merkleComponentHash\":\"18C036C20B32348D63684E09A13128A2C18F6A75650D3A5FB43853D716E5E219\"},\"transaction\":{\"deadline\":[10000,0],\"fee\":[0,0],\"message\":{\"payload\":\"746573742D6D657373616765\",\"type\":0},\"mosaics\":[{\"amount\":[3863990592,95248],\"id\":[3646934825,3576016193]}],\"recipient\":\"9050B9837EFAB4BBE8A4B9BB32D812F9885C00D8FC1650E142\",\"signature\":\"553E696EB4A54E43A11D180EBA57E4B89D0048C9DD2604A9E0608120018B9E02F6EE63025FEEBCED3293B622AF8581334D0BDAB7541A9E7411E7EE4EF0BC5D0E\",\"signer\":\"B4F12E7C9F6946091E2CB8B6D3A12B50D17CCBBF646386EA27CE2946A7423DCF\",\"type\":16724,\"version\":36867}}";
          
            var transferTransaction = new TransactionMapping().Apply(transferTransactionDto);

            ValidateStandaloneTransaction(transferTransaction, transferTransactionDto);
        }

        [TestMethod]
        public void ShouldCreateAggregateTransferTransaction()
        {
            var aggregateTransferTransactionDTO = "{\"meta\":{\"hash\":\"671653C94E2254F2A23EFEDB15D67C38332AED1FBD24B063C0A8E675582B6A96\",\"height\":[18160,0],\"id\":\"5A0069D83F17CF0001777E55\",\"index\":0,\"merkleComponentHash\":\"81E5E7AE49998802DABC816EC10158D3A7879702FF29084C2C992CD1289877A7\"},\"transaction\":{\"cosignatures\":[{\"signature\":\"5780C8DF9D46BA2BCF029DCC5D3BF55FE1CB5BE7ABCF30387C4637DDEDFC2152703CA0AD95F21BB9B942F3CC52FCFC2064C7B84CF60D1A9E69195F1943156C07\",\"signer\":\"A5F82EC8EBB341427B6785C8111906CD0DF18838FB11B51CE0E18B5E79DFF630\"}],\"deadline\":[3266625578,11],\"fee\":[0,0],\"signature\":\"939673209A13FF82397578D22CC96EB8516A6760C894D9B7535E3A1E068007B9255CFA9A914C97142A7AE18533E381C846B69D2AE0D60D1DC8A55AD120E2B606\",\"signer\":\"7681ED5023141D9CDCF184E5A7B60B7D466739918ED5DA30F7E71EA7B86EFF2D\",\"transactions\":[{\"meta\":{\"aggregateHash\":\"3D28C804EDD07D5A728E5C5FFEC01AB07AFA5766AE6997B38526D36015A4D006\",\"aggregateId\":\"5A0069D83F17CF0001777E55\",\"height\":[18160,0],\"id\":\"5A0069D83F17CF0001777E56\",\"index\":0},\"transaction\":{\"message\":{\"payload\":\"746573742D6D657373616765\",\"type\":0},\"mosaics\":[{\"amount\":[3863990592,95248],\"id\":[3646934825,3576016193]}],\"recipient\":\"9050B9837EFAB4BBE8A4B9BB32D812F9885C00D8FC1650E142\",\"signer\":\"B4F12E7C9F6946091E2CB8B6D3A12B50D17CCBBF646386EA27CE2946A7423DCF\",\"type\":16724,\"version\":36867}}],\"type\":16705,\"version\":36867}}";

            var aggregateTransferTransaction = new TransactionMapping().Apply(aggregateTransferTransactionDTO);

            ValidateAggregateTransaction((AggregateTransaction) aggregateTransferTransaction, aggregateTransferTransactionDTO);
        }

       [TestMethod]
        public void ShouldCreateStandaloneRootNamespaceCreationTransaction()
        {
            var namespaceCreationTransactionDTO = "{\"meta\":{\"hash\":\"18C036C20B32348D63684E09A13128A2C18F6A75650D3A5FB43853D716E5E219\",\"height\":[1,0],\"id\":\"59FDA0733F17CF0001772CA7\",\"index\":19,\"merkleComponentHash\":\"18C036C20B32348D63684E09A13128A2C18F6A75650D3A5FB43853D716E5E219\"},\"transaction\":{\"deadline\":[1,0],\"duration\":[1000,0],\"fee\":[0,0],\"name\":\"a2p1mg\",\"namespaceId\":[437145074,4152736179],\"namespaceType\":0,\"signature\":\"553E696EB4A54E43A11D180EBA57E4B89D0048C9DD2604A9E0608120018B9E02F6EE63025FEEBCED3293B622AF8581334D0BDAB7541A9E7411E7EE4EF0BC5D0E\",\"signer\":\"B4F12E7C9F6946091E2CB8B6D3A12B50D17CCBBF646386EA27CE2946A7423DCF\",\"type\":16718,\"version\":36867}}";

            var namespaceCreationTransaction = new TransactionMapping().Apply(namespaceCreationTransactionDTO);

            ValidateStandaloneTransaction(namespaceCreationTransaction, namespaceCreationTransactionDTO);
        }

        
        [TestMethod]
        public void ShouldCreateAggregateRootNamespaceCreationTransaction() 
        {
            var aggregateNamespaceCreationTransactionDTO = "{\"meta\":{\"height\":[35722,0],\"hash\":\"C67A419CE0C64C6E579DCE425E64B4ABDC9C912DB9ECAA760AB12C36C460A23C\",\"merkleComponentHash\":\"C67A419CE0C64C6E579DCE425E64B4ABDC9C912DB9ECAA760AB12C36C460A23C\",\"index\":0,\"id\":\"5B0341A388336A00015D0BB1\"},\"transaction\":{\"signature\":\"CB55E3AA2289CC58D03F4E7BA47114EF82054CFC39D6BCEC833D7E0682A3A8B49331E9AA5934D35698E8593486573508E8F426BFFF32CA513D919A445942E400\",\"signer\":\"B974668ABED344BE9C35EE257ACC246117EFFED939EAF42391AE995912F985FE\",\"version\":36867,\"type\":16705,\"fee\":[0,0],\"deadline\":[3057555945,15],\"cosignatures\":[],\"transactions\":[{\"meta\":{\"height\":[35722,0],\"aggregateHash\":\"C67A419CE0C64C6E579DCE425E64B4ABDC9C912DB9ECAA760AB12C36C460A23C\",\"aggregateId\":\"5B0341A388336A00015D0BB1\",\"index\":0,\"id\":\"5B0341A388336A00015D0BB2\"},\"transaction\":{\"signer\":\"B974668ABED344BE9C35EE257ACC246117EFFED939EAF42391AE995912F985FE\",\"version\":36867,\"type\":16718,\"namespaceType\":0,\"duration\":[10000,0],\"namespaceId\":[4173037395,1069002588],\"name\":\"happy233\"}}]}}";

            var aggregateNamespaceCreationTransaction = new TransactionMapping().Apply(aggregateNamespaceCreationTransactionDTO);

            ValidateAggregateTransaction((AggregateTransaction) aggregateNamespaceCreationTransaction, aggregateNamespaceCreationTransactionDTO);
        }



        [TestMethod]
        public void ShouldCreateStandaloneSubNamespaceCreationTransaction() 
        {
            var namespaceCreationTransactionDTO = "{\"meta\":{\"hash\":\"18C036C20B32348D63684E09A13128A2C18F6A75650D3A5FB43853D716E5E219\",\"height\":[1,0],\"id\":\"59FDA0733F17CF0001772CA7\",\"index\":19,\"merkleComponentHash\":\"18C036C20B32348D63684E09A13128A2C18F6A75650D3A5FB43853D716E5E219\"},\"transaction\":{\"deadline\":[1,0],\"fee\":[0,0],\"name\":\"0unius\",\"namespaceId\":[1970060410,3289875941],\"namespaceType\":1,\"parentId\":[3316183705,3829351378],\"signature\":\"553E696EB4A54E43A11D180EBA57E4B89D0048C9DD2604A9E0608120018B9E02F6EE63025FEEBCED3293B622AF8581334D0BDAB7541A9E7411E7EE4EF0BC5D0E\",\"signer\":\"B4F12E7C9F6946091E2CB8B6D3A12B50D17CCBBF646386EA27CE2946A7423DCF\",\"type\":16718,\"version\":36867}}";

            var namespaceCreationTransaction = new TransactionMapping().Apply(namespaceCreationTransactionDTO);

            ValidateStandaloneTransaction(namespaceCreationTransaction, namespaceCreationTransactionDTO);
        }

        [TestMethod]
        public void ShouldCreateAggregateSubNamespaceCreationTransaction() 
        {
            var aggregateNamespaceCreationTransactionDTO = "{\"meta\":{\"height\":[35835,0],\"hash\":\"4443519A5A23E2BC37D8B6043FF600A7935D13079D4E476DE8E04A4CBC16C024\",\"merkleComponentHash\":\"4443519A5A23E2BC37D8B6043FF600A7935D13079D4E476DE8E04A4CBC16C024\",\"index\":0,\"id\":\"5B03484988336A00015D0C29\"},\"transaction\":{\"signature\":\"504A6AF90D80A4EC6E7F103642D88EA9A9B74F42813501BBA65008A097C704A42CE5D4093BE0D9D9129ABFD9D290F2630C7F3BCC4E53410B7AD5FF05F376EC0C\",\"signer\":\"B974668ABED344BE9C35EE257ACC246117EFFED939EAF42391AE995912F985FE\",\"version\":36867,\"type\":16705,\"fee\":[0,0],\"deadline\":[3059256257,15],\"cosignatures\":[],\"transactions\":[{\"meta\":{\"height\":[35835,0],\"aggregateHash\":\"4443519A5A23E2BC37D8B6043FF600A7935D13079D4E476DE8E04A4CBC16C024\",\"aggregateId\":\"5B03484988336A00015D0C29\",\"index\":0,\"id\":\"5B03484988336A00015D0C2A\"},\"transaction\":{\"signer\":\"B974668ABED344BE9C35EE257ACC246117EFFED939EAF42391AE995912F985FE\",\"version\":36867,\"type\":16718,\"namespaceType\":1,\"parentId\":[4173037395,1069002588],\"namespaceId\":[4105337455,2403927416],\"name\":\"testing1\"}}]}}";

            var aggregateNamespaceCreationTransaction = new TransactionMapping().Apply(aggregateNamespaceCreationTransactionDTO);

            ValidateAggregateTransaction((AggregateTransaction) aggregateNamespaceCreationTransaction, aggregateNamespaceCreationTransactionDTO);
        }

        [TestMethod]
        public void ShouldCreateStandaloneMosaicCreationTransaction()
        {
            var mosaicCreationTransactionDTO = "{\"meta\":{\"hash\":\"18C036C20B32348D63684E09A13128A2C18F6A75650D3A5FB43853D716E5E219\",\"height\":[1,0],\"id\":\"59FDA0733F17CF0001772CA7\",\"index\":19,\"merkleComponentHash\":\"18C036C20B32348D63684E09A13128A2C18F6A75650D3A5FB43853D716E5E219\"},\"transaction\":{\"deadline\":[1,0],\"fee\":[0,0],\"mosaicId\":[3248159581,740240531],\"name\":\"ie7rfaqxiorum1jor\",\"parentId\":[3316183705,3829351378],\"properties\":[{\"id\":0,\"value\":[7,0]},{\"id\":1,\"value\":[6,0]},{\"id\":2,\"value\":[1000,0]}],\"signature\":\"553E696EB4A54E43A11D180EBA57E4B89D0048C9DD2604A9E0608120018B9E02F6EE63025FEEBCED3293B622AF8581334D0BDAB7541A9E7411E7EE4EF0BC5D0E\",\"signer\":\"B4F12E7C9F6946091E2CB8B6D3A12B50D17CCBBF646386EA27CE2946A7423DCF\",\"type\":16717,\"version\":36867}}";

            var mosaicCreationTransaction = new TransactionMapping().Apply(mosaicCreationTransactionDTO);

            ValidateStandaloneTransaction(mosaicCreationTransaction, mosaicCreationTransactionDTO);
        }

        [TestMethod]
        public void shouldCreateAggregateMosaicCreationTransaction()
        {
            var aggregateMosaicCreationTransactionDTO = "{\"meta\":{\"hash\":\"671653C94E2254F2A23EFEDB15D67C38332AED1FBD24B063C0A8E675582B6A96\",\"height\":[18160,0],\"id\":\"5A0069D83F17CF0001777E55\",\"index\":0,\"merkleComponentHash\":\"81E5E7AE49998802DABC816EC10158D3A7879702FF29084C2C992CD1289877A7\"},\"transaction\":{\"cosignatures\":[{\"signature\":\"5780C8DF9D46BA2BCF029DCC5D3BF55FE1CB5BE7ABCF30387C4637DDEDFC2152703CA0AD95F21BB9B942F3CC52FCFC2064C7B84CF60D1A9E69195F1943156C07\",\"signer\":\"A5F82EC8EBB341427B6785C8111906CD0DF18838FB11B51CE0E18B5E79DFF630\"}],\"deadline\":[3266625578,11],\"fee\":[0,0],\"signature\":\"939673209A13FF82397578D22CC96EB8516A6760C894D9B7535E3A1E068007B9255CFA9A914C97142A7AE18533E381C846B69D2AE0D60D1DC8A55AD120E2B606\",\"signer\":\"7681ED5023141D9CDCF184E5A7B60B7D466739918ED5DA30F7E71EA7B86EFF2D\",\"transactions\":[{\"meta\":{\"aggregateHash\":\"3D28C804EDD07D5A728E5C5FFEC01AB07AFA5766AE6997B38526D36015A4D006\",\"aggregateId\":\"5A0069D83F17CF0001777E55\",\"height\":[18160,0],\"id\":\"5A0069D83F17CF0001777E56\",\"index\":0},\"transaction\":{\"mosaicId\":[3248159581,740240531],\"name\":\"ie7rfaqxiorum1jor\",\"parentId\":[3316183705,3829351378],\"properties\":[{\"id\":0,\"value\":[7,0]},{\"id\":1,\"value\":[6,0]},{\"id\":2,\"value\":[1000,0]}],\"signer\":\"B4F12E7C9F6946091E2CB8B6D3A12B50D17CCBBF646386EA27CE2946A7423DCF\",\"type\":16717,\"version\":36867}}],\"type\":16705,\"version\":36867}}";

            var aggregateMosaicCreationTransaction = new TransactionMapping().Apply(aggregateMosaicCreationTransactionDTO);

            ValidateAggregateTransaction((AggregateTransaction) aggregateMosaicCreationTransaction, aggregateMosaicCreationTransactionDTO);
        }

        [TestMethod]
        public void ShouldCreateStandaloneMosaicSupplyChangeTransaction()
        {
            var supplyChangeTransactionDTO = "{\"meta\":{\"hash\":\"18C036C20B32348D63684E09A13128A2C18F6A75650D3A5FB43853D716E5E219\",\"height\":[1,0],\"id\":\"59FDA0733F17CF0001772CA7\",\"index\":19,\"merkleComponentHash\":\"18C036C20B32348D63684E09A13128A2C18F6A75650D3A5FB43853D716E5E219\"},\"transaction\":{\"deadline\":[1,0],\"delta\":[100000,0],\"direction\":1,\"fee\":[0,0],\"mosaicId\":[3070467832,2688515262],\"signature\":\"553E696EB4A54E43A11D180EBA57E4B89D0048C9DD2604A9E0608120018B9E02F6EE63025FEEBCED3293B622AF8581334D0BDAB7541A9E7411E7EE4EF0BC5D0E\",\"signer\":\"B4F12E7C9F6946091E2CB8B6D3A12B50D17CCBBF646386EA27CE2946A7423DCF\",\"type\":16973,\"version\":36867}}";
            
            var mosaicSupplyChange = new TransactionMapping().Apply(supplyChangeTransactionDTO);

            ValidateStandaloneTransaction(mosaicSupplyChange, supplyChangeTransactionDTO);
        }

        [TestMethod]
        public void ShouldCreateAggregateMosaicSupplyChangeTransaction()
        {
            var aggregateMosaicSupplyChangeTransactionDTO = "{\"meta\":{\"hash\":\"671653C94E2254F2A23EFEDB15D67C38332AED1FBD24B063C0A8E675582B6A96\",\"height\":[18160,0],\"id\":\"5A0069D83F17CF0001777E55\",\"index\":0,\"merkleComponentHash\":\"81E5E7AE49998802DABC816EC10158D3A7879702FF29084C2C992CD1289877A7\"},\"transaction\":{\"cosignatures\":[{\"signature\":\"5780C8DF9D46BA2BCF029DCC5D3BF55FE1CB5BE7ABCF30387C4637DDEDFC2152703CA0AD95F21BB9B942F3CC52FCFC2064C7B84CF60D1A9E69195F1943156C07\",\"signer\":\"A5F82EC8EBB341427B6785C8111906CD0DF18838FB11B51CE0E18B5E79DFF630\"}],\"deadline\":[3266625578,11],\"fee\":[0,0],\"signature\":\"939673209A13FF82397578D22CC96EB8516A6760C894D9B7535E3A1E068007B9255CFA9A914C97142A7AE18533E381C846B69D2AE0D60D1DC8A55AD120E2B606\",\"signer\":\"7681ED5023141D9CDCF184E5A7B60B7D466739918ED5DA30F7E71EA7B86EFF2D\",\"transactions\":[{\"meta\":{\"aggregateHash\":\"3D28C804EDD07D5A728E5C5FFEC01AB07AFA5766AE6997B38526D36015A4D006\",\"aggregateId\":\"5A0069D83F17CF0001777E55\",\"height\":[18160,0],\"id\":\"5A0069D83F17CF0001777E56\",\"index\":0},\"transaction\":{\"delta\":[100000,0],\"direction\":1,\"mosaicId\":[3070467832,2688515262],\"signer\":\"B4F12E7C9F6946091E2CB8B6D3A12B50D17CCBBF646386EA27CE2946A7423DCF\",\"type\":16973,\"version\":36867}}],\"type\":16705,\"version\":36867}}";

            var aggregateMosaicSupplyChangeTransaction = new TransactionMapping().Apply(aggregateMosaicSupplyChangeTransactionDTO);

            ValidateAggregateTransaction((AggregateTransaction) aggregateMosaicSupplyChangeTransaction, aggregateMosaicSupplyChangeTransactionDTO);
        }

        [TestMethod]
        public void ShouldCreateStandaloneMultisigModificationTransaction() 
        {
            var multisigModificationTransactionDTO = "{\"meta\":{\"hash\":\"18C036C20B32348D63684E09A13128A2C18F6A75650D3A5FB43853D716E5E219\",\"height\":[1,0],\"id\":\"59FDA0733F17CF0001772CA7\",\"index\":19,\"merkleComponentHash\":\"18C036C20B32348D63684E09A13128A2C18F6A75650D3A5FB43853D716E5E219\"},\"transaction\":{\"deadline\":[1,0],\"fee\":[0,0],\"minApprovalDelta\":1,\"minRemovalDelta\":1,\"modifications\":[{\"cosignatoryPublicKey\":\"589B73FBC22063E9AE6FBAC67CB9C6EA865EF556E5FB8B7310D45F77C1250B97\",\"type\":0}],\"signature\":\"553E696EB4A54E43A11D180EBA57E4B89D0048C9DD2604A9E0608120018B9E02F6EE63025FEEBCED3293B622AF8581334D0BDAB7541A9E7411E7EE4EF0BC5D0E\",\"signer\":\"B4F12E7C9F6946091E2CB8B6D3A12B50D17CCBBF646386EA27CE2946A7423DCF\",\"type\":16725,\"version\":36867}}";

            var multisigModificationTransaction = new TransactionMapping().Apply(multisigModificationTransactionDTO);

            ValidateStandaloneTransaction(multisigModificationTransaction, multisigModificationTransactionDTO);
        }

        [TestMethod]
        public void ShouldCreateAggregateMultisigModificationTransaction() 
        {
            var aggregateMultisigModificationTransactionDTO = "{\"meta\":{\"hash\":\"671653C94E2254F2A23EFEDB15D67C38332AED1FBD24B063C0A8E675582B6A96\",\"height\":[18160,0],\"id\":\"5A0069D83F17CF0001777E55\",\"index\":0,\"merkleComponentHash\":\"81E5E7AE49998802DABC816EC10158D3A7879702FF29084C2C992CD1289877A7\"},\"transaction\":{\"cosignatures\":[{\"signature\":\"5780C8DF9D46BA2BCF029DCC5D3BF55FE1CB5BE7ABCF30387C4637DDEDFC2152703CA0AD95F21BB9B942F3CC52FCFC2064C7B84CF60D1A9E69195F1943156C07\",\"signer\":\"A5F82EC8EBB341427B6785C8111906CD0DF18838FB11B51CE0E18B5E79DFF630\"}],\"deadline\":[3266625578,11],\"fee\":[0,0],\"signature\":\"939673209A13FF82397578D22CC96EB8516A6760C894D9B7535E3A1E068007B9255CFA9A914C97142A7AE18533E381C846B69D2AE0D60D1DC8A55AD120E2B606\",\"signer\":\"7681ED5023141D9CDCF184E5A7B60B7D466739918ED5DA30F7E71EA7B86EFF2D\",\"transactions\":[{\"meta\":{\"aggregateHash\":\"3D28C804EDD07D5A728E5C5FFEC01AB07AFA5766AE6997B38526D36015A4D006\",\"aggregateId\":\"5A0069D83F17CF0001777E55\",\"height\":[18160,0],\"id\":\"5A0069D83F17CF0001777E56\",\"index\":0},\"transaction\":{\"minApprovalDelta\":1,\"minRemovalDelta\":1,\"modifications\":[{\"cosignatoryPublicKey\":\"589B73FBC22063E9AE6FBAC67CB9C6EA865EF556E5FB8B7310D45F77C1250B97\",\"type\":0}],\"signer\":\"B4F12E7C9F6946091E2CB8B6D3A12B50D17CCBBF646386EA27CE2946A7423DCF\",\"type\":16725,\"version\":36867}}],\"type\":16705,\"version\":36867}}";

            var aggregateMultisigModificationTransaction = new TransactionMapping().Apply(aggregateMultisigModificationTransactionDTO);

            ValidateAggregateTransaction((AggregateTransaction) aggregateMultisigModificationTransaction, aggregateMultisigModificationTransactionDTO);
        }

        [TestMethod]
        public void ShouldCreateStandaloneLockFundsTransaction()
        {
            var lockFundsTransactionDTO = "{\"meta\": {\"height\": [22115,0],\"hash\": \"796602E7AA17E1BECD6A0302AD18CC4AE9CB8B2C5DF4EE602C80F0A98120238D\",\"merkleComponentHash\": \"796602E7AA17E1BECD6A0302AD18CC4AE9CB8B2C5DF4EE602C80F0A98120238D\",\"index\": 0,\"id\": \"5A86F7FF5F8AE10001776B6C\"},\"transaction\": {\"signature\": \"298C9BB956C318431FD7BE912480DE57B0A997820A8F85DA824A5A0B81B63E8A58AB31936B371A6B500E0CBDE59C00A56B62F127EAA3E2BE3DF6F5C27FD3BD07\",\"signer\": \"1026D70E1954775749C6811084D6450A3184D977383F0E4282CD47118AF37755\",\"version\": 36867,\"type\": 16716,\"fee\": [0,0],\"deadline\": [3498561481,13],\"duration\": [100,0],\"mosaicId\": [3646934825,3576016193],\"amount\": [10000000,0],\"hash\": \"49E9F58867FB9399F32316B99CCBC301A5790E5E0605E25F127D28CEF99740A3\"}}";

            var lockFundsTransaction = new TransactionMapping().Apply(lockFundsTransactionDTO);

            ValidateStandaloneTransaction(lockFundsTransaction, lockFundsTransactionDTO);
        }

        [TestMethod]
        public void ShouldCreateAggregateLockFundsTransaction()
        {
            var aggregateLockFundsTransactionDTO = "{\"meta\": {\"hash\": \"671653C94E2254F2A23EFEDB15D67C38332AED1FBD24B063C0A8E675582B6A96\",\"height\": [18160,0],\"id\": \"5A0069D83F17CF0001777E55\",\"index\": 0,\"merkleComponentHash\": \"81E5E7AE49998802DABC816EC10158D3A7879702FF29084C2C992CD1289877A7\"},\"transaction\": {\"cosignatures\": [{\"signature\": \"5780C8DF9D46BA2BCF029DCC5D3BF55FE1CB5BE7ABCF30387C4637DDEDFC2152703CA0AD95F21BB9B942F3CC52FCFC2064C7B84CF60D1A9E69195F1943156C07\",\"signer\": \"A5F82EC8EBB341427B6785C8111906CD0DF18838FB11B51CE0E18B5E79DFF630\"}],\"deadline\": [3266625578,11],\"fee\": [0,0],\"signature\": \"939673209A13FF82397578D22CC96EB8516A6760C894D9B7535E3A1E068007B9255CFA9A914C97142A7AE18533E381C846B69D2AE0D60D1DC8A55AD120E2B606\",\"signer\": \"7681ED5023141D9CDCF184E5A7B60B7D466739918ED5DA30F7E71EA7B86EFF2D\",\"transactions\": [{\"meta\": {\"aggregateHash\": \"3D28C804EDD07D5A728E5C5FFEC01AB07AFA5766AE6997B38526D36015A4D006\",\"aggregateId\": \"5A0069D83F17CF0001777E55\",\"height\": [18160,0],\"id\": \"5A0069D83F17CF0001777E56\",\"index\": 0},\"transaction\": {\"signer\": \"1026D70E1954775749C6811084D6450A3184D977383F0E4282CD47118AF37755\",\"version\": 36867,\"type\": 16716,\"duration\": [100,0],\"mosaicId\": [3646934825,3576016193],\"amount\": [10000000,0],\"hash\": \"49E9F58867FB9399F32316B99CCBC301A5790E5E0605E25F127D28CEF99740A3\"}}],\"type\": 16705,\"version\": 36867}}";

            var lockFundsTransaction = new TransactionMapping().Apply(aggregateLockFundsTransactionDTO);

            ValidateAggregateTransaction((AggregateTransaction) lockFundsTransaction, aggregateLockFundsTransactionDTO);
        }

        [TestMethod]
        public void ShouldCreateStandaloneSecretLockTransaction()
        {
            var secretLockTransactionDTO = "{\"meta\": {\"height\": [22211,0],\"hash\": \"B802E29269DC8DF68B63D8C802092D51854C42253E6F8083AE3304C17C0BEAF3\",\"merkleComponentHash\": \"B802E29269DC8DF68B63D8C802092D51854C42253E6F8083AE3304C17C0BEAF3\",\"index\": 0,\"id\": \"5A86FDCE5F8AE10001776BCF\"},\"transaction\": {\"signature\": \"9D66CA66BE5D02775A6ACD8913DC39D422FD60D36F1E67CEDE8B8615AD3258B2B1C9DBABA13208F571F2DD10C70B76DB6963E9BA237AC5281C2E2549B1F2D602\",\"signer\": \"846B4439154579A5903B1459C9CF69CB8153F6D0110A7A0ED61DE29AE4810BF2\",\"version\": 36867,\"type\": 16972,\"fee\": [0,0],\"deadline\": [3496454111,13],\"duration\": [100,0],\"mosaicId\": [3646934825,3576016193],\"amount\": [10000000,0],\"hashAlgorithm\": 0,\"secret\": \"428A9DEB1DC6B938AD7C83617E4A558D5316489ADE176AE0C821568A2AD6F700470901532716F83D43F2A7240FBB2C34BDD9536BCF6CC7601904782C385CD8B4\",\"recipient\": \"90C9B099BAEBB743A4D2D8D3B1520F6DD0A0E9D6C9D968C155\"}}";

            var secretLockTransaction = new TransactionMapping().Apply(secretLockTransactionDTO);

            ValidateStandaloneTransaction(secretLockTransaction, secretLockTransactionDTO);
        }

        [TestMethod]
        public void ShouldCreateAggregateSecretLockTransaction() 
        {
            var aggregateSecretLockTransactionDTO = "{\"meta\": {\"hash\": \"671653C94E2254F2A23EFEDB15D67C38332AED1FBD24B063C0A8E675582B6A96\",\"height\": [18160,0],\"id\": \"5A0069D83F17CF0001777E55\",\"index\": 0,\"merkleComponentHash\": \"81E5E7AE49998802DABC816EC10158D3A7879702FF29084C2C992CD1289877A7\"},\"transaction\": {\"cosignatures\": [{\"signature\": \"5780C8DF9D46BA2BCF029DCC5D3BF55FE1CB5BE7ABCF30387C4637DDEDFC2152703CA0AD95F21BB9B942F3CC52FCFC2064C7B84CF60D1A9E69195F1943156C07\",\"signer\": \"A5F82EC8EBB341427B6785C8111906CD0DF18838FB11B51CE0E18B5E79DFF630\"}],\"deadline\": [3266625578,11],\"fee\": [0,0],\"signature\": \"939673209A13FF82397578D22CC96EB8516A6760C894D9B7535E3A1E068007B9255CFA9A914C97142A7AE18533E381C846B69D2AE0D60D1DC8A55AD120E2B606\",\"signer\": \"7681ED5023141D9CDCF184E5A7B60B7D466739918ED5DA30F7E71EA7B86EFF2D\",\"transactions\": [{\"meta\": {\"aggregateHash\": \"3D28C804EDD07D5A728E5C5FFEC01AB07AFA5766AE6997B38526D36015A4D006\",\"aggregateId\": \"5A0069D83F17CF0001777E55\",\"height\": [18160,0],\"id\": \"5A0069D83F17CF0001777E56\",\"index\": 0},\"transaction\": {\"signer\": \"846B4439154579A5903B1459C9CF69CB8153F6D0110A7A0ED61DE29AE4810BF2\",\"version\": 36867,\"type\": 16972,\"duration\": [100,0],\"mosaicId\": [3646934825,3576016193],\"amount\": [10000000,0],\"hashAlgorithm\": 0,\"secret\": \"428A9DEB1DC6B938AD7C83617E4A558D5316489ADE176AE0C821568A2AD6F700470901532716F83D43F2A7240FBB2C34BDD9536BCF6CC7601904782C385CD8B4\",\"recipient\": \"90C9B099BAEBB743A4D2D8D3B1520F6DD0A0E9D6C9D968C155\"}}],\"type\": 16705,\"version\": 36867}}";

            var aggregateSecretLockTransaction = new TransactionMapping().Apply(aggregateSecretLockTransactionDTO);

            ValidateAggregateTransaction((AggregateTransaction) aggregateSecretLockTransaction, aggregateSecretLockTransactionDTO);
        }

        [TestMethod]
        public void ShouldCreateStandaloneSecretProofTransaction()
        {
            var secretProofTransactionDTO = "{\"meta\": {\"height\": [22212,0],\"hash\": \"A1BBEF9DF8F5170B43AFBB38BCA9140F38C7234C6F4AB306458F6AF2E2F0234A\",\"merkleComponentHash\": \"A1BBEF9DF8F5170B43AFBB38BCA9140F38C7234C6F4AB306458F6AF2E2F0234A\",\"index\": 0,\"id\": \"5A86FDDA5F8AE10001776BD2\"},\"transaction\": {\"signature\": \"7C52EA06C71843FD6B1AE30A04FECD53C0B78FE8A8A2925D96FE528401255CBB3F3156C99F2F3E4DEF01CD38A014B677AD4DB78733929C0C96BC28FD7D508D05\",\"signer\": \"74A6BD39F42535AA3608924A517A88E3B2C36B2DFC296CB379604A3FEE01C7B8\",\"version\": 36867,\"type\": 17228,\"fee\": [0,0],\"deadline\": [3496462687,13],\"hashAlgorithm\": 0,\"secret\": \"428A9DEB1DC6B938AD7C83617E4A558D5316489ADE176AE0C821568A2AD6F700470901532716F83D43F2A7240FBB2C34BDD9536BCF6CC7601904782C385CD8B4\",\"proof\": \"E08664BF179B064D9E3B\"}}";

            var secretProofTransaction = new TransactionMapping().Apply(secretProofTransactionDTO);

            ValidateStandaloneTransaction(secretProofTransaction, secretProofTransactionDTO);
        }

        [TestMethod]
        public void ShouldCreateAggregateSecretProofTransaction()
        {
            var aggregateSecretProofTransactionDTO = "{\"meta\": {\"hash\": \"671653C94E2254F2A23EFEDB15D67C38332AED1FBD24B063C0A8E675582B6A96\",\"height\": [18160,0],\"id\": \"5A0069D83F17CF0001777E55\",\"index\": 0,\"merkleComponentHash\": \"81E5E7AE49998802DABC816EC10158D3A7879702FF29084C2C992CD1289877A7\"},\"transaction\": {\"cosignatures\": [{\"signature\": \"5780C8DF9D46BA2BCF029DCC5D3BF55FE1CB5BE7ABCF30387C4637DDEDFC2152703CA0AD95F21BB9B942F3CC52FCFC2064C7B84CF60D1A9E69195F1943156C07\",\"signer\": \"A5F82EC8EBB341427B6785C8111906CD0DF18838FB11B51CE0E18B5E79DFF630\"}],\"deadline\": [3266625578,11],\"fee\": [0,0],\"signature\": \"939673209A13FF82397578D22CC96EB8516A6760C894D9B7535E3A1E068007B9255CFA9A914C97142A7AE18533E381C846B69D2AE0D60D1DC8A55AD120E2B606\",\"signer\": \"7681ED5023141D9CDCF184E5A7B60B7D466739918ED5DA30F7E71EA7B86EFF2D\",\"transactions\": [{\"meta\": {\"aggregateHash\": \"3D28C804EDD07D5A728E5C5FFEC01AB07AFA5766AE6997B38526D36015A4D006\",\"aggregateId\": \"5A0069D83F17CF0001777E55\",\"height\": [18160,0],\"id\": \"5A0069D83F17CF0001777E56\",\"index\": 0},\"transaction\": {\"signer\": \"74A6BD39F42535AA3608924A517A88E3B2C36B2DFC296CB379604A3FEE01C7B8\",\"version\": 36867,\"type\": 17228,\"hashAlgorithm\": 0,\"secret\": \"428A9DEB1DC6B938AD7C83617E4A558D5316489ADE176AE0C821568A2AD6F700470901532716F83D43F2A7240FBB2C34BDD9536BCF6CC7601904782C385CD8B4\",\"proof\": \"E08664BF179B064D9E3B\"}}],\"type\": 16705,\"version\": 36867}}";

            var aggregateSecretProofTransaction = new TransactionMapping().Apply(aggregateSecretProofTransactionDTO);

            ValidateAggregateTransaction((AggregateTransaction) aggregateSecretProofTransaction, aggregateSecretProofTransactionDTO);
        }

        private void ValidateStandaloneTransaction(Transaction transaction, string transactionDTO)
        {
            var tx = JsonConvert.DeserializeObject<TransactionInfoDTO>(transactionDTO);

            Assert.AreEqual(tx.Meta.Height, transaction.TransactionInfo.Height);

            if (transaction.TransactionInfo.Hash != null)
            {
                Assert.AreEqual(tx.Meta.Hash, transaction.TransactionInfo.Hash);
            }
            if (transaction.TransactionInfo.MerkleComponentHash != null)
            {
                Assert.AreEqual(tx.Meta.MerkleComponentHash, transaction.TransactionInfo.MerkleComponentHash);
            }
            if (transaction.TransactionInfo.Index != null)
            {
                Assert.AreEqual(transaction.TransactionInfo.Index, tx.Meta.Index);
            }
            if (transaction.TransactionInfo.Id != null)
            {
                Assert.AreEqual(tx.Meta.Id, transaction.TransactionInfo.Id);
            }

            Assert.IsNotNull(transaction.Signature);
            Assert.AreEqual(tx.Transaction.Signer, transaction.Signer.PublicKey);
            Assert.IsTrue(transaction.TransactionType.GetValue() == tx.Transaction.Type);
            int version = (int)Convert.ToInt64(tx.Transaction.Version.ToString("X").Substring(2, 2), 16);
            Assert.IsTrue(version == transaction.Version);
            int type = (int)Convert.ToInt64(tx.Transaction.Version.ToString("X").Substring(0, 2), 16);
            Assert.IsTrue((byte)type == transaction.NetworkType.GetNetworkByte());
            Assert.AreEqual(tx.Transaction.Fee, transaction.Fee);
            Assert.IsNotNull(transaction.Deadline);

            if (transaction.TransactionType == TransactionTypes.Types.Transfer)
            {
                ValidateTransferTx((TransferTransaction)transaction, transactionDTO);
            }
            else if (transaction.TransactionType == TransactionTypes.Types.RegisterNamespace)
            {
                ValidateNamespaceCreationTx((RegisterNamespaceTransaction)transaction, transactionDTO);
            }
            else if (transaction.TransactionType == TransactionTypes.Types.MosaicDefinition)
            {
                ValidateMosaicCreationTx((MosaicDefinitionTransaction)transaction, transactionDTO);
            }
            else if (transaction.TransactionType == TransactionTypes.Types.MosaicSupplyChange)
            {
                ValidateMosaicSupplyChangeTx((MosaicSupplyChangeTransaction)transaction, transactionDTO);
            }
            else if (transaction.TransactionType == TransactionTypes.Types.ModifyMultisigAccount)
            {
                ValidateMultisigModificationTx((ModifyMultisigAccountTransaction)transaction, transactionDTO);
            }
           else if (transaction.TransactionType == TransactionTypes.Types.LockFunds) 
           {
               ValidateLockFundsTx((LockFundsTransaction)transaction, transactionDTO);
           }
           else if (transaction.TransactionType == TransactionTypes.Types.SecretLock)
           {
               ValidateSecretLockTx((SecretLockTransaction)transaction, transactionDTO);
           }
           else if (transaction.TransactionType == TransactionTypes.Types.SecretProof)
           {
               validateSecretProofTx((SecretProofTransaction)transaction, transactionDTO);
           }
        }

        private void ValidateAggregateTransaction(AggregateTransaction aggregateTransaction, string aggregateTransactionDTO)
        {
            
            var tx = JsonConvert.DeserializeObject<AggregateTransactionInfoDTO>(aggregateTransactionDTO);

            Assert.AreEqual(tx.Meta.Height, aggregateTransaction.TransactionInfo.Height);

            if (aggregateTransaction.TransactionInfo.Hash != null)
            {
                Assert.AreEqual(tx.Meta.Hash, aggregateTransaction.TransactionInfo.Hash);
            }
            if (aggregateTransaction.TransactionInfo.MerkleComponentHash != null)
            {
                Assert.AreEqual(tx.Meta.MerkleComponentHash, aggregateTransaction.TransactionInfo.MerkleComponentHash);
            }
            if (aggregateTransaction.TransactionInfo.Index != null)
            {
                Assert.AreEqual(tx.Meta.Index, aggregateTransaction.TransactionInfo.Index);
            }
            if (aggregateTransaction.TransactionInfo.Id != null)
            {
                Assert.AreEqual(tx.Meta.Id, aggregateTransaction.TransactionInfo.Id);
            }
            if (aggregateTransaction.TransactionInfo.AggregateHash != null)
            {
                Assert.AreEqual(tx.Transaction.InnerTransactions[0].Meta.AggregateHash, aggregateTransaction.InnerTransactions[0].TransactionInfo.AggregateHash);               
            }
            if (aggregateTransaction.TransactionInfo.AggregateId != null)
            {
                Assert.AreEqual(tx.Transaction.InnerTransactions[0].Meta.AggregateId, aggregateTransaction.TransactionInfo.AggregateId);
            }

            Assert.AreEqual(tx.Transaction.Signature, aggregateTransaction.Signature);
            Assert.AreEqual(tx.Transaction.Signer, aggregateTransaction.Signer.PublicKey);
            int version = (int)Convert.ToInt64(tx.Transaction.Version.ToString("X").Substring(2, 2), 16);
            Assert.IsTrue(version == aggregateTransaction.Version);
            int type = (int)Convert.ToInt64(tx.Transaction.Version.ToString("X").Substring(0, 2), 16);
            Assert.IsTrue((byte)type == aggregateTransaction.NetworkType.GetNetworkByte());
            Assert.AreEqual(tx.Transaction.Fee, aggregateTransaction.Fee);
            Assert.IsNotNull(aggregateTransaction.Deadline);
           
            if (tx.Transaction.Cosignatures.Count > 0)
            {
                Assert.AreEqual(tx.Transaction.Cosignatures[0].Signature, aggregateTransaction.Cosignatures[0].Signature);
                Assert.AreEqual(tx.Transaction.Cosignatures[0].Signer, aggregateTransaction.Cosignatures[0].Signer.PublicKey);
            }

            for (var index = 0; index < aggregateTransaction.InnerTransactions.Count; index++)
            {
                ValidateStandaloneTransaction(aggregateTransaction.InnerTransactions[index], JObject.Parse(aggregateTransactionDTO)["transaction"]["transactions"].ToObject<List<JObject>>()[index].ToString());
            }
            
        }

        private void validateSecretProofTx(SecretProofTransaction transaction, string transactionDTO)
        {
            var tx = JsonConvert.DeserializeObject<SecretProofTransactionInfoDTO>(transactionDTO);

            Assert.IsTrue(HashType.GetRawValue(tx.Transaction.HashAlgorithm) == transaction.HashAlgo);
            Assert.AreEqual(transaction.SecretString(), tx.Transaction.Secret);
            Assert.AreEqual(tx.Transaction.Proof, transaction.ProofString());
        }

        private void ValidateSecretLockTx(SecretLockTransaction transaction, string transactionDTO)
        {

            var tx = JsonConvert.DeserializeObject<SecretLockTransactionInfoDTO>(transactionDTO);

            Assert.AreEqual(tx.Transaction.MosaicId, transaction.Mosaic.MosaicId.Id);
            Assert.AreEqual(tx.Transaction.Amount, transaction.Mosaic.Amount);
            Assert.AreEqual(tx.Transaction.Duration, transaction.Duration);
            Assert.IsTrue(tx.Transaction.HashAlgorithm == transaction.HashAlgo.GetHashTypeValue());
            Assert.AreEqual(tx.Transaction.Secret, transaction.SecretString());
            Assert.AreEqual(Address.CreateFromHex(tx.Transaction.Recipient).Plain, transaction.Recipient.Plain);
        }

        void ValidateMultisigModificationTx(ModifyMultisigAccountTransaction transaction, string transactionDTO)
        {
            var tx = JsonConvert.DeserializeObject<MultisigModificationTransactionInfoDTO>(transactionDTO);

            Assert.IsTrue(tx.Transaction.MinApprovalDelta == transaction.MinApprovalDelta);
            Assert.IsTrue(tx.Transaction.MinRemovalDelta == transaction.MinRemovalDelta);
            Assert.AreEqual(transaction.Modifications[0].PublicAccount.PublicKey,
                tx.Transaction.Modifications[0].CosignatoryPublicKey);
            Assert.IsTrue(tx.Transaction.Modifications[0].Type ==
                       transaction.Modifications[0].Type.GetValue());
        }

        private void ValidateMosaicSupplyChangeTx(MosaicSupplyChangeTransaction transaction, string transactionDTO)
        {
            var tx = JsonConvert.DeserializeObject<MosaicSupplyChangeTransactionInfoDTO>(transactionDTO);

            Assert.AreEqual(tx.Transaction.MosaicId, transaction.MosaicId.Id);
            Assert.AreEqual(tx.Transaction.Delta, transaction.Delta);
            Assert.AreEqual(tx.Transaction.Direction, transaction.SupplyType.GetValue());
        }

        private void ValidateLockFundsTx(LockFundsTransaction transaction, string transactionDTO)
        {
            var tx = JsonConvert.DeserializeObject<LockFundsTransactionInfoDTO>(transactionDTO);

            Assert.AreEqual(transaction.Mosaic.MosaicId.Id, tx.Transaction.MosaicId);
            Assert.AreEqual(transaction.Mosaic.Amount, tx.Transaction.Amount);
            Assert.AreEqual(transaction.Duration, tx.Transaction.Duration);
            Assert.AreEqual(transaction.Transaction.Hash, tx.Transaction.Hash);
        }

        private void ValidateMosaicCreationTx(MosaicDefinitionTransaction transaction, string transactionDTO)
        {
            var tx = JsonConvert.DeserializeObject<MosaicCreationTransactionInfoDTO>(transactionDTO);

            Assert.AreEqual(tx.Transaction.ParentId, transaction.NamespaceId.Id);
            Assert.AreEqual(tx.Transaction.MosaicId, transaction.MosaicId.Id);
            Assert.AreEqual(tx.Transaction.Name, transaction.MosaicName);
            Assert.IsTrue(transaction.Properties.Divisibility == (int)tx.Transaction.Properties[1].value);
            Assert.AreEqual(transaction.Properties.Duration, tx.Transaction.Properties[2].value);
            Assert.IsTrue(transaction.Properties.IsSupplyMutable);     
            Assert.IsTrue(transaction.Properties.IsTransferable);        
            Assert.IsTrue(transaction.Properties.IsLevyMutable);
        }

        private void ValidateTransferTx(TransferTransaction transaction, string transactionDTO)
        {
            var tx = JsonConvert.DeserializeObject<TransferTransactionInfoDTO>(transactionDTO);

            Assert.AreEqual(tx.Transaction.Recipient, transaction.Address.Plain);

            var mosaics = tx.Transaction.Mosaics.Select(m => new Mosaic(new MosaicId(BitConverter.ToUInt64(m.MosaicId.FromHex(), 0)), m.Amount)).ToList();

            if (mosaics != null && mosaics.Count > 0)
            {
                Assert.AreEqual(mosaics[0].MosaicId.Id, transaction.Mosaics[0].MosaicId.Id);
                Assert.AreEqual(mosaics[0].Amount, transaction.Mosaics[0].Amount);
            }

            try
            {
                Assert.AreEqual(Encoding.UTF8.GetString(tx.Transaction.Message.Payload.FromHex()), Encoding.UTF8.GetString(transaction.Message.GetPayload()));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            Assert.IsTrue(tx.Transaction.Type == transaction.TransactionType.GetValue());
        }

        private void ValidateNamespaceCreationTx(RegisterNamespaceTransaction transaction, string transactionDTO)
        {
            var tx = JsonConvert.DeserializeObject<NamespaceTransactionInfoDTO>(transactionDTO);

            Assert.IsTrue(tx.Transaction.NamespaceType == transaction.NamespaceType.GetValue());

            Assert.AreEqual(tx.Transaction.Name, transaction.NamespaceId.Name);

            Assert.AreEqual(tx.Transaction.Name, transaction.NamespaceId.Name);

            if (transaction.NamespaceType == NamespaceTypes.Types.RootNamespace)
            {
                Assert.AreEqual(tx.Transaction.Duration, transaction.Duration);
            }
            else
            {
                Assert.AreEqual(tx.Transaction.ParentId, transaction.ParentId.Id);
            }
        }
    }
}
