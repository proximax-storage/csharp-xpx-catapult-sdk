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

namespace ProximaX.Sirius.Chain.Sdk.Crypto.Core.Chaso.NaCl.Internal.Ed25519ref10
{
    internal static partial class FieldOperations
    {
        /*
		Replace (f,g) with (g,g) if b == 1;
		replace (f,g) with (f,g) if b == 0.

		Preconditions: b in {0,1}.
		*/

        //void fe_cmov(fe f,const fe g,unsigned int b)
        internal static void fe_cmov(ref FieldElement f, ref FieldElement g, int b)
        {
            var f0 = f.x0;
            var f1 = f.x1;
            var f2 = f.x2;
            var f3 = f.x3;
            var f4 = f.x4;
            var f5 = f.x5;
            var f6 = f.x6;
            var f7 = f.x7;
            var f8 = f.x8;
            var f9 = f.x9;
            var g0 = g.x0;
            var g1 = g.x1;
            var g2 = g.x2;
            var g3 = g.x3;
            var g4 = g.x4;
            var g5 = g.x5;
            var g6 = g.x6;
            var g7 = g.x7;
            var g8 = g.x8;
            var g9 = g.x9;
            var x0 = f0 ^ g0;
            var x1 = f1 ^ g1;
            var x2 = f2 ^ g2;
            var x3 = f3 ^ g3;
            var x4 = f4 ^ g4;
            var x5 = f5 ^ g5;
            var x6 = f6 ^ g6;
            var x7 = f7 ^ g7;
            var x8 = f8 ^ g8;
            var x9 = f9 ^ g9;
            b = -b;
            x0 &= b;
            x1 &= b;
            x2 &= b;
            x3 &= b;
            x4 &= b;
            x5 &= b;
            x6 &= b;
            x7 &= b;
            x8 &= b;
            x9 &= b;
            f.x0 = f0 ^ x0;
            f.x1 = f1 ^ x1;
            f.x2 = f2 ^ x2;
            f.x3 = f3 ^ x3;
            f.x4 = f4 ^ x4;
            f.x5 = f5 ^ x5;
            f.x6 = f6 ^ x6;
            f.x7 = f7 ^ x7;
            f.x8 = f8 ^ x8;
            f.x9 = f9 ^ x9;
        }
    }
}