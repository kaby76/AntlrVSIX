using System;
using System.Collections.Generic;
using System.Text;

namespace NWayDiff
{
    class Line
    {
        public string text;
        public int mask;  // a bitmask

        public bool in_file(int fileid)
        {
            return (this.mask & (1 << fileid)) != 0;
        }

        public Line()
        {
            text = null;
            mask = 0;
        }

        public Line(string s, int m)
        {
            text = s;
            this.mask = m;
        }
    }
}
