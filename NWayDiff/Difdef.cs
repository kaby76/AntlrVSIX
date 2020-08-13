using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace NWayDiff
{
    class Difdef
    {
        public const int MAX_FILES = 32;
        public int NUM_FILES;
        public Difdef_impl impl;

        public Difdef(int num_files)
        {
            NUM_FILES = num_files;
            impl = new Difdef_impl(num_files);
        }

        public void set_filter(Func<string, string> filter)
        {
            this.impl.filter = filter;
        }

        public void replace_file(int fileid, string fin)
        {
            this.impl.replace_file(fileid, fin);
        }

        public void set_up_sequece(int fileid, List<string> seq)
        {
            this.impl.set_up_sequence(fileid, seq);
        }

        public Diff merge()
        {
            int m = (1 << NUM_FILES) - 1;
            return impl.merge(m);
        }

        public Diff merge(int fileid1, int fileid2)
        {
            int m = (1 << fileid1) | (1 << fileid2);
            return impl.merge(m);
        }
        public Diff merge(HashSet<int> fileids)
        {
            int m = 0;
            foreach (var it in fileids)
            {
                m |= (1 << it);
            }
            return impl.merge(m);
        }

        public Diff simply_concatenate(List<List<string>> lists) { throw new NotImplementedException(); }

    }
}
