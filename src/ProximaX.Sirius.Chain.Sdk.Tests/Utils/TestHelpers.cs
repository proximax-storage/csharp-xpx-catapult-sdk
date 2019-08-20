using System.IO;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;

namespace ProximaX.Sirius.Chain.Sdk.Tests.Utils
{
    public static class TestHelper
    {
        public static IConfiguration GetConfig()
        {
            
            return new ConfigurationBuilder()
                .AddJsonFile(@"appsettings.json")
                .Build();
        }

        public static JObject LoadJsonFileToObject(string filePath)
        {
            return JObject.Parse(File.ReadAllText(filePath));
        }

        public static JArray LoadJsonFileToArray(string filePath)
        {
            return JArray.Parse(File.ReadAllText(filePath));
        }
    }
}
