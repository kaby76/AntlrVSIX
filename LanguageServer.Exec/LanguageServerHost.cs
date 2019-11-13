using LanguageServer.Handlers;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Server;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LanguageServer.Exec
{
    internal sealed class LanguageServerHost : IDisposable
    {
        private ILanguageServer _server;

        private LanguageServerWorkspace _workspace;

        private LanguageServerHost()
        {
        }

        public static async Task<LanguageServerHost> Create(
            Stream input,
            Stream output
            )
        {
            var result = new LanguageServerHost();
            await result.InitializeAsync(input, output);
            return result;
        }

        private async Task InitializeAsync(Stream input, Stream output)
        {
            _server = await OmniSharp.Extensions.LanguageServer.Server.LanguageServer.From(options => options
                .WithInput(input)
                .WithOutput(output)
                .OnInitialized(OnInitialized));

            var documentSelector = new DocumentSelector(
                LanguageServer.GrammarDescriptionFactory.AllLanguages
                    .Select(x => new DocumentFilter
                    {
                        Language = x.ToLowerInvariant()
                    }));

            var registrationOptions = new TextDocumentRegistrationOptions
            {
                DocumentSelector = documentSelector
            };

            _server.AddHandlers(
                new TextDocumentSyncHandler(_workspace, registrationOptions),
                new CompletionHandler(_workspace, registrationOptions),
                new DefinitionHandler(_workspace, registrationOptions),
                new WorkspaceSymbolsHandler(_workspace),
                new DocumentHighlightHandler(_workspace, registrationOptions),
                new DocumentSymbolsHandler(_workspace, registrationOptions),
                new HoverHandler(_workspace, registrationOptions),
                new SignatureHelpHandler(_workspace, registrationOptions));
        }

        private Task OnInitialized(ILanguageServer server, InitializeParams request, InitializeResult result)
        {
            _workspace = new LanguageServerWorkspace(request.RootPath);
            return Task.CompletedTask;
        }

        public Task WaitForExit => _server.WaitForExit;

        public void Dispose()
        {
            _server.Dispose();
        }
    }
}
