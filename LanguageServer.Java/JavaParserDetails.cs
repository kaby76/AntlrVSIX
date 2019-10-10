using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LanguageServer.Java
{
    class JavaParserDetails : ParserDetails
    {
        public JavaParserDetails(Workspaces.Document item)
            : base(item)
        { }
    }
}
