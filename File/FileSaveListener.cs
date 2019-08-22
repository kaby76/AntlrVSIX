namespace AntlrVSIX.File
{
    using AntlrVSIX.Extensions;
    using AntlrVSIX.Grammar;
    using Microsoft.VisualStudio.Commanding;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.Text.Editor.Commanding;
    using Microsoft.VisualStudio.Text.Editor.Commanding.Commands;
    using Microsoft.VisualStudio.Utilities;
    using System;
    using System.ComponentModel.Composition;
    using System.IO;

    [Export(typeof(ICommandHandler))]
    [ContentType("any")]
    [TextViewRole(PredefinedTextViewRoles.Editable)]
    public class FileSaveListener : ICommandHandler<SaveCommandArgs>
    {
        public string DisplayName => nameof(FileSaveListener);

        [ImportingConstructor]
        public FileSaveListener(IEditorCommandHandlerServiceFactory commandService)
        {
        }

        public bool ExecuteCommand(SaveCommandArgs args, CommandExecutionContext executionContext)
        {

            try
            {
                var view = args.TextView;
                var ffn = view.GetFilePath();
                var grammar_description = GrammarDescriptionFactory.Create(ffn);
                if (grammar_description != null)
                {
                    var buffer = view.TextBuffer;
                    var code = buffer.GetBufferText();
                    ParserDetails.Parse(code, ffn);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex);
            }

            return true;
        }

        public CommandState GetCommandState(SaveCommandArgs args)
        {
            return CommandState.Available;
        }
    }
}
