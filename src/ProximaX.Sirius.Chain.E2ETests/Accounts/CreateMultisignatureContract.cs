using ProximaX.Sirius.Chain.E2ETests.Common;
using Xunit.Abstractions;
using Xunit.Gherkin.Quick;

namespace ProximaX.Sirius.Chain.E2ETests.Accounts
{
    [FeatureFile("./Features/Accounts/create_a_multisignature_contract.feature")]
    public class CreateMultisignatureContract : BaseTest
    {
        public CreateMultisignatureContract(ITestOutputHelper log) : base(log)
        {
        }

        [Given(@"^(\\w+) defined a (-?\\d+) of (-?\\d+) multisignature contract called (.*) with (-?\\d+) required for removal with cosignatories:$")]
        public void DefinedMultiSignatureContract()
        {
            Log.WriteLine("Define multisign");
        }

        [And(@"^(\\w+) published the bonded contract")]
        public void PublishBondedTransaction()
        {
            Log.WriteLine("PublishBondedTransaction");
        }

        [When(@"^all the required cosignatories sign the transaction$")]
        public void CosignMultiSignatureAccount()
        {
            Log.WriteLine("CosignMultiSignatureAccount");
        }

        [And(@"^(\\w+) account is convert to multisig$")]
        public void VerifyMultiSignatureAccount()
        {
            Log.WriteLine("VerifyMultiSignatureAccount");
        }
    }
}
