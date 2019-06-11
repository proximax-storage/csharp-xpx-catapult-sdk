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

namespace ProximaX.Sirius.Sdk.Crypto.Core.Chaso.NaCl.Internal.Ed25519ref10
{
    internal static partial class FieldOperations
    {
        /*
		h = f - g
		Can overlap h with f or g.

		Preconditions:
		   |f| bounded by 1.1*2^25,1.1*2^24,1.1*2^25,1.1*2^24,etc.
		   |g| bounded by 1.1*2^25,1.1*2^24,1.1*2^25,1.1*2^24,etc.

		Postconditions:
		   |h| bounded by 1.1*2^26,1.1*2^25,1.1*2^26,1.1*2^25,etc.
		*/

        internal static void fe_sub(out FieldElement h, ref FieldElement f, ref FieldElement g)
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
            var h0 = f0 - g0;
            var h1 = f1 - g1;
            var h2 = f2 - g2;
            var h3 = f3 - g3;
            var h4 = f4 - g4;
            var h5 = f5 - g5;
            var h6 = f6 - g6;
            var h7 = f7 - g7;
            var h8 = f8 - g8;
            var h9 = f9 - g9;
            h.x0 = h0;
            h.x1 = h1;
            h.x2 = h2;
            h.x3 = h3;
            h.x4 = h4;
            h.x5 = h5;
            h.x6 = h6;
            h.x7 = h7;
            h.x8 = h8;
            h.x9 = h9;
        }
    }
}