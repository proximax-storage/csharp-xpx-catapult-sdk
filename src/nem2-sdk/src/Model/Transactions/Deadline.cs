// ***********************************************************************
// Assembly         : nem2-sdk
// Author           : kailin
// Created          : 01-15-2018
//
// Last Modified By : kailin
// Last Modified On : 02-01-2018
// ***********************************************************************
// <copyright file="AggregateTransaction.cs" company="Nem.io">
// Copyright 2018 NEM
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
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;

namespace io.nem2.sdk.Model.Transactions
{
    public class Deadline
    {
        internal DateTime EpochDate { get; set; }

        internal ulong Ticks { get; }

        public Deadline(TimeSpan time)
        {
            EpochDate = new DateTime(2016, 4, 1).ToUniversalTime();

            var now = DateTime.Now.ToUniversalTime();

            var deadline = now - EpochDate;

            Ticks = (ulong)deadline.Add(time).TotalMilliseconds;
        }

        public Deadline(ulong ticks)
        {
            EpochDate = new DateTime(2016, 4, 1).ToUniversalTime();

            Ticks = ticks;
        }

        public static Deadline CreateHours(int hours)
        {
            return new Deadline(TimeSpan.FromHours(hours));
        }

        public static Deadline CreateMinutes(int mins)
        {
            return new Deadline(TimeSpan.FromMinutes(mins));
        }

        public virtual ulong GetInstant()
        {
            return Ticks;
        }

        public DateTime GetLocalDateTime(TimeZoneInfo timeZoneInfo)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(new DateTime((long)Ticks), timeZoneInfo);
        }

        public DateTime GetLocalDateTime()
        {
            return TimeZoneInfo.ConvertTimeFromUtc(new DateTime((long)Ticks), TimeZoneInfo.Local);
        }
    }
}
