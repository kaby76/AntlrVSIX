using System;
using System.Collections.Generic;
using System.Text;

namespace NWayDiff
{
    class Diff
    {
        public int dimension;
        public List<Line> lines;
        public int mask;

        public Diff assign(Diff rhs)
        {
            this.mask = rhs.mask;
            this.lines = rhs.lines;
            return this;
        }
        public bool includes_file(int fileid)
        {
            return (this.mask & (1 << fileid)) != 0;
        }

        public int all_files_mask()
        {
            return this.mask;
        }

        public Diff(int num_files, int mask)
        {
            dimension = num_files;
            this.lines = new List<Line>();
            this.mask = mask;
        }
        public void append(Diff rhs)
        {
            this.lines.AddRange(rhs.lines);
        }
    };
}
