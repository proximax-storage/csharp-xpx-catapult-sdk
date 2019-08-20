using System;
using System.Collections.Generic;
using System.Text;

namespace ProximaX.Sirius.Chain.Sdk.Model.Reciepts
{
    public enum ReceiptVersion
    {
        //Balance transfer receipt version.
        BALANCE_TRANSFER = 0x1,

        //Balance change receipt version
        BALANCE_CHANGE = 0x1,

        //Artifact expiry receipt version
        ARTIFACT_EXPIRY = 0x1,

        //Transaction statement version
        TRANSACTION_STATEMENT = 0x1,

        //Resolution statement version
        RESOLUTION_STATEMENT = 0x1,

        //Resolution statement version
        INFLATION_RECEIPT = 0x1
    }
}
