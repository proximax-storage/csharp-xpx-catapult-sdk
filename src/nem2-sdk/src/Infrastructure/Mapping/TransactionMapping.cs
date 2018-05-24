using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using io.nem2.sdk.Core.Crypto.Chaso.NaCl;
using io.nem2.sdk.Core.Utils;
using io.nem2.sdk.Infrastructure.Buffers.Model;
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
                return new AggregateTransactionMapping().apply(input);
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

        internal ulong ExtractBigInteger(JObject input, string identifier)
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
    }

    internal class AggregateTransactionMapping  : TransactionMapping
    {        
        public AggregateTransaction apply(string input)
        {
            var tx = JsonConvert.DeserializeObject<AggregateTransactionInfoDTO>(input);

            var txInfo = CreateTransactionInfo(JObject.Parse(input));

            var deadline = new Deadline(tx.Transaction.Deadline);

            var transaction = JObject.Parse(input)["transaction"] as JObject;

            List<Transaction> txs = new List<Transaction>();
        
            for (int i = 0; i < transaction["transactions"].ToList().Count; i++)
            {
                var innerTransaction = transaction["transactions"].ToList()[i] as JObject;

                var innerInnerTransaction  = innerTransaction["transaction"].ToObject<JObject>();
                innerInnerTransaction.Add("deadline",tx.Transaction.Deadline);
                innerInnerTransaction.Add("fee", tx.Transaction.Fee);
                innerInnerTransaction.Add("signature", transaction["signature"]);
                innerTransaction["transaction"] = innerInnerTransaction;
                if (innerTransaction["meta"] == null)
                {
                    innerTransaction.Add("meta", JObject.Parse(input)["meta"]);
                }
               
                txs.Add(new TransactionMapping().Apply(innerTransaction.ToString()));

            }

            var cosignatures = new List<AggregateTransactionCosignature>();

            if (transaction["cosignatures"] != null)
            {
                cosignatures = transaction["cosignatures"]
                        .Select(i =>  new AggregateTransactionCosignature(
                            i["signature"].ToString(),
                    new PublicAccount(i["signer"].ToString(), ExtractNetworkType(tx.Transaction.Version))
                    )).ToList();
            }

            return new AggregateTransaction(
                    ExtractNetworkType(tx.Transaction.Version),
                    3,
                    tx.Transaction.Type.GetRawValue(),            
                    deadline,
                    tx.Transaction.Fee,
                    txs,
                    cosignatures,
                    tx.Transaction.Signature,
                    new PublicAccount(tx.Transaction.Signer, ExtractNetworkType(tx.Transaction.Version)),
                    txInfo
            );
        }
    }

    internal class TransferTransactionMapping : TransactionMapping
    {
        internal new TransferTransaction Apply(string input)
        {
            var tx = JsonConvert.DeserializeObject<TransferTransactionInfoDTO>(input);

            var txInfo = TransactionInfo.Create(tx.Meta.Height, tx.Meta.Index, tx.Meta.Id, tx.Meta.Hash, tx.Meta.MerkleComponentHash);
            var deadline = new Deadline(tx.Transaction.Deadline);
            var mosaics = tx.Transaction.Mosaics.Select(m => new Mosaic(new MosaicId(BitConverter.ToUInt64(m.MosaicId.FromHex(), 0)),m.Amount)).ToList();

            IMessage message;

            try
            {
                 message = PlainMessage.Create(Encoding.UTF8.GetString(tx.Transaction.Message.Payload.FromHex()));
            }
            catch (Exception)
            {
                try
                {
                    message = PlainMessage.Create(tx.Transaction.Message.Payload);
                }
                catch (Exception)
                {
                    message = EmptyMessage.Create();
                }
            }

            return new TransferTransaction(
                ExtractNetworkType(tx.Transaction.Version),
                ExtractTransactionVersion(tx.Transaction.Version),
                deadline,
                tx.Transaction.Fee,
                Address.CreateFromEncoded(tx.Transaction.Recipient), 
                mosaics,
                message,
                tx.Transaction.Signature,
                new PublicAccount(tx.Transaction.Signer, ExtractNetworkType(tx.Transaction.Version)),
                txInfo
            );
        }
    }

    internal class MultisigModificationTransactionMapping : TransactionMapping
    {  
        public new ModifyMultisigAccountTransaction Apply(string input)
        {
            var tx = JsonConvert.DeserializeObject<MultisigModificationTransactionInfoDTO>(input);

            var txInfo = TransactionInfo.Create(tx.Meta.Height, tx.Meta.Index, tx.Meta.Id, tx.Meta.Hash, tx.Meta.MerkleComponentHash);
            var deadline = new Deadline(tx.Transaction.Deadline);

            List<MultisigCosignatoryModification> modifications = tx.Transaction.Modifications != null
                ? tx
                    .Transaction.Modifications
                    .Select(e =>
                        new MultisigCosignatoryModification(
                            MultisigCosignatoryModificationType.GetRawValue(e.Type),
                            new PublicAccount(e.CosignatoryPublicKey, ExtractNetworkType(tx.Transaction.Version))
                        )
                    ).ToList() : new List<MultisigCosignatoryModification>();

            return new ModifyMultisigAccountTransaction(
                ExtractNetworkType(tx.Transaction.Version),   
                3,
                deadline,
                tx.Transaction.Fee,
                tx.Transaction.MinApprovalDelta,
                tx.Transaction.MinRemovalDelta,
                modifications,
                tx.Transaction.Signature,
                new PublicAccount(tx.Transaction.Signer, ExtractNetworkType(tx.Transaction.Version)),
                txInfo
            );
        }
    }

    internal class NamespaceCreationTransactionMapping : TransactionMapping
    { 
        public new RegisterNamespaceTransaction Apply(string input)
        {
            var tx = JsonConvert.DeserializeObject<NamespaceTransactionInfoDTO>(input);
            var transactionInfo = TransactionInfo.Create(tx.Meta.Height, tx.Meta.Index, tx.Meta.Id, tx.Meta.Hash, tx.Meta.MerkleComponentHash);
            var namespaceType = NamespaceTypes.GetRawValue(tx.Transaction.NamespaceType);

            return new RegisterNamespaceTransaction(
                ExtractNetworkType(tx.Transaction.Version),
                ExtractTransactionVersion(tx.Transaction.Version),
                new Deadline(tx.Transaction.Deadline), 
                tx.Transaction.Fee,
                tx.Transaction.NamespaceType,
                namespaceType == NamespaceTypes.Types.RootNamespace ? tx.Transaction.Duration : 0,
                namespaceType == NamespaceTypes.Types.SubNamespace ? new NamespaceId(tx.Transaction.ParentId) : null,
                new NamespaceId(tx.Transaction.Name),
                new PublicAccount(tx.Transaction.Signer, ExtractNetworkType(tx.Transaction.Version)),
                tx.Transaction.Signature,
                transactionInfo
            );          
        }
    }

    internal class MosaicDefinitionTransactionMapping: TransactionMapping
    {
        internal new MosaicDefinitionTransaction Apply(string input)
        {
            var tx = JsonConvert.DeserializeObject<MosaicCreationTransactionInfoDTO>(input);
            var transaction = tx.Transaction; 
            var mosaicProperties = tx.Transaction.Properties;
            var flags = "00" + Convert.ToString((long)mosaicProperties[0].value, 2);
            var bitMapFlags = flags.Substring(flags.Length - 3, 3);

            var properties = new MosaicProperties(bitMapFlags.ToCharArray()[2] == '1',
                bitMapFlags.ToCharArray()[1] == '1',
                bitMapFlags.ToCharArray()[0] == '1',
                (int)mosaicProperties[1].value,
                mosaicProperties.Count == 3 ? mosaicProperties[2].value : 0);

            return new MosaicDefinitionTransaction(
                ExtractNetworkType(tx.Transaction.Version),
                ExtractTransactionVersion(tx.Transaction.Version),
                new Deadline(tx.Transaction.Deadline),
                transaction.Fee,
                transaction.Name,
                new NamespaceId(transaction.ParentId),
                new MosaicId(transaction.MosaicId),
                properties,
                transaction.Signature,
                new PublicAccount(transaction.Signer, ExtractNetworkType(tx.Transaction.Version)),
                TransactionInfo.Create(tx.Meta.Height, tx.Meta.Index, tx.Meta.Id, tx.Meta.Hash, tx.Meta.MerkleComponentHash)
            );     
        }       
    }

    internal class MosaicSupplyChangeTransactionMapping : TransactionMapping
    {
        internal new MosaicSupplyChangeTransaction Apply(string input)
        {
            var tx = JsonConvert.DeserializeObject<MosaicSupplyChangeTransactionInfoDTO>(input);
            var transactionInfo = TransactionInfo.Create(tx.Meta.Height, tx.Meta.Index, tx.Meta.Id, tx.Meta.Hash, tx.Meta.MerkleComponentHash);
            var transaction = tx.Transaction;

            return new MosaicSupplyChangeTransaction(
                ExtractNetworkType(tx.Transaction.Version),
                ExtractTransactionVersion(tx.Transaction.Version),
                new Deadline(transaction.Deadline),
                transaction.Fee,
                new MosaicId(transaction.MosaicId),
                MosaicSupplyType.GetRawValue(transaction.Direction),
                transaction.Delta,
                transaction.Signature,
                new PublicAccount(transaction.Signer, ExtractNetworkType(tx.Transaction.Version)),
                transactionInfo
            );
        }
    }

    internal class LockFundsTransactionMapping : TransactionMapping
    {   
        public new LockFundsTransaction Apply(string input)
        {
            var tx = JsonConvert.DeserializeObject<LockFundsTransactionInfoDTO>(input);
            var transactionInfo = TransactionInfo.Create(tx.Meta.Height, tx.Meta.Index, tx.Meta.Id, tx.Meta.Hash, tx.Meta.MerkleComponentHash);
            var transaction = tx.Transaction;

            var mosaic = new Mosaic(new MosaicId(transaction.MosaicId), transaction.Amount);

            return new LockFundsTransaction(
                ExtractNetworkType(tx.Transaction.Version),
                ExtractTransactionVersion(tx.Transaction.Version),
                new Deadline(transaction.Deadline), 
                transaction.Fee,
                mosaic,
                transaction.Duration,
                new SignedTransaction("", transaction.Hash, "", TransactionTypes.Types.AggregateBonded),
                transaction.Signature,
                new PublicAccount(transaction.Signer, ExtractNetworkType(tx.Transaction.Version)),
                transactionInfo
            );
        }
    }

    internal class SecretLockTransactionMapping : TransactionMapping
    {   
        internal new SecretLockTransaction Apply(string input)
        {
            var tx = JsonConvert.DeserializeObject<SecretLockTransactionInfoDTO>(input);
            var transactionInfo = TransactionInfo.Create(tx.Meta.Height, tx.Meta.Index, tx.Meta.Id, tx.Meta.Hash, tx.Meta.MerkleComponentHash);
            var transaction = tx.Transaction;

            var mosaic = new Mosaic(new MosaicId(transaction.MosaicId), transaction.Amount);

            return new SecretLockTransaction(
                ExtractNetworkType(tx.Transaction.Version),
                ExtractTransactionVersion(tx.Transaction.Version),
                new Deadline(transaction.Deadline), 
                transaction.Fee,
                mosaic,
                transaction.Duration,
                HashType.GetRawValue(transaction.HashAlgorithm),
                transaction.Secret,
                Address.CreateFromHex(transaction.Recipient),
                transaction.Signature,
                new PublicAccount(transaction.Signer, ExtractNetworkType(tx.Transaction.Version)),
                transactionInfo
            );
        }
    }

    internal class SecretProofTransactionMapping : TransactionMapping
    {
        public new SecretProofTransaction Apply(string input)
        {
            var tx = JsonConvert.DeserializeObject<SecretProofTransactionInfoDTO>(input);
            var transactionInfo = TransactionInfo.Create(tx.Meta.Height, tx.Meta.Index, tx.Meta.Id, tx.Meta.Hash, tx.Meta.MerkleComponentHash);
            var transaction = tx.Transaction;

            return new SecretProofTransaction(
                ExtractNetworkType(tx.Transaction.Version),
                ExtractTransactionVersion(tx.Transaction.Version),
                new Deadline(transaction.Deadline), 
                transaction.Fee,
                HashType.GetRawValue(transaction.HashAlgorithm),
                transaction.Secret,
                transaction.Proof,
                transaction.Signature,
                new PublicAccount(transaction.Signer, ExtractNetworkType(tx.Transaction.Version)),
                transactionInfo
            );
        }
    }
}
