
namespace ProximaX.Sirius.Chain.E2ETests.Config
{
    public class AppConfig
    {
        public string Host { get; set; }
        public string Protocol { get; set; }
        public int Port { get; set; }

        public string GenerationHash { get; set; }

        public string SeedAccountPK { get; set; }

        public string BaseUrl => $"{Protocol}://{Host}:{Port}";

        private AppConfig(string protocol, string host, int port, string generationHash, string seedAccountPK)
        {
            Protocol = protocol;
            Host = host;
            Port = port;
            GenerationHash = generationHash;
            SeedAccountPK = seedAccountPK;
        }

        public static AppConfig Create(string filePath = @"appsettings.json")
        {
            var config = ConfigLoader.GetConfiguration(filePath);

            var env = config[$"ActiveEnvironment"] ?? "Dev";

            var protocol = config[$"Environments:{env}:Protocol"];
            var host = config[$"Environments:{env}:Host"];
            var port = int.Parse(config[$"Environments:{env}:Port"]);
            var generationHash = config[$"Environments:{env}:Hash"];
            var seedAccountPK = config[$"Environments:{env}:SeedAccountPrivateKey"];

            return new AppConfig(protocol, host, port, generationHash, seedAccountPK);
        }
    }
}
