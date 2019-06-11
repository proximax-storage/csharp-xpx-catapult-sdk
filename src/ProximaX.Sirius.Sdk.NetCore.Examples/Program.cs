using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ProximaX.Sirius.Sdk.Client;

namespace Proximax.Sirius.Sdk.NetCore.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            GetNetworkTypeInfo().Wait();
        }

        private static async Task GetNetworkTypeInfo()
        {
            var siriusClient = new SiriusClient("https://bctestnet1.xpxsirius.io");

            var networkType = await siriusClient.NetworkHttp.GetNetworkType();

            Console.WriteLine($"Current Network Type {networkType}");
        }
    }
}
