using System.Collections.Immutable;
using System.Threading;
using System.Threading.Tasks;
using OmniSharp.Extensions.Embedded.MediatR;
using OmniSharp.Extensions.LanguageServer.Protocol;
using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;

namespace LanguageServer.Handlers
{
    internal sealed class WorkspaceSymbolsHandler : IWorkspaceSymbolsHandler
    {
        private readonly LanguageServer.Exec.LanguageServerWorkspace _workspace;

        public WorkspaceSymbolsHandler(LanguageServer.Exec.LanguageServerWorkspace workspace)
        {
            _workspace = workspace;
        }

        public async Task<SymbolInformationContainer> Handle(WorkspaceSymbolParams request, CancellationToken token)
        {
            return new SymbolInformationContainer();
            //var symbols = ImmutableArray.CreateBuilder<SymbolInformation>();

            //foreach (var document in _workspace.CurrentDocuments.Documents)
            //{
            //    await Helpers.FindSymbolsInDocument(searchService, document, request.Query, token, symbols);
            //}

            //return new SymbolInformationContainer(symbols);
        }

        public void SetCapability(WorkspaceSymbolCapability capability) { }

        object IRegistration<object>.GetRegistrationOptions()
        {
            return null;
        }
    }
}
