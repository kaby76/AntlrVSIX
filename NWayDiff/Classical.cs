using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NWayDiff
{
    class Classical
    {
        public static List<string> classical_lcs(List<string> a, List<string> b, int i, int j, Dictionary<Tuple<int, int>, List<string>> memo)
        {
            Tuple<int, int> key = new Tuple<int, int>(i, j);
            memo.TryGetValue(key, out List<string> val);
            if (val != null)
                return val;
            else if (i == 0 || j == 0)
            {
                List<string> result = new List<string>();
                memo[key] = result;
                return result;
            }
            else if (a[i-1] == b[j-1])
            {
                int oldi = i;
                while (i > 0 && j > 0 && a[i-1] == b[j-1])
                {
                    --i;
                    --j;
                }
                List<string> result = classical_lcs(a, b, i, j, memo);
                throw new Exception();
                // result.insert(result.end(), a.begin() + i, a.begin() + oldi);
                return result;
            }
            else
            {
                List<string> result1 = classical_lcs(a, b, i - 1, j, memo);
                List<string> result2 = classical_lcs(a, b, i, j - 1, memo);
                if (result1.Count > result2.Count)
                {
                    memo[key] = result1;
                    return result1;
                }
                else
                {
                    memo[key] = result2;
                    return result2;
                }
            }
        }
    }
}
