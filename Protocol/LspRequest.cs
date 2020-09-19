namespace Protocol
{
    //
    // Summary:
    //     Strongly typed object used to specify a LSP requests's parameter and return types.
    //
    // Type parameters:
    //   TIn:
    //     The parameter type.
    //
    //   TOut:
    //     The return type.
    public class LspRequest<TIn, TOut>
    {
        //
        // Summary:
        //     Initializes a new instance of the Microsoft.VisualStudio.LanguageServer.Protocol.LspRequest`2
        //     class.
        //
        // Parameters:
        //   name:
        //     The name of the JSON-RPC request
        public LspRequest(string name) { Name = name; }

        //
        // Summary:
        //     Gets the name of the JSON-RPC request.
        public string Name { get; }
    }
}
