namespace AntlrVSIX.Intellisense
{
    using Microsoft.VisualStudio.Language.Intellisense;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Utilities;
    using System.Collections.Generic;
    using System.ComponentModel.Composition;

    [Export(typeof(IIntellisenseControllerProvider))]
    [Name("Template QuickInfo Controller")]
    [ContentType("any")]
    internal class TemplateQuickInfoControllerProvider : IIntellisenseControllerProvider
    {
        [Import]
        internal IQuickInfoBroker QuickInfoBroker { get; set; }

        public IIntellisenseController TryCreateIntellisenseController(ITextView textView,
            IList<ITextBuffer> subjectBuffers)
        {
            return new TemplateQuickInfoController(textView, subjectBuffers, this);
        }
    }
}