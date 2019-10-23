using System;
using System.Collections.Generic;
using System.Text;

namespace Symtab
{
    public class NamespaceScope : BaseScope
    {
        public NamespaceScope(string name, string file, IScope enclosingScope) : base(enclosingScope)
        {
            Name = name;
            File = file;
        }

        public string File { get; set; }

    }
}
