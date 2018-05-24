using System;
using io.nem2.sdk.Model.Transactions;

namespace test.Model.Transactions
{
    public class FakeDeadline : Deadline
    {
        public FakeDeadline(TimeSpan time) : base(time)
        {
                
        }

        public static FakeDeadline Create()
        {
            return new FakeDeadline(new TimeSpan(1));
        }

        public override ulong GetInstant()
        {
            return 1;
        }
    }
}
