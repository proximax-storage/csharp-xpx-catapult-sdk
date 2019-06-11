
using ProximaX.Sirius.Sdk.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using FluentAssertions;
using Flurl.Http.Testing;
using Newtonsoft.Json;
using ProximaX.Sirius.Sdk.Infrastructure.DTO;
using ProximaX.Sirius.Sdk.Model.Accounts;
using ProximaX.Sirius.Sdk.Model.Blockchain;
using ProximaX.Sirius.Sdk.Model.Mosaics;
using ProximaX.Sirius.Sdk.Model.Transactions;
using ProximaX.Sirius.Sdk.Tests.Utils;
using ProximaX.Sirius.Sdk.Utils;
using Xunit;

namespace ProximaX.Sirius.Sdk.Tests.Infrastructure
{
    public class AccountHttpTests : BaseTest
    {
        private readonly AccountHttp _accountHttp;

        public AccountHttpTests()
        {
            _accountHttp = new AccountHttp(BaseUrl);
        }


        [Fact]
        public async Task Get_AccountInfo_By_Address()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToObject(@"Testdata\\Account\\GetAccountInfoByAddress.json");

                httpTest.RespondWithJson(fakeJson);

                const string rawAddress = "SDEACLIEMSZUQCVZGFOAPCMZ6WYQCCAMMHOOYFLE";
                var address = Address.CreateFromRawAddress(rawAddress);
                var accountInfo = await _accountHttp.GetAccountInfo(address);
                accountInfo.Should().NotBeNull();
                accountInfo.Address.Plain.Should().BeEquivalentTo(rawAddress);

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
                transactions.First().TransactionType.Should().BeEquivalentTo(TransactionType.TRANSFER);
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
                transactions.First().TransactionType.Should().BeEquivalentTo(TransactionType.TRANSFER);
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

                var account = PublicAccount.CreateFromPublicKey("B00D4317CC4FEB2976DF3EBEEF6A788B769F6F37B61883F24282ACA034C15DDF", NetworkType.MIJIN_TEST);

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
        /*

       [Fact]
       public async Task Get_Account_Transactions_By_PublicKey_With_Query_Should_Return_Account_Transaction_List()
       {
           var accountHttp = new AccountHttp(BaseUrl);
           var networkType = await new NetworkHttp(BaseUrl).GetNetworkType();
           var publicAccount = PublicAccount.CreateFromPublicKey(PUBLIC_TEST_PUBLIC_KEY, networkType);
           var query = new QueryParams(10, null, Order.DESC);
           var transactions = await accountHttp.Transactions(publicAccount, query);
           transactions.Should().HaveCountGreaterThan(0);

       }

       [Fact]
       public async Task Get_Account_Property_Info_By_PublicAccount__Should_Return_Account_Property_Info()
       {
           var publicKey = "5798B5DA682079A5C2AF3EDD48D1A2AC7940CCF9A0370597A5D27F6A210BFCC6";
           var expectedAddress = "VA5H5UEJA57AYSYGOHCE2B5XWNOM7MYPTQGPTMUF";
           var expectedPropertyType = PropertyTypeExtension.GetRawValue(129);
           var accountHttp = new AccountHttp(BaseUrl);
           var networkType = await new NetworkHttp(BaseUrl).GetNetworkType();
           var publicAccount = PublicAccount.CreateFromPublicKey(publicKey,networkType);
           var accountProperty = await accountHttp.GetAccountProperty(publicAccount);
           accountProperty.AccountProperties.Address.Plain.Should().BeEquivalentTo(expectedAddress);
           accountProperty.AccountProperties.Properties.Should().HaveCountGreaterThan(0);
           accountProperty.AccountProperties.Properties[0].PropertyType.Should().BeEquivalentTo(expectedPropertyType);

       }


       [Fact]
       public async Task Get_Account_Property_Info_By_Address__Should_Return_Account_Property_Info()
       {
          // var publicKey = "5798B5DA682079A5C2AF3EDD48D1A2AC7940CCF9A0370597A5D27F6A210BFCC6";
           var expectedAddress = "VA5H5UEJA57AYSYGOHCE2B5XWNOM7MYPTQGPTMUF";
           var expectedPropertyType = PropertyTypeExtension.GetRawValue(129);
           var accountHttp = new AccountHttp(BaseUrl);
           //var networkType = await new NetworkHttp(BaseUrl).GetNetworkType();
           var address = Address.CreateFromRawAddress(expectedAddress);

           var accountProperty = await accountHttp.GetAccountProperty(address);

       }

       [Fact]
       public async Task Get_AccountProperties_By_Addresses_Should_Return_Account_Property_List()
       {
           var accountHttp = new AccountHttp(BaseUrl);
           var expectedAddress1 = "VA5H5UEJA57AYSYGOHCE2B5XWNOM7MYPTQGPTMUF";
           var expectedAddress2 = "VDQLBSMQ65GAGUTIO6EOZWVDYLAHQNTB5YV7QXDL";
           var address1 = Address.CreateFromRawAddress(expectedAddress1);
           var address2 = Address.CreateFromRawAddress(expectedAddress2);
           var list = new List<Address> { address1,address2 };

           var accountPropertyList = await accountHttp.GetAccountProperties(list);

           Assert.IsTrue(accountPropertyList.Count > 1);
           Assert.AreEqual(expectedAddress1, accountPropertyList[0].AccountProperties.Address.Plain);
           Assert.AreEqual(expectedAddress2, accountPropertyList[1].AccountProperties.Address.Plain);
       }
   */
    }
}
