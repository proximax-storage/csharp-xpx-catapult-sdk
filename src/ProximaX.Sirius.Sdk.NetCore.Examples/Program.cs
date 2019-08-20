using ProximaX.Sirius.Chain.Sdk.Client;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using System;
using System.Reactive.Linq;
using System.Threading.Tasks;


namespace ProximaX.Sirius.Chain.Sdk.NetCore.Examples
{
    class Program
    {
        static void Main(string[] args)
        {
            GetAccountInfo().Wait();
            GetNetworkTypeInfo().Wait();
        }

        private static async Task GetNetworkTypeInfo()
        {
            var siriusClient = new SiriusClient("https://bctestnet1.xpxsirius.io");

            var networkType = await siriusClient.NetworkHttp.GetNetworkType();

            Console.WriteLine($"Current Network Type {networkType}");
        }

        public static async Task GetAccountInfo()
        {
            string privateKey = "67FDDE70AD258FE1450203F6D63EF1129044FE0D5321EDD92FB451532B35ED43";

            // Create account from pk
            var account = Account.CreateFromPrivateKey(privateKey, NetworkType.TEST_NET);

            Console.WriteLine($"{nameof(Account)} : {account.Address.Plain}");

            // Creates an instance of SiriusClient
            var client = new SiriusClient("https://bctestnet1.xpxsirius.io");

            // Gets the account information
            var accountInfo = await client.AccountHttp.GetAccountInfo(account.Address);

            Console.WriteLine($"{nameof(AccountInfo)} : {accountInfo}");

            // List mosaics
            foreach (var asset in accountInfo.Mosaics)
            {
                Console.WriteLine($"My asset : {asset.Id},No units: {asset.Amount}");
            }

            Console.ReadKey();

            // Get transactions
            var transactions = await client.AccountHttp.Transactions(account.PublicAccount);

            foreach (var tx in transactions)
            {
                Console.WriteLine($"Transaction Info {tx}");
            }

            Console.ReadKey();
        }
    }
}
