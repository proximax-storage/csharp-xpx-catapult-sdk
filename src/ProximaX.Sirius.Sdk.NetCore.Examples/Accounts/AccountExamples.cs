using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ProximaX.Sirius.Chain.Sdk.Client;

using ProximaX.Sirius.Chain.Sdk.Infrastructure;
using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Blockchain;
using ProximaX.Sirius.Chain.Sdk.Model.Mosaics;
using ProximaX.Sirius.Chain.Sdk.Model.Namespaces;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions;
using ProximaX.Sirius.Chain.Sdk.Model.Transactions.Messages;
using ProximaX.Sirius.Chain.Sdk.Utils;

namespace ProximaX.Sirius.Chain.Sdk.NetCore.Examples.Accounts
{
    public  class AccountExamples
    {
        private readonly SiriusClient _client;

        public AccountExamples()
        {
            _client = new SiriusClient("https://bctestnet1.xpxsirius.io");
        }

        public static void GenerateNewAccount()
        {
            var account = Account.GenerateNewAccount(NetworkType.MIJIN_TEST);

            Console.WriteLine($"{nameof(Account)} : {account}");
        }

        public  void CreateNewAccountUsingKeyPairAddress()
        {
            var account = Account.GenerateNewAccount(NetworkType.MIJIN_TEST);

            Console.WriteLine($"{nameof(Account)} : {account}");
        }

        public  void CreateNewAccountUsingPrivateKey()
        {
            var account = Account.CreateFromPrivateKey("85CFAB0E6079DAA58D7FF0990ACA64E571EC58527A16DB9391C87C436261190C", NetworkType.MIJIN_TEST);

            Console.WriteLine($"{nameof(Account)} : {account}");
        }

        public void GetAccountInfo_Rx()
        {
            var account = Account.GenerateNewAccount(NetworkType.MIJIN_TEST);

            _client.AccountHttp.GetAccountInfo(account.Address)
                .Subscribe(info => Console.WriteLine($"{nameof(AccountInfo)} : {info}"),
                error => Console.WriteLine($"Error: {error}")
            );
          
        }

        public async Task GetAccountInfoAsync()
        {
            var account = Account.GenerateNewAccount(NetworkType.MIJIN_TEST);

            var accountInfo = await _client.AccountHttp.GetAccountInfo(account.Address);
            
            Console.WriteLine($"{nameof(AccountInfo)} : {accountInfo}");

            foreach (var asset in accountInfo.Mosaics)
            {
                Console.WriteLine($"My asset : {asset.Id}. No units: {asset.Amount}");
            }
        }

        public async Task GetAccountMetadataAsync()
        {
            var account = Account.GenerateNewAccount(NetworkType.MIJIN_TEST);

            var accountMetadata = await _client.MetadataHttp.GetMetadataFromAddress(account.Address);

            foreach (var kv in accountMetadata.Fields)
            {
              Console.WriteLine($"Key: {kv.Key}. Value: {kv.Value}");
            }

        }

        public async Task GetAccountFilterAsync()
        {
            var account = Account.GenerateNewAccount(NetworkType.MIJIN_TEST);

            var accountFilter = await _client.AccountHttp.GetAccountProperty(account.Address);

            // Verify the list of block account addresses that don't want
            // to receive transactions from
            var blockAddressFilter =
                accountFilter.AccountProperties.Properties.Single(ap =>
                    ap.PropertyType == PropertyType.BLOCK_ADDRESS);

            foreach (var blockedAddress in blockAddressFilter.Values)
            {
                Console.WriteLine($"Blocked Address {Address.CreateFromHex(blockedAddress.ToString())}");
            }
        }

        public async Task GetAccountFilterMosaicAsync()
        {
            var account = Account.GenerateNewAccount(NetworkType.MIJIN_TEST);

            var accountFilter = await _client.AccountHttp.GetAccountProperty(account.Address);

            // Verify the list of mosaic allowed to receive from transaction
            var allowMosaicFilter =
                accountFilter.AccountProperties.Properties.Single(ap =>
                    ap.PropertyType == PropertyType.ALLOW_MOSAIC);

            foreach (var am in allowMosaicFilter.Values)
            {
                // The mosaic id is an array of uint
                // Need to convert to the List<uint>
                var arrayIds = JsonConvert.DeserializeObject<List<uint>>(am.ToString());

                // Create new mosaic id
                var mosaicId = new MosaicId(arrayIds.FromUInt8Array());

                // Display the mosaic id
                Console.WriteLine($"Mosaic Id {mosaicId}");
            }
  
        }

        public async Task GetAccountTransactionsAsync()
        {
            var account = Account.GenerateNewAccount(NetworkType.MIJIN_TEST);

            var transactions = await _client.AccountHttp.Transactions(account.PublicAccount);

            foreach (var tx in transactions)
            {
                Console.WriteLine($"Transaction Info {tx}");
            }
        }

        public async Task GetAccountTransactionsWithPageSizeAsync()
        {
            var account = Account.GenerateNewAccount(NetworkType.MIJIN_TEST);

            // set page size up to 50 transactions
            const int pageSize = 50;

            // create query string
            var queryParams = new QueryParams(pageSize, "");

            var transactions = await _client.AccountHttp.Transactions(account.PublicAccount, queryParams);

            foreach (var tx in transactions)
            {
                Console.WriteLine($"Transaction Info {tx}");
            }

            // to get more than 100 transactions, you need to make further request
            // with the last transaction identifier known returned by the
            // previous request
            queryParams = new QueryParams(pageSize, transactions.Last().TransactionInfo.Id);

            transactions = await _client.AccountHttp.Transactions(account.PublicAccount, queryParams);

            foreach (var tx in transactions)
            {
                Console.WriteLine($"Transaction Info {tx}");
            }
        }

        public async Task LinkAccountAddressToNamespaceAsync()
        {
            // This is a registered namespace
            var namespaceId = new NamespaceId("proximax.io");

            // The account with network currency
            var account = Account.CreateFromPrivateKey("85CFAB0E6079DAA58D7FF0990ACA64E571EC58527A16DB9391C87C436261190C", NetworkType.MIJIN_TEST);

            // Creates the alias transaction to link the namespace
            // to the account address
            var namespaceAddressLinkTransaction = AliasTransaction.CreateForAddress(
                account.Address,
                namespaceId,
                AliasActionType.LINK,
                Deadline.Create(),
                NetworkType.MIJIN_TEST
            );

            var generationHash = _client.BlockHttp.GetGenerationHash().Wait();

            // Signs the transaction
            var signedTransaction = account.Sign(namespaceAddressLinkTransaction, generationHash);

            // Announce the transaction to the network
            await _client.TransactionHttp.Announce(signedTransaction);

            // Verifies the namespace linked to the address
            var namespaceInfo = await _client.NamespaceHttp.GetNamespace(namespaceId);

            Console.WriteLine($"Namespace links info {namespaceInfo.Alias}");

            // Send some money to the namespace instead of account address
            var seedAccount = Account.CreateFromPrivateKey("85CFAB0E6079DAA58D7FF0990ACA64E571EC58527A16DB9391C87C436261190C", NetworkType.MIJIN_TEST);

            var transferTransaction = TransferTransaction.Create(
                Deadline.Create(),
                namespaceInfo.Id,
                new List<Mosaic>()
                {
                    NetworkCurrencyMosaic.CreateRelative(10)
                },
                PlainMessage.Create("Send to 10 xpx to namespace"),
                _client.NetworkHttp.GetNetworkType().Wait());

            // Signs the transaction
            signedTransaction = seedAccount.Sign(transferTransaction, generationHash);

            // Announce the transaction to the network
            await _client.TransactionHttp.Announce(signedTransaction);

            // Verifies the amount sent to the address linked to the namespace
            var accountInfo = await _client.AccountHttp.GetAccountInfo(account.Address);

            Console.WriteLine($"Check account amount info {accountInfo.Mosaics}");

        }
    }
}
