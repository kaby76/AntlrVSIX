using System;
using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Commanding;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Editor.Commanding;
using Microsoft.VisualStudio.Text.Editor.Commanding.Commands;
using Microsoft.VisualStudio.Utilities;
using System.IO;
using AntlrVSIX.Extensions;
using AntlrVSIX.Grammar;

namespace AntlrVSIX.File
{
    [Export(typeof(ICommandHandler))]
    [Name("Antlr")]
    [ContentType(Constants.ContentType)]
    [TextViewRole(PredefinedTextViewRoles.Document)]
    public class FileSaveListener : ICommandHandler<SaveCommandArgs>
    {
        public string DisplayName => nameof(FileSaveListener);

        private readonly IEditorCommandHandlerServiceFactory _commandService;

        [ImportingConstructor]
        public FileSaveListener(IEditorCommandHandlerServiceFactory commandService)
        {
            _commandService = commandService;
        }

        public bool ExecuteCommand(SaveCommandArgs args, CommandExecutionContext executionContext)
        {

            try
            {
                var view = args.TextView;
                var ffn = view.GetFilePath();
                if (Path.GetExtension(ffn).IsAntlrSuffix())
                {
                    StreamReader sr = new StreamReader(ffn);
                    ParserDetails foo = new ParserDetails();
                    ParserDetails._per_file_parser_details[ffn] = foo;
                    foo.Parse(sr.ReadToEnd(), ffn);
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
