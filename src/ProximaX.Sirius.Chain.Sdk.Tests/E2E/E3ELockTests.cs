using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Xunit.Abstractions;

namespace ProximaX.Sirius.Chain.Sdk.Tests.E2E
{
    public class E3ELockTests : IClassFixture<E2EBaseFixture>
    {
        private readonly E2EBaseFixture Fixture;
        private readonly ITestOutputHelper Log;

        public E3ELockTests(E2EBaseFixture fixture, ITestOutputHelper log)
        {
            Fixture = fixture;
            Log = log;
        }
    }
}