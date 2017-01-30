/* Copyright (c) 2012-2016 The ANTLR Project. All rights reserved.
 * Use of this file is governed by the BSD 3-clause license that
 * can be found in the LICENSE.txt file in the project root.
 */

using System;

namespace org.antlr.codebuff.misc
{

    public class Pair<A, B>
    {
        public readonly A a;
        public readonly B b;

        public Pair(A a, B b)
        {
            this.a = a;
            this.b = b;
        }

        public override bool Equals(object obj)
        {
            if (obj == this)
            {
                return true;
            }
            else if (!(obj is Pair<A, B>))
            {
                return false;
            }

            Pair<A, B> other = (Pair<A, B>)obj;
            if (a != null)
            {
                if (b != null)
                    return a.Equals(other.a) && b.Equals(other.b);
                else
                    return a.Equals(other.a) && other.b == null;
            }
            else
            {
                if (b != null)
                    return other.a == null && b.Equals(other.b);
                else
                    return other.a == null && other.b == null;
            }
        }

        public override int GetHashCode()
        {
            int hash = MurmurHash.Initialize();
            hash = MurmurHash.Update(hash, a);
            hash = MurmurHash.Update(hash, b);
            return MurmurHash.Finish(hash, 2);
        }

        public override String ToString()
        {
            return String.Format("({0}, {1})", a, b);
        }
    }

}