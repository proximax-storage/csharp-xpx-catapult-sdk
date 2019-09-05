using ProximaX.Sirius.Chain.E2ETests.Config;
using ProximaX.Sirius.Chain.Sdk.Client;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using System.Collections.Generic;
using System.Reactive.Linq;
using Xunit.Abstractions;
using Xunit.Gherkin.Quick;

namespace ProximaX.Sirius.Chain.E2ETests.Common
{
    public class BaseTest: Feature
    {
        protected readonly SiriusClient SiriusClient;

        protected readonly SiriusWebSocketClient SiriusWebSocketClient;

        protected readonly NetworkType NetworkType;

        protected readonly string GenerationHash;

        protected readonly IDictionary<string, Account> UserAccountRepository = new Dictionary<string, Account>();

        protected readonly ITestOutputHelper Log;

        public BaseTest(ITestOutputHelper log)
        {
            Log = log;
            var appConfig = AppConfig.Create();

            SiriusClient = new SiriusClient(appConfig.BaseUrl);
            SiriusWebSocketClient = new SiriusWebSocketClient(appConfig.Host, appConfig.Port);
            NetworkType = SiriusClient.NetworkHttp.GetNetworkType().Wait();
            GenerationHash = SiriusClient.BlockHttp.GetGenerationHash().Wait();

            var signerAccount = Account.CreateFromPrivateKey(appConfig.SeedAccountPK, NetworkType);
            if(!UserAccountRepository.ContainsKey(UserAccounts.Alice))
            {
                UserAccountRepository.Add(UserAccounts.Alice, signerAccount);
            }
        }
    }
}
