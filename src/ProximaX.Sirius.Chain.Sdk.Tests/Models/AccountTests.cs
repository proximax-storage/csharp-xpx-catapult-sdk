using System;
using FluentAssertions;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Exceptions;
using ProximaX.Sirius.Chain.Sdk.Tests.Utils;
using Xunit;

namespace ProximaX.Sirius.Chain.Sdk.Tests.Models
{
    public class AccountTests
    {

        [Fact]
        public void Create_New_Address_Returns_Plain()
        {
            var address = new Address(Constants.RAW_ADDRESS, NetworkType.MIJIN);

            address.Plain.Should().BeEquivalentTo(Constants.PLAIN_ADDRESS);

        }

        [Fact]
        public void Create_New_Address_From_Raw_Address_Returns_Plain()
        {

            var address = Address.CreateFromRawAddress(Constants.RAW_ADDRESS);

            address.Plain.Should().BeEquivalentTo(Constants.PLAIN_ADDRESS);
        }

        [Fact]
        public void Create_New_Address_From_Invalid_Raw_Address_Returns_Exception()
        {
            Action act = () => Address.CreateFromRawAddress(Constants.INVALID_RAW_ADDRESS);

            act.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Fact]
        public void Create_New_Address_From_Raw_Address_Returns_NetworkType_MIJIN()
        {

            var address = Address.CreateFromRawAddress(Constants.MIJIN_PLAIN_ADDRESS);

            address.NetworkType.Should().BeEquivalentTo(NetworkType.MIJIN);

        }


        [Fact]
        public void Create_New_Address_From_Raw_Address_Returns_NetworkType_MIJIN_TEST()
        {

            var address = Address.CreateFromRawAddress(Constants.MIJIN_TEST_PLAIN_ADDRESS);


            address.NetworkType.Should().BeEquivalentTo(NetworkType.MIJIN_TEST);

        }

        [Fact]
        public void Create_New_Address_From_Raw_Address_Returns_NetworkType_PUBLIC()
        {

            var address = Address.CreateFromRawAddress(Constants.PUBLIC_PLAIN_ADDRESS);
            address.NetworkType.Should().BeEquivalentTo(NetworkType.PUBLIC);
        }

        [Fact]
        public void Create_New_Address_From_Raw_Address_Returns_NetworkType_PUBLIC_TEST()
        {

            var address = Address.CreateFromRawAddress(Constants.PUBLIC_TEST_PLAIN_ADDRESS);

            address.NetworkType.Should().BeEquivalentTo(NetworkType.PUBLIC_TEST);

        }


        [Fact]
        public void Create_New_Address_From_Raw_Address_Returns_NetworkType_PRIVATE()
        {

            var address = Address.CreateFromRawAddress(Constants.PRIVATE_PLAIN_ADDRESS);

            address.NetworkType.Should().BeEquivalentTo(NetworkType.PRIVATE);

        }

        [Fact]
        public void Create_New_Address_From_Raw_Address_Returns_NetworkType_PRIVATE_TEST()
        {

            var address = Address.CreateFromRawAddress(Constants.PRIVATE_TEST_PLAIN_ADDRESS);
            address.NetworkType.Should().BeEquivalentTo(NetworkType.PRIVATE_TEST);

        }

        [Fact]
        public void Create_New_Address_From_Raw_Address_Returns_NetworkType_NOT_SUPPORT()
        {

          
            Action act = () =>
            {
               Address.CreateFromRawAddress(Constants.NOT_SUPPORT_RAW_ADDRESS);
            };
            act.Should().Throw<TypeNotSupportException>();
          

        }

        [Fact]
        public void Create_New_Address_From_Public_Key_NetworkType_MIJIN_TEST()
        {
         
            var address = Address.CreateFromPublicKey(Constants.MIJIN_TEST_PUBLIC_KEY, NetworkType.MIJIN_TEST);

            address.NetworkType.Should().BeEquivalentTo(NetworkType.MIJIN_TEST);
 
        }

        [Fact]
        public void Should_Generate_New_Account()
        {
            var account = Account.GenerateNewAccount(NetworkType.MIJIN_TEST);

            account.Address.Plain.Should().NotBeEmpty();
            account.PrivateKey.Should().NotBeEmpty();
            account.PublicKey.Should().NotBeEmpty();
            account.PublicAccount.Should().NotBeNull();
        }

        [Fact]
        public void Should_Create_New_Account_Using_Address_KeyPair()
        {
            var address = Address.CreateFromPublicKey(Constants.MIJIN_TEST_PUBLIC_KEY, NetworkType.MIJIN_TEST);
            var keyPair = KeyPair.CreateFromPrivateKey(Constants.PRIVATE_KEY);
            var account = new Account(address, keyPair);

            account.Address.Plain.Should().NotBeEmpty();
            account.PrivateKey.Should().NotBeEmpty();
            account.PublicKey.Should().NotBeEmpty();
            account.PublicAccount.Should().NotBeNull();
        }

        [Fact]
        public void Should_Create_New_Account_Using_Private_Key()
        {

            var account = Account.CreateFromPrivateKey(Constants.PRIVATE_KEY, NetworkType.MIJIN_TEST);

            account.Address.Plain.Should().NotBeEmpty();
            account.PrivateKey.Should().NotBeEmpty();
            account.PublicKey.Should().NotBeEmpty();
            account.PublicAccount.Should().NotBeNull();
        }
    }
}
