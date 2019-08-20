using System;
using FluentAssertions;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Tests.Utils;
using Xunit;

namespace ProximaX.Sirius.Chain.Sdk.Tests.Models
{
    public class PublicAccountTests
    {
        [Fact]
        public void Should_Create_From_Construction_PUBLIC()
        {

            var publicAccount = new PublicAccount(Constants.PUBLIC_PUBLIC_KEY, NetworkType.MAIN_NET);
            publicAccount.Address.Pretty.Should().BeEquivalentTo(Constants.PUBLIC_PRETTY_ADDRESS);
            publicAccount.Address.Plain.Should().BeEquivalentTo(Constants.PUBLIC_PLAIN_ADDRESS);
        }

        [Fact]
        public void Should_Create_From_Construction_PUBLIC_TEST()
        {
            var publicAccount = new PublicAccount(Constants.PUBLIC_TEST_PUBLIC_KEY, NetworkType.TEST_NET);
            publicAccount.Address.Pretty.Should().BeEquivalentTo(Constants.PUBLIC_TEST_PRETTY_ADDRESS);
            publicAccount.Address.Plain.Should().BeEquivalentTo(Constants.PUBLIC_TEST_PLAIN_ADDRESS);
        }

        [Fact]
        public void Should_Create_From_Construction_With_No_Public_Key()
        {
            Action act = () =>
            {
                var publicAccount = new PublicAccount(null, NetworkType.MAIN_NET);

            };
            act.Should().Throw<ArgumentException>();
        }

        [Fact]
        public void Should_Create_From_Construction_With_Public_Key_Size_Is_Not_64()
        {
            Action act = () =>
            {
                var publicAccount = new PublicAccount(Constants.INVALID_PUBLIC_KEY, NetworkType.MAIN_NET);

            };
            act.Should().Throw<ArgumentOutOfRangeException>();


        }

        [Fact]
        public void Should_Create_From_Construction_With_Public_Key_Is_Not_HEX()
        {
            Action act = () =>
            {
                var publicAccount = new PublicAccount(Constants.INVALID_FORMAT_PUBLIC_KEY, NetworkType.MAIN_NET);

            };
            act.Should().Throw<FormatException>();

        }


        [Fact]
        public void Should_Create_From_Public_Key_PUBLIC()
        {

            var publicAccount = PublicAccount.CreateFromPublicKey(Constants.PUBLIC_PUBLIC_KEY, NetworkType.MAIN_NET);
            publicAccount.Address.Pretty.Should().BeEquivalentTo(Constants.PUBLIC_PRETTY_ADDRESS);
            publicAccount.Address.Plain.Should().BeEquivalentTo(Constants.PUBLIC_PLAIN_ADDRESS);

        }

        [Fact]
        public void Should_Create_From_Public_Key_PUBLIC_TEST()
        {

            var publicAccount = PublicAccount.CreateFromPublicKey(Constants.PUBLIC_TEST_PUBLIC_KEY, NetworkType.TEST_NET);
            publicAccount.Address.Pretty.Should().BeEquivalentTo(Constants.PUBLIC_TEST_PRETTY_ADDRESS);
            publicAccount.Address.Plain.Should().BeEquivalentTo(Constants.PUBLIC_TEST_PLAIN_ADDRESS);
        }

        [Fact]
        public void Compare_Public_Account_On_Same_Network()
        {

            var publicAccount = new PublicAccount(Constants.PUBLIC_TEST_PUBLIC_KEY, NetworkType.TEST_NET);
            var publicAccount2 = new PublicAccount(Constants.PUBLIC_TEST_PUBLIC_KEY, NetworkType.TEST_NET);
            publicAccount2.Should().BeEquivalentTo(publicAccount);
          
        }

        [Fact]
        public void Compare_Public_Account_On_Same_Different_Network()
        {

            var publicAccount = new PublicAccount(Constants.PUBLIC_TEST_PUBLIC_KEY, NetworkType.MAIN_NET);
            var publicAccount2 = new PublicAccount(Constants.PUBLIC_TEST_PUBLIC_KEY, NetworkType.TEST_NET);
            publicAccount.Should().NotBe(publicAccount2);
            
        }


    }
}
