using System;

namespace org.antlr.codebuff.misc
{
    public class Triple<L, M, R>
    {

        private L left;
        private M middle;
        private R right;

        public Triple(L left, M middle, R right)
        {
            if (left == null)
            {
                throw new Exception("Left value is not effective.");
            }
            if (middle == null)
            {
                throw new Exception("Middle value is not effective.");
            }
            if (right == null)
            {
                throw new Exception("Right value is not effective.");
            }
            this.left = left;
            this.middle = middle;
            this.right = right;
        }

        public L getLeft()
        {
            return this.left;
        }

        public L a { get { return this.left; } }

        public M getMiddle()
        {
            return this.middle;
        }

        public M b { get { return this.middle; } }

        public R getRight()
        {
            return this.right;
        }

        public R c { get { return this.right; } }

        public override int GetHashCode()
        {
            const int prime = 31;
            int result = 1;
            result = prime*result + ((left == null) ? 0 : left.GetHashCode());
            result = prime*result + ((middle == null) ? 0 : middle.GetHashCode());
            result = prime*result + ((right == null) ? 0 : right.GetHashCode());
            return result;
        }

        public bool equals(Object obj)
        {
            if (this == obj)
                return true;
            if (obj == null)
                return false;
            if (GetType() != obj.GetType())
                return false;
            Triple<Object, Object, Object> other = (Triple<Object, Object, Object>) obj;
            if (left == null)
            {
                if (other.left != null)
                    return false;
            }
            else if (!left.Equals(other.left))
                return false;
            if (middle == null)
            {
                if (other.middle != null)
                    return false;
            }
            else if (!middle.Equals(other.middle))
                return false;
            if (right == null)
            {
                if (other.right != null)
                    return false;
            }
            else if (!right.Equals(other.right))
                return false;
            return true;
        }

        public override String ToString()
        {
            return "<" + left + "," + middle + "," + right + ">";
        }
    }
}