namespace Symtab
{
    using Antlr4.Runtime;

    public class ChannelSymbol : BaseSymbol, ISymbol
    {
        public ChannelSymbol(string name, IToken token) : base(name, token)
        {
        }
    }
}
