using ProximaX.Sirius.Chain.Sdk.Model.Namespaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace ProximaX.Sirius.Chain.Sdk.Model.Reciepts
{
   public class ResolutionEntry
    {
        public ResolutionEntry(Alias resolved, ReceiptSource source)
        {
            Resolved = resolved;
            Source = source;
        }

        public Alias Resolved { get; }
        public ReceiptSource Source { get; }
    }
}
