using ProximaX.Sirius.Chain.Sdk.Client;
using ProximaX.Sirius.Chain.Sdk.Infrastructure;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Tests.Utils;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;

namespace ProximaX.Sirius.Chain.Sdk.Tests.E2E
{
    public class E2ETestBase
    {
        protected readonly SiriusClient SiriusClient;

        protected readonly SiriusWebSocketClient SiriusWebSocketClient;

        protected readonly NetworkType NetworkType;

        protected readonly Account SeedAccount;

        protected readonly string GenerationHash;

        public E2ETestBase()
        {
            var env = GetEnvironment();

            SiriusClient = new SiriusClient(env.BaseUrl);
            SiriusWebSocketClient = new SiriusWebSocketClient(env.Host, env.Port);
            NetworkType = SiriusClient.NetworkHttp.GetNetworkType().Wait();
            SeedAccount = Account.CreateFromPrivateKey(env.SeedAccountPK, NetworkType);
           
        }


        private TestEnvironment GetEnvironment()
        {
            var environment = TestHelper.GetConfig()[$"ActiveEnvironment"] ?? "DEV";

            string env;
            switch (environment.ToUpper())
            {
                case "DEV":
                    env = "Dev";
                    break;
                case "BCSTAGE":
                    env = "BcStage";
                    break;
                case "BCTESTNET":
                    env = "BcTestNet";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Environment), environment, null);
            }

            var protocol = TestHelper.GetConfig()[$"Environments:{env}:Protocol"];
            var host = TestHelper.GetConfig()[$"Environments:{env}:Host"];
            var port = TestHelper.GetConfig()[$"Environments:{env}:Port"];
            var generationHash = TestHelper.GetConfig()[$"Environments:{env}:Hash"];
            var seedAccountPK = TestHelper.GetConfig()[$"Environments:{env}:SeedAccountPrivateKey"]; 
            return new TestEnvironment(host, protocol, Convert.ToInt32(port), generationHash,seedAccountPK);

        }

        protected void WatchForFailure(SignedTransaction transaction)
        {
            SiriusWebSocketClient.Listener.TransactionStatus(Address.CreateFromPublicKey(transaction.Signer, NetworkType))
                .Subscribe(
                    e =>
                    {
                        Console.WriteLine(e.Status);

                    });
        }
    }
}
