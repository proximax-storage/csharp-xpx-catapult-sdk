using System;
using ProximaX.Sirius.Sdk.Tests.Utils;

namespace ProximaX.Sirius.Sdk.Tests
{
    public class BaseTest
    {
        private EnvironmentSelection _environment;

        public string BaseUrl { get; set; }

        protected BaseTest()
        {
          
            string env;
            switch (_environment)
            {
                case EnvironmentSelection.DEV:
                    env = "Dev";
                    break;
                case EnvironmentSelection.BC_STAGE:
                    env = "BcStage";
                    break;
                case EnvironmentSelection.BC_TEST_NET:
                    env = "BcTestNet";
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(Environment), _environment, null);
            }

            var protocol = TestHelper.GetConfig()[$"Environments:{env}:Protocol"];
            var host = TestHelper.GetConfig()[$"Environments:{env}:Host"];
            var port = TestHelper.GetConfig()[$"Environments:{env}:Port"];

            BaseUrl = $"{protocol}://{host}:{port}";
        }

        public BaseTest UseEnvironment(EnvironmentSelection environment)
        {
            _environment = environment;
            return this;
        }
    }
}
