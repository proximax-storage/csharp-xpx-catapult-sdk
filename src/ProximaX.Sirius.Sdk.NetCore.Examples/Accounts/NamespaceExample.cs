using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using ProximaX.Sirius.Sdk.Client;
using ProximaX.Sirius.Sdk.Model.Accounts;
using ProximaX.Sirius.Sdk.Model.Blockchain;
using ProximaX.Sirius.Sdk.Model.Namespaces;
using ProximaX.Sirius.Sdk.Model.Transactions;

namespace Proximax.Sirius.Sdk.NetCore.Examples.Accounts
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

            var signedTransaction = account.Sign(registerNamespaceTransaction);

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
