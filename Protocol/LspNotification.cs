using System;
using System.Collections.Generic;
using System.Text;

namespace Protocol
{
    //
    // Summary:
    //     Strongly typed object used to specify a LSP notification's parameter type.
    //
    // Type parameters:
    //   TIn:
    //     The parameter type.
    public class LspNotification<TIn>
    {
        //
        // Summary:
        //     Initializes a new instance of the Microsoft.VisualStudio.LanguageServer.Protocol.LspNotification`1
        //     class.
        //
        // Parameters:
        //   name:
        //     The name of the JSON-RPC notification
        public LspNotification(string name) { Name = name; }

        //
        // Summary:
        //     Gets the name of the JSON-RPC notification.
        public string Name { get; }
    }
}
