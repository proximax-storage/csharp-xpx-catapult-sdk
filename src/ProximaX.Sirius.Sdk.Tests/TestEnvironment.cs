using System;
using System.Collections.Generic;
using System.Text;
using ProximaX.Sirius.Sdk.Model.Accounts;
using ProximaX.Sirius.Sdk.Tests.Utils;

namespace ProximaX.Sirius.Sdk.Tests
{
    public class TestEnvironment
    {
        public string Host { get; set; }
        public string Protocol { get; set; }

        public int Port { get; set; }

        public string BaseUrl => $"{Protocol}://{Host}:{Port}";

        public EnvironmentSelection EnvironmentSelection { get; set; }

        public TestEnvironment(string host = null, string protocol = null, int port = default,
            EnvironmentSelection selection = EnvironmentSelection.DEV)
        {
            Host = host;
            Protocol = protocol;
            Port = port;
            EnvironmentSelection = selection;
        }
    }
}
