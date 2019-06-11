// Copyright 2019 ProximaX
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.0
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System;

namespace ProximaX.Sirius.Sdk.Model.Transactions
{
    /// <summary>
    ///     Deadline
    /// </summary>
    public class Deadline
    {
        private Deadline(TimeSpan time)
        {
            var deadline = DateTimeOffset.UtcNow.AddTicks(time.Ticks);

            Ticks = (ulong) deadline.ToUnixTimeMilliseconds() - 1459468800000;
        }

        public Deadline(ulong deadline)
        {
            Ticks = deadline;
        }

        /// <summary>
        ///     The ticks
        /// </summary>
        public ulong Ticks { get; }

        public static Deadline Create(double deadline = 1, ChronoUnit unit = ChronoUnit.HOURS)
        {
            if (deadline <= 0)
                throw new Exception("Deadline should be greater than 0");

            switch (unit)
            {
                case ChronoUnit.HOURS:
                    return new Deadline(TimeSpan.FromHours(deadline));
                case ChronoUnit.MINUTES:
                    return new Deadline(TimeSpan.FromMinutes(deadline));
                default:
                    return new Deadline(TimeSpan.FromHours(deadline));
            }
        }

        public DateTime GetLocalDateTime(TimeZoneInfo timeZoneInfo)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(DateTimeOffset.FromUnixTimeMilliseconds((long) Ticks).UtcDateTime,
                timeZoneInfo);
        }

        public DateTime GetLocalDateTime()
        {
            return DateTimeOffset.FromUnixTimeMilliseconds((long) Ticks).LocalDateTime;
        }
    }
}