using System;
using FluentAssertions;
using ProximaX.Sirius.Sdk.Model.Accounts;
using ProximaX.Sirius.Sdk.Model.Blockchain;
using ProximaX.Sirius.Sdk.Tests.Utils;
using Xunit;

namespace ProximaX.Sirius.Sdk.Tests.Models
{
    public class PublicAccountTests
    {
        [Fact]
        public void Should_Create_From_Construction_PUBLIC()
        {

            var publicAccount = new PublicAccount(Constants.PUBLIC_PUBLIC_KEY, NetworkType.PUBLIC);
            publicAccount.Address.Pretty.Should().BeEquivalentTo(Constants.PUBLIC_PRETTY_ADDRESS);
            publicAccount.Address.Plain.Should().BeEquivalentTo(Constants.PUBLIC_PLAIN_ADDRESS);
        }

        [Fact]
        public void Should_Create_From_Construction_PUBLIC_TEST()
        {
            var publicAccount = new PublicAccount(Constants.PUBLIC_TEST_PUBLIC_KEY, NetworkType.PUBLIC_TEST);
            publicAccount.Address.Pretty.Should().BeEquivalentTo(Constants.PUBLIC_TEST_PRETTY_ADDRESS);
            publicAccount.Address.Plain.Should().BeEquivalentTo(Constants.PUBLIC_TEST_PLAIN_ADDRESS);
        }

        [Fact]
        public void Should_Create_From_Construction_With_No_Public_Key()
        {
            Action act = () =>
            {
                var publicAccount = new PublicAccount(null, NetworkType.PUBLIC);

            };
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Should_Create_From_Construction_With_Public_Key_Size_Is_Not_64()
        {
            Action act = () =>
            {
                var publicAccount = new PublicAccount(Constants.INVALID_PUBLIC_KEY, NetworkType.PUBLIC);

            };
            act.Should().Throw<ArgumentOutOfRangeException>();


        }

        [Fact]
        public void Should_Create_From_Construction_With_Public_Key_Is_Not_HEX()
        {
            Action act = () =>
            {
                var publicAccount = new PublicAccount(Constants.INVALID_FORMAT_PUBLIC_KEY, NetworkType.PUBLIC);

            };
            act.Should().Throw<FormatException>();

        }


        [Fact]
        public void Should_Create_From_Public_Key_PUBLIC()
        {

            var publicAccount = PublicAccount.CreateFromPublicKey(Constants.PUBLIC_PUBLIC_KEY, NetworkType.PUBLIC);
            publicAccount.Address.Pretty.Should().BeEquivalentTo(Constants.PUBLIC_PRETTY_ADDRESS);
            publicAccount.Address.Plain.Should().BeEquivalentTo(Constants.PUBLIC_PLAIN_ADDRESS);

        }

        [Fact]
        public void Should_Create_From_Public_Key_PUBLIC_TEST()
        {

            var publicAccount = PublicAccount.CreateFromPublicKey(Constants.PUBLIC_TEST_PUBLIC_KEY, NetworkType.PUBLIC_TEST);
            publicAccount.Address.Pretty.Should().BeEquivalentTo(Constants.PUBLIC_TEST_PRETTY_ADDRESS);
            publicAccount.Address.Plain.Should().BeEquivalentTo(Constants.PUBLIC_TEST_PLAIN_ADDRESS);
        }

        [Fact]
        public void Compare_Public_Account_On_Same_Network()
        {

            var publicAccount = new PublicAccount(Constants.PUBLIC_TEST_PUBLIC_KEY, NetworkType.PUBLIC_TEST);
            var publicAccount2 = new PublicAccount(Constants.PUBLIC_TEST_PUBLIC_KEY, NetworkType.PUBLIC_TEST);
            publicAccount2.Should().BeEquivalentTo(publicAccount);
          
        }

        [Fact]
        public void Compare_Public_Account_On_Same_Different_Network()
        {

            var publicAccount = new PublicAccount(Constants.PUBLIC_TEST_PUBLIC_KEY, NetworkType.PUBLIC);
            var publicAccount2 = new PublicAccount(Constants.PUBLIC_TEST_PUBLIC_KEY, NetworkType.PUBLIC_TEST);
            publicAccount.Should().NotBe(publicAccount2);
            
        }


    }
}
