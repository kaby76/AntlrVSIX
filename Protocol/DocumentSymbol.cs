using System.Runtime.Serialization;

namespace Protocol
{
    //
    // Summary:
    //     Represents programming constructs like variables, classes, interfaces etc. that
    //     appear in a document. Document symbols can be hierarchical and they have two
    //     ranges: one that encloses its definition and one that points to its most interesting
    //     range, e.g. the range of an identifier.
    [DataContract]
    public class DocumentSymbol
    {
        public DocumentSymbol() { }

        //
        // Summary:
        //     Gets or sets the name of this symbol.
        [DataMember(IsRequired = true, Name = "name")]
        public string Name { get; set; }
        //
        // Summary:
        //     Gets or sets more detail for this symbol, e.g the signature of a function.
        [DataMember(Name = "detail")]
        public string Detail { get; set; }
        //
        // Summary:
        //     Gets or sets the Microsoft.VisualStudio.LanguageServer.Protocol.SymbolKind of
        //     this symbol.
        [DataMember(Name = "kind")]
        public SymbolKind Kind { get; set; }
        //
        // Summary:
        //     Gets or sets a value indicating whether this symbol is deprecated.
        [DataMember(Name = "deprecated")]
        public bool? Deprecated { get; set; }
        //
        // Summary:
        //     Gets or sets the range enclosing this symbol not including leading/trailing whitespace
        //     but everything else like comments.This information is typically used to determine
        //     if the clients cursor is inside the symbol to reveal in the symbol in the UI.
        [DataMember(IsRequired = true, Name = "range")]
        public Range Range { get; set; }
        //
        // Summary:
        //     Gets or sets the range that should be selected and revealed when this symbol
        //     is being picked, e.g the name of a function. Must be contained by the `range`.
        [DataMember(IsRequired = true, Name = "selectionRange")]
        public Range SelectionRange { get; set; }
        //
        // Summary:
        //     Gets or sets the children of this symbol, e.g. properties of a class.
        [DataMember(Name = "children")]
        public DocumentSymbol[] Children { get; set; }
    }
}
