using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using OmniSharp.Extensions.LanguageServer.Protocol.Server;
using System.Threading;
using System.Threading.Tasks;
using CompletionList = OmniSharp.Extensions.LanguageServer.Protocol.Models.CompletionList;

namespace LanguageServer.Handlers
{
    internal sealed class CompletionHandler : ICompletionHandler
    {
        private readonly LanguageServer.Exec.LanguageServerWorkspace _workspace;
        private readonly TextDocumentRegistrationOptions _registrationOptions;

        public CompletionHandler(LanguageServer.Exec.LanguageServerWorkspace workspace, TextDocumentRegistrationOptions registrationOptions)
        {
            _workspace = workspace;
            _registrationOptions = registrationOptions;
        }

        public CompletionRegistrationOptions GetRegistrationOptions()
        {
            return new CompletionRegistrationOptions
            {
                DocumentSelector = _registrationOptions.DocumentSelector,
                TriggerCharacters = new Container<string>(".", ":"),
                ResolveProvider = false
            };
        }

        public async Task<CompletionList> Handle(CompletionParams request, CancellationToken token)
        {
            var (document, position) = _workspace.GetLogicalDocument(request);
            return new CompletionList();


            //Microsoft.CodeAnalysis.Completion.CompletionTrigger trigger;
            //if (request.Context.TriggerKind == OmniSharp.Extensions.LanguageServer.Protocol.Models.CompletionTriggerKind.TriggerCharacter)
            //{
            //    trigger = Microsoft.CodeAnalysis.Completion.CompletionTrigger.CreateInsertionTrigger(request.Context.TriggerCharacter[0]);
            //}
            //else
            //{
            //    trigger = Microsoft.CodeAnalysis.Completion.CompletionTrigger.Invoke;
            //}

            //var completionList = await completionService.GetCompletionsAsync(document, position, trigger, cancellationToken: token);
            //if (completionList == null)
            //{
            //    return new CompletionList();
            //}

            //var completionItems = completionList.Items
            //    .Select(x => ConvertCompletionItem(document, completionList.Rules, x))
            //    .ToArray();

            //return completionItems;
        }

        public void SetCapability(CompletionCapability capability) { }
    }
}
