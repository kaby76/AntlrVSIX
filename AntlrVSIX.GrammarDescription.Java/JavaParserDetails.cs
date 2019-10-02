using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AntlrVSIX.GrammarDescription.Java
{
    class JavaParserDetails : ParserDetails
    {
        public Pass1Listener _pass1;
        public Pass2Listener _pass2;
    }
}
