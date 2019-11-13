using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Workspaces
{
    public class TextChange
    {
        Range _range;
        string _replacement;

        public TextChange(Range range, string replacement)
        {
            _range = range;
            _replacement = replacement;
        }
    }
}
