using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace ProximaX.Sirius.Chain.E2ETests.Config
{
    public static class ConfigLoader
    {
        public static IConfiguration GetConfiguration(string path)
        {

            var configFile = string.IsNullOrEmpty(path) ? @"appsettings.json": path;

            return new ConfigurationBuilder()
                 .AddJsonFile(configFile)
                 .Build();
        }
    }
}
