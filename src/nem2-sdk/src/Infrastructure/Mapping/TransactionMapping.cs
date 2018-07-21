using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using io.nem2.sdk.Core.Crypto.Chaso.NaCl;
using io.nem2.sdk.Core.Utils;
using io.nem2.sdk.Model.Accounts;
using io.nem2.sdk.Model.Blockchain;
using io.nem2.sdk.Model.Mosaics;
using io.nem2.sdk.Model.Namespace;
using io.nem2.sdk.Model.Transactions;
using io.nem2.sdk.Model.Transactions.Messages;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SimpleJson;

namespace io.nem2.sdk.Infrastructure.Mapping
{
    internal class TransactionMapping   {
        
        internal virtual Transaction Apply(string input)
        {      
            var transaction = JObject.Parse(input)["transaction"];

            var type = transaction["type"].ToObject<int>();

            if (type == TransactionTypes.Types.Transfer.GetValue())
            {
                return new TransferTransactionMapping().Apply(input);
            }        
            if (type == TransactionTypes.Types.RegisterNamespace.GetValue())
            {
                return new NamespaceCreationTransactionMapping().Apply(input);
            }
            if (type == TransactionTypes.Types.MosaicDefinition.GetValue())
            {
                return new MosaicDefinitionTransactionMapping().Apply(input);
            }
            if (type == TransactionTypes.Types.MosaicSupplyChange.GetValue())
            {
                return new MosaicSupplyChangeTransactionMapping().Apply(input);
            }
            if (type == TransactionTypes.Types.ModifyMultisigAccount.GetValue())
            {
                return new MultisigModificationTransactionMapping().Apply(input);
            }
            if (type == TransactionTypes.Types.AggregateComplete.GetValue() || type == TransactionTypes.Types.AggregateBonded.GetValue())
            {
                return new AggregateTransactionMapping().Apply(input);
            }
            if (type == TransactionTypes.Types.LockFunds.GetValue())
            {
                return new LockFundsTransactionMapping().Apply(input);
            }
            if (type == TransactionTypes.Types.SecretLock.GetValue())
            {
                return new SecretLockTransactionMapping().Apply(input);
            }
            if (type == TransactionTypes.Types.SecretProof.GetValue())
            {
                return new SecretProofTransactionMapping().Apply(input);
            }

            throw new Exception("Unimplemented Transaction type");
        }

        internal ulong ExtractBigInteger(JToken input, string identifier)
        {
            return JsonConvert.DeserializeObject<uint[]>(input[identifier].ToString()).FromUInt8Array();
        }

        internal int ExtractInteger(JsonObject input, string identifier)
        {
            return int.Parse(input[identifier].ToString());
        }

        internal int ExtractTransactionVersion(int version)
        {
            return (int)Convert.ToInt64(version.ToString("X").Substring(2, 2), 16);
        }

        internal NetworkType.Types ExtractNetworkType(int version)
        {           
            var networkType = (int)Convert.ToInt64(version.ToString("X").Substring(0, 2), 16);
          
            return NetworkType.GetRawValue(networkType);
        }

        internal TransactionInfo CreateTransactionInfo(JObject jsonObject)
        {
            jsonObject = jsonObject["meta"].ToObject<JObject>();

            if (jsonObject["hash"] != null && jsonObject["id"] != null)
            {
                return TransactionInfo.Create(ExtractBigInteger(jsonObject,"height"),
                        Int32.Parse(jsonObject["index"].ToString()),
                        jsonObject["id"].ToString(),
                        jsonObject["hash"].ToString(),
                        jsonObject["merkleComponentHash"].ToString());
            }

            if (jsonObject["aggregateHash"] != null && jsonObject["id"] != null)
            {
                return TransactionInfo.CreateAggregate(ExtractBigInteger(
                        jsonObject,"height"),
                        Int32.Parse(jsonObject["index"].ToString()),
                        jsonObject["id"].ToString(),
                        jsonObject["aggregateHash"].ToString(),
                        jsonObject["aggregateId"].ToString());
            }

            return TransactionInfo.Create(ExtractBigInteger(jsonObject, "height"),
                        jsonObject["hash"].ToString(),
                        jsonObject["merkleComponentHash"].ToString());
        }

        protected IMessage RetrieveMessage(JToken msg)
        {
            try
            {
                return PlainMessage.Create(Encoding.UTF8.GetString(msg["payload"].ToString().FromHex()));
            }
            catch (Exception)
            {
                try
                {
                    return PlainMessage.Create(msg["payload"].ToString());
                }
                catch (Exception)
                {
                    return EmptyMessage.Create();
                }
            }
        }

        protected TransferTransaction ToTransferTransaction(JObject tx, TransactionInfo txInfo)
        { 
            return new TransferTransaction(
                ExtractNetworkType(int.Parse(tx["transaction"]["version"].ToString())),
                ExtractTransactionVersion(int.Parse(tx["transaction"]["version"].ToString())),
                new Deadline(ExtractBigInteger(tx["transaction"], "deadline")),
                ExtractBigInteger(tx["transaction"], "fee"),
                Address.CreateFromHex(tx["transaction"]["recipient"].ToString()),
                tx["transaction"]["mosaics"].Select(m => new Mosaic(new MosaicId(ExtractBigInteger(m, "id")), ExtractBigInteger(m, "amount"))).ToList(),
                RetrieveMessage(tx["message"]),
                tx["transaction"]["signature"]?.ToString(),
                new PublicAccount(tx["transaction"]["signer"].ToString(), ExtractNetworkType(int.Parse(tx["transaction"]["version"].ToString()))),
                txInfo
            );
        }

        protected ModifyMultisigAccountTransaction ToModificationTransaction(JToken tx, TransactionInfo txInfo)
        {
            List<MultisigCosignatoryModification> modifications = tx["transaction"]["modifications"] != null
                ? tx["transaction"]["modifications"]
                    .Select(e =>
                        new MultisigCosignatoryModification(
                            MultisigCosignatoryModificationType.GetRawValue(byte.Parse(e["type"].ToString())),
                            new PublicAccount(e["cosignatoryPublicKey"].ToString(), ExtractNetworkType(int.Parse(tx["transaction"]["version"].ToString())))
                        )
                    ).ToList() : new List<MultisigCosignatoryModification>();
            
            return new ModifyMultisigAccountTransaction(
                ExtractNetworkType(int.Parse(tx["transaction"]["version"].ToString())),
                ExtractTransactionVersion(int.Parse(tx["transaction"]["version"].ToString())),
                new Deadline(ExtractBigInteger(tx["transaction"], "deadline")),
                ExtractBigInteger(tx["transaction"], "fee"),
                int.Parse(tx["transaction"]["minApprovalDelta"].ToString()),
                int.Parse(tx["transaction"]["minRemovalDelta"].ToString()),
                modifications,
                tx["transaction"]["signature"]?.ToString(),
                new PublicAccount(tx["transaction"]["signer"].ToString(), ExtractNetworkType(int.Parse(tx["transaction"]["version"].ToString()))),
                txInfo
            );
        }

        protected MosaicDefinitionTransaction ToMosaicDefinitionTransaction(JToken tx, TransactionInfo txInfo)
        {
            var mosaicProperties = tx["transaction"]["properties"];
            var flags = "00" + Convert.ToString((int)ExtractBigInteger(mosaicProperties[0], "value"), 2);
            var bitMapFlags = flags.Substring(flags.Length - 3, 3);

            var properties = new MosaicProperties(
                bitMapFlags.ToCharArray()[2] == '1',
                bitMapFlags.ToCharArray()[1] == '1',
                bitMapFlags.ToCharArray()[0] == '1',
                (int)ExtractBigInteger(mosaicProperties[1], "value"),
                mosaicProperties.ToList().Count == 3 ? ExtractBigInteger(mosaicProperties[2], "value") : 0);

            return new MosaicDefinitionTransaction(
                ExtractNetworkType(int.Parse(tx["transaction"]["version"].ToString())),
                ExtractTransactionVersion(int.Parse(tx["transaction"]["version"].ToString())),
                new Deadline(ExtractBigInteger(tx["transaction"], "deadline")),
                ExtractBigInteger(tx["transaction"], "fee"),
                tx["transaction"]["name"].ToString(),
                new NamespaceId(ExtractBigInteger(tx["transaction"], "parentId")),
                new MosaicId(ExtractBigInteger(tx["transaction"],"mosaicId")),
                properties,
                tx["transaction"]["signature"]?.ToString(),
                new PublicAccount(tx["transaction"]["signer"].ToString(), ExtractNetworkType(int.Parse(tx["transaction"]["version"].ToString()))),
                txInfo
            );
        }

        protected MosaicSupplyChangeTransaction ToMosaicSupplychangeTransaction(JToken tx, TransactionInfo txInfo)
        {
            return new MosaicSupplyChangeTransaction(
                ExtractNetworkType(int.Parse(tx["transaction"]["version"].ToString())),
                ExtractTransactionVersion(int.Parse(tx["transaction"]["version"].ToString())),
                new Deadline(ExtractBigInteger(tx["transaction"], "deadline")),
                ExtractBigInteger(tx["transaction"], "fee"),
                new MosaicId(ExtractBigInteger(tx["transaction"],"mosaicId")),
                MosaicSupplyType.GetRawValue(byte.Parse(tx["transaction"]["direction"].ToString())),
                ExtractBigInteger(tx["transaction"], "delta"),
                tx["transaction"]["signature"]?.ToString(),
                new PublicAccount(tx["transaction"]["signer"].ToString(), ExtractNetworkType(int.Parse(tx["transaction"]["version"].ToString()))),
                txInfo
            );
        }

        protected RegisterNamespaceTransaction ToNamespaceCreationTransaction(JToken tx, TransactionInfo txInfo)
        {
            return new RegisterNamespaceTransaction(
                ExtractNetworkType(int.Parse(tx["transaction"]["version"].ToString())),
                ExtractTransactionVersion(int.Parse(tx["transaction"]["version"].ToString())),
                new Deadline(ExtractBigInteger(tx["transaction"], "deadline")),
                ExtractBigInteger(tx["transaction"], "fee"),
                byte.Parse(tx["transaction"]["namespaceType"].ToString()),
                NamespaceTypes.GetRawValue(byte.Parse(tx["transaction"]["namespaceType"].ToString())) == NamespaceTypes.Types.RootNamespace ? ExtractBigInteger(tx["transaction"], "duration") : 0,
                NamespaceTypes.GetRawValue(byte.Parse(tx["transaction"]["namespaceType"].ToString())) == NamespaceTypes.Types.SubNamespace ? new NamespaceId(ExtractBigInteger(tx["transaction"], "parentId")) : null,
                new NamespaceId(tx["transaction"]["name"].ToString()),
                tx["transaction"]["signature"]?.ToString(),
                new PublicAccount(tx["transaction"]["signer"].ToString(), ExtractNetworkType(int.Parse(tx["transaction"]["version"].ToString()))),              
                txInfo
            );
        }

        protected LockFundsTransaction ToLockFundsTransaction(JToken tx, TransactionInfo txInfo)
        {
            return new LockFundsTransaction(
                ExtractNetworkType(int.Parse(tx["transaction"]["version"].ToString())),
                ExtractTransactionVersion(int.Parse(tx["transaction"]["version"].ToString())),
                new Deadline(ExtractBigInteger(tx["transaction"], "deadline")),
                ExtractBigInteger(tx["transaction"], "fee"),
                new Mosaic(new MosaicId(ExtractBigInteger(tx["transaction"], "mosaicId")), ExtractBigInteger(tx["transaction"], "amount")),
                ExtractBigInteger(tx["transaction"], "duration"),
                new SignedTransaction("", tx["transaction"]["hash"].ToString(), "", TransactionTypes.Types.AggregateBonded),
                tx["transaction"]["signature"]?.ToString(),
                new PublicAccount(tx["transaction"]["signer"].ToString(), ExtractNetworkType(int.Parse(tx["transaction"]["version"].ToString()))),
                txInfo
            );
        }

        protected SecretLockTransaction ToSecretLockTransaction(JToken tx, TransactionInfo txInfo)
        {
            return new SecretLockTransaction(
                ExtractNetworkType(int.Parse(tx["transaction"]["version"].ToString())),
                ExtractTransactionVersion(int.Parse(tx["transaction"]["version"].ToString())),
                new Deadline(ExtractBigInteger(tx["transaction"], "deadline")),
                ExtractBigInteger(tx["transaction"], "fee"),
                new Mosaic(new MosaicId(ExtractBigInteger(tx["transaction"],"mosaicId")), ExtractBigInteger(tx["transaction"], "amount")),
                ExtractBigInteger(tx["transaction"], "duration"),
                HashType.GetRawValue(byte.Parse(tx["transaction"]["hashAlgorithm"].ToString())),
                tx["transaction"]["secret"].ToString(),
                Address.CreateFromHex(tx["transaction"]["recipient"].ToString()),
                tx["transaction"]["signature"]?.ToString(),
                new PublicAccount(tx["transaction"]["signer"].ToString(), ExtractNetworkType(int.Parse(tx["transaction"]["version"].ToString()))),
                txInfo
            );
        }

        protected SecretProofTransaction ToSecretProofTransaction(JToken tx, TransactionInfo txInfo)
        {
            return new SecretProofTransaction(
                ExtractNetworkType(int.Parse(tx["transaction"]["version"].ToString())),
                ExtractTransactionVersion(int.Parse(tx["transaction"]["version"].ToString())),
                new Deadline(ExtractBigInteger(tx["transaction"], "deadline")),
                ExtractBigInteger(tx["transaction"], "fee"),
                HashType.GetRawValue(byte.Parse(tx["transaction"]["hashAlgorithm"].ToString())),
                tx["transaction"]["secret"].ToString(),
                tx["transaction"]["proof"].ToString(),
                tx["transaction"]["signature"]?.ToString(),
                new PublicAccount(tx["transaction"]["signer"].ToString(), ExtractNetworkType(int.Parse(tx["transaction"]["version"].ToString()))),
                txInfo
            );
        }
    }

    internal class AggregateTransactionMapping  : TransactionMapping
    {
        private List<Transaction> MapInnerTransactions(JObject transaction)
        {
            List<Transaction> txs = new List<Transaction>();

            for (int i = 0; i < transaction["transaction"]["transactions"].ToList().Count; i++)
            {
                var innerTransaction = transaction["transaction"]["transactions"].ToList()[i] as JObject;

                var innerInnerTransaction = innerTransaction["transaction"].ToObject<JObject>();
                innerInnerTransaction.Add("deadline", transaction["transaction"]["deadline"]);
                innerInnerTransaction.Add("fee", transaction["transaction"]["fee"]);
                innerInnerTransaction.Add("signature", transaction["signature"]);
                innerTransaction["transaction"] = innerInnerTransaction;
                if (innerTransaction["meta"] == null)
                {
                    innerTransaction.Add("meta", transaction["meta"]);
                }

                txs.Add(new TransactionMapping().Apply(innerTransaction.ToString()));
            }

            return txs;
        }

        private List<AggregateTransactionCosignature> MapCosignatures(JObject transaction)
        {
            var cosignatures = new List<AggregateTransactionCosignature>();

            if (transaction["transaction"]["cosignatures"] != null)
            {
                cosignatures = transaction["transaction"]["cosignatures"]
                    .Select(i => new AggregateTransactionCosignature(
                        i["signature"].ToString(),
                        new PublicAccount(i["signer"].ToString(), ExtractNetworkType(int.Parse(transaction["transaction"]["version"].ToString())))
                    )).ToList();
            }

            return cosignatures;
        }

        internal new AggregateTransaction Apply(string input)
        {
            var tx = JObject.Parse(input);

            return new AggregateTransaction(
                    ExtractNetworkType(int.Parse(tx["transaction"]["version"].ToString())),
                    ExtractTransactionVersion(int.Parse(tx["transaction"]["version"].ToString())),
                    ushort.Parse(tx["transaction"]["type"].ToString()).GetRawValue(),
                    new Deadline(ExtractBigInteger(tx["transaction"], "deadline")),
                    ExtractBigInteger(tx["transaction"], "fee"),
                    MapInnerTransactions(tx),
                    MapCosignatures(tx),
                    tx["transaction"]["signature"].ToString(),
                    new PublicAccount(tx["transaction"]["signer"].ToString(), ExtractNetworkType(int.Parse(tx["transaction"]["version"].ToString()))),
                    CreateTransactionInfo(JObject.Parse(input))
            );
        }
    }

    internal class TransferTransactionMapping : TransactionMapping
    {
        internal new TransferTransaction Apply(string input)
        {
            var tx = JObject.Parse(input);
            return ToTransferTransaction(tx, CreateTransactionInfo(tx));
        }
    }

    internal class MultisigModificationTransactionMapping : TransactionMapping
    {  
        internal new ModifyMultisigAccountTransaction Apply(string input)
        {
            var tx =JObject.Parse(input);
            return ToModificationTransaction(tx, CreateTransactionInfo(tx));
        }
    }

    internal class NamespaceCreationTransactionMapping : TransactionMapping
    { 
        internal new RegisterNamespaceTransaction Apply(string input)
        {
            var tx = JObject.Parse(input);
            return ToNamespaceCreationTransaction(tx, CreateTransactionInfo(tx));
        }
    }

    internal class MosaicDefinitionTransactionMapping: TransactionMapping
    {
        internal new MosaicDefinitionTransaction Apply(string input)
        {
            var tx = JObject.Parse(input);
            return ToMosaicDefinitionTransaction(tx, CreateTransactionInfo(tx));
        }       
    }

    internal class MosaicSupplyChangeTransactionMapping : TransactionMapping
    {
        internal new MosaicSupplyChangeTransaction Apply(string input)
        {
            var tx = JObject.Parse(input);
            return ToMosaicSupplychangeTransaction(tx, CreateTransactionInfo(tx));
        }
    }

    internal class LockFundsTransactionMapping : TransactionMapping
    {
        internal new LockFundsTransaction Apply(string input)
        {
            var tx =JObject.Parse(input);
            return ToLockFundsTransaction(tx, CreateTransactionInfo(tx));
        }
    }

    internal class SecretLockTransactionMapping : TransactionMapping
    {   
        internal new SecretLockTransaction Apply(string input)
        {
            var tx = JObject.Parse(input);
            return ToSecretLockTransaction(tx, CreateTransactionInfo(tx));
        }
    }

    internal class SecretProofTransactionMapping : TransactionMapping
    {
        public new SecretProofTransaction Apply(string input)
        {
            var tx = JObject.Parse(input);
            return ToSecretProofTransaction(tx, CreateTransactionInfo(tx));
        }
    }
}
