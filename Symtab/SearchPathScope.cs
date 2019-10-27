using System;
using System.Collections.Generic;
using System.Text;

namespace Symtab
{
    public class SearchPathScope : BaseScope
    {
        List<IScope> ordered_scopes = new List<IScope>();

        public SearchPathScope(IScope enclosingScope) : base(enclosingScope)
        {
        }
    }
}
