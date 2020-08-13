using System;
using System.Collections.Generic;
using System.Text;

namespace NWayDiff
{
    class Difdef_StringSet
    {
        public int NUM_FILES;
        public Dictionary<string, List<int>> unique_lines;
        public Difdef_StringSet(int num_files)
        {
            NUM_FILES = num_files;
            unique_lines = new Dictionary<string, List<int>>();
        }

        public string add(int fileid, string text)
        {
            unique_lines.TryGetValue(text, out List<int> p);
            if (p == null)
            {
                var d = new List<int>();
                for (int i = 0; i < NUM_FILES; ++i) d.Add(0);
                d[fileid] = 1;
                unique_lines[text] = d;
                p = d;
            }
            else
            {
                p[fileid] = p[fileid] + 1;
            }
            return text;
        }

        public List<int> lookup(string text)
        {
            return unique_lines[text];
        }
    }
}
