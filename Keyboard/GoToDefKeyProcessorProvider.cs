namespace AntlrVSIX.Keyboard
{
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Utilities;
    using System.ComponentModel.Composition;

    [Export(typeof(IKeyProcessorProvider))]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    [ContentType("code")]
    [Name("Antlr")]
    [Order(Before = "VisualStudioKeyboardProcessor")]
    internal sealed class GoToDefKeyProcessorProvider : IKeyProcessorProvider
    {
        public KeyProcessor GetAssociatedProcessor(IWpfTextView view)
        {
            return view.Properties.GetOrCreateSingletonProperty(typeof(GoToDefKeyProcessor),
                () => new GoToDefKeyProcessor(CtrlKeyState.GetStateForView(view)));
        }
    }
}