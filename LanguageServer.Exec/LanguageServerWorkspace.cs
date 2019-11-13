using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using System;
using System.Collections.Generic;
using Workspaces;

namespace LanguageServer.Exec
{
    internal sealed class LanguageServerWorkspace : Workspace
    {
        private readonly string _rootPath;

        public LanguageServerWorkspace(string rootPath)
        {
            _rootPath = rootPath;
        }

        public Document GetDocument(Uri uri)
        {
            var name = Helpers.FromUri(uri);
            var doc = Workspace.Instance.FindDocument(name);
            return doc;
        }

        public (Document logicalDocument, int position) GetLogicalDocument(TextDocumentPositionParams textDocumentPositionParams)
        {
            Document document = GetDocument(textDocumentPositionParams.TextDocument.Uri);
            int documentPosition = LanguageServer.Module.GetIndex(
                (int) textDocumentPositionParams.Position.Line,
                (int) textDocumentPositionParams.Position.Character,
                document);
            return (document, documentPosition);
        }

        public Document OpenDocument(Uri uri, string text, string languageId)
        {
            var filePath = Helpers.FromUri(uri);
            var documentId = DocumentId.CreateNewId(filePath);
            var document = new Document(filePath, filePath);
            document.Code = text;
            return document;
        }

        public Document UpdateDocument(Document document, IEnumerable<TextChange> changes)
        {
 //           var newText = document.ApplyChanges(changes);
 //           document.Code = newText;
            return document;
        }

        public void CloseDocument(DocumentId documentId)
        {
           // OnDocumentClosed(documentId);
        }
    }
}
