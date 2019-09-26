using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http.Testing;
using Newtonsoft.Json;
using ProximaX.Sirius.Chain.Sdk.Infrastructure;
using ProximaX.Sirius.Chain.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Tests.Utils;
using ProximaX.Sirius.Chain.Sdk.Utils;
using Xunit;

namespace ProximaX.Sirius.Chain.Sdk.Tests.Infrastructure
{
    public class AccountHttpTests : BaseTest
    {
        private readonly AccountHttp _accountHttp;

        public AccountHttpTests()
        {
            _accountHttp = new AccountHttp(BaseUrl) { NetworkType = NetworkType.MIJIN_TEST };
        }


        [Fact]
        public async Task Get_AccountInfo_By_Address()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToObject(@"Testdata\\Account\\GetAccountInfoByAddress.json");

                httpTest.RespondWithJson(fakeJson);

                const string rawAddress = "SDR2YLGNPGFLSDRG2CGFBEXCJUHVC2ZWBODHRLJN";
                var address = Address.CreateFromRawAddress(rawAddress);
                var accountInfo = await _accountHttp.GetAccountInfo(address);
                accountInfo.Should().NotBeNull();
                accountInfo.Address.Plain.Should().BeEquivalentTo(rawAddress);

            }
        }

        [Fact]
        public async Task Get_AccountInfo_WithLinkedAccount_By_Address()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToObject(@"Testdata\\Account\\GetAccountInfoWithLinkedAccountKey.json");

                httpTest.RespondWithJson(fakeJson);

                const string rawAddress = "SBIGZPKNMZHB7ZUJOMD5IYNVUIIM5KUQG53BYTRZ";
                var address = Address.CreateFromRawAddress(rawAddress);
                var accountInfo = await _accountHttp.GetAccountInfo(address);
                accountInfo.Should().NotBeNull();
                accountInfo.Address.Plain.Should().BeEquivalentTo(rawAddress);
                accountInfo.LinkedAccountKey.Should().BeEquivalentTo("29646721A55041C411BDF5A8428B94CEB47C7B6295F9559C3F3ACD70963321AA");
            }
        }

        
        [Fact]
        public async Task Get_MultiSignAccountInfo_By_Address()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToObject(@"Testdata\\Account\\GetMultiSigAccountByAddress.json");

                httpTest.RespondWithJson(fakeJson);

                const string rawAddress = "SDCLRIBJKS4HTRYL2GN5MZRNHSRCKJSMGQ44FDIC";
                var address = Address.CreateFromRawAddress(rawAddress);
                var accountInfo = await _accountHttp.GetMultisigAccountInfo(address);
                accountInfo.Should().NotBeNull();
                accountInfo.MinApproval.Should().Be(1);
                accountInfo.MinRemoval.Should().Be(1);
                accountInfo.Cosignatories.Should().HaveCount(2);

            }
        }

        [Fact]
        public async Task Get_MultiSignAccountGraph_By_Address()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToArray(@"Testdata\\Account\\GetMultiSigAccountGraphByAddress.json");

                httpTest.RespondWithJson(fakeJson);

                const string rawAddress = "SCE5YTZZCMV44MGCXGTKV7PRBOZVNAXY66QSOGNK";
                var address = Address.CreateFromRawAddress(rawAddress);
                var graphInfo = await _accountHttp.GetMultisigAccountGraphInfo(address);
                graphInfo.GetLevelsNumber().Should().NotBeNull();
                graphInfo.MultisigAccounts.Should().HaveCountGreaterThan(0);

            }
        }

        [Fact]
        public async Task Get_AccountInfo_By_PublicKey()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToObject(@"Testdata\\Account\\GetAccountInfoByPublicKey.json");

                httpTest.RespondWithJson(fakeJson);

                const string publicKey = "20D57BED1C5DA5C224D83B22BEFF1CCC7D5D94054C9B75E83E08EF3ED5F992CD";
                var account = PublicAccount.CreateFromPublicKey(publicKey, NetworkType.MIJIN_TEST);
                var accountInfo = await _accountHttp.GetAccountInfo(account.Address);
                accountInfo.Should().NotBeNull();
                accountInfo.PublicAccount.PublicKey.Should().BeEquivalentTo(publicKey);
            }
        }


        [Fact]
        public async Task Get_AccountInfo_List_By_Addresses_Should_Return_Account_Info_List()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToArray(@"Testdata\\Account\\GetAccountListByAddresses.json");

                httpTest.RespondWithJson(fakeJson);

                var addresses = new List<Address>()
                {
                    Address.CreateFromRawAddress("SDEACLIEMSZUQCVZGFOAPCMZ6WYQCCAMMHOOYFLE"),
                    Address.CreateFromRawAddress("SBSSBYV57ISAZ2ATDAMMVXCQHJOP736NJDCRSUZV")
                };

                var accountsInfo = await _accountHttp.GetAccountsInfo(addresses);

                accountsInfo.Should().HaveCount(2);
                accountsInfo.Select(a => a.Address.Plain == "SDEACLIEMSZUQCVZGFOAPCMZ6WYQCCAMMHOOYFLE").Should()
                    .NotBeNull();
                accountsInfo.Select(a => a.Address.Plain == "SBSSBYV57ISAZ2ATDAMMVXCQHJOP736NJDCRSUZV").Should()
                    .NotBeNull();
                accountsInfo.Single(a => a.Address.Plain == "SBSSBYV57ISAZ2ATDAMMVXCQHJOP736NJDCRSUZV").Mosaics.Should()
                    .HaveCount(1);
            }

        }

        [Fact]
        public async Task Get_Confirmed_Transaction_From_Account()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToArray(@"Testdata\\Account\\GetConfirmedTxFromAccount.json");

                httpTest.RespondWithJson(fakeJson);

                var account = Account.CreateFromPrivateKey("3E1AB96DDB060152BCE84E5CB857140BAF95F61777BBF6E33B4BE7158189CC0A", NetworkType.MIJIN_TEST);

                var transactions = await _accountHttp.Transactions(account.PublicAccount, new QueryParams(10, "", Order.DESC));

                transactions.Should().HaveCount(1);
                transactions.First().TransactionType.Should().BeEquivalentTo(EntityType.TRANSFER);
                transactions.First().TransactionInfo.Should().NotBeNull();
            }
        }

        [Fact]
        public async Task Get_UnConfirmed_Transaction_From_Account()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToArray(@"Testdata\\Account\\GetUnConfirmedTxFromAccount.json");

                httpTest.RespondWithJson(fakeJson);

                var account = PublicAccount.CreateFromPublicKey("B00D4317CC4FEB2976DF3EBEEF6A788B769F6F37B61883F24282ACA034C15DDF", NetworkType.MIJIN_TEST);

                var transactions = await _accountHttp.UnconfirmedTransactions(account, new QueryParams(10, "", Order.DESC));

                transactions.Should().HaveCount(1);
                transactions.First().TransactionType.Should().BeEquivalentTo(EntityType.TRANSFER);
                transactions.First().TransactionInfo.Should().NotBeNull();
            }
        }

        [Fact]
        public async Task Get_OutGoing_Transaction_From_Account()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToArray(@"Testdata\\Account\\GetOutgoingTxFromAccount.json");

                httpTest.RespondWithJson(fakeJson);

                var account = PublicAccount.CreateFromPublicKey("D03918E35573C66578B5A0EED723FE2A46208783E13498751D9315115CA06D4B", NetworkType.MIJIN_TEST);

                var transactions = await _accountHttp.OutgoingTransactions(account, new QueryParams(10, "", Order.DESC));

                transactions.Should().HaveCountGreaterOrEqualTo(1);

            }
        }


        [Fact]
        public async Task Get_Incoming_Transaction_From_Account()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToArray(@"Testdata\\Account\\GetIncomingTxFromAccount.json");

                httpTest.RespondWithJson(fakeJson);

                var account = PublicAccount.CreateFromPublicKey("D70ECC83DC6BDE5F8E1070A98ECB9B65B473F72E675E1366137AF56A55ECC902", NetworkType.MIJIN_TEST);

                var transactions = await _accountHttp.IncomingTransactions(account, new QueryParams(10, "", Order.DESC));

                transactions.Should().HaveCountGreaterOrEqualTo(1);

            }
        }

        [Fact]
        public async Task Get_Account_Filter_By_Address()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToObject(@"Testdata\\Account\\GetAccountPropertyByAddress.json");

                httpTest.RespondWithJson(fakeJson);

                var address = Address.CreateFromHex("90E1FC47EADF62FE30ABFE9B930C214FA9BD49D0EFC8E1885A");
                var blockedAddress = Address.CreateFromHex("9066F9F5B10FD7779CDB286CF953C69EB0B7E2EDAD135CED22");

                var accountFilter = await _accountHttp.GetAccountProperty(address);
                accountFilter.Should().NotBeNull();

                var blockAddressFilter =
                    accountFilter.AccountProperties.Properties.Single(ap =>
                        ap.PropertyType == PropertyType.BLOCK_ADDRESS);

                foreach (var ba in blockAddressFilter.Values)
                {
                    var ad = Address.CreateFromHex(ba.ToString());
                    ad.Should().NotBeNull();
                }

                blockAddressFilter.Should().NotBeNull();
                var resBlockedAddress = blockAddressFilter.Values.Single(a => Address.CreateFromHex(a.ToString()).Plain == blockedAddress.Plain);
                resBlockedAddress.Should().NotBeNull();

            }

        }

        [Fact]
        public async Task Get_Account_Filter_By_Mosaic()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToObject(@"Testdata\\Account\\GetAccountPropertyByMosaic.json");

                httpTest.RespondWithJson(fakeJson);

                var address = Address.CreateFromHex("9031C36EC5207123E20D7FEE91B73284B9B8EC354ADA948147");

                var accountFilter = await _accountHttp.GetAccountProperty(address);

                var allowMosaicFilter =
                    accountFilter.AccountProperties.Properties.Single(ap =>
                        ap.PropertyType == PropertyType.ALLOW_MOSAIC);
                allowMosaicFilter.Should().NotBeNull();

                foreach (var bm in allowMosaicFilter.Values)
                {
                    var arrayIds = JsonConvert.DeserializeObject<List<uint>>(bm.ToString());
                    var mosaicId = new MosaicId(arrayIds.FromUInt8Array());
                    mosaicId.Should().NotBeNull();
                }

                var expectedMosaicId = new MosaicId("0DC67FBE1CAD29E3");
                var allowMosaic = allowMosaicFilter.Values
                    .Single(a =>
                    {
                        var m = JsonConvert.DeserializeObject<UInt64DTO>(a.ToString());
                        return (new MosaicId(m.ToUInt64())).HexId == expectedMosaicId.HexId;
                    });
                allowMosaic.Should().NotBeNull();
                
            }

        }
       
    }
}
