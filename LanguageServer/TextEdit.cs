using System;
using System.Collections.Generic;
using System.Text;

namespace LanguageServer
{
    public class TextEdit
    {
        public Workspaces.Range range;
        public string NewText;
    }
}
