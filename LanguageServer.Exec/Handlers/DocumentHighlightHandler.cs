//using System.Collections.Generic;
//using System.Collections.Immutable;
//using System.Threading;
//using System.Threading.Tasks;
//using OmniSharp.Extensions.LanguageServer.Protocol.Client.Capabilities;
//using OmniSharp.Extensions.LanguageServer.Protocol.Models;
//using OmniSharp.Extensions.LanguageServer.Protocol.Server;

//namespace LanguageServer.Handlers
//{
//    internal sealed class DocumentHighlightHandler : IDocumentHighlightHandler
//    {
//        private readonly LanguageServer.Exec.LanguageServerWorkspace _workspace;
//        private readonly TextDocumentRegistrationOptions _registrationOptions;

//        public DocumentHighlightHandler(LanguageServer.Exec.LanguageServerWorkspace workspace, TextDocumentRegistrationOptions registrationOptions)
//        {
//            _workspace = workspace;
//            _registrationOptions = registrationOptions;
//        }

//        public TextDocumentRegistrationOptions GetRegistrationOptions() => _registrationOptions;

//        public async Task<DocumentHighlightContainer> Handle(DocumentHighlightParams request, CancellationToken token)
//        {
//            var (document, position) = _workspace.GetLogicalDocument(request);


//            var result = new List<DocumentHighlight>();

//            //foreach (var documentHighlights in documentHighlightsList)
//            //{
//            //    if (documentHighlights.Document != document)
//            //    {
//            //        continue;
//            //    }

//            //    foreach (var highlightSpan in documentHighlights.HighlightSpans)
//            //    {
//            //        result.Add(new DocumentHighlight
//            //        {
//            //            Kind = DocumentHighlightKind.Write,
//            //            Range = Helpers.ToRange(document.SourceText, highlightSpan.TextSpan)
//            //        });
//            //    }
//            //}

//            return result;
//        }

//        public void SetCapability(DocumentHighlightCapability capability) { }
//    }
//}
