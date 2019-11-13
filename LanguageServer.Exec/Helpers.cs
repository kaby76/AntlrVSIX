using System;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using OmniSharp.Extensions.LanguageServer.Protocol.Models;
using Range = OmniSharp.Extensions.LanguageServer.Protocol.Models.Range;
using Workspaces;

namespace LanguageServer.Exec
{
    internal static class Helpers
    {
        public static string FromUri(Uri uri)
        {
            if (uri.Segments.Length > 1)
            {
                if (uri.Segments[1].IndexOf("%3a", StringComparison.OrdinalIgnoreCase) > -1)
                {
                    return FromUri(new Uri(uri.AbsoluteUri.Replace("%3a", ":").Replace("%3A", ":")));
                }
            }
            return uri.LocalPath;
        }

        public static Uri ToUri(string filePath)
        {
            filePath = filePath.Replace('\\', '/');

            return (!filePath.StartsWith("/"))
                ? new Uri($"file:///{filePath}")
                : new Uri($"file://{filePath}");
        }

        //public static Range ToRange(SourceText sourceText, TextSpan textSpan)
        //{
        //    var linePositionSpan = sourceText.Lines.GetLinePositionSpan(textSpan);

        //    return new Range
        //    {
        //        Start = new Position
        //        {
        //            Line = linePositionSpan.Start.Line,
        //            Character = linePositionSpan.Start.Character
        //        },
        //        End = new Position
        //        {
        //            Line = linePositionSpan.End.Line,
        //            Character = linePositionSpan.End.Character
        //        }
        //    };
        //}


        //public static TextChange ToTextChange(Document document, Range changeRange, string insertString)
        //{
        //    var startPosition = document.SourceText.Lines.GetPosition(ToLinePosition(changeRange.Start));
        //    var endPosition = document.SourceText.Lines.GetPosition(ToLinePosition(changeRange.End));

        //    return new TextChange(
        //        TextSpan.FromBounds(startPosition, endPosition), 
        //        insertString);
        //}

        //private static LinePosition ToLinePosition(Position position)
        //{
        //    return new LinePosition((int)position.Line, (int) position.Character);
        //}

        //public static async Task FindSymbolsInDocument(
        //    INavigateToSearchService searchService,
        //    Document document,
        //    string searchPattern,
        //    CancellationToken cancellationToken,
        //    ImmutableArray<SymbolInformation>.Builder resultsBuilder)
        //{
        //    var foundSymbols = await searchService.SearchDocumentAsync(document, searchPattern, cancellationToken);

        //    resultsBuilder.AddRange(foundSymbols
        //       .Select(r => new SymbolInformation
        //       {
        //           ContainerName = r.AdditionalInformation,
        //           Kind = GetSymbolKind(r.Kind),
        //           Location = new Location
        //           {
        //               Uri = ToUri(r.NavigableItem.SourceSpan.File.FilePath),
        //               Range = ToRange(r.NavigableItem.Document.SourceText, r.NavigableItem.SourceSpan.Span)
        //           },
        //           Name = r.Name
        //       }));
        //}

        //private static SymbolKind GetSymbolKind(string symbolType)
        //{
        //    switch (symbolType)
        //    {
        //        case NavigateToItemKind.Class:
        //            return SymbolKind.Class;

        //        case NavigateToItemKind.Structure:
        //            return SymbolKind.Struct;

        //        case NavigateToItemKind.Module:
        //            return SymbolKind.Namespace;

        //        case NavigateToItemKind.Interface:
        //            return SymbolKind.Interface;

        //        case NavigateToItemKind.Field:
        //            return SymbolKind.Field;

        //        case NavigateToItemKind.Method:
        //            return SymbolKind.Method;

        //        default:
        //            return SymbolKind.Variable;
        //    }
        //}

        public static string ToLspLanguage(string language)
        {
            return language.ToLowerInvariant();
        }
    }
}
