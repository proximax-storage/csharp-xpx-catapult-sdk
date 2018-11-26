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



        internal TransactionInfo CreateTransactionInfo(JObject jsonObject)
        {
            jsonObject = jsonObject["meta"].ToObject<JObject>();

            if (jsonObject["hash"] != null && jsonObject["id"] != null)
            {
                return TransactionInfo.Create(jsonObject.ExtractBigInteger("height"),
                        Int32.Parse(jsonObject["index"].ToString()),
                        jsonObject["id"].ToString(),
                        jsonObject["hash"].ToString(),
                        jsonObject["merkleComponentHash"].ToString());
            }

            if (jsonObject["aggregateHash"] != null && jsonObject["id"] != null)
            {
                return TransactionInfo.CreateAggregate(
                        jsonObject.ExtractBigInteger("height"),
                        Int32.Parse(jsonObject["index"].ToString()),
                        jsonObject["id"].ToString(),
                        jsonObject["aggregateHash"].ToString(),
                        jsonObject["aggregateId"].ToString());
            }

            return TransactionInfo.Create(
                        jsonObject.ExtractBigInteger("height"),
                        jsonObject["hash"].ToString(),
                        jsonObject["merkleComponentHash"].ToString());
        }

        protected IMessage RetrieveMessage(JToken msg)
        {
            try
            {
                if (msg != null && msg["type"].ToString() == "0")
                {
                    return PlainMessage.Create(Encoding.UTF8.GetString(msg["payload"].ToString().FromHex()));
                } else if (msg != null && msg["type"].ToString() == "1")
                {
                    return SecureMessage.CreateFromEncodedPayload(msg["payload"].ToString().FromHex());
                }
                else
                {
                    return EmptyMessage.Create();
                }
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
                int.Parse(tx["transaction"]["version"].ToString()).ExtractNetworkType(),
                int.Parse(tx["transaction"]["version"].ToString()).ExtractVersion(),
                new Deadline(tx["transaction"].ExtractBigInteger("deadline")),
                tx["transaction"].ExtractBigInteger("fee"),
                Address.CreateFromHex(tx["transaction"]["recipient"].ToString()),
                tx["transaction"]["mosaics"] == null ? new List<Mosaic>() 
                    : tx["transaction"]["mosaics"].Select(m => new Mosaic(new MosaicId(m.ExtractBigInteger("id")), m.ExtractBigInteger("amount"))).ToList(),
                RetrieveMessage(tx["transaction"]["message"]),
                tx["transaction"]["signature"]?.ToString(),
                new PublicAccount(tx["transaction"]["signer"].ToString(), int.Parse(tx["transaction"]["version"].ToString()).ExtractNetworkType()),
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
                            new PublicAccount(e["cosignatoryPublicKey"].ToString(), int.Parse(tx["transaction"]["version"].ToString()).ExtractNetworkType())
                        )
                    ).ToList() : new List<MultisigCosignatoryModification>();
            
            return new ModifyMultisigAccountTransaction(
                int.Parse(tx["transaction"]["version"].ToString()).ExtractNetworkType(),
                int.Parse(tx["transaction"]["version"].ToString()).ExtractVersion(),
                new Deadline(tx["transaction"].ExtractBigInteger("deadline")),
                tx["transaction"].ExtractBigInteger("fee"),
                int.Parse(tx["transaction"]["minApprovalDelta"].ToString()),
                int.Parse(tx["transaction"]["minRemovalDelta"].ToString()),
                modifications,
                tx["transaction"]["signature"]?.ToString(),
                new PublicAccount(tx["transaction"]["signer"].ToString(),  int.Parse(tx["transaction"]["version"].ToString()).ExtractNetworkType()),
                txInfo
            );
        }

        protected MosaicDefinitionTransaction ToMosaicDefinitionTransaction(JToken tx, TransactionInfo txInfo)
        {
            var mosaicProperties = tx["transaction"]["properties"];
            var flags = "00" + Convert.ToString((int)mosaicProperties[0].ExtractBigInteger("value"), 2);
            var bitMapFlags = flags.Substring(flags.Length - 3, 3);

            var properties = new MosaicProperties(
                bitMapFlags.ToCharArray()[2] == '1',
                bitMapFlags.ToCharArray()[1] == '1',
                bitMapFlags.ToCharArray()[0] == '1',
                (int)mosaicProperties[1].ExtractBigInteger("value"),
                mosaicProperties.ToList().Count == 3 ? mosaicProperties[2].ExtractBigInteger("value") : 0);

            return new MosaicDefinitionTransaction(
                int.Parse(tx["transaction"]["version"].ToString()).ExtractNetworkType(),
                int.Parse(tx["transaction"]["version"].ToString()).ExtractVersion(),
                new Deadline(tx["transaction"].ExtractBigInteger("deadline")),
                tx["transaction"].ExtractBigInteger("fee"),
                tx["transaction"]["name"].ToString(),
                new NamespaceId(tx["transaction"].ExtractBigInteger("parentId")),
                new MosaicId(tx["transaction"].ExtractBigInteger("mosaicId")),
                properties,
                tx["transaction"]["signature"]?.ToString(),
                new PublicAccount(tx["transaction"]["signer"].ToString(),  int.Parse(tx["transaction"]["version"].ToString()).ExtractNetworkType()),
                txInfo
            );
        }

        protected MosaicSupplyChangeTransaction ToMosaicSupplychangeTransaction(JToken tx, TransactionInfo txInfo)
        {
            return new MosaicSupplyChangeTransaction(
                int.Parse(tx["transaction"]["version"].ToString()).ExtractNetworkType(),
                int.Parse(tx["transaction"]["version"].ToString()).ExtractVersion(),
                new Deadline(tx["transaction"].ExtractBigInteger("deadline")),
                tx["transaction"].ExtractBigInteger("fee"),
                new MosaicId(tx["transaction"].ExtractBigInteger("mosaicId")),
                MosaicSupplyType.GetRawValue(byte.Parse(tx["transaction"]["direction"].ToString())),
                tx["transaction"].ExtractBigInteger("delta"),
                tx["transaction"]["signature"]?.ToString(),
                new PublicAccount(tx["transaction"]["signer"].ToString(),  int.Parse(tx["transaction"]["version"].ToString()).ExtractNetworkType()),
                txInfo
            );
        }

        protected RegisterNamespaceTransaction ToNamespaceCreationTransaction(JToken tx, TransactionInfo txInfo)
        {
            return new RegisterNamespaceTransaction(
                int.Parse(tx["transaction"]["version"].ToString()).ExtractNetworkType(),
                int.Parse(tx["transaction"]["version"].ToString()).ExtractVersion(),
                new Deadline(tx["transaction"].ExtractBigInteger("deadline")),
                tx["transaction"].ExtractBigInteger("fee"),
                byte.Parse(tx["transaction"]["namespaceType"].ToString()),
                NamespaceTypes.GetRawValue(byte.Parse(tx["transaction"]["namespaceType"].ToString())) == NamespaceTypes.Types.RootNamespace ? tx["transaction"].ExtractBigInteger("duration") : 0,
                NamespaceTypes.GetRawValue(byte.Parse(tx["transaction"]["namespaceType"].ToString())) == NamespaceTypes.Types.SubNamespace ? new NamespaceId(tx["transaction"].ExtractBigInteger("parentId")) : null,
                new NamespaceId(tx["transaction"]["name"].ToString()),
                tx["transaction"]["signature"]?.ToString(),
                new PublicAccount(tx["transaction"]["signer"].ToString(),  int.Parse(tx["transaction"]["version"].ToString()).ExtractNetworkType()),              
                txInfo
            );
        }

        protected LockFundsTransaction ToLockFundsTransaction(JToken tx, TransactionInfo txInfo)
        {
            return new LockFundsTransaction(
                int.Parse(tx["transaction"]["version"].ToString()).ExtractNetworkType(),
                int.Parse(tx["transaction"]["version"].ToString()).ExtractVersion(),
                new Deadline(tx["transaction"].ExtractBigInteger("deadline")),
                tx["transaction"].ExtractBigInteger("fee"),
                new Mosaic(new MosaicId(tx["transaction"].ExtractBigInteger("mosaicId")), tx["transaction"].ExtractBigInteger("amount")),
                tx["transaction"].ExtractBigInteger("duration"),
                new SignedTransaction("", tx["transaction"]["hash"].ToString(), "", TransactionTypes.Types.AggregateBonded),
                tx["transaction"]["signature"]?.ToString(),
                new PublicAccount(tx["transaction"]["signer"].ToString(),  int.Parse(tx["transaction"]["version"].ToString()).ExtractNetworkType()),
                txInfo
            );
        }

        protected SecretLockTransaction ToSecretLockTransaction(JToken tx, TransactionInfo txInfo)
        {
            return new SecretLockTransaction(
                 int.Parse(tx["transaction"]["version"].ToString()).ExtractNetworkType(),
                int.Parse(tx["transaction"]["version"].ToString()).ExtractVersion(),
                 new Deadline(tx["transaction"].ExtractBigInteger("deadline")),
                tx["transaction"].ExtractBigInteger("fee"),
                new Mosaic(new MosaicId(tx["transaction"].ExtractBigInteger("mosaicId")), tx["transaction"].ExtractBigInteger("amount")),
                tx["transaction"].ExtractBigInteger("duration"),
                HashType.GetRawValue(byte.Parse(tx["transaction"]["hashAlgorithm"].ToString())),
                tx["transaction"]["secret"].ToString(),
                Address.CreateFromHex(tx["transaction"]["recipient"].ToString()),
                tx["transaction"]["signature"]?.ToString(),
                new PublicAccount(tx["transaction"]["signer"].ToString(),  int.Parse(tx["transaction"]["version"].ToString()).ExtractNetworkType()),
                txInfo
            );
        }

        protected SecretProofTransaction ToSecretProofTransaction(JToken tx, TransactionInfo txInfo)
        {
            return new SecretProofTransaction(
                 int.Parse(tx["transaction"]["version"].ToString()).ExtractNetworkType(),
                int.Parse(tx["transaction"]["version"].ToString()).ExtractVersion(),
                 new Deadline(tx["transaction"].ExtractBigInteger("deadline")),
                tx["transaction"].ExtractBigInteger("fee"),
                HashType.GetRawValue(byte.Parse(tx["transaction"]["hashAlgorithm"].ToString())),
                tx["transaction"]["secret"].ToString(),
                tx["transaction"]["proof"].ToString(),
                tx["transaction"]["signature"]?.ToString(),
                new PublicAccount(tx["transaction"]["signer"].ToString(),  int.Parse(tx["transaction"]["version"].ToString()).ExtractNetworkType()),
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
                        new PublicAccount(i["signer"].ToString(), int.Parse(transaction["transaction"]["version"].ToString()).ExtractNetworkType())
                    )).ToList();
            }

            return cosignatures;
        }

        internal new AggregateTransaction Apply(string input)
        {
            var tx = JObject.Parse(input);

            return new AggregateTransaction(
                     int.Parse(tx["transaction"]["version"].ToString()).ExtractNetworkType(),
                    int.Parse(tx["transaction"]["version"].ToString()).ExtractVersion(),
                    ushort.Parse(tx["transaction"]["type"].ToString()).GetRawValue(),
                     new Deadline(tx["transaction"].ExtractBigInteger("deadline")),
                    tx["transaction"].ExtractBigInteger("fee"),
                    MapInnerTransactions(tx),
                    MapCosignatures(tx),
                    tx["transaction"]["signature"].ToString(),
                    new PublicAccount(tx["transaction"]["signer"].ToString(),  int.Parse(tx["transaction"]["version"].ToString()).ExtractNetworkType()),
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
