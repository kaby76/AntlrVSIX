namespace LspAntlr
{
    using LanguageServer;
    using Microsoft.VisualStudio.Shell;
    using Microsoft.VisualStudio.Text;
    using Microsoft.VisualStudio.Text.Editor;
    using Microsoft.VisualStudio.TextManager.Interop;
    using System;
    using System.ComponentModel.Design;

    public class GoToVisitorCommand
    {
        private readonly AntlrLanguageClient _package;
        private readonly MenuCommand _menu_item1;
        private readonly MenuCommand _menu_item2;
        private readonly MenuCommand _menu_item3;
        private readonly MenuCommand _menu_item4;

        private GoToVisitorCommand(AntlrLanguageClient package)
        {
            _package = package ?? throw new ArgumentNullException(nameof(package));
            OleMenuCommandService commandService = ((IServiceProvider)ServiceProvider).GetService(
                typeof(IMenuCommandService)) as OleMenuCommandService ?? throw new Exception("Command service not found.");

            {
                // Set up hook for context menu.
                CommandID menuCommandID = new CommandID(new Guid(LspAntlr.Constants.guidVSPackageCommandCodeWindowContextMenuCmdSet), 0x7005);
                _menu_item1 = new MenuCommand(MenuItemCallbackListener, menuCommandID)
                {
                    Enabled = true,
                    Visible = true
                };
                commandService.AddCommand(_menu_item1);
            }
            {
                // Set up hook for context menu.
                CommandID menuCommandID = new CommandID(new Guid(LspAntlr.Constants.guidVSPackageCommandCodeWindowContextMenuCmdSet), 0x7006);
                _menu_item2 = new MenuCommand(MenuItemCallbackVisitor, menuCommandID)
                {
                    Enabled = true,
                    Visible = true
                };
                commandService.AddCommand(_menu_item2);
            }
            {
                // Set up hook for context menu.
                CommandID menuCommandID = new CommandID(new Guid(LspAntlr.Constants.guidMenuAndCommandsCmdSet), 0x7005);
                _menu_item3 = new MenuCommand(MenuItemCallbackListener, menuCommandID)
                {
                    Enabled = true,
                    Visible = true
                };
                commandService.AddCommand(_menu_item3);
            }
            {
                // Set up hook for context menu.
                CommandID menuCommandID = new CommandID(new Guid(LspAntlr.Constants.guidMenuAndCommandsCmdSet), 0x7006);
                _menu_item4 = new MenuCommand(MenuItemCallbackVisitor, menuCommandID)
                {
                    Enabled = true,
                    Visible = true
                };
                commandService.AddCommand(_menu_item4);
            }
        }

        public bool Enabled
        {
            set
            {
                if (_menu_item1 != null)
                {
                    _menu_item1.Enabled = value;
                }

                if (_menu_item2 != null)
                {
                    _menu_item2.Enabled = value;
                }

                if (_menu_item3 != null)
                {
                    _menu_item3.Enabled = value;
                }

                if (_menu_item4 != null)
                {
                    _menu_item4.Enabled = value;
                }
            }
        }

        public bool Visible
        {
            set { }
        }

        public static GoToVisitorCommand Instance { get; private set; }

        private AntlrLanguageClient ServiceProvider => _package;

        public static void Initialize(AntlrLanguageClient package)
        {
            Instance = new GoToVisitorCommand(package);
        }

        private void MenuItemCallbackVisitor(object sender, EventArgs e)
        {
            MenuItemCallback(sender, e, true);
        }

        private void MenuItemCallbackListener(object sender, EventArgs e)
        {
            MenuItemCallback(sender, e, false);
        }

#pragma warning disable VSTHRD100
        private async void MenuItemCallback(object sender, EventArgs e, bool visitor)
#pragma warning restore VSTHRD100
        {
            try
            {
                ////////////////////////
                /// Go to visitor.
                ////////////////////////

                IVsTextManager manager = ((IServiceProvider)ServiceProvider).GetService(typeof(VsTextManagerClass)) as IVsTextManager;
                if (manager == null)
                {
                    return;
                }

                manager.GetActiveView(1, null, out IVsTextView view);
                if (view == null)
                {
                    return;
                }

                view.GetCaretPos(out int l, out int c);
                view.GetBuffer(out IVsTextLines buf);
                if (buf == null)
                {
                    return;
                }

                IWpfTextView xxx = AntlrLanguageClient.AdaptersFactory.GetWpfTextView(view);
                ITextBuffer buffer = xxx.TextBuffer;
                string orig_ffn = buffer.GetFFN();
                if (orig_ffn == null)
                {
                    return;
                }

                Workspaces.Document document = Workspaces.Workspace.Instance.FindDocument(orig_ffn);
                if (document == null)
                {
                    return;
                }

                int pos = LanguageServer.Module.GetIndex(l, c, document);
                CMGotoResult symbol = null;
                if (visitor)
                {
                    symbol = AntlrLanguageClient.CMGotoVisitor(orig_ffn, pos);
                }
                else
                {
                    bool is_enter = CtrlKeyState.GetStateForView(xxx).Enabled;
                    symbol = AntlrLanguageClient.CMGotoListener(orig_ffn, is_enter, pos);
                }
                if (symbol == null)
                {
                    return;
                }

                {
                    string class_file_path = symbol.TextDocument.LocalPath;
                    int index = symbol.Start;
                    // Open to this line in editor.
                    IVsTextView vstv = IVsTextViewExtensions.FindTextViewFor(class_file_path);
                    {
                        IVsTextViewExtensions.ShowFrame(ServiceProvider, class_file_path);
                        vstv = IVsTextViewExtensions.FindTextViewFor(class_file_path);
                    }

                    IWpfTextView wpftv = AntlrLanguageClient.AdaptersFactory.GetWpfTextView(vstv);
                    if (wpftv == null)
                    {
                        return;
                    }

                    // Create new span in the appropriate view.
                    ITextSnapshot cc = wpftv.TextBuffer.CurrentSnapshot;
                    vstv.GetLineAndColumn(index, out int line_number, out int colum_number);

                    // Put cursor on symbol.
                    wpftv.Caret.MoveTo(new SnapshotPoint(cc, index)); // This sets cursor, bot does not center.
                                                                      // Center on cursor.
                                                                      //wpftv.Caret.EnsureVisible(); // This works, sort of. It moves the scroll bar, but it does not CENTER! Does not really work!
                    if (line_number > 0)
                    {
                        vstv.CenterLines(line_number - 1, 2);
                    }
                    else
                    {
                        vstv.CenterLines(line_number, 1);
                    }

                    return;
                }
            }
            catch (Exception exception)
            {
                Logger.Log.Notify(exception.StackTrace);
            }
        }
    }
}
