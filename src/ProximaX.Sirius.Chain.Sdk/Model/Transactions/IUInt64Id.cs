using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace ProximaX.Sirius.Chain.Sdk.Model.Transactions
{
    public interface IUInt64Id
    {
        ulong Id { get; }

        string HexId { get;  }

        string Name { get;  }
    }
}

