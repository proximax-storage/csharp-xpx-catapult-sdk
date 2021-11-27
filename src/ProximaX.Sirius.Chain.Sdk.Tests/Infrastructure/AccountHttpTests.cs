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
using Xunit.Abstractions;

namespace ProximaX.Sirius.Chain.Sdk.Tests.Infrastructure
{
    public class AccountHttpTests : BaseTest
    {
        private readonly AccountHttp _accountHttp;
        private readonly ITestOutputHelper Log;

        public AccountHttpTests(ITestOutputHelper log)
        {
            _accountHttp = new AccountHttp(BaseUrl) { NetworkType = NetworkType.TEST_NET };
            Log = log;
        }

        [Fact]
        public async Task Get_AccountInfo_By_Address()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToObject(@"Testdata\\Account\\GetAccountInfoByAddress.json");

                httpTest.RespondWithJson(fakeJson);

                const string rawAddress = "VDPQS6FBYDN3SD2QJPHUWRYWNHSSOQ2Q35VI7TDP";
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

                const string rawAddress = "VCQ6KSND4ISJPVTVT4F25KMA3S7NZTZDFTTDRFF6";
                var address = Address.CreateFromRawAddress(rawAddress);
                var accountInfo = await _accountHttp.GetAccountInfo(address);
                accountInfo.Should().NotBeNull();
                accountInfo.Address.Plain.Should().BeEquivalentTo(rawAddress);
                accountInfo.LinkedAccountKey.Should().BeEquivalentTo("E212EFD035396EEF6C9661A546145623E1AEF53129EDA5D7720F1CD79A25AE9C");
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

                const string publicKey = "D9A659A3AA42FD62BE88E1D96B0F10EB91F6097F8D24EC8FD7C94EC6455735EC";
                var account = PublicAccount.CreateFromPublicKey(publicKey, NetworkType.TEST_NET);
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
                    Address.CreateFromRawAddress("VDM4RM6SFOM4P4HYWBSAFPKZJFOC5JMCALRPDFRA"),
                    Address.CreateFromRawAddress("VDPQS6FBYDN3SD2QJPHUWRYWNHSSOQ2Q35VI7TDP")
                };

                var accountsInfo = await _accountHttp.GetAccountsInfo(addresses);

                accountsInfo.Should().HaveCount(2);
                accountsInfo.Select(a => a.Address.Plain == "VDM4RM6SFOM4P4HYWBSAFPKZJFOC5JMCALRPDFRA").Should().Equals(true);
                accountsInfo.Select(a => a.Address.Plain == "VDPQS6FBYDN3SD2QJPHUWRYWNHSSOQ2Q35VI7TDP").Should().Equals(true);
                accountsInfo.Single(a => a.Address.Plain == "VDPQS6FBYDN3SD2QJPHUWRYWNHSSOQ2Q35VI7TDP").Mosaics.Should().HaveCount(4);
            }
        }

        [Fact]
        public async Task Get_Confirmed_Transaction_From_Account()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToObject(@"Testdata\\Account\\GetConfirmedTxFromAccount.json");

                httpTest.RespondWithJson(fakeJson);
                var account = PublicAccount.CreateFromPublicKey("6482AC2A82AC884B87B10D54120B28DB94AF56C596EA04AE54E77A4CE8A196F0", NetworkType.TEST_NET);

                var transactions = await _accountHttp.Transactions(account);
                transactions.Transactions.Should().HaveCount(1);
                transactions.Should().NotBeNull();
            }
        }

        [Fact]
        public async Task Get_Account_Properties_By_Address_List()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson = TestHelper.LoadJsonFileToArray(@"Testdata\\Account\\GetAccountPropertiesByAddressList.json");

                httpTest.RespondWithJson(fakeJson);

                var addresses = new List<Address>()
                {
                    Address.CreateFromRawAddress("VDM4RM6SFOM4P4HYWBSAFPKZJFOC5JMCALRPDFRA"),
                    Address.CreateFromRawAddress("VAGC7UPKXNHIAYCOXUM3URJH7H7VFQETMYKLNVG4")
                };

                var accountFilter = await _accountHttp.GetAccountProperties(addresses);
                accountFilter.Select(info => info.AccountProperties.Address.Plain).Should().BeEquivalentTo("VDM4RM6SFOM4P4HYWBSAFPKZJFOC5JMCALRPDFRA", "VAGC7UPKXNHIAYCOXUM3URJH7H7VFQETMYKLNVG4");
            }
        }

        [Fact]
        public async Task Get_Account_Property_By_PublicKey()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToObject(@"Testdata\\Account\\GetAccountPropertyByAddress.json");

                httpTest.RespondWithJson(fakeJson);

                var account = PublicAccount.CreateFromPublicKey("EC49CB3C5E0F565F9D27F90FD5830E21DF08205353C837540088C18936E41397", NetworkType.TEST_NET);
                var blockedAddress = Address.CreateFromHex("A8DE118FEE2EA3EC52C03E37CD5F5764E3CDA9819D497692D5");

                var accountFilter = await _accountHttp.GetAccountProperty(account);
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
        public async Task Get_Account_Property_By_Address()
        {
            using (var httpTest = new HttpTest())
            {
                var fakeJson =
                    TestHelper.LoadJsonFileToObject(@"Testdata\\Account\\GetAccountPropertyByAddress.json");

                httpTest.RespondWithJson(fakeJson);

                var address = Address.CreateFromHex("A8D9C8B3D22B99C7F0F8B06402BD59495C2EA58202E2F19620");
                var blockedAddress = Address.CreateFromHex("A8DE118FEE2EA3EC52C03E37CD5F5764E3CDA9819D497692D5");

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

                var address = Address.CreateFromRawAddress("VCRUM4I6M4DDYE3O3LILVIRUAJIGXMVWP3YRIVQC");

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

                var expectedMosaicId = new MosaicId("64D53F46233BA3A0");
                var allowMosaic = allowMosaicFilter.Values.Single(a =>
                    {
                        var m = JsonConvert.DeserializeObject<UInt64DTO>(a.ToString());
                        return (new MosaicId(m.ToUInt64())).HexId == expectedMosaicId.HexId;
                    });
                allowMosaic.Should().NotBeNull();
            }
        }
    }
}