using System;
using System.Reactive.Linq;
using System.Threading.Tasks;
using ProximaX.Sirius.Chain.Sdk.Client;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Namespaces;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;

namespace ProximaX.Sirius.Chain.SdkNetCore.Examples.Accounts
{
    public class NamespaceExample
    {
        private readonly SiriusClient client;

        public NamespaceExample()
        {
            client = new SiriusClient("https://bctestnet1.xpxsirius.io");
        }

        public async Task RegisterNewNamespace()
        {
            var namespaceName = "proximax";

            // Creates a new namespace
            var namespaceId = new NamespaceId(namespaceName);

            // Verifies the namespace is available
            var namespaceInfo = await client.NamespaceHttp.GetNamespace(namespaceId);

            if (namespaceInfo != null)
            {
                throw new ArgumentNullException($"Namespace already exits");
            }

            var registerNamespaceTransaction = RegisterNamespaceTransaction.CreateRootNamespace(
                    Deadline.Create(),
                    namespaceName,
                    (ulong)100,
                    NetworkType.MIJIN_TEST);

            var account = Account.CreateFromPrivateKey("85CFAB0E6079DAA58D7FF0990ACA64E571EC58527A16DB9391C87C436261190C"
                , NetworkType.MIJIN_TEST);

            var generationHash = client.BlockHttp.GetGenerationHash().Wait();


            var signedTransaction = account.Sign(registerNamespaceTransaction, generationHash);

            // Announce transaction
            await client.TransactionHttp.Announce(signedTransaction);

            // Check namespace info
            namespaceInfo = await client.NamespaceHttp.GetNamespace(namespaceId);

            Console.WriteLine($"Namespace info {namespaceInfo}");

            var parentNamespaceId = new NamespaceId("proximax");

            // create a subnamespace
            var registerSubNamespaceTransaction = RegisterNamespaceTransaction.CreateSubNamespace(
                Deadline.Create(),
                namespaceName,
                parentNamespaceId,
                NetworkType.MIJIN_TEST
            );

        }

    }
}
