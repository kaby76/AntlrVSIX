namespace AntlrVSIX.ErrorTagger
{
    using AntlrVSIX.Extensions;
    using LanguageServer;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Adornments;
    using Microsoft.VisualStudio.Text.Tagging;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Windows.Threading;

    class ErrorTagger : ITagger<IErrorTag>
    {
        private ITextBuffer _buffer;

        public ErrorTagger(ITextBuffer buffer)
        {
            _buffer = buffer;
            var doc = _buffer.GetFFN().Result;
            if (doc == null) return;
            var gd = LanguageServer.GrammarDescriptionFactory.Create(doc);
            if (gd == null) return;
            if (!gd.DoErrorSquiggles) return;

            ThreadHelper.Generic.BeginInvoke(DispatcherPriority.ApplicationIdle, () =>
            {
                var span = new SnapshotSpan(buffer.CurrentSnapshot, 0, _buffer.CurrentSnapshot.Length);
                TagsChanged?.Invoke(this, new SnapshotSpanEventArgs(span));
            });
        }

        private void DocumentValidated(object sender, EventArgs e)
        {
            var span = new SnapshotSpan(_buffer.CurrentSnapshot, 0, _buffer.CurrentSnapshot.Length);
            TagsChanged?.Invoke(this, new SnapshotSpanEventArgs(span));
        }

        public IEnumerable<ITagSpan<IErrorTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            var ffn = _buffer.GetFFN().Result;
            if (ffn == null) yield break;
            var grammar_description = LanguageServer.GrammarDescriptionFactory.Create(ffn);
            if (grammar_description == null) yield break;
            var document = Workspaces.Workspace.Instance.FindDocument(ffn);
            if (document == null) yield break;
            var details = ParserDetailsFactory.Create(document);
            if (!grammar_description.DoErrorSquiggles) yield break;
            foreach (SnapshotSpan curSpan in spans)
            {
                SnapshotPoint start = curSpan.Start;
                SnapshotPoint end = curSpan.End;
                var text = curSpan.GetText();
                int curLocStart = start.Position;
                int curLocEnd = end.Position;
                var sorted_combined_tokens = LanguageServer.Module.GetErrors(
                    new Workspaces.Range(curLocStart, curLocEnd),
                    document);
                foreach (var p in sorted_combined_tokens)
                {
                    int start_token_start = p.Start.Value;
                    int end_token_end = p.End.Value;
                    int length = end_token_end - start_token_start + 1;

                    // Make sure the length doesn't go past the end of the current span.
                    if (start_token_start + length > curLocEnd)
                        length = curLocEnd - start_token_start;

                    var a = curSpan.Snapshot.TextBuffer.CurrentSnapshot;
                    var b = curSpan.Snapshot;

                    var tokenSpan = new SnapshotSpan(
                        curSpan.Snapshot.TextBuffer.CurrentSnapshot,
                        new Span(start_token_start, length));

                    ErrorCategory type = ErrorCategory.Error;

                    if (tokenSpan.IntersectsWith(curSpan))
                    {
                        string errorType = GetErrorType(type);
                        TagSpan<IErrorTag> t = new TagSpan<IErrorTag>(tokenSpan, new ErrorTag(errorType));
                        yield return t;
                    }
                }
            }
        }

        public static string GetErrorType(ErrorCategory errorType)
        {
            switch (errorType)
            {
                case ErrorCategory.Error:
                    return PredefinedErrorTypeNames.SyntaxError;
                case ErrorCategory.Warning:
                    return PredefinedErrorTypeNames.Warning;
            }
            return ErrorFormatDefinition.Suggestion;
        }

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;
    }
}
