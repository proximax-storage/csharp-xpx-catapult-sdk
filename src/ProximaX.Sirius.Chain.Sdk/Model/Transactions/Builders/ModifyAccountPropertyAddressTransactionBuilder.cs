using ProximaX.Sirius.Chain.Sdk.Model.Accounts;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions.Builders
{
    public class ModifyAccountPropertyAddressTransactionBuilder : ModifyAccountPropertyTransactionBuilder<Address>
    {
        public ModifyAccountPropertyAddressTransactionBuilder() : 
            base(EntityType.MODIFY_ACCOUNT_PROPERTY_ADDRESS, EntityVersion.MODIFY_ACCOUNT_PROPERTY_ADDRESS.GetValue())
        {
        }

        public override ModifyAccountPropertyTransaction<Address> Build()
        {
            
            // use or calculate maxFee
            var maxFee = MaxFee ?? GetMaxFeeCalculation(AddressModification.CalculatePayloadSize(Modifications.Count));

            // create transaction instance
            return ModifyAccountPropertyTransaction<Address>.CreateForAddress(Deadline, maxFee, PropertyType,(List<AccountPropertyModification<Address>>)Modifications,NetworkType); 

        }
    }
}
