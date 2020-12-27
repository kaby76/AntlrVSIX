namespace Symtab
{
    using Antlr4.Runtime;
    using System.Collections.Generic;

    public class ChannelSymbol : BaseSymbol, ISymbol
    {
        public ChannelSymbol(string name, IList<IToken> token) : base(name, token)
        {
        }
    }
}
