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

namespace ProximaX.Sirius.Sdk.Model.Mosaics
{
    /// <summary>
    ///     The mosaic properties structure describes mosaic properties.
    /// </summary>
    public class MosaicProperties
    {
        /// <summary>
        ///     Constructor
        /// </summary>
        /// <param name="supplyMutable">The mosaic supply mutability.</param>
        /// <param name="transferable">The mosaic transferability.</param>
        /// <param name="levyMutable">The mosaic levy mutability.</param>
        /// <param name="divisibility">The mosaic divisibility.</param>
        /// <param name="duration">The number of blocks the mosaic will be active.</param>
        /// <exception cref="System.Exception">Duration must be set</exception>
        /// <exception cref="System.ArgumentException">Divisibility must be between 0 and 6</exception>
        public MosaicProperties(bool supplyMutable, bool transferable, bool levyMutable, int divisibility,
            ulong duration)
        {
            if (divisibility < 0 || divisibility > 6)
                throw new ArgumentException("Divisibility must be between 0 and 6");
            IsSupplyMutable = supplyMutable;
            IsTransferable = transferable;
            IsLevyMutable = levyMutable;
            Divisibility = divisibility;
            Duration = duration;
        }

        /// <summary>
        ///     The mosaic supply mutability.
        /// </summary>
        /// <value><c>true</c> if [supply mutable]; otherwise, <c>false</c>.</value>
        public bool IsSupplyMutable { get; }

        /// <summary>
        ///     The mosaic transferability.
        /// </summary>
        /// <value><c>true</c> if transferable; otherwise, <c>false</c>.</value>
        public bool IsTransferable { get; }

        /// <summary>
        ///     The mosaic levy mutability.
        /// </summary>
        /// <value><c>true</c> if [levy mutable]; otherwise, <c>false</c>.</value>
        public bool IsLevyMutable { get; }

        /// <summary>
        ///     The mosaic divisibility.
        /// </summary>
        /// <value>The divisibility.</value>
        public int Divisibility { get; }

        /// <summary>
        ///     The number of blocks the mosaic will be active.
        /// </summary>
        /// <value>The duration.</value>
        public ulong Duration { get; }

        /// <summary>
        ///     Create Mosaic Properties
        /// </summary>
        /// <param name="supplyMutable"></param>
        /// <param name="transferable"></param>
        /// <param name="levyMutable"></param>
        /// <param name="divisibility"></param>
        /// <param name="duration"></param>
        /// <returns></returns>
        public static MosaicProperties Create(bool supplyMutable, bool transferable, bool levyMutable, int divisibility,
            ulong duration)
        {
            return new MosaicProperties(supplyMutable, transferable, levyMutable, divisibility, duration);
        }

        public override string ToString()
        {
            return
                $"{nameof(IsSupplyMutable)}: {IsSupplyMutable}, {nameof(IsTransferable)}: {IsTransferable}, {nameof(IsLevyMutable)}: {IsLevyMutable}, {nameof(Divisibility)}: {Divisibility}, {nameof(Duration)}: {Duration}";
        }
    }
}