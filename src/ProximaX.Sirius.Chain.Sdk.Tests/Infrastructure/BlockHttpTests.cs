using FluentAssertions;
using Flurl.Http.Testing;
using ProximaX.Sirius.Chain.Sdk.Infrastructure;
using ProximaX.Sirius.Chain.Sdk.Tests.Utils;
using System;
using System.Collections.Generic;
using System.Reactive.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace ProximaX.Sirius.Chain.Sdk.Tests.Infrastructure
{
    public class BlockHttpTests: BaseTest
    {
        private readonly BlockHttp _blockchainHttp;

        public BlockHttpTests()
        {
            _blockchainHttp = new BlockHttp(BaseUrl);
        }

     
    }
}
