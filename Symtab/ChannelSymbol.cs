namespace Symtab
{
    using Antlr4.Runtime;

    public class ChannelSymbol : BaseSymbol, Symbol
    {
        public ChannelSymbol(string name, IToken token) : base(name, token)
        {
        }
    }
}
