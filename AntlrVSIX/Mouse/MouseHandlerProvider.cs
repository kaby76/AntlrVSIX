namespace AntlrVSIX.Mouse
{
    using AntlrVSIX.Extensions;
    using AntlrVSIX.Keyboard;
    using Microsoft.VisualStudio.OLE.Interop;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Shell.Interop;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Classification;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text.Operations;
    using Microsoft.VisualStudio.Utilities;
    using System.ComponentModel.Composition;

    [Export(typeof(IMouseProcessorProvider))]
    [ContentType("text")]
    [Name("Antlr")]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    [Order(Before = "WordSelection")]
    internal sealed class MouseHandlerProvider : IMouseProcessorProvider
    {
        [Import]
        IClassifierAggregatorService AggregatorFactory = null;

        [Import]
        ITextStructureNavigatorSelectorService NavigatorService = null;

        [Import]
        SVsServiceProvider GlobalServiceProvider = null;

        public IMouseProcessor GetAssociatedProcessor(IWpfTextView view)
        {
            ITextBuffer buffer = view.TextBuffer;
            if (buffer == null) return null;
            string ffn = buffer.GetFFN().Result;
            var grammar_description = LanguageServer.GrammarDescriptionFactory.Create(ffn);
            if (grammar_description == null) return null;
            IOleCommandTarget shellCommandDispatcher = GetShellCommandDispatcher(view);
            if (shellCommandDispatcher == null) return null;
            IClassifier ag1 = AggregatorFactory.GetClassifier(buffer);
            return new MouseHandler(view,
                                           shellCommandDispatcher,
                                           GlobalServiceProvider,
                                           ag1,
                                           NavigatorService.GetTextStructureNavigator(buffer),
                                           CtrlKeyState.GetStateForView(view));
        }

        IOleCommandTarget GetShellCommandDispatcher(ITextView view)
        {
            return GlobalServiceProvider.GetService(typeof(SUIHostCommandDispatcher)) as IOleCommandTarget;
        }
    }
}
