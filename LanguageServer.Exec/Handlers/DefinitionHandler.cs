using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace LanguageServer.Handlers
{
    internal sealed class DefinitionHandler : IDefinitionHandler
    {
        private readonly LanguageServer.Exec.LanguageServerWorkspace _workspace;
        private readonly TextDocumentRegistrationOptions _registrationOptions;

        public DefinitionHandler(LanguageServer.Exec.LanguageServerWorkspace workspace, TextDocumentRegistrationOptions registrationOptions)
        {
            _workspace = workspace;
            _registrationOptions = registrationOptions;
        }

        public TextDocumentRegistrationOptions GetRegistrationOptions() => _registrationOptions;

        public async Task<OmniSharp.Extensions.LanguageServer.Protocol.Models.LocationOrLocationLinks> Handle(DefinitionParams request, CancellationToken token)
        {
            var (document, position) = _workspace.GetLogicalDocument(request);
            var definitions = LanguageServer.Module.FindDef(position, document);
            var locations = new List<LocationOrLocationLink>();
            foreach (var definition in definitions)
            {
                var (ls, cs) = Module.GetLineColumn(definition.Range.Start.Value, document);
                var (le, ce) = Module.GetLineColumn(definition.Range.End.Value, document);
                locations.Add(new LocationOrLocationLink(
                    new OmniSharp.Extensions.LanguageServer.Protocol.Models.Location()
                    {
                        Uri = new System.Uri(definition.Uri.FullPath),
                        Range = new OmniSharp.Extensions.LanguageServer.Protocol.Models.Range(
                            new Position(ls, cs),
                            new Position(le, ce))
                    }));
            }

            return locations;
        }

        public void SetCapability(DefinitionCapability capability) { }
    }
}
