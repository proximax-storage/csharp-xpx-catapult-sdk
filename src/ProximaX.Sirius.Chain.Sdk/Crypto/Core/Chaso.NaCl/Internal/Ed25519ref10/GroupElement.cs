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
    /*
    ge means group element.

    Here the group is the set of pairs (x,y) of field elements (see fe.h)
    satisfying -x^2 + y^2 = 1 + d x^2y^2
    where d = -121665/121666.

    Representations:
      ge_p2 (projective): (X:Y:Z) satisfying x=X/Z, y=Y/Z
      ge_p3 (extended): (X:Y:Z:T) satisfying x=X/Z, y=Y/Z, XY=ZT
      ge_p1p1 (completed): ((X:Z),(Y:T)) satisfying x=X/Z, y=Y/T
      ge_precomp (Duif): (y+x,y-x,2dxy)
    */

    internal struct GroupElementP2
    {
        internal FieldElement X;
        internal FieldElement Y;
        internal FieldElement Z;
    }

    internal struct GroupElementP3
    {
        internal FieldElement X;
        internal FieldElement Y;
        internal FieldElement Z;
        internal FieldElement T;
    }

    internal struct GroupElementP1P1
    {
        internal FieldElement X;
        internal FieldElement Y;
        internal FieldElement Z;
        internal FieldElement T;
    }

    internal struct GroupElementPreComp
    {
        internal FieldElement yplusx;
        internal FieldElement yminusx;
        internal FieldElement xy2d;

        internal GroupElementPreComp(FieldElement yplusx, FieldElement yminusx, FieldElement xy2d)
        {
            this.yplusx = yplusx;
            this.yminusx = yminusx;
            this.xy2d = xy2d;
        }
    }

    internal struct GroupElementCached
    {
        internal FieldElement YplusX;
        internal FieldElement YminusX;
        internal FieldElement Z;
        internal FieldElement T2d;
    }
}