using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using ProximaX.Sirius.Chain.Sdk.Model.Exceptions;
using ProximaX.Sirius.Chain.Sdk.Model.Namespaces;
using ProximaX.Sirius.Chain.Sdk.Utils;


namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions
{
    public class Recipient
    {
        private Recipient(Address address, NamespaceId namespaceId)
        {
            Address = address;
            NamespaceId = namespaceId;
        }

        public Recipient(Address address) : this(address, null)
        {

        }

        public Recipient(NamespaceId namespaceId) : this(null, namespaceId)
        {

        }

        public static Recipient From(Address address)
        {
            return new Recipient(address);
        }

        public static Recipient From(NamespaceId namespaceId)
        {
            return new Recipient(namespaceId);
        }

        public byte[] GetBytes()
        {
            if (Address != null)
            {
                return Address.Plain.FromBase32String();
            }
            else if (NamespaceId != null)
            {
                return NamespaceId.AliasToRecipient();
            }

            throw new IllegalStateException("Recipient is not specific");
            
        }

        public Address Address { get; }
        public NamespaceId NamespaceId { get; }
    }
}
